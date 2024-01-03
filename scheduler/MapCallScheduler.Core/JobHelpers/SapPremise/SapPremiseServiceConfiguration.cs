using MapCallScheduler.Library.JobHelpers.Sap;

namespace MapCallScheduler.JobHelpers.SapPremise
{
    public class SapPremiseServiceConfiguration : SapServiceConfigurationBase, ISapPremiseServiceConfiguration
    {
        #region Constants

        public const string GROUP_NAME = "sapPremiseService";

        #endregion

        #region Properties

        public override string GroupName => GROUP_NAME;

        #endregion
    }
}