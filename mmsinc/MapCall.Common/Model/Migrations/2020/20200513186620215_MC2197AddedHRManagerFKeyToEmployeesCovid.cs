using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200513186620215), Tags("Production")]
    public class MC2197AddedHRManagerFKeyToEmployees : Migration
    {
        public override void Up()
        {
            Alter.Table("tblEmployee").AddForeignKeyColumn("HumanResourcesManagerId", "tblEmployee", "tblEmployeeID")
                 .Nullable();
            Alter.Table("CovidIssues").AddForeignKeyColumn("HumanResourcesManagerId", "tblEmployee", "tblEmployeeID")
                 .Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("tblEmployee", "HumanResourcesManagerId", "tblEmployee", "tblEmployeeID");
            Delete.ForeignKeyColumn("CovidIssues", "HumanResourcesManagerId", "tblEmployee", "tblEmployeeID");
        }
    }
}
