using MapCallScheduler.Library.JobHelpers.Sap;

namespace MapCallScheduler.JobHelpers.SapChemical
{
    public class SapChemicalServiceConfiguration : SapServiceConfigurationBase, ISapChemicalServiceConfiguration
    {
        #region Constants

        public const string GROUP_NAME = "sapChemicalService";

        #endregion

        #region Properties

        public override string GroupName => GROUP_NAME;

        #endregion
    }
}
