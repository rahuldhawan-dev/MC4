using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180309121742601), Tags("Production")]
    public class AddMOOperatingCenters : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"INSERT INTO [dbo].[OperatingCenters]
           ([CoInfo]
           ,[CSNum]
           ,[FaxNum]
           ,[MailAdd]
           ,[MailCo]
           ,[MailCSZ]
           ,[OperatingCenterCode]
           ,[OperatingCenterName]
           ,[ServContactNum]
           ,[WorkOrdersEnabled]
           ,[State]
           ,[Address]
           ,[City]
           ,[Zip]
           ,[Authorized_Staffing_Union]
           ,[Authorized_Staffing_Management]
           ,[Authorized_Staffing_Total]
           ,[PermitsOMUserName]
           ,[PermitsCapitalUserName]
           ,[RSADivisionNumber]
           ,[StateId]
           ,[IsActive]
           ,[StateRegionId]
           ,[HydrantInspectionFrequency]
           ,[HydrantInspectionFrequencyUnitId]
           ,[LargeValveInspectionFrequency]
           ,[LargeValveInspectionFrequencyUnitId]
           ,[SmallValveInspectionFrequency]
           ,[SmallValveInspectionFrequencyUnitId]
           ,[MaximumOverflowGallons]
           ,[DefaultServiceReplacementWBSNumberId]
           ,[InfoMasterMapId]
           ,[InfoMasterMapLayerName]
           ,[HasWorkOrderInvoicing]
           ,[SAPEnabled]
           ,[IsContractedOperations]
           ,[SAPWorkOrdersEnabled]
           ,[OperatedByOperatingCenterId]
           ,[UsesValveInspectionFrequency]
           ,[CoordinateId]
           ,[MapId]
           ,[MarkoutsEditable])
     SELECT
			'Missouri American Water Company' as CoInfo,
          NULL as CSNum, 
          NULL as FaxNum, 
           '329 S Small' as MailAdd, 
          'Missori American Water' as MailCo, 
           'Brunswick, MO 65236' as MailCSZ, 
          'MO8' as OperatingCenterCode, 
          'Brunswick' as OperatingCenterName, 
          NULL as ServContactNum, 
           1 as WorkOrdersEnabled, 
           'MO' as State, 
           '329 S Small' as Address, 
           'Brunswick' as City, 
         '65236' as Zip, 
          0 as Authorized_Staffing_Union, 
           0 as Authorized_Staffing_Management, 
           0 as Authorized_Staffing_Total, 
           NULL as PermitsOMUserName, 
           NULL as PermitsCapitalUserName, 
           NULL as RSADivisionNumber, 
          12 as StateId, 
           1 as IsActive, 
         NULL as StateRegionId, 
           1 as HydrantInspectionFrequency, 
           4 as HydrantInspectionFrequencyUnitId, 
           2 as LargeValveInspectionFrequency, 
         4 as LargeValveInspectionFrequencyUnitId, 
           4 as SmallValveInspectionFrequency, 
           4 as SmallValveInspectionFrequencyUnitId, 
           NULL as MaximumOverflowGallons, 
           NULL as DefaultServiceReplacementWBSNumberId, 
           NULL as InfoMasterMapId, 
           NULL as InfoMasterMapLayerName, 
           0 as HasWorkOrderInvoicing, 
           1 as SAPEnabled, 
           0 as IsContractedOperations, 
           1 as SAPWorkOrdersEnabled, 
           NULL as OperatedByOperatingCenterId, 
           1 as UsesValveInspectionFrequency, 
           NULL as CoordinateId, 
		   '3da4d0459e3746b1a95a43fe8f9f5daf' as MapId, 
           1 as MarkoutsEditable");
            Execute.Sql(@"INSERT INTO [dbo].[OperatingCenters]
           ([CoInfo]
           ,[CSNum]
           ,[FaxNum]
           ,[MailAdd]
           ,[MailCo]
           ,[MailCSZ]
           ,[OperatingCenterCode]
           ,[OperatingCenterName]
           ,[ServContactNum]
           ,[WorkOrdersEnabled]
           ,[State]
           ,[Address]
           ,[City]
           ,[Zip]
           ,[Authorized_Staffing_Union]
           ,[Authorized_Staffing_Management]
           ,[Authorized_Staffing_Total]
           ,[PermitsOMUserName]
           ,[PermitsCapitalUserName]
           ,[RSADivisionNumber]
           ,[StateId]
           ,[IsActive]
           ,[StateRegionId]
           ,[HydrantInspectionFrequency]
           ,[HydrantInspectionFrequencyUnitId]
           ,[LargeValveInspectionFrequency]
           ,[LargeValveInspectionFrequencyUnitId]
           ,[SmallValveInspectionFrequency]
           ,[SmallValveInspectionFrequencyUnitId]
           ,[MaximumOverflowGallons]
           ,[DefaultServiceReplacementWBSNumberId]
           ,[InfoMasterMapId]
           ,[InfoMasterMapLayerName]
           ,[HasWorkOrderInvoicing]
           ,[SAPEnabled]
           ,[IsContractedOperations]
           ,[SAPWorkOrdersEnabled]
           ,[OperatedByOperatingCenterId]
           ,[UsesValveInspectionFrequency]
           ,[CoordinateId]
           ,[MapId]
           ,[MarkoutsEditable])
     SELECT
			'Missouri American Water Company' as CoInfo,
          NULL as CSNum, 
          NULL as FaxNum, 
           '5402 Bus 50 W # 3' as MailAdd, 
          'Missori American Water' as MailCo, 
           'Jefferson City, MO 65109' as MailCSZ, 
          'MO12' as OperatingCenterCode, 
          'Jefferson City' as OperatingCenterName, 
          NULL as ServContactNum, 
           1 as WorkOrdersEnabled, 
           'MO' as State, 
           '5402 Bus 50 W # 3' as Address, 
           'Jefferson City' as City, 
         '65109' as Zip, 
          0 as Authorized_Staffing_Union, 
           0 as Authorized_Staffing_Management, 
           0 as Authorized_Staffing_Total, 
           NULL as PermitsOMUserName, 
           NULL as PermitsCapitalUserName, 
           NULL as RSADivisionNumber, 
          12 as StateId, 
           1 as IsActive, 
         NULL as StateRegionId, 
           1 as HydrantInspectionFrequency, 
           4 as HydrantInspectionFrequencyUnitId, 
           2 as LargeValveInspectionFrequency, 
         4 as LargeValveInspectionFrequencyUnitId, 
           4 as SmallValveInspectionFrequency, 
           4 as SmallValveInspectionFrequencyUnitId, 
           NULL as MaximumOverflowGallons, 
           NULL as DefaultServiceReplacementWBSNumberId, 
           NULL as InfoMasterMapId, 
           NULL as InfoMasterMapLayerName, 
           0 as HasWorkOrderInvoicing, 
           1 as SAPEnabled, 
           0 as IsContractedOperations, 
           1 as SAPWorkOrdersEnabled, 
           NULL as OperatedByOperatingCenterId, 
           1 as UsesValveInspectionFrequency, 
           NULL as CoordinateId, 
		   '3da4d0459e3746b1a95a43fe8f9f5daf' as MapId, 
           1 as MarkoutsEditable");
        }

        public override void Down()
        {
            DeleteOperatingCenter("MO8");
            DeleteOperatingCenter("MO12");
        }

        private void DeleteOperatingCenter(string opCode)
        {
            Execute.Sql($@"DECLARE @deleteId int;
SELECT @deleteId = OperatingCenterId FROM OperatingCenters where OperatingCenterCode = '{opCode}';
DELETE FROM OperatingCenterAssetTypes where OperatingCenterId = @deleteId;
DELETE FROM OperatingCenters where OperatingCenterId = @deleteId;");
        }
    }
}
