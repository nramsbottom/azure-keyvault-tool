using System.Collections.Generic;

namespace KeyVaultTool
{
    public interface ISecretSource
    {
        Dictionary<string, string> GetAllSecrets(string keyVaultName);
    }
}
