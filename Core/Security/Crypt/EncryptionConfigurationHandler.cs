using System.Configuration;
using System.Xml;

namespace Core.Security.Crypt
{
    public class EncryptionConfigurationHandler : IConfigurationSectionHandler
    {


        #region IConfigurationSectionHandler Members

        public object Create(object parent, object configContext, XmlNode node)
        {
            EncryptionConfiguration config = new EncryptionConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }

        #endregion


    }
}
