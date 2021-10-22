using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace KeyVaultTool
{

    class UserSecretsXmlSecretWriter : ISecretWriter
    {
        public string WriteSecrets(Dictionary<string, string> secrets)
        {
            var sb = new StringBuilder();
            var writer = new StringWriter(sb);
            var xml = new XmlTextWriter(writer);

            xml.WriteStartDocument(true);
            xml.WriteStartElement("root");
            xml.WriteStartElement("secrets");
            xml.WriteAttributeString("ver", "1.0");
            foreach (var secret in secrets)
            {
                xml.WriteStartElement("secret");
                xml.WriteAttributeString("name", secret.Key);
                xml.WriteAttributeString("value", secret.Value);
                xml.WriteEndElement();
            }
            xml.WriteEndElement();
            xml.WriteEndElement();

            return sb.ToString();
        }
    }
}
