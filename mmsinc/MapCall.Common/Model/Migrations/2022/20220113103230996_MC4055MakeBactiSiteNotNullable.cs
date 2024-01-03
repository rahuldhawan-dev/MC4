using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220113103230996), Tags("Production")]
    public class MC4055MakeBactiSiteNotNullable : Migration
    {
        public override void Up()
        {
            Execute.Sql("update SampleSites set Bacti_Site = 0 where Bacti_Site is null;");
            Rename.Column("Bacti_Site").OnTable("SampleSites").To("BactiSite");
            Alter.Column("BactiSite").OnTable("SampleSites").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Rename.Column("BactiSite").OnTable("SampleSites").To("Bacti_Site");
            Alter.Column("Bacti_Site").OnTable("SampleSites").AsBoolean().Nullable();
        }
    }
}

