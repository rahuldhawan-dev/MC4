using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class WellTestMap : ClassMap<WellTest>
    {
        #region Constructors

        public WellTestMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.ProductionWorkOrder).Not.Nullable();
            References(x => x.Equipment).Not.Nullable();
            References(x => x.Employee).Not.Nullable();
            References(x => x.GradeType).Not.Nullable();

            Map(x => x.DateOfTest).Not.Nullable();
            Map(x => x.PumpingRate).Not.Nullable();
            Map(x => x.MeasurementPoint).Not.Nullable().Precision(7).Scale(2);
            Map(x => x.StaticWaterLevel).Not.Nullable().Precision(7).Scale(2);
            Map(x => x.PumpingWaterLevel).Not.Nullable().Precision(7).Scale(2);
        }

        #endregion
    }
}
