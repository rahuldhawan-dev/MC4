using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20171228105635780), Tags("Production")]
    public class WO0000000208252 : Migration
    {
        public override void Up()
        {
            Execute.Sql("CREATE TABLE EmployeeSpokeWithNurse(Id int PRIMARY KEY, Description varchar(255) NOT NULL)");

            Execute.Sql("INSERT INTO EmployeeSpokeWithNurse(Id, Description) VALUES(1, 'Not Offered')");
            Execute.Sql("INSERT INTO EmployeeSpokeWithNurse(Id, Description) VALUES(2, 'Offered - Employee Accepted')");
            Execute.Sql("INSERT INTO EmployeeSpokeWithNurse(Id, Description) VALUES(3, 'Offered - Employee Refused')");

            Alter.Table("Incidents").AddForeignKeyColumn("EmployeeSpeakToNurseId", "EmployeeSpokeWithNurse");

            Execute.Sql(
                "Update Incidents set dbo.Incidents.EmployeeSpeakToNurseId = 2 From EmployeeSpokeWithNurse where Incidents.EmployeeSpokeToNurse = 1");
            Execute.Sql(
                "Update Incidents set dbo.Incidents.EmployeeSpeakToNurseId = 3 From EmployeeSpokeWithNurse where Incidents.EmployeeSpokeToNurse = 0");

            Delete.Column("EmployeeSpokeToNurse").FromTable("Incidents");
        }

        public override void Down()
        {
            Create.Column("EmployeeSpokeToNurse").OnTable("Incidents").AsBoolean().Nullable();
            // repopulates columns that we deleted in Up()
            Execute.Sql("Update Incidents SET EmployeeSpokeToNurse = 0 WHERE Incidents.EmployeeSpeakToNurseId = 3");
            Execute.Sql("Update Incidents SET EmployeeSpokeToNurse = 1 WHERE Incidents.EmployeeSpeakToNurseId = 2");

            Delete.ForeignKeyColumn("Incidents", "EmployeeSpeakToNurseId", "EmployeeSpokeWithNurse");

            Delete.Table("EmployeeSpokeWithNurse");
        }
    }
}
