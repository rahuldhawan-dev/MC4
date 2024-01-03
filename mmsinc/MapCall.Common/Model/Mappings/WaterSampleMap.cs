using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class WaterSampleMap : ClassMap<WaterSample>
    {
        public WaterSampleMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.SampleIdMatrix).Column("SampleMatrixId").Nullable();
            References(x => x.UnitOfMeasure).Nullable();

            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.SampleDate).Nullable();
            Map(x => x.CollectedBy).Nullable();
            Map(x => x.AnalysisPerformedBy).Nullable();
            Map(x => x.SampleValue).Nullable();
            Map(x => x.Notes).Nullable();
            Map(x => x.NonDetect).Not.Nullable();
            Map(x => x.IsInvalid).Not.Nullable();

            HasMany(x => x.WaterSampleDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.WaterSampleNotes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
        }
    }
}
