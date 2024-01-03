using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220701104538663), Tags("Production")]
    public class MC727UpdateProductionWorkOrderPriorities : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                UPDATE ProductionWorkOrderPriorities
                SET Description = 'Routine - Off Scheduled To be Deleted'
                WHERE Description = 'Routine - Off Scheduled' AND Id <> 6;
                IF NOT EXISTS (Select Description FROM ProductionWorkOrderPriorities WHERE Description = 'Routine - Off Scheduled')
                BEGIN
                SET IDENTITY_INSERT ProductionWorkOrderPriorities ON;
                INSERT INTO ProductionWorkOrderPriorities(Id, Description) VALUES(6, 'Routine - Off Scheduled');
                SET IDENTITY_INSERT ProductionWorkOrderPriorities OFF;
                END
                IF EXISTS (SELECT Description FROM ProductionWorkOrderPriorities WHERE Description = 'Routine - Off Scheduled To be Deleted')
                BEGIN                
                UPDATE ProductionWorkOrders
                SET PriorityId = 6 WHERE PriorityId IN (SELECT Id AS PriorityId FROM ProductionWorkOrderPriorities WHERE Description = 'Routine - Off Scheduled To be Deleted');
                DELETE FROM ProductionWorkOrderPriorities WHERE Description = 'Routine - Off Scheduled To be Deleted';
                END
            ");
        }

        public override void Down()
        {
            Execute.Sql(@"
                UPDATE ProductionWorkOrders
                SET PriorityId = NULL WHERE PriorityId = 6;
                DELETE FROM ProductionWorkOrderPriorities WHERE Description = 'Routine - Off Scheduled';
            ");
        }
    }
}
