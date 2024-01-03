using MapCallScheduler.Library.JobHelpers.Sap;

namespace MapCallScheduler.JobHelpers.SapEmployee
{
    public class SapEmployeeServiceConfiguration : SapServiceConfigurationBase, ISapEmployeeServiceConfiguration
    {
        #region Constants

        public const string GROUP_NAME = "sapEmployeeService";

        #endregion

        #region Properties

        public override string GroupName => GROUP_NAME;

        #endregion
    }
}