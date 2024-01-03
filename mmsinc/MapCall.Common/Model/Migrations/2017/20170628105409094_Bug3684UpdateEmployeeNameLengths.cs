using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170628105409094), Tags("Production")]
    public class Bug3684UpdateEmployeeNameLengths : Migration
    {
        public override void Up()
        {
            Alter.Table("tblEmployee")
                 .AlterColumn("First_Name").AsCustom("varchar(40)").Nullable()
                 .AlterColumn("Last_Name").AsCustom("varchar(40)").Nullable()
                 .AlterColumn("EmployeeID").AsCustom("nvarchar(255)").Nullable().Unique();
        }

        public override void Down()
        {
            // No down for string lengths because it'll error if existing lengths are too long.
            // Otherwise, last name used to be 25, first name was 20.
            Alter.Table("tblEmployee")
                 .AlterColumn("EmployeeID").AsCustom("nvarchar(255)").Nullable();
            Delete.Index("IX_tblEmployee_EmployeeID").OnTable("tblEmployee");
        }
    }
}
