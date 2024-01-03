using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170605095816645), Tags("Production")]
    public class AddBooleanFieldToSampleSitesForBug3760 : Migration
    {
        public override void Up()
        {
            Alter.Table("tblWQSample_Sites").AddColumn("CustomerParticipationEffortsExhausted").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("CustomerParticipationEffortsExhausted").FromTable("tblWQSample_Sites");
        }
    }
}
