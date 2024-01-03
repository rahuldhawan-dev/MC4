using System.Configuration;
using System.Xml;
using MMSINC.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.Utilities
{
    [TestClass]
    public class RSAEncryptionHelperTest
    {
        private TestRSAEncryptionHelper _target;

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new TestRSAEncryptionHelper();
        }

        [TestMethod]
        public void TestPrivateAndPublicKeysAreReadFromConfigFile()
        {
            Assert.AreEqual(
                XmlConvert.DecodeName(ConfigurationManager.AppSettings[RSAEncryptionHelper.ConfigKeys.PUBLIC_KEY]),
                _target.GetPublicKey());

            Assert.AreEqual(
                XmlConvert.DecodeName(ConfigurationManager.AppSettings[RSAEncryptionHelper.ConfigKeys.PRIVATE_KEY]),
                _target.GetPrivateKey());
        }

        [TestMethod]
        public void TestTokenVerifies()
        {
            //var rsa = new RSACryptoServiceProvider();
            //var privateParameters = rsa.ExportParameters(true);
            //var publicParameters = rsa.ExportParameters(false);

            //var x = rsa.ToXmlString(true);
            //var y = rsa.ToXmlString(false);
            // Measuring & Monitoring Services Inc. -- "132,40,142,22,40,212,165,4,114,145,37,213,125,175,108,247,134,162,2,144,219,196,74,184,208,111,44,207,119,168,178,73,14,29,209,44,183,28,200,24,251,116,106,221,231,180,15,28,99,167,5,207,25,18,175,78,105,130,84,171,202,6,187,0,46,6,177,118,117,62,26,113,9,173,123,105,202,165,169,127,2,115,158,116,43,121,121,168,161,48,75,30,2,158,4,12,149,191,199,107,201,47,62,162,41,140,86,64,43,170,157,202,203,5,165,127,218,137,89,114,121,31,254,213,0,185,7,1"
            // New Jersey American Water NJ7 -- "68,40,96,230,124,92,29,48,49,101,46,125,47,112,251,109,43,179,21,101,252,78,103,254,20,15,38,12,40,4,187,40,76,41,52,123,119,48,205,226,212,62,180,51,199,146,126,227,72,40,136,13,74,217,29,131,73,128,208,104,167,237,176,169,172,157,236,55,145,153,245,27,245,81,41,249,147,8,40,5,238,97,1,204,234,50,43,27,84,201,170,66,22,190,65,205,234,141,231,116,78,80,164,224,237,38,165,9,151,216,175,5,65,30,104,225,77,103,63,45,201,235,43,144,30,64,245,137"
            var token = "Measuring & Monitoring Services Inc.";

            var encryptedToken = _target.Encrypt(token);
            var decryptedToken = _target.Decrypt(encryptedToken);

            Assert.AreEqual(token, decryptedToken);
        }
    }

    public class TestRSAEncryptionHelper : RSAEncryptionHelper
    {
        public string GetPublicKey()
        {
            return PublicKey;
        }

        public string GetPrivateKey()
        {
            return PrivateKey;
        }
    }
}
