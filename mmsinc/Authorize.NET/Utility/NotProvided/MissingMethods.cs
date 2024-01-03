using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Xml;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;

namespace AuthorizeNet.Utility.NotProvided
{
    /// <summary>
    /// Utility class providing functionality not yet implemented in the Authorize.Net SDK.
    /// </summary>
    public class MissingMethods
    {
        #region Constants

        public struct AppSettingsKeys
        {
            public const string AUTHORIZE_NET_LOGIN_ID = "AuthorizeNetLoginId",
                                AUTHORIZE_NET_TX_KEY =
                                    "AuthorizeNetTransactionKey";
        }

        public struct ApiUrls
        {
            public const string TESTING =
                "https://apitest.authorize.net/xml/v1/request.api",
                PRODUCTION = "https://api.authorize.net/xml/v1/request.api";
        }

        #endregion

        #region GetHostedSessionKey (GetHostedProfilePage)

        /// <summary>
        /// from http://community.developer.authorize.net/t5/Integration-and-Testing/How-to-call-GetHostedProfilePage/td-p/14208
        /// TODO: continue to make this less ugly
        /// </summary>
        public static string GetHostedSessionKey(UInt32 customerProfileId, string url, bool isLive)
        {
            var loginId = ConfigurationManager.AppSettings
                .EnsureValue(AppSettingsKeys.AUTHORIZE_NET_LOGIN_ID);
            var transactionKey = ConfigurationManager.AppSettings
                .EnsureValue(AppSettingsKeys.AUTHORIZE_NET_TX_KEY);
            var serviceUrl = isLive ? ApiUrls.PRODUCTION : ApiUrls.TESTING;
            var requestXML =
                String.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                    "<getHostedProfilePageRequest xmlns=\"AnetApi/xml/v1/schema/AnetApiSchema.xsd\">" +
                        "<merchantAuthentication>" +
                            "<name>{0}</name>" +
                            "<transactionKey>{1}</transactionKey>" +
                        "</merchantAuthentication>" +
                        "<customerProfileId>{2}</customerProfileId>" +
                        "<hostedProfileSettings>" +
                            "<setting>" +
                                "<settingName>hostedProfileIFrameCommunicatorUrl</settingName>" +
                                "<settingValue>{3}</settingValue>" +
                            "</setting>" +
                        "</hostedProfileSettings>" +
                   "</getHostedProfilePageRequest>",
                    loginId,
                    transactionKey,
                    customerProfileId, url);

            var req = (HttpWebRequest)
                    WebRequest.Create(serviceUrl);
            req.CachePolicy = new RequestCachePolicy(
                RequestCacheLevel.NoCacheNoStore);
            req.KeepAlive = false;
            req.Timeout = 30000; //30 seconds
            req.Method = "POST";
            byte[] byte1 = null;
            var encoding = new ASCIIEncoding();
            byte1 = encoding.GetBytes(requestXML);
            req.ContentType = "text/xml";
            req.ContentLength = byte1.Length;
            var reqStream = req.GetRequestStream();
            reqStream.Write(byte1, 0, byte1.Length);
            reqStream.Close();

            var resp = req.GetResponse();
            var read = resp.GetResponseStream();
            var io = new StreamReader(read,
                new ASCIIEncoding());
            var data = io.ReadToEnd();
            resp.Close();

            //parse out value
            var doc = new XmlDocument();
            doc.LoadXml(data);
            var token = doc.GetElementsByTagName("token");

            // Are you seeing an error here? Probably a null reference exception?
            // If you look at the full body of the xml response, you'll likely see
            // an error code "E00040" with text "The record cannot be found.". This
            // means the payment profile is missing for the user. If you're dealing with
            // MapCall, you can wipe out the CustomerProfileId in tblPermissions for that
            // user. Afterwards, go to their user profile in MVC and hit the "Update Payment Information"
            // button to create a new profile. 
            //
            // If you're dealing with the Permits site, you need to login to the permits
            // site as the user in question and update their payment information there. 
            // This user's based on the OperatingCenter. If you're dealing with issues
            // related to the permits API in QA, the easiest thing to do is to create a new
            // user on the permits QA site, setting up the payment info, then updating the
            // PermitsOMUserName for the OperatingCenter being tested against with the new
            // username account. Make sure the new user on the permits site is part of the 
            // "New Jersey American Water" company, otherwise authentication will fail here.
            return token[0].InnerText;
        }

        #endregion
    }

    public interface IExtendedCustomerGateway : ICustomerGateway
    {
        #region Methods

        string GetHostedSessionKey(UInt32 customerProfileId, string url, bool isLive);

        #endregion
    }
}
