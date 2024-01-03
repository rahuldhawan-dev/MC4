using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200409142345250), Tags("Production")]
    public class AddTableForMC2173 : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("ReleaseReasons", "Recovered", "Instructed by Doctor");
            Alter.Table("CovidIssues").AddColumn("EstimatedReleaseDate").AsDate().Nullable();
            Alter.Table("CovidIssues").AddForeignKeyColumn("ReleaseReasonId", "ReleaseReasons");
        }

        public override void Down()
        {
            Delete.Table("ReleaseReasons");
            Delete.Column("EstimatedReleaseDate").FromTable("CovidIssues");
            Delete.Column("ReleaseReasonId").FromTable("CovidIssues");
        }
    }
}
