using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150608095824360), Tags("Production")]
    public class AddEquipmentModelDocumentDataTypesForBug2417 : Migration
    {
        public override void Up()
        {
            this.CreateDocumentType("EquipmentModels", "EquipmentModel", "Equipment Model Document");
        }

        public override void Down()
        {
            this.DeleteDataType("EquipmentModels");
        }
    }
}
