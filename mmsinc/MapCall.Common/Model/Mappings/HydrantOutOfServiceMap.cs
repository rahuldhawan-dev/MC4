using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class HydrantOutOfServiceMap : ClassMap<HydrantOutOfService>
    {
        public HydrantOutOfServiceMap()
        {
            Table("HydrantsOutOfService");
            Id(x => x.Id);
            Map(x => x.BackInServiceDate).Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.FireDepartmentContact, "FDContact")
               .Nullable()
               .Length(HydrantOutOfService.StringLengths.FIRE_CONTACT);
            Map(x => x.FireDepartmentFax, "FDFax")
               .Nullable()
               .Length(HydrantOutOfService.StringLengths.FIRE_FAX);
            Map(x => x.FireDepartmentPhone, "FDPhone")
               .Nullable()
               .Length(HydrantOutOfService.StringLengths.FIRE_PHONE);
            Map(x => x.OutOfServiceDate)
               .Not.Nullable();

            References(x => x.BackInServiceByUser)
               .Nullable();
            References(x => x.Hydrant)
               .Not.Nullable();
            References(x => x.OutOfServiceByUser)
               .Not.Nullable();
        }
    }
}
