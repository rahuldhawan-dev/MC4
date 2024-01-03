using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20181016131031122), Tags("Production")]
    public class RemoveFieldReqForRecurringProjectMainsMC686 : Migration
    {
        public override void Up()
        {
            Alter.Table("RecurringProjectMains").AlterColumn("LikelyhoodOfFailure").AsDecimal(18, 2).Nullable()
                 .AlterColumn("ConsequenceOfFailure").AsDecimal(18, 2).Nullable();
            Execute.Sql(
                "update Operatingcenters set infomasterMapId = '643046c09fcc40e58586130b7105da51', InfoMasterMapLayerName='NJ_InfoMasterRisk2018_NJDBs_8895' where OperatingCenterCode = 'EW1';");
            Execute.Sql(
                "update Operatingcenters set infomasterMapId = '157b8ec6683e476fbf075b2aa7349e34', InfoMasterMapLayerName = 'NJ_InfoMasterRisk2018_NJDBs_8895' where OperatingCenterCode = 'EW2';");
            Execute.Sql(
                "update Operatingcenters set infomasterMapId = '7f8155e1d499485ba470c09572542bd1', InfoMasterMapLayerName = 'NJ_InfoMasterRisk2018_NJDBs_8895' where OperatingCenterCode = 'NJ3';");
            Execute.Sql(
                "update Operatingcenters set infomasterMapId = 'b81175e6b39443c58369528d45c4147e', InfoMasterMapLayerName = 'NJ_InfoMasterRisk2018_NJDBs_8895' where OperatingCenterCode = 'NJ4';");
            Execute.Sql(
                "update Operatingcenters set infomasterMapId = '2c219f224b2d4045aa0c1c353083c672', InfoMasterMapLayerName = 'NJ_InfoMasterRisk2018_NJDBs_8895' where OperatingCenterCode = 'NJ5';");
            Execute.Sql(
                "update Operatingcenters set infomasterMapId = '7f324be5668147b0aa98487344ec2799', InfoMasterMapLayerName = 'NJ_InfoMasterRisk2018_NJDBs_8895' where OperatingCenterCode = 'NJ6';");
            Execute.Sql(
                "update Operatingcenters set infomasterMapId = 'ca9097883a594a9bb9a602d75638a46d', InfoMasterMapLayerName = 'NJ_InfoMasterRisk2018_NJDBs_8895' where OperatingCenterCode = 'NJ7';");
            Execute.Sql(
                "update Operatingcenters set infomasterMapId = 'e789d3ee1cf649afa47d8f5305f93f1e', InfoMasterMapLayerName = 'NJ_InfoMasterRisk2018_NJDBs_8895' where OperatingCenterCode = 'NJ8';");
        }

        public override void Down() { }
    }
}
