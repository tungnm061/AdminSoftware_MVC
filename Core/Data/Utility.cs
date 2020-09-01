using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Core.Data
{
    public class Utility
    {
        #region Convert datatype
        /// <summary>
        /// Get the native type based on the database type
        /// </summary>
        /// <param name="dbType">The database type to convert</param>
        /// <returns>The equivalent managed type, otherwise the DBNull type</returns>
        public static Type ConvertType(SqlDbType dbType)
        {
            Type toReturn = typeof(DBNull);

            switch (dbType)
            {
                case SqlDbType.NVarChar:
                    toReturn = typeof(string);
                    break;

                case SqlDbType.NText:
                    toReturn = typeof(string);
                    break;

                case SqlDbType.NChar:
                    toReturn = typeof(string);
                    break;

                case SqlDbType.Date:
                    toReturn = typeof(DateTime);
                    break;

                case SqlDbType.DateTime:
                    toReturn = typeof(DateTime);
                    break;
                case SqlDbType.DateTime2:
                    toReturn = typeof(DateTime);
                    break;
                case SqlDbType.Time:
                    toReturn = typeof(DateTime);
                    break;
                    
                case SqlDbType.BigInt:
                    toReturn = typeof(long);
                    break;
                    
                case SqlDbType.SmallInt:
                    toReturn = typeof(byte);
                    break;

                case SqlDbType.Int:
                    toReturn = typeof(int);
                    break;

                case SqlDbType.Char:
                    toReturn = typeof(string);
                    break;

                case SqlDbType.VarChar:
                    toReturn = typeof(string);
                    break;
                case SqlDbType.Money:
                    toReturn = typeof(Decimal);
                    break;

                case SqlDbType.Binary:
                    toReturn = typeof(byte[]);
                    break;

                case SqlDbType.Decimal:
                    toReturn = typeof(decimal);
                    break;

                case SqlDbType.Float:
                    toReturn = typeof(Double);
                    break;
                case SqlDbType.UniqueIdentifier:
                    toReturn = typeof(Guid);
                    break;

                case SqlDbType.Bit:
                    toReturn = typeof(bool);
                    break;
                default:
                    toReturn = typeof(string);
                    break;
            }

            return toReturn;
        }

        /// <summary>
        /// Get Value from Dynamic value
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dt"></param>
        /// <param name="dtRow"></param>
        /// <param name="field"></param>
        public static void GetValueByType(DataColumn column, dynamic dt, ref DataRow dtRow, string field)
        {
            if (dt != null)
            {
                var type = column.DataType;
                var typeName = type.Name;
                if (typeName.Equals(typeof (string).Name))
                {
                    dtRow[field] = Convert.ToString(dt);
                    var valString = dtRow[field].ToString();
                    if (valString.Length > column.MaxLength)
                    {
                        dtRow[field] = valString.Substring(0, column.MaxLength - 1);
                    }
                }
                else if (typeName.Equals(typeof (int).Name))
                {
                    dtRow[field] = Convert.ToInt32(Convert.ToString(dt));
                }
                else if (typeName.Equals(typeof (DateTime).Name))
                {
                    dtRow[field] = Convert.ToDateTime(Convert.ToString(dt));
                }
                else if (typeName.Equals(typeof (long).Name))
                {
                    dtRow[field] = Convert.ToInt64(Convert.ToString(dt));
                }
                else if (typeName.Equals(typeof (byte).Name))
                {
                    dtRow[field] = Convert.ToByte(Convert.ToString(dt));
                }
                else if (typeName.Equals(typeof (decimal).Name))
                {
                    dtRow[field] = Convert.ToDecimal(Convert.ToString(dt));
                }
                else if (typeName.Equals(typeof (Double).Name))
                {
                    dtRow[field] = Convert.ToDouble(Convert.ToString(dt));
                }
                else if (typeName.Equals(typeof (Guid).Name))
                {
                    dtRow[field] = Convert.ToString(Convert.ToString(dt));
                }
                else if (typeName.Equals(typeof (bool).Name))
                {
                    dtRow[field] = Convert.ToBoolean(Convert.ToString(dt));
                }
            }

        }
        #endregion


        #region GenerateRandomCode
        /// <summary>
        /// Randoms the code by lenght.
        /// </summary>
        /// <param name="lenght">The lenght.</param>
        /// <returns>String</returns>
        public static string RandomCodeByLenght(int lenght)
        {
            Random random = new Random();
            const string _chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            char[] buffer = new char[lenght];
            for (int i = 0; i < lenght; i++)
            {
                buffer[i] = _chars[random.Next(_chars.Length)];
            }
            return new string(buffer);
        }
        #endregion

        #region CookiesControl
        /// <summary>
        /// Saves the cookie.
        /// </summary>
        /// <param name="_cookieName">Name of the cookie.</param>
        /// <param name="_value">The value.</param>
        /// <param name="_expires">The expires.</param>
        public static void SaveCookie(string _cookieName, string _value, int _expires)
        {
            var _cookies = new HttpCookie(_cookieName) { Value = EncryptASE(_value), Expires = DateTime.Now.AddDays(_expires) };
            _cookies.HttpOnly = true;// cookie not available in js
            HttpContext.Current.Response.Cookies.Add(_cookies);//Save userID to cookie when remember checked.
        }

        /// <summary>
        /// Clears the cookie.
        /// </summary>
        /// <param name="_cookieName">Name of the cookie.</param>
        public static void ClearCookie(string _cookieName) 
        {
            var _cookies = new HttpCookie(_cookieName) { Expires = DateTime.Now.AddDays(-365) };
            HttpContext.Current.Response.Cookies.Add(_cookies);
        }
        #endregion

        #region ASE256 Encrypt
        private static string AesIV = ConfigurationManager.AppSettings["AesIV"];
        private static string AesKey = ConfigurationManager.AppSettings["AesKey"];

        /// <summary>
        /// Encrypts the ASE.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>string</returns>
        public static string EncryptASE(string text)
        {
            try
            {
                RijndaelManaged aes = new RijndaelManaged();
                aes.BlockSize = 128;
                aes.KeySize = 256;
                aes.IV = Convert.FromBase64String(AesIV);
                aes.Key = Convert.FromBase64String(AesKey);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Convert string to byte array
                byte[] src = Encoding.Unicode.GetBytes(text);

                // encryption
                using (ICryptoTransform encrypt = aes.CreateEncryptor())
                {
                    byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);

                    // Convert byte array to Base64 strings
                    return Convert.ToBase64String(dest);
                }
            }
            catch (Exception ex)
            {
                return text;
            }
        }

        /// <summary>
        /// Decrypts the ASE.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string DecryptASE(string text)
        {
            try
            {
                RijndaelManaged aes = new RijndaelManaged();
                aes.BlockSize = 128;
                aes.KeySize = 256;
                aes.IV = Convert.FromBase64String(AesIV);
                aes.Key = Convert.FromBase64String(AesKey);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Convert Base64 strings to byte array
                byte[] src = System.Convert.FromBase64String(text);


                // decryption
                using (ICryptoTransform decrypt = aes.CreateDecryptor())
                {
                    byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
                    return Encoding.Unicode.GetString(dest);
                }
            }
            catch
            {
                return text;

            }
        }

        public static Stream EncryptFile(Stream inputStream)
        {
            var algorithm = new RijndaelManaged { KeySize = 256, BlockSize = 128 };
            var key = new Rfc2898DeriveBytes(AesIV, Encoding.ASCII.GetBytes(AesKey));

            algorithm.Key = key.GetBytes(algorithm.KeySize / 8);
            algorithm.IV = key.GetBytes(algorithm.BlockSize / 8);

            try
            {
                return new CryptoStream(inputStream, algorithm.CreateEncryptor(), CryptoStreamMode.Read);
            }
            catch
            {
                return inputStream;
            }
        }

        public static Stream DecryptFile(Stream inputStream)
        {
            var algorithm = new RijndaelManaged { KeySize = 256, BlockSize = 128 };
            var key = new Rfc2898DeriveBytes(AesIV, Encoding.ASCII.GetBytes(AesKey));

            algorithm.Key = key.GetBytes(algorithm.KeySize / 8);
            algorithm.IV = key.GetBytes(algorithm.BlockSize / 8);

            try
            {
                return new CryptoStream(inputStream, algorithm.CreateDecryptor(), CryptoStreamMode.Read);
            }
            catch
            {
                return inputStream;
            }
        }
        #endregion
    }
}
