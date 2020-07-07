using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WebAPIControllers.Extensions;

namespace WebAPIControllers
{

    public class AKVChangeToken : IChangeToken
    {
        public bool HasChanged => false;

        public bool ActiveChangeCallbacks => false;

        public IDisposable RegisterChangeCallback(Action<object> callback, object state)
        {
            return null;
        }
    }

    public class AKVConfigurationProvider : IConfigurationProvider
    {
        private string _secret;
        private string _appclientid;
        private string _keyvaultbase;

        public enum AuthMethod { LocalCert, Secret, MSI }

        Tuple<string, string> result = null;
        string[] keys = { };
        AuthMethod _useLocalCert;

        public AKVConfigurationProvider(AuthMethod useLocalCert, string secret, string keyvaultbase, string appclientid)
        {
            _useLocalCert = useLocalCert;
            _secret = secret;
            _keyvaultbase = keyvaultbase;
            _appclientid = appclientid;
        }

        public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
        {
            return keys.AsEnumerable();
        }

        public IChangeToken GetReloadToken()
        {
            return new AKVChangeToken();
        }

        public void Load()
        {
            //we need to fulfil a synchronous contract with an async implementation
            //hence we need to wait on the calling thread for the async op to complete
            using (AutoResetEvent ar = new AutoResetEvent(false))
            {
                Exception fail = null;
                //TODO handle the error continuation
                switch (_useLocalCert)
                {
                    case AuthMethod.LocalCert:
                        KeyvaultSecrets.GetAKVSecretsUsingCert(_keyvaultbase).ContinueWith((a) =>
                        {
                            result = a.Result;
                            ar.Set();
                        }, System.Threading.Tasks.TaskContinuationOptions.OnlyOnRanToCompletion);

                        KeyvaultSecrets.GetAKVSecretsUsingCert(_keyvaultbase).ContinueWith((a) =>
                        {

                            fail = a.Exception.InnerException != null ? a.Exception.InnerException : a.Exception;
                            ar.Set();
                        }, System.Threading.Tasks.TaskContinuationOptions.OnlyOnFaulted);
                        break;
                    case AuthMethod.Secret:
                        KeyvaultSecrets.GetAKVSecretsUsingSecret(_secret, _appclientid, _keyvaultbase).ContinueWith((a) =>
                        {
                            result = a.Result;
                            ar.Set();
                        }, System.Threading.Tasks.TaskContinuationOptions.OnlyOnRanToCompletion);

                        KeyvaultSecrets.GetAKVSecretsUsingSecret(_secret, _appclientid, _keyvaultbase).ContinueWith((a) =>
                        {

                            fail = a.Exception.InnerException != null ? a.Exception.InnerException : a.Exception;
                            ar.Set();
                        }, System.Threading.Tasks.TaskContinuationOptions.OnlyOnFaulted);
                        break;
                    case AuthMethod.MSI:
                        KeyvaultSecrets.GetAKVSecretsUsingMSI(_keyvaultbase).ContinueWith((a) =>
                        {
                            result = a.Result;
                            ar.Set();
                        }, System.Threading.Tasks.TaskContinuationOptions.OnlyOnRanToCompletion);

                        KeyvaultSecrets.GetAKVSecretsUsingMSI(_keyvaultbase).ContinueWith((a) =>
                        {

                            fail = a.Exception.InnerException != null ? a.Exception.InnerException : a.Exception;
                            ar.Set();
                        }, System.Threading.Tasks.TaskContinuationOptions.OnlyOnFaulted);
                        break;
                }
                bool signaled = ar.WaitOne(10000); //TODO constant
                if (fail != null)
                {
                    throw fail;
                }
                if (!signaled)
                {
                    throw new Exception("Config load timeout exceeded");
                }
            }
        }

        public void Set(string key, string value)
        {
            throw new NotImplementedException();
        }

        public bool TryGet(string key, out string value)
        {
            switch (key)
            {
                case "sqlconnectionstring":
                    value = result.Item2;
                    return true;
            }
            value = "";
            return false;
        }
    }

    public class AKVConfigurationSourceSecret : IConfigurationSource
    {
        string _secret;
        string _aadappid;
        string _keyvaulturl;

        public AKVConfigurationSourceSecret(string keyvaulturl, string secret, string aadappid)
        {
            _secret = secret;
            _aadappid = aadappid;
            _keyvaulturl = keyvaulturl;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new AKVConfigurationProvider(AKVConfigurationProvider.AuthMethod.Secret, _secret, _keyvaulturl, _aadappid);
        }
    }

    public class AKVConfigurationSourceCert : IConfigurationSource
    {
        string _aadappid;
        string _keyvaulturl;

        public AKVConfigurationSourceCert(string keyvaulturl, string aadappid)
        {
            _aadappid = aadappid;
            _keyvaulturl = keyvaulturl;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new AKVConfigurationProvider(AKVConfigurationProvider.AuthMethod.LocalCert, "", _keyvaulturl, _aadappid);
        }
    }

    public class AKVConfigurationSourceMSI : IConfigurationSource
    {
        string _aadappid;
        string _keyvaulturl;

        public AKVConfigurationSourceMSI(string keyvaulturl, string aadappid)
        {
            _aadappid = aadappid;
            _keyvaulturl = keyvaulturl;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new AKVConfigurationProvider(AKVConfigurationProvider.AuthMethod.MSI, "", _keyvaulturl, _aadappid);
        }
    }

    public static class AKVConfigurationSourceExtensions
    {
        public static void AddAKVwithAppRegistrationCertAuth(this IConfigurationBuilder builder, string keyvaulturl, string aadappid)
        {
            var source = new AKVConfigurationSourceCert(keyvaulturl, aadappid);
            builder.Add(source);
        }

        public static void AddAKVwithAppRegistrationSecretAuth(this IConfigurationBuilder builder, string keyvaulturl, string secret, string aadappid)
        {
            var source = new AKVConfigurationSourceSecret(keyvaulturl, secret, aadappid);
            builder.Add(source);
        }

        public static void AddAKVwithMSIAuth(this IConfigurationBuilder builder, string keyvaulturl, string aadappid)
        {
            var source = new AKVConfigurationSourceMSI(keyvaulturl, aadappid);
            builder.Add(source);
        }
    }
}
