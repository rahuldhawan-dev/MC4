using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230103203355047), Tags("Production")]
    public class MC4938AddDueDateField : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Column("DueDate")
                  .OnTable("ProductionWorkOrders")
                  .AsDateTime()
                  .Nullable();
        }
    }
}

