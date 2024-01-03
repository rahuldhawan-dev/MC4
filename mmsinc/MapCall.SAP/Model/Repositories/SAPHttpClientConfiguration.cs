using System;
using System.Configuration;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;

namespace MapCall.SAP.Model.Repositories
{
    public class SAPHttpClientConfiguration : IExtendedSAPHttpClientConfiguration
    {
        public string UserName => ConfigurationManager.AppSettings.EnsureValue("SAPWebServiceUserName");
        public string Password => ConfigurationManager.AppSettings.EnsureValue("SAPWebServicePassword");
        public Uri BaseAddress => new Uri(ConfigurationManager.AppSettings.EnsureValue("SAPWebServiceUrl"));
    }
}
