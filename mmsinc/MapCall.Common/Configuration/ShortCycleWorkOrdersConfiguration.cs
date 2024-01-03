using System;
using System.Configuration;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;
using MMSINC.Utilities.ActiveMQ;

namespace MapCall.Common.Configuration
{
    public class ShortCycleWorkOrdersConfiguration : IActiveMQConfiguration
    {
        #region Properties

        public string Url => $"{Scheme}://{Host}:{Port}";
        public string Scheme => GetConfigValue("work_orders_amq_scheme");
        public string Host => GetConfigValue("work_orders_amq_host");
        public int Port => Int32.Parse(GetConfigValue("work_orders_amq_port"));
        public string User => GetConfigValue("work_orders_amq_user");
        public string Password => GetConfigValue("work_orders_amq_password");

        #endregion

        #region Private Methods

        private string GetConfigValue(string property)
        {
            return ConfigurationManager.AppSettings.EnsureValue(property);
        }

        #endregion
    }
}
