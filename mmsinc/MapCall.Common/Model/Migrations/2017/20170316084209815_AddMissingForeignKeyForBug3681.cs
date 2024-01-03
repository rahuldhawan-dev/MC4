using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170316084209815), Tags("Production")]
    public class AddMissingForeignKeyForBug3681 : Migration
    {
        public override void Up()
        {
            Alter.Column("tblEmployeeID").OnTable("EmployeeLink")
                 .AsForeignKey("tblEmployeeID", "tblEmployee", "tblEmployeeID");
        }

        public override void Down()
        {
            Delete.ForeignKey(Utilities.CreateForeignKeyName("EmployeeLink", "tblEmployee", "tblEmployeeId"))
                  .OnTable("EmployeeLink");
        }
    }
}
