using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201113121747729), Tags("Production")]
    public class MC1338SewerOverflowReasonRemoveColumn : Migration
    {
        public override void Up()
        {
            Delete.ForeignKeyColumn("SewerOverflows", "SewerOverflowCauseID", "SewerOverFlowCauses",
                "SewerOverflowCauseID");
        }

        public override void Down()
        {
            Alter.Table("SewerOverFlows")
                 .AddForeignKeyColumn("SewerOverflowCauseID", "SewerOverFlowCauses", "SewerOverflowCauseID");

            Execute.Sql(@"with cteMinimumCauses (SewerOverflowId, SewerOverflowCauseId)
as
(
    select
        SewerOverflowId,
        min(SewerOverflowCauseId) as SewerOverflowCauseId
    from
        SewerOverflowReasons
    group by
        SewerOverflowId
)
update SewerOverflows
set SewerOverflowCauseId = cteMinimumCauses.SewerOverflowCauseId
from cteMinimumCauses
where cteMinimumCauses.SewerOverflowId = SewerOverflows.SewerOverflowId");
        }
    }
}
