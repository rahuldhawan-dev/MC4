using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201006121747729), Tags("Production")]
    public class MC1338SewerOverflowReasonMultiSelect : Migration
    {
        public override void Up()
        {
            Create.Table("SewerOverflowReasons")
                  .WithForeignKeyColumn("SewerOverflowID", "SewerOverflows", "SewerOverflowID").NotNullable()
                  .WithForeignKeyColumn("SewerOverflowCauseID", "SewerOverFlowCauses", "SewerOverflowCauseID")
                  .NotNullable();

            Execute.Sql(
                "INSERT INTO SewerOverflowReasons( SewerOverFlowID, SewerOverflowCauseID) SELECT SewerOverFlowId, SewerOverflowCauseID FROM SewerOverflows WHERE SewerOverflowCauseID is not null");
        }

        public override void Down()
        {
            Delete.Table("SewerOverflowReasons");
        }
    }
}
