using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230519131906283), Tags("Production")]
    public class MC5752_AdjustCurrentMarkoutsViewToAccountForNonNJRequirements : Migration
    {
        public const string VIEW_NAME = MC5451_FixCurrentMarkoutView.VIEW_NAME;

        public const string OLD_VIEW_SQL = MC5451_FixCurrentMarkoutView.NEW_VIEW_SQL;

        public const string NEW_VIEW_SQL = @"
-- we store values without regard to timezone, despite them being Eastern US Time, but GETDATE() returns a
-- value in sql server time, so comparisons end up being off by 4 or 5 hours.  this accounts for that, in
-- quite a tricky way
WITH CurrentDateTime AS (
    SELECT DATEADD(
        minute,
        DATEPART(tzoffset, GETDATE() AT TIME ZONE 'UTC' AT TIME ZONE 'Eastern Standard Time'),
        GETDATE()) AS RightNow
-- for convenience, so we can control the selection columns in a single spot and
-- just use * everywhere else
), FilteredMarkouts AS (
    SELECT m.WorkOrderID,
           m.MarkoutID,
           m.ExpirationDate,    
           m.ReadyDate
      FROM Markouts m
-- markouts which are currently ready and not yet expired
), CurrentMarkouts AS (    
    SELECT m.*    
      FROM FilteredMarkouts m
INNER JOIN CurrentDateTime cdt ON m.ReadyDate <= cdt.RightNow AND m.ExpirationDate >= cdt.RightNow
-- current markouts pared down so we have the latest ExpirationDate by
-- WorkOrderID
), LastExpiringCurrentMarkouts AS ( 
    SELECT m.*         
      FROM CurrentMarkouts m
 LEFT JOIN CurrentMarkouts later
        ON later.WorkOrderID = m.WorkOrderID
       AND later.ExpirationDate > m.ExpirationDate
     WHERE later.MarkoutID IS NULL
-- remove duplicates from the above
), DuplicatesRemovedCurrentMarkouts AS (  
    SELECT m.*         
      FROM LastExpiringCurrentMarkouts m
 LEFT JOIN LastExpiringCurrentMarkouts dup
        ON dup.WorkOrderID = m.WorkOrderID
       AND dup.MarkoutID > m.MarkoutID
     WHERE dup.ExpirationDate IS NULL
-- markouts which have a ReadyDate in the future
), FutureMarkouts AS (    
    SELECT m.*     
      FROM FilteredMarkouts m
INNER JOIN CurrentDateTime cdt ON m.ReadyDate > cdt.RightNow
-- future markouts pared down so we have the earliest ExpirationDate by
-- WorkOrderID
), SoonestExpiringFutureMarkouts AS (       
    SELECT m.*         
      FROM FutureMarkouts m
 LEFT JOIN FutureMarkouts sooner
        ON sooner.WorkOrderID = m.WorkOrderID
       AND sooner.ExpirationDate < m.ExpirationDate
     WHERE sooner.MarkoutID IS NULL
-- remove duplicates from the above
), DuplicatesRemovedFutureMarkouts AS ( 
    SELECT m.*     
      FROM SoonestExpiringFutureMarkouts m
 LEFT JOIN SoonestExpiringFutureMarkouts dup
        ON dup.WorkOrderID = m.WorkOrderID
       AND dup.MarkoutID > m.MarkoutID
     WHERE dup.ExpirationDate IS NULL
-- markouts which have an ExpirationDate in the past
), ExpiredMarkouts AS (    
    SELECT m.*      
      FROM FilteredMarkouts m
INNER JOIN CurrentDateTime cdt ON m.ReadyDate < cdt.RightNow
-- expired markouts pared down so we have the latest ExpirationDate by
-- WorkOrderID
), LastExpiredMarkouts AS ( 
    SELECT m.*       
      FROM ExpiredMarkouts m
 LEFT JOIN ExpiredMarkouts later
        ON later.WorkOrderID = m.WorkOrderID
       AND later.ExpirationDate > m.ExpirationDate
     WHERE later.MarkoutID IS NULL
-- remove duplicates from the above
), DuplicatesRemovedExpiredMarkouts AS ( 
    SELECT m.*       
      FROM LastExpiredMarkouts m
 LEFT JOIN LastExpiredMarkouts dup
        ON dup.WorkOrderID = m.WorkOrderID
       AND dup.MarkoutID > m.MarkoutID
     WHERE dup.ExpirationDate IS NULL) 
-- if we have a ""current"" markout, use that.  if we have a ""future"" markout, 
-- use that.  if we have an ""expired"" markout, use that.  any future columns 
-- which might need to be returned from the Markouts table via this view will  
-- also need to be COALESCEd here   
    SELECT wo.WorkOrderID,      
           COALESCE(              
               currentMO.MarkoutID, 
               futureMO.MarkoutID,  
               expiredMO.MarkoutID) AS MarkoutID,      
           COALESCE(           
               currentMO.ExpirationDate,       
               futureMO.ExpirationDate,      
               expiredMO.ExpirationDate) AS ExpirationDate,  
           COALESCE(           
               currentMO.ReadyDate,   
               futureMO.ReadyDate,    
               expiredMO.ReadyDate) AS ReadyDate
      FROM WorkOrders wo
 LEFT JOIN DuplicatesRemovedCurrentMarkouts currentMO
        ON currentMO.WorkOrderID = wo.WorkOrderID
 LEFT JOIN DuplicatesRemovedFutureMarkouts futureMO
        ON futureMO.WorkOrderID = wo.WorkOrderID
 LEFT JOIN DuplicatesRemovedExpiredMarkouts expiredMO
        ON expiredMO.WorkOrderID = wo.WorkOrderID";

        public override void Up()
        {
            Execute.Sql($"ALTER VIEW [{VIEW_NAME}] AS{NEW_VIEW_SQL}");
        }

        public override void Down()
        {
            Execute.Sql($"ALTER VIEW [{VIEW_NAME}] AS{OLD_VIEW_SQL}");
        }
    }
}

