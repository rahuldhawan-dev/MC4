using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180418113546960), Tags("Production")]
    public class SampleSiteChangesMC251 : Migration
    {
        public override void Up()
        {
            Execute.Sql("INSERT INTO SampleSiteAvailability (Description) VALUES ('Seasonal-Summer')");
            Execute.Sql("INSERT INTO SampleSiteAvailability (Description) VALUES ('Seasonal-Winter')");
            Alter.Table("tblWQSample_Sites").AddColumn("PremiseNumber").AsString(10).Nullable();
            Execute.Sql("sp_rename 'tblWQSample_Sites.Address', 'StreetNumber', 'COLUMN'");
        }

        public override void Down()
        {
            Execute.Sql("DELETE FROM SampleSiteAvailability WHERE Description = 'Seasonal-Summer'");
            Execute.Sql("DELETE FROM SampleSiteAvailability WHERE Description = 'Seasonal-Winter'");
            Delete.Column("PremiseNumber").FromTable("tblWQSample_Sites");
            Execute.Sql("sp_rename 'tblWQSample_Sites.StreetNumber', 'Address', 'COLUMN'");
        }
    }
}
