using MMSINC.Utilities.APIM;

namespace MapCall.LIMS.Configuration
{
    public class LIMSClientConfiguration : APIMClientConfiguration, ILIMSClientConfiguration
    {
        #region Constructors

        public LIMSClientConfiguration() : base(LIMSClientConfigurationSection.SECTION_NAME) { }

        #endregion  
    }
}
