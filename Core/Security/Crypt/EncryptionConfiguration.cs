using System;
using System.Configuration;
using System.Xml;

namespace Core.Security.Crypt
{
    public class EncryptionConfiguration
    {

        private XmlNode rsaNode;

        public void LoadValuesFromConfigurationXml(XmlNode node)
        {

            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "RSAKeyValue")
                    rsaNode = child;
            }
        }

        public String RsaKey
        {
            get
            {
                if (rsaNode != null)
                    return rsaNode.OuterXml;

                return String.Empty;

            }

        }

        public static EncryptionConfiguration GetConfig()
        {
            return (EncryptionConfiguration)ConfigurationManager.GetSection("system.web/Encryption");
        }


    }
}
