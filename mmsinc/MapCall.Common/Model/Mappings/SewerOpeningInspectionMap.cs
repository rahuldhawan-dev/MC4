using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SewerOpeningInspectionMap : ClassMap<SewerOpeningInspection>
    {
        public SewerOpeningInspectionMap()
        {
            LazyLoad();

            Id(x => x.Id);

            References(x => x.SewerOpening).Not.Nullable();
            References(x => x.InspectedBy).Nullable();

            Map(x => x.DateInspected).Not.Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.RimToWaterLevelDepth).Not.Nullable();
            Map(x => x.RimHeightAboveBelowGrade).Not.Nullable();
            Map(x => x.PipesIn).Nullable();
            Map(x => x.PipesOut).Nullable();
            Map(x => x.AmountOfDebrisGritCubicFeet).Nullable();
            Map(x => x.Remarks).Nullable();

            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
        }
    }
}
