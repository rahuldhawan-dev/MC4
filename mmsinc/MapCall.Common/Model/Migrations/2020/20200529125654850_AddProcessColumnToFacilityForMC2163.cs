using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200529125654850), Tags("Production")]
    public class AddProcessColumnToFacilityForMC2163 : Migration
    {
        public override void Up()
        {
            Alter.Table("tblFacilities").AddForeignKeyColumn("ProcessId", "ProcessStages", "ProcessStageID");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("tblFacilities", "ProcessId", "ProcessStages", "ProcessStageID");
        }
    }
}
