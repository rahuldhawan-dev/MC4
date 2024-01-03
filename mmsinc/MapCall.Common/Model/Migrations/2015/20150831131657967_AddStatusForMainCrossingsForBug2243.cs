using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150831131657967), Tags("Production")]
    public class AddStatusForMainCrossingsForBug2243 : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("MainCrossingStatuses", "In Service", "Out of Service", "Pending",
                "Pending Retirement", "Retired");
            Alter.Table("MainCrossings").AddForeignKeyColumn("MainCrossingStatusId", "MainCrossingStatuses").Nullable();
            Execute.Sql("UPDATE MainCrossings Set MainCrossingStatusId = 1");
            Alter.Column("MainCrossingStatusId").OnTable("MainCrossings").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("MainCrossings", "MainCrossingStatusId", "MainCrossingStatuses");
            Delete.Table("MainCrossingStatuses");
        }
    }
}
