using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210520152611020), Tags("Production")]
    public class MC2882AddReviewsToPlans : Migration
    {
        public override void Up()
        {
            Create.Table("PlanReviews")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("PlanId", "EmergencyResponsePlans").NotNullable().Indexed()
                  .WithColumn("ReviewDate").AsDateTime().NotNullable()
                  .WithForeignKeyColumn("ReviewedById", "tblEmployee", "tblEmployeeId").NotNullable()
                  .WithColumn("ReviewChangeNotes").AsAnsiString(255).Nullable()
                  .WithColumn("NextReviewDate").AsDateTime().NotNullable()
                  .WithForeignKeyColumn("CreatedByUserId", "tblPermissions", "RecID").NotNullable()
                  .WithColumn("CreatedAt").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("PlanReviews");
        }
    }
}