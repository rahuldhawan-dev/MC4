using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160209092804392), Tags("Production")]
    public class AddGeoEFieldForBug2791 : Migration
    {
        public override void Up()
        {
            Alter.Table("Equipment").AddColumn("GeoEFunctionalLocation").AsAnsiString(18).Nullable();
            Alter.Table("Hydrants").AddColumn("GeoEFunctionalLocation").AsAnsiString(18).Nullable();
            Alter.Table("SewerManholes").AddColumn("GeoEFunctionalLocation").AsAnsiString(18).Nullable();
            Alter.Table("Valves").AddColumn("GeoEFunctionalLocation").AsAnsiString(18).Nullable();
        }

        public override void Down()
        {
            Delete.Column("GeoEFunctionalLocation").FromTable("Equipment");
            Delete.Column("GeoEFunctionalLocation").FromTable("Hydrants");
            Delete.Column("GeoEFunctionalLocation").FromTable("SewerManholes");
            Delete.Column("GeoEFunctionalLocation").FromTable("Valves");
        }
    }
}
