using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20231220133955738), Tags("Production")]
    public class MC6325_UpdateProductionWorkOrderRequiresSupervisorApprovalView : Migration
    {
        public const string UPDATE_VIEW = @"ALTER VIEW [ProductionWorkOrderRequiresSupervisorApproval] AS
        SELECT
            pwo.Id AS [ProductionWorkOrderId]
	          ,CASE
                WHEN (pwo.ApprovedOn IS NULL AND pwo.DateCancelled IS NULL and pwo.DateCompleted IS NOT NULL AND (pwd.OrderTypeId = 1 OR pwd.OrderTypeId = 3 OR pwd.OrderTypeId = 4)) THEN 1
                ELSE 0
		        END AS [RequiresSupervisorApproval]
        FROM
            ProductionWorkOrders pwo
        INNER JOIN
            ProductionWorkDescriptions pwd
        ON
        pwo.ProductionWorkDescriptionId = pwd.Id";

        public const string ROLLBACK_UPDATE_VIEW = @"ALTER VIEW [ProductionWorkOrderRequiresSupervisorApproval] AS
        SELECT
            pwo.Id AS [ProductionWorkOrderId]
	          ,CASE
                WHEN (pwo.ApprovedOn IS NULL AND pwo.DateCancelled IS NULL and pwo.DateCompleted IS NOT NULL AND (pwd.OrderTypeId = 3 OR pwd.OrderTypeId = 4)) THEN 1
                ELSE 0
		        END AS [RequiresSupervisorApproval]
        FROM
            ProductionWorkOrders pwo
        INNER JOIN
            ProductionWorkDescriptions pwd
        ON
        pwo.ProductionWorkDescriptionId = pwd.Id";

        public override void Up()
        {
            Execute.Sql(UPDATE_VIEW);
        }

        public override void Down()
        {
            Execute.Sql(ROLLBACK_UPDATE_VIEW);
        }
    }
}

