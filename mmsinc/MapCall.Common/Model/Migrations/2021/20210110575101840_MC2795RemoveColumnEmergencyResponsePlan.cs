using FluentMigrator;
using FluentMigrator.Expressions;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20210110575101840), Tags("Production")]
    public class MC2795RemoveColumnEmergencyResponsePlan : Migration
    {
        public override void Up()
        {
            Delete.ForeignKeyColumn("EmergencyResponsePlans", "PlanSubcategoryId", "EmergencyPlanSubCategories", "Id");
            Delete.Table("EmergencyPlanSubCategories");
        }

        public override void Down()
        {
            this.CreateLookupTableWithValues("EmergencyPlanSubCategories", 50,
                "Not Applicable");
            Alter.Table("EmergencyResponsePlans")
                 .AddForeignKeyColumn("PlanSubcategoryId", "EmergencyPlanSubCategories");
        }
    }
}
