namespace MapCall.SAP.Model.Repositories
{
    public class ExtendedSAPHttpClientFactory : IExtendedSapHttpClientFactory
    {
        #region Private Members

        private readonly IExtendedSAPHttpClientConfiguration _configuration;

        #endregion

        #region Constructors

        public ExtendedSAPHttpClientFactory(IExtendedSAPHttpClientConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region Exposed Methods

        public ISAPHttpClient Build()
        {
            return new SAPHttpClient {
                UserName = _configuration.UserName,
                Password = _configuration.Password,
                BaseAddress = _configuration.BaseAddress
            };
        }

        #endregion
    }
}
