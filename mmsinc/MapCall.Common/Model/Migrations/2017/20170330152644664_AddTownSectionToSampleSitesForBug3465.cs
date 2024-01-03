using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170330152644664), Tags("Production")]
    public class AddTownSectionToSampleSitesForBug3465 : Migration
    {
        public override void Up()
        {
            Alter.Table("tblWQSample_Sites")
                 .AddForeignKeyColumn("TownSectionId", "TownSections", "TownSectionId");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("tblWQSample_Sites", "TownSectionId", "TownSections",
                "TownSectionId");
        }
    }
}
