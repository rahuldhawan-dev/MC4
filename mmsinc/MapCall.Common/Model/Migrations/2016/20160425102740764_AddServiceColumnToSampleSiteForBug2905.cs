using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160425102740764), Tags("Production")]
    public class AddServiceColumnToSampleSiteForBug2905 : Migration
    {
        public struct TableNames
        {
            public const string SAMPLE_SITES = "tblWQSample_Sites", SERVICES = "Services", STREETS = "Streets";
        }

        public override void Up()
        {
            Alter.Table(TableNames.SAMPLE_SITES)
                 .AddForeignKeyColumn("ServiceId", TableNames.SERVICES).Nullable()
                 .AddForeignKeyColumn("StreetId", TableNames.STREETS, "StreetID").Nullable()
                 .AddColumn("LeadCopperSite").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn(TableNames.SAMPLE_SITES, "ServiceId", TableNames.SERVICES);
            Delete.ForeignKeyColumn(TableNames.SAMPLE_SITES, "StreetId", TableNames.STREETS, "StreetID");
            Delete.Column("LeadCopperSite").FromTable(TableNames.SAMPLE_SITES);
        }
    }
}
