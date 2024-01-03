using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceServiceInstallationMaterialMap : ClassMap<ServiceServiceInstallationMaterial>
    {
        #region Constants

        public const string TABLE_NAME = "ServicesServiceInstallationMaterials";

        #endregion

        #region Constructors

        public ServiceServiceInstallationMaterialMap()
        {
            Table(TABLE_NAME);

            CompositeId()
               .KeyReference(x => x.ServiceInstallationMaterial, "ServiceInstallationMaterialId")
               .KeyReference(x => x.Service, "ServiceId");

            References(x => x.Service).Not.Nullable().ReadOnly();
            References(x => x.ServiceInstallationMaterial).Not.Nullable().ReadOnly();
        }

        #endregion
    }
}
