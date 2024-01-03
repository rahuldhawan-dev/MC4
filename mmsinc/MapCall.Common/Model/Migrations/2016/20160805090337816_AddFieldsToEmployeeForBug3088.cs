using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160805090337816), Tags("Production")]
    public class AddFieldsToEmployeeForBug3088 : Migration
    {
        public override void Up()
        {
            Alter.Table("tblEmployee")
                 .AddColumn("ValidEssentialEmployeeCard").AsBoolean().NotNullable().WithDefaultValue(false)
                 .AddColumn("GETSWPSCard").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("GETSWPSCard").FromTable("tblEmployee");
            Delete.Column("ValidEssentialEmployeeCard").FromTable("tblEmployee");
        }
    }
}
