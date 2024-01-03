using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221212075343519), Tags("Production")]
    public class MC1763_AddWorkOrderFieldToEquipment : Migration
    {
        public override void Up()
        {
            Alter.Table("Equipment").AddForeignKeyColumn("ReplacementProductionWorkOrderId", "ProductionWorkOrders");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Equipment", "ReplacementProductionWorkOrderId", "ProductionWorkOrders");
        }
    }
}
