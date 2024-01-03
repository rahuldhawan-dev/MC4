using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200409111230449), Tags("Production")]
    public class MC2170RenameCovidIssueFields : Migration
    {
        public override void Up()
        {
            Rename.Column("QuarantineStartDate").OnTable("CovidIssues").To("StartDate");
            Rename.Column("QuarantineReleaseDate").OnTable("CovidIssues").To("ReleaseDate");
        }

        public override void Down()
        {
            Rename.Column("StartDate").OnTable("CovidIssues").To("QuarantineStartDate");
            Rename.Column("ReleaseDate").OnTable("CovidIssues").To("QuarantineReleaseDate");
        }
    }
}
