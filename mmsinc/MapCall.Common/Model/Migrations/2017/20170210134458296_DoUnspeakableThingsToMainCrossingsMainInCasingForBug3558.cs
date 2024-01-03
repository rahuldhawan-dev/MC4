using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170210134458296), Tags("Production")]
    public class DoUnspeakableThingsToMainCrossingsMainInCasingForBug3558 : Migration
    {
        public override void Up()
        {
            Alter.Column("MainInCasing").OnTable("MainCrossings").AsInt32().Nullable();

            Execute.Sql(
                $"UPDATE MainCrossings SET MainInCasing = MainInCasing + 1 WHERE MainInCasing IS NOT NULL");

            Create.LookupTable("MainInCasingStatuses");

            Insert.IntoTable("MainInCasingStatuses")
                  .Rows(
                       new {Description = "No"}, new {Description = "Yes"}, new {Description = "Unknown"},
                       new {Description = "High Probability"}, new {Description = "Low Probability"});

            Rename.Column("MainInCasing").OnTable("MainCrossings").To("MainInCasingId");
            Alter.Column("MainInCasingId")
                 .OnTable("MainCrossings")
                 .AsForeignKey("MainInCasingId", "MainInCasingStatuses");
        }

        public override void Down()
        {
            this.DeleteForeignKeyIfItExists("MainCrossings", "MainInCasingId", "MainInCasingStatuses");

            Delete.Table("MainInCasingStatuses");

            Rename.Column("MainInCasingId").OnTable("MainCrossings").To("MainInCasing");

            Execute.Sql(
                $"UPDATE MainCrossings SET MainInCasing = CASE WHEN MainInCasing > 2 THEN NULL ELSE MainInCasing - 1 END");

            Alter.Column("MainInCasing").OnTable("MainCrossings").AsBoolean().Nullable();
        }
    }
}
