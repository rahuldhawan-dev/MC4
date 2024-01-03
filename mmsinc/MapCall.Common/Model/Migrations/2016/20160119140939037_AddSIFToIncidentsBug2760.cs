using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160119140939037), Tags("Production")]
    public class AddSIFToIncidentsBug2760 : Migration
    {
        public override void Up()
        {
            Create.Column("IsSeriousInjuryOrFatality").OnTable("Incidents")
                  .AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("IsSeriousInjuryOrFatality").FromTable("Incidents");
        }
    }
}
