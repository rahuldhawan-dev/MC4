using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150708145045483), Tags("Production")]
    public class AddFacilityToHydrantAndValveForBug2336 : Migration
    {
        public override void Up()
        {
            Alter.Table("Hydrants").AddForeignKeyColumn("FacilityId", "tblFacilities", "RecordId");
            Alter.Table("Valves").AddForeignKeyColumn("FacilityId", "tblFacilities", "RecordId");
            Execute.Sql(
                "IF NOT EXISTS (SELECT 1 FROM ValveBillings where Description = 'COMPANY') INSERT INTO ValveBillings VALUES('Company');");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Hydrants", "FacilityId", "tblFacilities", "RecordId");
            Delete.ForeignKeyColumn("Valves", "FacilityId", "tblFacilities", "RecordId");
        }
    }
}
