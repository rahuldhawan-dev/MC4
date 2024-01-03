using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class GasMonitorCalibrationMap : ClassMap<GasMonitorCalibration>
    {
        public const string TABLE_NAME = "GasMonitorCalibrations";

        public GasMonitorCalibrationMap()
        {
            Id(x => x.Id);

            Map(x => x.CalibrationDate).Not.Nullable();
            Map(x => x.CalibrationPassed).Not.Nullable();
            Map(x => x.CalibrationFailedNotes).Length(int.MaxValue).Nullable();
            Map(x => x.CreatedAt).Not.Nullable();

            References(x => x.CreatedBy, "CreatedByUserId").Not.Nullable();
            References(x => x.GasMonitor).Not.Nullable();

            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
