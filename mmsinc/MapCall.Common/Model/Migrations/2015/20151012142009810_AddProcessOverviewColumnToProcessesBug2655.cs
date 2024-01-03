using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151012142009810), Tags("Production")]
    public class AddProcessOverviewColumnToProcessesBug2655 : Migration
    {
        public override void Up()
        {
            Alter.Table("Processes")
                 .AddColumn("ProcessOverview").AsText().Nullable();
        }

        public override void Down()
        {
            Delete.Column("ProcessOverview").FromTable("Processes");
        }
    }
}
