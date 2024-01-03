using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160127111516367), Tags("Production")]
    public class MoveTownAbbreviationToTownOperatingCentersForBug2772Part2 : Migration
    {
        public override void Up()
        {
            Alter.Table("OperatingCentersTowns").AddColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable();
        }

        public override void Down()
        {
            Delete.Column("Id").FromTable("OperatingCentersTowns");
        }
    }
}
