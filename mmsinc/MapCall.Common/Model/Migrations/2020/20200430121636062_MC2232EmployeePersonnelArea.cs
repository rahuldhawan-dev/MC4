using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200430121636062), Tags("Production")]
    public class MC2232EmployeePersonnelArea : Migration
    {
        public override void Up()
        {
            Alter.Table("tblEmployee")
                 .AddForeignKeyColumn("PersonnelAreaId", "PersonnelAreas").Nullable();

            // TODO: Deal with ReportingLocation on CovidIssues
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("tblEmployee", "PersonnelAreaId", "PersonnelAreas");
        }
    }
}
