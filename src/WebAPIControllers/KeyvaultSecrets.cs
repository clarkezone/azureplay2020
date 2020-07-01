using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WebAPIControllers.Extensions
{
    public static class KeyvaultSecrets
    {
        const string appclientid = "b053899e-e0a3-4794-9400-864df5a1faf0";
        const string keyvaultbase = "https://clarkezoneplayvault.vault.azure.net/";
        const string appinsightskeyname = "APPINSIGHTSINSTRUMENTATIONKEY";
        const string certissuer = "czazplayappregauth";
        const string sqlconnectionstringkeyname = "sqlconnectionstring";

        public static string AppInsightsKeyName => appinsightskeyname;

        public static string SqlConnectionStringKeyName => sqlconnectionstringkeyname;

        // Extension method that configures KeyVault config provider using MSI Auth
        public static IWebHostBuilder ConfigureKeyvaultMSI(this IWebHostBuilder builder)
        {
            return builder.ConfigureAppConfiguration((context, config) =>
            {
                var builtConfig = config.Build();

                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var keyVaultClient = new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(
                        azureServiceTokenProvider.KeyVaultTokenCallback));

                config.AddAzureKeyVault(
                    //TODO store kv name in config
                    //$"https://{builtConfig["KeyVaultName"]}.vault.azure.net/",
                    keyvaultbase,
                    keyVaultClient,
                    new DefaultKeyVaultSecretManager());
            });
        }

        // Extension method that configures KeyVault config provider using App registration with 509 Cert
        public static IWebHostBuilder ConfigureKeyvaultAppRegistration(this IWebHostBuilder builder)
        {
            return builder.ConfigureAppConfiguration((context, config) =>
            {
                var builtConfig = config.Build();

                config.AddAzureKeyVault(
                    //TODO store kv name in config
                    //$"https://{builtConfig["KeyVaultName"]}.vault.azure.net/",
                    keyvaultbase,
                    appclientid,
                    GetCertbyIssuer(certissuer)
                    );
            });
        }

        public static async Task<Tuple<string, string>> GetAKVSecretsUsingSecret(string appclientsecret)
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

            var appinsightskey = await client.GetSecretAsync(keyvaultbase, appinsightskeyname);
            var sqlconnectionstring = await client.GetSecretAsync(keyvaultbase, sqlconnectionstringkeyname);
            return new Tuple<string, string>(appinsightskey.Value, sqlconnectionstring.Value);
        }

        public static async Task<Tuple<string, string>> GetAKVSecretsUsingCert()
        {
            KeyVaultClient client = CreateKVClientForIssuer(certissuer);

            var appinsightskey = await client.GetSecretAsync(keyvaultbase, appinsightskeyname);
            var sqlconnectionstring = await client.GetSecretAsync(keyvaultbase, sqlconnectionstringkeyname);
            return new Tuple<string, string>(appinsightskey.Value, sqlconnectionstring.Value);
        }

        private static KeyVaultClient CreateKVClientForIssuer(string certissuer)
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
