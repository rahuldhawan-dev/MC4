using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ApcInspectionItemMap : ClassMap<ApcInspectionItem>
    {
        public ApcInspectionItemMap()
        {
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.Facility).Not.Nullable();
            References(x => x.Type).Not.Nullable();
            References(x => x.AssignedTo).Not.Nullable();
            References(x => x.FacilityInspectionRatingType).Nullable();

            Map(x => x.Area).Not.Nullable().Length(50);
            Map(x => x.Description).Not.Nullable().Length(50);
            Map(x => x.DateReported).Not.Nullable();
            Map(x => x.DateRectified).Nullable();
            Map(x => x.DateInspected).Nullable();
            Map(x => x.Score).Nullable();
            Map(x => x.Percentage).Nullable();

            HasMany(x => x.ItemDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.ItemNotes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.FacilityInspectionFormAnswers).KeyColumn("ApcInspectionItemId").Inverse().Cascade.AllDeleteOrphan();
            HasManyToMany(x => x.FacilityInspectionAreaTypes)
               .Table("APCInspectionItemsFacilityInspectionAreaTypes")
               .ParentKeyColumn("APCInspectionItemId")
               .ChildKeyColumn("FacilityInspectionAreaTypeId")
               .LazyLoad()
               .Cascade.None();
        }
    }
}
