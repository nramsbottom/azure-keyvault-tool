using System.Collections.Generic;
using System.Text.Json;

namespace KeyVaultTool
{
    class JsonSecretWriter : ISecretWriter
    {
        public string WriteSecrets(Dictionary<string, string> secrets)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(secrets, new JsonSerializerOptions()
            {
                WriteIndented = true
            });
            return json;
        }
    }
}
