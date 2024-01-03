using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140919161855919), Tags("Production")]
    public class AddProgramCoordinatorTablesForBug2099 : Migration
    {
        public struct TableNames
        {
            public const string TRAINING_CONTACT_HOURS_PROGRAM_COORDINATORS = "TrainingContactHoursProgramCoordinators",
                                TRAINING_RECORDS = "tblTrainingRecords";
        }

        public const string
            FK_PROGRAM_COORDINATOR_EMPLOYEES_EMPLOYEE_ID =
                "FK_TrainingContactHoursProgramCoordinators_Employees_ProgramCoordinatorId";

        public override void Up()
        {
            Create.Table(TableNames.TRAINING_CONTACT_HOURS_PROGRAM_COORDINATORS)
                  .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn("ProgramCoordinatorId").AsInt32().NotNullable()
                  .ForeignKey(FK_PROGRAM_COORDINATOR_EMPLOYEES_EMPLOYEE_ID, "tblEmployee", "tblEmployeeId");
            Alter.Table(TableNames.TRAINING_RECORDS).AddForeignKeyColumn("ProgramCoordinator",
                TableNames.TRAINING_CONTACT_HOURS_PROGRAM_COORDINATORS);
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn(TableNames.TRAINING_RECORDS, "ProgramCoordinator",
                TableNames.TRAINING_CONTACT_HOURS_PROGRAM_COORDINATORS);
            Delete.ForeignKey(FK_PROGRAM_COORDINATOR_EMPLOYEES_EMPLOYEE_ID)
                  .OnTable(TableNames.TRAINING_CONTACT_HOURS_PROGRAM_COORDINATORS);
            Delete.Table(TableNames.TRAINING_CONTACT_HOURS_PROGRAM_COORDINATORS);
        }
    }
}
