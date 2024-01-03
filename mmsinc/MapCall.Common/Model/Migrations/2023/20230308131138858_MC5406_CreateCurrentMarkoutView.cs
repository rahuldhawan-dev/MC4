using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230308131138858), Tags("Production")]
    public class MC5406_CreateCurrentMarkoutView : Migration
    {
        public const string VIEW_NAME = "CurrentMarkoutsView";
        public const string DROP_SQL = "DROP VIEW [" + VIEW_NAME + "];";

        public const string CREATE_SQL = "CREATE VIEW [" + VIEW_NAME + @"] AS" + VIEW_SQL;

        public const string VIEW_SQL = @"
WITH CurrentMarkouts AS (    
SELECT m.WorkOrderID, 
       m.MarkoutID, 
	   m.ExpirationDate,
	   m.ReadyDate
      FROM Markouts m
     WHERE m.ReadyDate <= GETDATE() AND m.ExpirationDate >= GETDATE()), 
LastExpiringCurrentMarkouts AS (       
    SELECT m.* FROM CurrentMarkouts m
    LEFT JOIN CurrentMarkouts later
           ON later.WorkOrderID = m.WorkOrderID
          AND later.ExpirationDate > m.ExpirationDate
        WHERE later.MarkoutID IS NULL), 
FutureMarkouts AS ( SELECT m.WorkOrderID, m.MarkoutID, m.ExpirationDate, m.ReadyDate
      FROM Markouts m
     WHERE m.ReadyDate > GETDATE()),
SoonestExpiringFutureMarkouts AS ( SELECT m.* FROM FutureMarkouts m
    LEFT JOIN FutureMarkouts sooner
           ON sooner.WorkOrderID = m.WorkOrderID
          AND sooner.ExpirationDate < m.ExpirationDate
        WHERE sooner.MarkoutID IS NULL), 
ExpiredMarkouts AS ( SELECT m.WorkOrderID, m.MarkoutID, m.ExpirationDate, m.ReadyDate
      FROM Markouts m
     WHERE m.ExpirationDate < GETDATE()), 
LastExpiredMarkouts AS ( SELECT m.* FROM ExpiredMarkouts m
    LEFT JOIN ExpiredMarkouts later
           ON later.WorkOrderID = m.WorkOrderID
          AND later.ExpirationDate > m.ExpirationDate
        WHERE later.MarkoutID IS NULL)   
SELECT wo.WorkOrderID, 
       COALESCE( currentMO.MarkoutID, futureMO.MarkoutID, expiredMO.MarkoutID) AS MarkoutID,
	   COALESCE( currentMO.ExpirationDate, futureMO.ExpirationDate, expiredMO.ExpirationDate) AS ExpirationDate,
	   COALESCE( currentMO.ReadyDate, futureMO.ReadyDate, expiredMO.ReadyDate) AS ReadyDate
     FROM WorkOrders wo
LEFT JOIN LastExpiringCurrentMarkouts currentMO
       ON currentMO.WorkOrderID = wo.WorkOrderID
LEFT JOIN SoonestExpiringFutureMarkouts futureMO
       ON futureMO.WorkOrderID = wo.WorkOrderID
LEFT JOIN LastExpiredMarkouts expiredMO
       ON expiredMO.WorkOrderID = wo.WorkOrderID";

        public override void Up()
        {
            Execute.Sql(CREATE_SQL);
        }

        public override void Down()
        {
            Execute.Sql(DROP_SQL);
        }
    }
}

