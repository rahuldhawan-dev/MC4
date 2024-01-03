using MMSINC.Utilities.APIM;

namespace MapCall.LIMS.Configuration
{
    public class LIMSClientConfigurationSection : APIMConfigurationSection
    {
        #region Constants

        public const string SECTION_NAME = "lims";

        #endregion

        #region Constructors

        public LIMSClientConfigurationSection() : base(SECTION_NAME) { }

        #endregion  
    }
}
