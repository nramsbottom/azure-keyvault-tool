using System.Collections.Generic;

namespace KeyVaultTool
{
    interface ISecretWriter
    {
        string WriteSecrets(Dictionary<string, string> secrets);
    }
}
