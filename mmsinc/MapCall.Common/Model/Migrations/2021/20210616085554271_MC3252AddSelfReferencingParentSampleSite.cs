using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210616085554271), Tags("Production")]
    public class MC3252AddSelfReferencingParentSampleSite : Migration
    {
        public override void Up()
        {
            Alter.Table("SampleSites").AddForeignKeyColumn("ParentSampleSiteId", "SampleSites");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("SampleSites", "ParentSampleSiteId", "SampleSites");
        }
    }
}

