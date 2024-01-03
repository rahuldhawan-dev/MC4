using MapCallScheduler.Library.JobHelpers.Sap;

namespace MapCallScheduler.JobHelpers.LeakAlert
{
    public class LeakAlertServiceConfiguration : SapServiceConfigurationBase, ILeakAlertServiceConfiguration
    {
        #region Constants

        public const string GROUP_NAME = "leakAlertService";

        #endregion

        #region Properties

        public override string GroupName => GROUP_NAME;

        #endregion
    }
}
