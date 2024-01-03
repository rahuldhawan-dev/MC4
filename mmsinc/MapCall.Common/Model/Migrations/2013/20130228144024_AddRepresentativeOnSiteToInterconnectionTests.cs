using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130228144024), Tags("Production")]
    public class AddRepresentativeOnSiteToInterconnectionTests : Migration
    {
        public override void Up()
        {
            Create.Column("RepresentativeOnSite").OnTable("InterconnectionTests").AsCustom("varchar(50)").Nullable();
        }

        public override void Down()
        {
            Delete.Column("RepresentativeOnSite").FromTable("InterconnectionTests");
        }
    }
}
