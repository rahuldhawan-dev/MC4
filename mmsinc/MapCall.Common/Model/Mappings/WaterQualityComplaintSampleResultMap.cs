using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class WaterQualityComplaintSampleResultMap : ClassMap<WaterQualityComplaintSampleResult>
    {
        #region Constructors

        public WaterQualityComplaintSampleResultMap()
        {
            Table("WaterQualityComplaintSampleResults");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.WaterConstituent).Nullable();
            References(x => x.Complaint).Nullable();

            Map(x => x.SampleDate).Not.Nullable();
            Map(x => x.SampleValue).Not.Nullable().Length(50);
            Map(x => x.UnitOfMeasure).Nullable().Length(25);
            Map(x => x.AnalysisPerformedBy).Not.Nullable().Length(50);
        }

        #endregion
    }
}
