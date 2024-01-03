using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131121155604), Tags("Production")]
    public class AddRSADivisionNumberToOperatingCenters : Migration
    {
        public override void Up()
        {
            Alter.Table("OperatingCenters").AddColumn("RSADivisionNumber").AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.Column("RSADivisionNumber").FromTable("OperatingCenters");
        }
    }
}
