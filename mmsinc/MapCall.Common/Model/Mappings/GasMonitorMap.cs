using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations._2020;
using MMSINC.ClassExtensions.StringExtensions;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Mapping;

namespace MapCall.Common.Model.Mappings
{
    public class GasMonitorMap : ClassMap<GasMonitor>
    {
        public const string TABLE_NAME = "GasMonitors";

        public GasMonitorMap()
        {
            Id(x => x.Id).Not.Nullable();

            Map(x => x.CalibrationFrequencyDays).Not.Nullable();
            // WE DO NOT WANT SQLITE TRYING TO CREATE THIS AS A REAL THING
            HasOne(x => x.MostRecentPassingGasMonitorCalibration).LazyLoad().Fetch.Join().Cascade.None();
            References(x => x.AssignedEmployee).Nullable();
            References(x => x.Equipment).Not.Nullable();

            HasMany(x => x.Calibrations).KeyColumn("GasMonitorId").Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }

    public class MostRecentGasMonitorCalibrationMap : ClassMap<MostRecentGasMonitorCalibration>
    {
        public MostRecentGasMonitorCalibrationMap()
        {
            Table("MostRecentGasMonitorCalibration");
            ReadOnly();
            LazyLoad();
            Id(x => x.Id).Not.Nullable();
            Map(x => x.DueCalibration).Not.Nullable();
            Map(x => x.CalibrationDate).Nullable();
            Map(x => x.NextDueDate).Nullable();
            SchemaAction.None();
        }
    }

    public class MostRecentGasMonitorCalibrationViewMap : AbstractAuxiliaryDatabaseObject
    {
        public struct Sql
        {
            public const string CREATE_VIEW = AddCTEViewForMostRecentGasMonitorCalibrationForMC2569.CREATE_VIEW,
                                DROP_VIEW = AddCTEViewForMostRecentGasMonitorCalibrationForMC2569.DROP_VIEW;
        }

        public override string SqlCreateString(Dialect dialect, IMapping p, string defaultCatalog, string defaultSchema)
        {
            return Sql.CREATE_VIEW.ToSqlite();
        }

        public override string SqlDropString(Dialect dialect, string defaultCatalog, string defaultSchema)
        {
            return Sql.DROP_VIEW;
        }
    }
}
