using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Core.Security.Crypt
{
    public class CryptoHelper
    {

        private RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();


        public CryptoHelper()
        {
            EncryptionConfiguration config = EncryptionConfiguration.GetConfig();
            rsaProvider.FromXmlString(config.RsaKey);

        }

        public String RsaPublicKey
        {
            get { return rsaProvider.ToXmlString(false); }
        }

        public String RsaPrivateKey
        {
            get { return rsaProvider.ToXmlString(true); }
        }

        public string Encrypt(string clearText)
        {

            byte[] encryptedStr;
            encryptedStr = rsaProvider.Encrypt(Encoding.ASCII.GetBytes(clearText), false);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i <= encryptedStr.Length - 1; i++)
            {
                if (i != encryptedStr.Length - 1)
                {
                    stringBuilder.Append(encryptedStr[i] + "~");
                }
                else
                {
                    stringBuilder.Append(encryptedStr[i]);
                }
            }
            return stringBuilder.ToString();

        }

        public string Decrypt(string encryptedText)
        {

            byte[] decryptedStr = rsaProvider.Decrypt(StringToByteArray(encryptedText.Trim()), false);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i <= decryptedStr.Length - 1; i++)
            {
                stringBuilder.Append(Convert.ToChar(decryptedStr[i]));
            }
            return stringBuilder.ToString();

        }

        public byte[] StringToByteArray(string inputText)
        {
            string[] s;
            s = inputText.Trim().Split('~');
            byte[] b = new byte[s.Length];

            for (int i = 0; i <= s.Length - 1; i++)
            {
                b[i] = Convert.ToByte(s[i]);
            }
            return b;
        }


        public static string Hash(string cleanText)
        {
            if (cleanText != null)
            {
                Byte[] clearBytes = new UnicodeEncoding().GetBytes(cleanText);

                Byte[] hashedBytes
                    = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);

                return BitConverter.ToString(hashedBytes);
            }
            else
            {
                return String.Empty;
            }

        }

        //

        // TODO: move to config, should not be hard coded
        private static byte[] key_192 = new byte[] 
			{10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10,
				10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10};

        private static byte[] iv_128 = new byte[]
			{10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10,
				10, 10, 10, 10};

        public static string EncryptRijndaelManaged(string value)
        {
            if (value == string.Empty) return string.Empty;

            RijndaelManaged crypto = new RijndaelManaged();
            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(
                memoryStream,
                crypto.CreateEncryptor(key_192, iv_128),
                CryptoStreamMode.Write);

            StreamWriter streamWriter = new StreamWriter(cryptoStream);

            streamWriter.Write(value);
            streamWriter.Flush();
            cryptoStream.FlushFinalBlock();
            memoryStream.Flush();

            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }

        public static string DecryptRijndaelManaged(string value)
        {
            if (value == string.Empty) return string.Empty;

            RijndaelManaged crypto = new RijndaelManaged();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(value));

            CryptoStream cryptoStream = new CryptoStream(
                memoryStream,
                crypto.CreateDecryptor(key_192, iv_128),
                CryptoStreamMode.Read);

            StreamReader streamReader = new StreamReader(cryptoStream);

            return streamReader.ReadToEnd();
        }
        
        public bool DecryptAndVerifyData(string input, out string[] values)
        {
            string xml = DecryptRijndaelManaged(input);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            values = null;

            XmlNode node = xmlDoc.GetElementsByTagName("s")[0];
            node.ParentNode.RemoveChild(node);

            byte[] signature = Convert.FromBase64String(node.InnerText);

            byte[] data = Encoding.ASCII.GetBytes(xmlDoc.InnerXml);
            if (!rsaProvider.VerifyData(data, "SHA1", signature))
                return false;

            int count;
            for (count = 0; count < 100; count++)
            {
                if (xmlDoc.GetElementsByTagName("v" + count.ToString())[0] == null)
                    break;
            }

            values = new string[count];

            for (int i = 0; i < count; i++)
                values[i] = xmlDoc.GetElementsByTagName("v" + i.ToString())[0].InnerText;

            return true;
        }
    }
}
