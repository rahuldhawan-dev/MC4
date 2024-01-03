using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220407102238491), Tags("Production")]
    // ReSharper disable once InconsistentNaming
    public class MC4350SampleSiteAddCrossStreet : Migration
    {
        public override void Up()
        {
            Alter.Table("SampleSites").AddForeignKeyColumn("CrossStreetId", "Streets", "StreetId");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("SampleSites", "CrossStreetId", "Streets", "StreetId");
        }
    }
}

