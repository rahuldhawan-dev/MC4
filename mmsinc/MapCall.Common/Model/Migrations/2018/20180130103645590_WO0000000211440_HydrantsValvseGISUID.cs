using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180130103645590), Tags("Production")]
    public class WO0000000211440_HydrantsValvseGISUID : Migration
    {
        public override void Up()
        {
            Rename.Column("GeoEFunctionalLocation").OnTable("Hydrants").To("GISUID");
            Alter.Column("GISUID").OnTable("Hydrants").AsAnsiString(40).Nullable();

            Rename.Column("GeoEFunctionalLocation").OnTable("Valves").To("GISUID");
            Alter.Column("GISUID").OnTable("Valves").AsAnsiString(40).Nullable();
        }

        public override void Down()
        {
            // Do not revert the increased string length on these columns. Data could be lost.
            Rename.Column("GISUID").OnTable("Hydrants").To("GeoEFunctionalLocation");
            Rename.Column("GISUID").OnTable("Valves").To("GeoEFunctionalLocation");
        }
    }
}
