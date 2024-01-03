using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170323111757911), Tags("Production")]
    public class Bug3702Employees : Migration
    {
        public override void Up()
        {
            Alter.Table("tblEmployee").AddColumn("Address2").AsString(255).Nullable();
        }

        public override void Down()
        {
            Delete.Column("Address2").FromTable("tblEmployee");
        }
    }
}
