using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230707152650657), Tags("Production")]
    public class MC5309AddWorkCompletionByDateToWorkOrders : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("WorkOrders").AddColumn("WorkCompletionByDate").AsDateTime().Nullable();
        }
    }
}

