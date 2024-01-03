using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200827154250824), Tags("Production")]
    public class MC1107AddSupplementNoColumnToWorkOrders : Migration
    {
        public override void Up()
        {
            Alter.Table("WorkOrders").AddColumn("ApartmentAddtl").AsAnsiString(30).Nullable();
            Alter.Column("ApartmentNumber").OnTable("Services").AsAnsiString(30).Nullable();
        }

        public override void Down()
        {
            Alter.Column("ApartmentNumber").OnTable("Services").AsAnsiString(15).Nullable();
            Delete.Column("ApartmentAddtl").FromTable("WorkOrders");
        }
    }
}
