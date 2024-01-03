using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160830103356130), Tags("Production")]
    public class AlterTierClassificationsForBug3145 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "DROP INDEX [IX_LeadCopperTierClassifications_Description] ON [dbo].[LeadCopperTierClassifications]");
            Alter.Column("Description").OnTable("LeadCopperTierClassifications").AsAnsiString(100).NotNullable();
            Alter.Table("LeadCopperTierClassifications").AddColumn("Tier").AsAnsiString(12).Nullable();
            Execute.Sql(
                "UPDATE LeadCopperTierClassifications SET Tier =  'Tier 1',       Description = 'Lead Service' WHERE Id = 1");
            Execute.Sql(
                "UPDATE LeadCopperTierClassifications SET Tier =  'Tier 2',       Description = 'Building & Multifamily Residences with Copper Pipes & Lead Solder installed after 1982' WHERE Id = 2");
            Execute.Sql(
                "UPDATE LeadCopperTierClassifications SET Tier =  'Tier 3',       Description = 'Single Family Structures that contain Copper Pipe with Lead Solder installed before 1983' WHERE Id = 3");
            Execute.Sql(
                "UPDATE LeadCopperTierClassifications SET Tier =  'Tier 3-Other', Description = 'Other' WHERE Id = 4");
            Execute.Sql(
                "INSERT INTO LeadCopperTierClassifications VALUES('Single Family with Copper & Lead Solder installed after 1982','Tier 1');");
            Execute.Sql(
                "INSERT INTO LeadCopperTierClassifications VALUES('Building & Multifamily Residences containing Lead Pipe or Service Lines','Tier 2');");

            Execute.Sql(
                "Update tblWQSample_Sites SET LeadCopperTierClassificationID = 5 WHERE LeadCopperTierClassificationID = 1 AND LeadCopperTierSampleCategoryId = 2");
            //Execute.Sql("Update tblWQSample_Sites SET LeadCopperTierClassification = 6 WHERE LeadCopperTierClassification = 2 AND LeadCopperTierSampleCategoryId = 2");
        }

        public override void Down()
        {
            Execute.Sql("UPDATE LeadCopperTierClassifications SET Description = 'Tier 1' WHERE Id = 1");
            Execute.Sql("UPDATE LeadCopperTierClassifications SET Description = 'Tier 2' WHERE Id = 2");
            Execute.Sql("UPDATE LeadCopperTierClassifications SET Description = 'Tier 3' WHERE Id = 3");
            Execute.Sql("UPDATE LeadCopperTierClassifications SET Description = 'Other' WHERE Id = 4");
            Execute.Sql("UPDATE LeadCopperTierClassifications SET Description = 'Tier 1' WHERE Id = 5");
            Execute.Sql("UPDATE LeadCopperTierClassifications SET Description = 'Tier 2' WHERE Id = 6");
            Delete.Column("Tier").FromTable("LeadCopperTierClassifications");
        }
    }
}
