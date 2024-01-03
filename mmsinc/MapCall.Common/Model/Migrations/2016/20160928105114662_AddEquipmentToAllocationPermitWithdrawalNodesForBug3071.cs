using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160928105114662), Tags("Production")]
    public class AddEquipmentToAllocationPermitWithdrawalNodesForBug3071 : Migration
    {
        public override void Up()
        {
            Create.ManyToManyTable("AllocationPermitWithdrawalNodes", "Equipment",
                "AllocationPermitWithdrawalNodeID", "EquipmentID");
        }

        public override void Down()
        {
            Delete.ManyToManyTable("AllocationPermitWithdrawalNodes", "Equipment");
        }
    }
}
