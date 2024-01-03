using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230718113248482), Tags("Production")]
    public class MC5520_AddWorkOrderRequesterOption : Migration 
    {
        const int WorkOrderRequesterId = 9;
        const string WorkOrderRequesterDescription = "NSI";
    
        public override void Up()
        {
            Execute.Sql(@"SET IDENTITY_INSERT WorkOrderRequesters ON;" +
                        "INSERT INTO WorkOrderRequesters ([WorkOrderRequesterID], [Description]) VALUES (" + WorkOrderRequesterId + ", '" + WorkOrderRequesterDescription + "');" + 
                        "SET IDENTITY_INSERT WorkOrderRequesters OFF;");
        }

        public override void Down()
        {
            Delete.FromTable("WorkOrderRequesters").Row(new { Description = WorkOrderRequesterDescription });
        }
    }
}

