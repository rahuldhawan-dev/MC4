using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190716121356356), Tags("Production")]
    public class AddArcFlashHazardAnalysisStudyPartyToFacilitiesForMC1432 : Migration
    {
        public override void Up()
        {
            Create.Column("ArcFlashHazardAnalysisStudyParty").OnTable("tblFacilities").AsString(50).Nullable();
        }

        public override void Down()
        {
            Delete.Column("ArcFlashHazardAnalysisStudyParty").FromTable("tblFacilities");
        }
    }
}
