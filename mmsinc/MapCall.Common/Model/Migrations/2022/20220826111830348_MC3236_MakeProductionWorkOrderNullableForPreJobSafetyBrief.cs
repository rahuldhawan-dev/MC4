using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220826111830348), Tags("Production")]
    public class MC3236_MakeProductionWorkOrderNullableForPreJobSafetyBrief : Migration
    {
        public override void Up()
        {
            Alter.Table("ProductionPreJobSafetyBriefs")
                 .AddForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterId")
                 .AddForeignKeyColumn("FacilityId", "tblFacilities", "RecordID")
                 .AlterColumn("ProductionWorkOrderId").AsInt32().Nullable();
            
            Execute.Sql(@"
UPDATE briefs 
SET briefs.OperatingCenterId = orders.OperatingCenterId,
    briefs.FacilityId = orders.FacilityId
FROM ProductionPreJobSafetyBriefs briefs
INNER JOIN ProductionWorkOrders orders ON briefs.ProductionWorkOrderId = orders.Id");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn(
                "ProductionPreJobSafetyBriefs",
                "FacilityId",
                "tblFacilities",
                "RecordID");
            
            Delete.ForeignKeyColumn(
                "ProductionPreJobSafetyBriefs",
                "OperatingCenterId",
                "OperatingCenters",
                "OperatingCenterId");
        }
    }
}

