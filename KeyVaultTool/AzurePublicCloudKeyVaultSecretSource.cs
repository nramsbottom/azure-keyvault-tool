using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;

namespace KeyVaultTool
{
    class AzurePublicCloudKeyVaultSecretSource : ISecretSource
    {
        public Dictionary<string, string> GetAllSecrets(string keyVaultName)
        {
            var vaultUrl = new Uri($"https://{keyVaultName}.vault.azure.net/");
            return GetAllSecrets(vaultUrl);
        }

        static Dictionary<string, string> GetAllSecrets(Uri vaultUrl)
        {
            if (vaultUrl is null)
            {
                throw new ArgumentNullException(nameof(vaultUrl));
            }

            var secrets = new Dictionary<string, string>();
            var client = new SecretClient(vaultUri: vaultUrl, credential: new DefaultAzureCredential());
            var allSecrets = client.GetPropertiesOfSecrets();

            foreach (SecretProperties property in allSecrets)
            {
                KeyVaultSecret secret = client.GetSecret(property.Name);
                secrets[secret.Name] = secret.Value;
            }

            return secrets;
        }

    }
}
