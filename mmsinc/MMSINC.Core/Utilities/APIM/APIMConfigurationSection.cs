using System.Configuration;

namespace MMSINC.Utilities.APIM
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// The representation of configuration sections used to define communication with various services in APIM.
    /// </summary>
    /// <seealso cref="System.Configuration.ConfigurationSection" />
    public abstract class APIMConfigurationSection : ConfigurationSection
    {
        #region Constants

        /// <summary>
        /// The port which is used when a configuration setting for port is invalid or not given.
        /// </summary>
        /// <remarks>
        /// A UriBuilder created with a port of -1 will use the default port for the provided scheme.
        /// </remarks>
        public const int DEFAULT_PORT = -1;

        public readonly struct Keys
        {
            public const string SCHEME = "scheme",
                                HOST = "host",
                                PATH = "path",
                                PORT = "port",
                                API_KEY = "apiKey",
                                TIMEOUT_IN_MINUTES = "timeoutInMinutes";
        }

        #endregion

        #region Constructors

        protected APIMConfigurationSection(string sectionName)
        {
            SectionName = sectionName;
        }

        #endregion

        #region Properties

        public string SectionName { get; }

        [ConfigurationProperty(Keys.SCHEME, IsRequired = true)]
        public string Scheme => this[Keys.SCHEME].ToString();

        [ConfigurationProperty(Keys.HOST, IsRequired = true)]
        public string Host => this[Keys.HOST].ToString();

        [ConfigurationProperty(Keys.PORT, DefaultValue = DEFAULT_PORT)]
        public int Port => (int)this[Keys.PORT];

        [ConfigurationProperty(Keys.PATH, IsRequired = true)]
        public string Path => this[Keys.PATH].ToString();

        [ConfigurationProperty(Keys.API_KEY, IsRequired = true)]
        public string ApiKey => this[Keys.API_KEY].ToString();

        [ConfigurationProperty(Keys.TIMEOUT_IN_MINUTES, IsRequired = true)]
        public int TimeoutInMinutes => (int)this[Keys.TIMEOUT_IN_MINUTES];

        #endregion
    }
}
