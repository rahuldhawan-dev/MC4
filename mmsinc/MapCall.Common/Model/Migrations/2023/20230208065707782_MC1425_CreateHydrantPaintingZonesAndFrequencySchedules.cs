using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230208065707782), Tags("Production")]
    public class MC1425_CreateHydrantPaintingZonesAndFrequencySchedules : AutoReversingMigration
    {
        public override void Up()
        {
            Create
               .Column("PaintingFrequency")
               .OnTable("Hydrants")
               .AsInt32().Nullable();

            Create.ForeignKeyColumn(
                "Hydrants",
                "PaintingFrequencyUnitId",
                "RecurringFrequencyUnits");

            Create
               .Column("PaintingZone")
               .OnTable("Hydrants")
               .AsInt32().Nullable();

            Create
               .Column("PaintingZoneStartYear")
               .OnTable("OperatingCenters")
               .AsInt32().Nullable();

            Create
               .Column("HydrantPaintingFrequency")
               .OnTable("OperatingCenters")
               .AsInt32().Nullable();

            Create.ForeignKeyColumn(
                "OperatingCenters",
                "HydrantPaintingFrequencyUnitId",
                "RecurringFrequencyUnits");
        }
    }
}

