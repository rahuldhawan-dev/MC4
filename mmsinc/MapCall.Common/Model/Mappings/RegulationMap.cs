using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class RegulationMap : ClassMap<Regulation>
    {
        public RegulationMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("RegulationID");

            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.RegulationShort).Column("Regulation").Length(Regulation.StringLengths.REGULATION_SHORT);
            Map(x => x.Title).Column("RegulationTitle").Length(2147483647);
            Map(x => x.Statute).Length(255);
            Map(x => x.Citation).Length(255);
            Map(x => x.EffectiveDate);
            Map(x => x.Purpose).Length(2147483647);
            Map(x => x.GeneralDescription).Length(2147483647);
            Map(x => x.Requirements).Length(2147483647);
            Map(x => x.UtilitiesCovered).Length(2147483647);
            Map(x => x.CostImpact).Length(50);
            Map(x => x.Notes).Length(2147483647);
            Map(x => x.AllAreas).Not.Nullable();
            Map(x => x.FieldOperations).Not.Nullable();
            Map(x => x.Production).Not.Nullable();
            Map(x => x.Environmental).Not.Nullable();
            Map(x => x.WaterQuality).Not.Nullable();
            Map(x => x.Engineering).Not.Nullable();

            References(x => x.Agency).Not.Nullable();
            References(x => x.Status).Not.Nullable();

            HasMany(x => x.TrainingRequirements).KeyColumn("RegulationId");
            HasMany(x => x.RegulationDocuments).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.RegulationNotes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
