using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160209131716498), Tags("Production")]
    public class AddFieldForBug2791ToServices : Migration
    {
        public override void Up()
        {
            Alter.Table("tblNJAWService").AddColumn("GeoEFunctionalLocation").AsAnsiString(18).Nullable();
        }

        public override void Down()
        {
            Delete.Column("GeoEFunctionalLocation").FromTable("tblNJAWService");
        }
    }
}
