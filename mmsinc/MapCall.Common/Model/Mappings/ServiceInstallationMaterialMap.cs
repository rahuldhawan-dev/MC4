using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceInstallationMaterialMap : ClassMap<ServiceInstallationMaterial>
    {
        #region Constructors

        public ServiceInstallationMaterialMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Column("Id").Not.Nullable();

            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.ServiceCategory).Not.Nullable();
            References(x => x.ServiceSize).Not.Nullable();

            Map(x => x.SortOrder);
            Map(x => x.Description);
            Map(x => x.PartQuantity);
            Map(x => x.PartSize);
        }

        #endregion
    }
}
