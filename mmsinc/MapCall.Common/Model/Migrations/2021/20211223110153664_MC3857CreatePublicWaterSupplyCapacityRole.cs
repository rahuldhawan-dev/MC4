using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20211223110153664), Tags("Production")]
    public class MC3857CreatePublicWaterSupplyCapacityRole : Migration
    {
        public override void Up() => this.CreateModule("PWSID Capacity", "Engineering", 96);

        public override void Down() => this.DeleteModuleAndAssociatedRoles("Engineering", "PWSID Capacity");
    }
}