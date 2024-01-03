using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class AwiaComplianceMap : ClassMap<AwiaCompliance>
    {
        public AwiaComplianceMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            
            References(x => x.State).Not.Nullable();
            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.CertificationType).Not.Nullable();
            References(x => x.CreatedBy).Not.Nullable();
            References(x => x.CertifiedBy).Not.Nullable();

            Map(x => x.DateAccepted).Not.Nullable();
            Map(x => x.DateSubmitted).Not.Nullable();
            Map(x => x.RecertificationDue).Not.Nullable();

            HasManyToMany(x => x.PublicWaterSupplies)
               .Table("AwiaCompliancePublicWaterSupplies")
               .ParentKeyColumn("AwiaComplianceId")
               .ChildKeyColumn("PublicWaterSupplyId")
               .Cascade
               .None();
            HasMany(x => x.AwiaComplianceNotes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.AwiaComplianceDocuments).KeyColumn("LinkedId").Inverse().Cascade.None();
        }
    }
}
