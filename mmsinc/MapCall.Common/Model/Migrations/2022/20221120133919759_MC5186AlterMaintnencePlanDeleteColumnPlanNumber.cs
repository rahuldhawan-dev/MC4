using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221120133919759), Tags("Production")]
    public class MC5186AlterMaintnencePlanColumnPlanNumberRemoveComputed : Migration
    {
        public override void Up()
        {
            Delete.Column("PlanNumber").FromTable("MaintenancePlans");
        }

        public override void Down()
        {
            Execute.Sql("ALTER TABLE MaintenancePlans ADD PlanNumber AS ('9' + Right('00000000' + cast(Id as varchar(8)), 8))");
        }
    }
}

