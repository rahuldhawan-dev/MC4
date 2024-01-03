using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160930113635463), Tags("Production")]
    public class AddInfoMasterColumnsToOperatingCentersForBug2823 : Migration
    {
        public override void Up()
        {
            Delete.Table("InfoMasterMaps");

            Alter.Table("OperatingCenters").AddColumn("InfoMasterMapId").AsFixedLengthString(32).Nullable();
            Alter.Table("OperatingCenters").AddColumn("InfoMasterMapLayerName").AsAnsiString(255).Nullable();

            Execute.Sql(
                "UPDATE OperatingCenters SET InfoMasterMapId = 'af174de08bba4ea6aa936c277411d868', InfoMasterMapLayerName = 'wPressurizedMain_NJ3_9417' WHERE OperatingCenterCode = 'NJ3';");
            Execute.Sql(
                "UPDATE OperatingCenters SET InfoMasterMapId = 'ab7db258fe734847bb5ebf41f6f09111', InfoMasterMapLayerName = 'IM_Comprehensive_Detail_6051' WHERE OperatingCenterCode = 'NJ4';");
            Execute.Sql(
                "UPDATE OperatingCenters SET InfoMasterMapId = 'ffd3b585cbe744f786b321d6ff05586b', InfoMasterMapLayerName = 'wPressurizedMain_SW_Complete_7441' WHERE OperatingCenterCode = 'NJ5';");
            Execute.Sql(
                "UPDATE OperatingCenters SET InfoMasterMapId = 'ab7db258fe734847bb5ebf41f6f09111', InfoMasterMapLayerName = 'IM_Comprehensive_Detail_6051' WHERE OperatingCenterCode = 'NJ7';");

            Execute.Sql(
                "UPDATE OperatingCenters SET InfoMasterMapId = '3b828a4a65b54f0ba31b4195219dec08', InfoMasterMapLayerName = 'InfoMaster_Central_3522' WHERE OperatingCenterCode = 'EW1';");
            Execute.Sql(
                "UPDATE OperatingCenters SET InfoMasterMapId = '3b828a4a65b54f0ba31b4195219dec08', InfoMasterMapLayerName = 'InfoMaster_Central_3522' WHERE OperatingCenterCode = 'EW2';");
            Execute.Sql(
                "UPDATE OperatingCenters SET InfoMasterMapId = '9be7aab8e1b048ebbb7f337fa885b5d0', InfoMasterMapLayerName = 'Infomaster_North_801' WHERE OperatingCenterCode = 'NJ6';");
            Execute.Sql(
                "UPDATE OperatingCenters SET InfoMasterMapId = '9be7aab8e1b048ebbb7f337fa885b5d0', InfoMasterMapLayerName = 'Infomaster_North_801' WHERE OperatingCenterCode = 'NJ8';");
        }

        public override void Down()
        {
            Delete.Column("InfoMasterMapId").FromTable("OperatingCenters");
            Delete.Column("InfoMasterMapLayerName").FromTable("OperatingCenters");

            Create.Table("InfoMasterMaps")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterID")
                  .WithColumn("MapId").AsFixedLengthString(32).NotNullable();
        }
    }
}
