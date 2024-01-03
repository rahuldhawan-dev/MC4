using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160421141621506), Tags("Production")]
    public class AddDataTypeForBug2891 : Migration
    {
        public override void Up()
        {
            this.CreateDocumentType(CreateAPCInspectionItemsTableForBug2891.TableNames.APC_INSPECTION_ITEMS,
                "APC Inspection Type", "APC Inspection Type Document");
        }

        public override void Down()
        {
            this.DeleteDataType(CreateAPCInspectionItemsTableForBug2891.TableNames.APC_INSPECTION_ITEMS);
        }
    }
}
