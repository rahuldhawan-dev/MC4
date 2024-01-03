using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220228085859720), Tags("Production")]
    public class MC4111AddingClaimsCarrierIdColumnToIncidentsTable : Migration
    {
        public override void Up()
        {
            Alter.Table("Incidents").AddColumn("ClaimsCarrierId").AsString(255).Nullable();
        }

        public override void Down()
        {
            Delete.Column("ClaimsCarrierId").FromTable("Incidents");
        }
    }
}

