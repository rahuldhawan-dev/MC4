using System;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace MMSINC.Utilities
{
    public class RSAEncryptionHelper : IEncryptionHelper
    {
        #region Constants

        public const int CIPHER_BITWIDTH = 2048;

        public struct ConfigKeys
        {
            public const string PUBLIC_KEY = "RsaPublicKey", PRIVATE_KEY = "RsaPrivateKey";
        }

        #endregion

        #region Private Members

        private UnicodeEncoding _encoder;

        #endregion

        #region Properties

        protected UnicodeEncoding Encoder
        {
            get { return _encoder ?? (_encoder = new UnicodeEncoding()); }
        }

        protected string PublicKey
        {
            get { return XmlConvert.DecodeName(ConfigurationManager.AppSettings[ConfigKeys.PUBLIC_KEY]); }
        }

        protected string PrivateKey
        {
            get { return XmlConvert.DecodeName(ConfigurationManager.AppSettings[ConfigKeys.PRIVATE_KEY]); }
        }

        #endregion

        #region Exposed Methods

        public string Decrypt(string data)
        {
            var rsa = new RSACryptoServiceProvider(CIPHER_BITWIDTH);
            var dataArray = data.Split(new[] {','});
            var dataByte = new byte[dataArray.Length];
            for (var i = 0; i < dataArray.Length; i++)
                dataByte[i] = Convert.ToByte(dataArray[i]);
            rsa.FromXmlString(PrivateKey);
            var decryptedByte = rsa.Decrypt(dataByte, true);
            return Encoder.GetString(decryptedByte);
        }

        public string Encrypt(string data)
        {
            var rsa = new RSACryptoServiceProvider(CIPHER_BITWIDTH);
            rsa.FromXmlString(PublicKey);
            var dataToEncrypt = Encoder.GetBytes(data);
            var encryptedByteArray = rsa.Encrypt(dataToEncrypt, true).ToArray();
            var length = encryptedByteArray.Count();
            var item = 0;
            var sb = new StringBuilder();
            foreach (var x in encryptedByteArray)
            {
                item++;
                sb.Append(x);

                if (item < length)
                    sb.Append(",");
            }

            return sb.ToString();
        }

        #endregion
    }

    public interface IEncryptionHelper
    {
        string Encrypt(string data);
        string Decrypt(string data);
    }
}
