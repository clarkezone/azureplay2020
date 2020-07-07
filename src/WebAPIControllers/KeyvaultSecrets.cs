using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.KeyVault;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Services.AppAuthentication;
using DataLayerModernSQL.Services;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Microsoft.Extensions.Hosting;

namespace WebAPIControllers.Extensions
{
    public static class KeyvaultSecrets
    {
        const string certissuer = "czazplayappregauth";
        const string sqlconnectionstringkeyname = "sqlconnectionstring";

        public static string SqlConnectionStringKeyName => sqlconnectionstringkeyname;

        // Extension method that configures KeyVault config provider using MSI Auth
        public static void ConfigureKeyvaultMSI(this IConfigurationBuilder config, string keyvaulturl, string aadappid)
        {
            if (string.IsNullOrEmpty(keyvaulturl) || string.IsNullOrEmpty(aadappid))
            {
                throw new ArgumentException("missing keyvault URI or aadappid");
            }

            config.AddAKVwithMSIAuth(keyvaulturl, aadappid);

            //TODO: if this worked we'd use it but there is a dependency failure
            //var azureServiceTokenProvider = new AzureServiceTokenProvider();
            //var keyVaultClient = new createkvclientformsi

            //config.AddAzureKeyVault(
            //    //TODO store kv name in config
            //    //$"https://{builtConfig["KeyVaultName"]}.vault.azure.net/",
            //    keyvaultbase,
            //    keyVaultClient,
            //    new DefaultKeyVaultSecretManager());
        }

        // Extension method that configures KeyVault config provider using App registration with 509 Cert
        public static void ConfigureKeyvaultAppRegistration(this IConfigurationBuilder config, string keyvaulturl, string secret, string aadappid)
        {
            if (string.IsNullOrEmpty(keyvaulturl) || string.IsNullOrEmpty(aadappid))
            {
                throw new ArgumentException("missing keyvault URI or aadappid");
            }

            config.AddAKVwithAppRegistrationSecretAuth(keyvaulturl, secret, aadappid);

            //TODO: if this worked we'd use it but there is a dependency failure
            //config.AddAzureKeyVault(
            //    //TODO store kv name in config
            //    //$"https://{builtConfig["KeyVaultName"]}.vault.azure.net/",
            //    keyvaultbase,
            //    appclientid,
            //    GetCertbyIssuer(certissuer)
            //    );
            //});
        }

        //https://stackoverflow.com/questions/52278205/how-to-read-appsetting-json-for-service-fabric-solution
        public static IWebHostBuilder UseCommonConfiguration(this IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;

                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                if (env.IsDevelopment())
                {
                    var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                    if (appAssembly != null)
                    {
                        config.AddUserSecrets(appAssembly, optional: true);
                    }
                }

                config.AddEnvironmentVariables();
            });

            return builder;
        }

        public static async Task<Tuple<string, string>> GetAKVSecretsUsingSecret(string appclientsecret, string appclientid, string keyvaultbase)
        {
            var client = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(async (string authority, string resource, string scope) =>
            {
                var authContext = new AuthenticationContext(authority);
                var credential = new ClientCredential(appclientid, appclientsecret);
                AuthenticationResult result = await authContext.AcquireTokenAsync(resource, credential);
                if (result == null)
                {
                    throw new InvalidOperationException("Failed to retrieve JWT token");
                }
                return result.AccessToken;
            }
            ));

            var sqlconnectionstring = await client.GetSecretAsync(keyvaultbase, sqlconnectionstringkeyname);
            return new Tuple<string, string>("", sqlconnectionstring.Value);
        }

        public static async Task<Tuple<string, string>> GetAKVSecretsUsingCert(string keyvaultbase)
        {
            var client = CreateKVClientForIssuer(certissuer, keyvaultbase);

            var sqlconnectionstring = await client.GetSecretAsync(keyvaultbase, sqlconnectionstringkeyname);
            return new Tuple<string, string>("", sqlconnectionstring.Value);
        }

        public static async Task<Tuple<string, string>> GetAKVSecretsUsingMSI(string keyvaultbase, ILogger<DataService> logger = null)
        {
            var client = CreateKVClientForMSI();
            if (logger != null)
            {
                logger.LogInformation("After create client");
            }

            var sqlconnectionstring = await client.GetSecretAsync(keyvaultbase, sqlconnectionstringkeyname);
            if (logger != null)
            {
                logger.LogInformation("After get connstring" + sqlconnectionstring.Value);
            }
            return new Tuple<string, string>("", sqlconnectionstring.Value);
        }

        private static KeyVaultClient CreateKVClientForIssuer(string certissuer, string appclientid)
        {
            var cert = GetCertbyIssuer(certissuer);

            if (cert == null)
            {
                throw new InvalidEnumArgumentException("Certificate not found");
            }

            var client = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(async (string authority, string resource, string scope) =>
            {
                var authContext = new AuthenticationContext(authority);
                var certassertion = new ClientAssertionCertificate(appclientid, cert);
                AuthenticationResult result = await authContext.AcquireTokenAsync(resource, certassertion);
                if (result == null)
                {
                    throw new InvalidOperationException("Failed to retrieve JWT token");
                }
                return result.AccessToken;
            }
            ));
            return client;
        }

        private static KeyVaultClient CreateKVClientForMSI()
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(
                    azureServiceTokenProvider.KeyVaultTokenCallback));
            return keyVaultClient;
        }

        private static X509Certificate2 GetCertbyIssuer(string FindValue)
        {
            using (var store = new X509Store(StoreLocation.CurrentUser))
            {
                store.Open(OpenFlags.ReadOnly);
                var certs = store.Certificates
                    .Find(X509FindType.FindByIssuerName,
                        FindValue, false);
                if (certs.Count > 0)
                {
                    return certs[0];
                }
            }
            return null;
        }
    }
}
