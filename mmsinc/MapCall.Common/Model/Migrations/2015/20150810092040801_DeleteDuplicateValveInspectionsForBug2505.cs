using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150810092040801), Tags("Production")]
    public class DeleteDuplicateValveInspectionsForBug2505 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"delete
	ValveInspections
from
	ValveInspections
LEFT OUTER JOIN (
	SELECT MIN(ID) as ID, ValveId, DateInspected
	FROM ValveInspections
	GROUP BY ValveId, DateInspected
) as KeepRows ON
	ValveInspections.ID = KeepRows.ID
where
	KeepRows.ID is null");
        }

        public override void Down() { }
    }
}
