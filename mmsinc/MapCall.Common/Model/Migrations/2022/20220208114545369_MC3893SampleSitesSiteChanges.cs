using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220208114545369), Tags("Production")]
    public class MC3893SampleSitesSiteChanges : Migration
    {
        public override void Up()
        {
            Execute.Sql("update SampleSites set IsAlternateSite = 0 where IsAlternateSite is null;");
            Alter.Column("IsAlternateSite").OnTable("SampleSites").AsBoolean().NotNullable().WithDefaultValue(false);
            Delete.Column("CustomerParticipationEffortsExhausted").FromTable("SampleSites");
            Delete.Column("IsCustomerRequest").FromTable("SampleSites");
        }

        public override void Down()
        {
            Alter.Column("IsAlternateSite").OnTable("SampleSites").AsBoolean().Nullable();
            Create.Column("CustomerParticipationEffortsExhausted").OnTable("SampleSites").AsBoolean().Nullable();
            Create.Column("IsCustomerRequest").OnTable("SampleSites").AsBoolean().NotNullable().WithDefaultValue(false);
        }
    }
}

