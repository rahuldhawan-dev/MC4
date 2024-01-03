using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210216111845468), Tags("Production")]
    public class MC2986ProductionPreJobSafetyBriefUpdates : Migration
    {
        public override void Up()
        {
            Rename.Table("ProductionPreJobSafetyBriefEmployees").To("ProductionPreJobSafetyBriefWorkers");
            Alter.Column("EmployeeId").OnTable("ProductionPreJobSafetyBriefWorkers").AsInt32().Nullable();
            Create.Column("Contractor").OnTable("ProductionPreJobSafetyBriefWorkers").AsAnsiString(255).Nullable();
        }

        public override void Down()
        {
            Delete.Column("Contractor").FromTable("ProductionPreJobSafetyBriefWorkers");
            // Deleting the Contractor column will make any column with a null Employee entirely useless,
            // so it's safe to delete that row and set Employee back to NotNullable.
            Alter.Column("EmployeeId").OnTable("ProductionPreJobSafetyBriefWorkers").AsInt32().NotNullable();

            Rename.Table("ProductionPreJobSafetyBriefWorkers").To("ProductionPreJobSafetyBriefEmployees");
        }
    }
}

