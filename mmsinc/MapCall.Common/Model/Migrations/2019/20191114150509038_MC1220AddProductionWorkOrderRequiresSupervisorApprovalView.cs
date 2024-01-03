using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20191114150509038), Tags("Production")]
    public class MC1220AddView : Migration
    {
        public const string CREATE_VIEW =
                                @"
CREATE VIEW [ProductionWorkOrderRequiresSupervisorApproval] AS
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
    pwo.ProductionWorkDescriptionId = pwd.Id",
                            DROP_VIEW = "DROP VIEW [ProductionWorkOrderRequiresSupervisorApproval]";

        public override void Up()
        {
            Execute.Sql(CREATE_VIEW);
        }

        public override void Down()
        {
            Execute.Sql(DROP_VIEW);
        }
    }
}
