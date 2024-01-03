using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220621150520407), Tags("Production")]
    public class MC4593AddFacilityFacilityAreaIdToProductionWorkOrder : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("ProductionWorkOrders")
                 .AddForeignKeyColumn("FacilityFacilityAreaId", "FacilitiesFacilityAreas", nullable: true);
        }
    }
}
