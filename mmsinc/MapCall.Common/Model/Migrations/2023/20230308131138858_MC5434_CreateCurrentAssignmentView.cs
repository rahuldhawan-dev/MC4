using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230309141449061), Tags("Production")]
    public class MC5434_CreateCurrentAssignmentView : Migration
    {
        public const string VIEW_NAME = "CurrentAssignmentView";
        public const string DROP_SQL = "DROP VIEW [" + VIEW_NAME + "];";

        public const string CREATE_SQL = "CREATE VIEW [" + VIEW_NAME + @"] AS" + VIEW_SQL;

        public const string VIEW_SQL = @"
WITH MaxDates AS (
SELECT  WorkOrderID, MAX(AssignedFor) MaxDate
FROM CrewAssignments ic GROUP BY WorkOrderID
)
SELECT  
ca.WorkOrderID,
ca.CrewAssignmentID,
ca.CrewID,
ca.AssignedFor,
c.Description AS CrewName
FROM CrewAssignments ca 
LEFT JOIN Crews c ON ca.CrewID = c.CrewID
INNER JOIN MaxDates ON ca.WorkOrderID = MaxDates.WorkOrderID 
and MaxDates.MaxDate = ca.AssignedFor";

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

