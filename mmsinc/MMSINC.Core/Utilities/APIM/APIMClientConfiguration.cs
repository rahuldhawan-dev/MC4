using System;
using System.Configuration;

namespace MMSINC.Utilities.APIM
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// A wrapper for a hydrated set of configuration settings for the given configuration section name.
    /// </summary>
    /// <seealso cref="MMSINC.Utilities.APIM.IAPIMClientConfiguration" />
    public class APIMClientConfiguration : IAPIMClientConfiguration
    {
        #region Private Members

        private Uri _apiUri;
        private APIMConfigurationSection _section;
        private readonly string _configurationSectionName;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="APIMClientConfiguration"/> class.
        /// </summary>
        /// <param name="configurationSectionName">The name of the configuration section that this class should hydrate it's properties from.</param>
        /// <exception cref="System.ArgumentException">Must not be empty - configurationSectionName</exception>
        protected APIMClientConfiguration(string configurationSectionName)
        {
            if (string.IsNullOrEmpty(configurationSectionName))
            {
                throw new ArgumentException("Must not be empty", nameof(configurationSectionName));
            }

            _configurationSectionName = configurationSectionName;
        }

        #endregion

        #region Properties

        public APIMConfigurationSection Section => _section ?? (_section = (APIMConfigurationSection)ConfigurationManager.GetSection(_configurationSectionName));

        public string Scheme => Section.Scheme;

        public string Host => Section.Host;

        public int Port => Section.Port;

        public string Path => Section.Path;

        public string ApiKey => Section.ApiKey;

        public int TimeoutInMinutes => Section.TimeoutInMinutes;

        public Uri ApiUri
        {
            get
            {
                if (_apiUri == null)
                {
                    var uriBuilder = new UriBuilder(Host) {
                        Scheme = Scheme,
                        Path = Path,
                        Port = Port
                    };

                    _apiUri = uriBuilder.Uri;
                }

                return _apiUri;
            }
        }

        #endregion
    }
}