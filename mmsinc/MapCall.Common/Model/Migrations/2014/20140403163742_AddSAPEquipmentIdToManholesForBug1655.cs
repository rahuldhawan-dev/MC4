using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140403163742), Tags("Production")]
    public class AddSAPEquipmentIdToManholesForBug1655 : Migration
    {
        public override void Up()
        {
            Alter.Table("SewerManholes").AddColumn("SAPEquipmentID").AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.Column("SAPEquipmentID").FromTable("SewerManholes");
        }
    }
}
