using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230922115355748), Tags("Production")]
    public class MC6127RenameWorkCompletionByDateColumnToPlannedCompletionByDate : Migration
    {
        public override void Up()
        {
            Rename.Column("WorkCompletionByDate").OnTable("WorkOrders").To("PlannedCompletionDate");
        }

        public override void Down()
        {
            Rename.Column("PlannedCompletionDate").OnTable("WorkOrders").To("WorkCompletionByDate");
        }
    }
}

