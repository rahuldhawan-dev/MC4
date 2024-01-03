using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceMaterialMap : ClassMap<ServiceMaterial>
    {
        #region Constructors

        public ServiceMaterialMap()
        {
            Id(x => x.Id, "ServiceMaterialID");

            Map(x => x.Description);
            Map(x => x.Code).Length(ServiceMaterial.StringLengths.CODE).Nullable();
            Map(x => x.IsEditEnabled).Not.Nullable();

            References(x => x.CustomerEPACode).Nullable();
            References(x => x.CompanyEPACode).Nullable();

            HasMany(x => x.OperatingCentersServiceMaterials)
               .KeyColumn("ServiceMaterialId").Cascade.AllDeleteOrphan().Inverse();

            HasMany(x => x.ServiceMaterialEPACodeOverrides).KeyColumn("ServiceMaterialId");
        }

        #endregion
    }
}
