using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230316143016258), Tags("Production")]
    public class MC5471_FixCurrentAssignmentView : Migration
    {
        public const string VIEW_NAME = MC5434_CreateCurrentAssignmentView.VIEW_NAME;

        public const string OLD_VIEW_SQL = MC5434_CreateCurrentAssignmentView.VIEW_SQL;

        public const string NEW_VIEW_SQL = @" WITH LatestAssignments AS (
	SELECT ca.WorkOrderID, 
		   ca.CrewAssignmentID,
		   ca.CrewID,
		   ca.AssignedFor
	  FROM CrewAssignments ca 
 LEFT JOIN CrewAssignments later 
		ON later.WorkOrderID = ca.WorkOrderID 
	   AND later.AssignedFor > ca.AssignedFor 
	 WHERE later.CrewAssignmentID IS NULL
),RemovedDuplicates AS ( 
    SELECT ca.* 
	  FROM LatestAssignments ca 
 LEFT JOIN LatestAssignments duplicate 
        ON duplicate.WorkOrderID = ca.WorkOrderID 
       AND duplicate.CrewAssignmentID > ca.CrewAssignmentID 
	 WHERE duplicate.AssignedFor IS NULL
) 
	SELECT ca.*, c.Description as [CrewName] 
	  FROM RemovedDuplicates ca 
 LEFT JOIN Crews c ON ca.CrewID = c.CrewID";

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

