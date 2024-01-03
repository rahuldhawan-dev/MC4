using MapCallScheduler.Library.JobHelpers.Sap;

namespace MapCallScheduler.JobHelpers.SapMaterial
{
    public class SapMaterialServiceConfiguration : SapServiceConfigurationBase, ISapMaterialServiceConfiguration
    {
        #region Constants

        public const string GROUP_NAME = "sapMaterialService";

        #endregion

        #region Properties

        public override string GroupName => GROUP_NAME;

        #endregion
    }
}