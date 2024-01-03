using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160817162027908), Tags("Production")]
    public class AddPortableFieldToEquipmentForBug3102 : Migration
    {
        public override void Up()
        {
            Alter.Table("Equipment").AddColumn("Portable").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("Portable").FromTable("Equipment");
        }
    }
}
