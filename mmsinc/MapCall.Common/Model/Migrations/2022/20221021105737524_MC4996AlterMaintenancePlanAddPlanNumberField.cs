using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221021105737524), Tags("Production")]
    public class MC4996AlterMaintenancePlanAddPlanNumberField : Migration
    {
        public override void Up()
        {
            Execute.Sql("ALTER TABLE MaintenancePlans ADD PlanNumber AS ('9' + Right('00000000' + cast(Id as varchar(8)), 8))");
        }

        public override void Down()
        {
            Execute.Sql("ALTER TABLE MaintenancePlans DROP Column PlanNumber");
        }
    }
}

