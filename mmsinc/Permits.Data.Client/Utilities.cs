using System;
using System.Configuration;

namespace Permits.Data.Client
{
    public class Utilities
    {
        #region Constants

        public const string BASE_ADDRESS_CONFIG_KEY = "PermitsBaseAddress";

        #endregion

        #region Properties

        public static Uri BaseAddress
        {
            get { return new Uri(ConfigurationManager.AppSettings[BASE_ADDRESS_CONFIG_KEY]); }
        }

        #endregion
    }
}
