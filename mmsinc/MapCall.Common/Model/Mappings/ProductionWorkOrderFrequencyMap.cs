using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ProductionWorkOrderFrequencyMap : ClassMap<ProductionWorkOrderFrequency>
    {
        #region Constructors

        public ProductionWorkOrderFrequencyMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Name).Length(ProductionWorkOrderFrequency.StringLengths.NAME).Not.Nullable();
            Map(x => x.Abbreviation).Length(ProductionWorkOrderFrequency.StringLengths.ABBREVIATION).Nullable();
            Map(x => x.Description).Length(ProductionWorkOrderFrequency.StringLengths.DESCRIPTION).Nullable();
            Map(x => x.SortOrder).Not.Nullable();
            Map(x => x.ForecastYearSpan).Not.Nullable();
        }

        #endregion
    }
}
