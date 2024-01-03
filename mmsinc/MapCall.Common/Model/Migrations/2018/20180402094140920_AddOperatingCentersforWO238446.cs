using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180402094140920), Tags("Production")]
    public class AddOperatingCentersforWO238446 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
INSERT INTO [dbo].[OperatingCenters]
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
			'Kentucky American Water Company' as CoInfo,
          NULL as CSNum, 
          NULL as FaxNum, 
          NULL as MailAdd, 
          NULL as MailCo, 
          NULL as MailCSZ, 
          'KY30' as OperatingCenterCode, 
          'Northern' as OperatingCenterName, 
          NULL as ServContactNum, 
           1 as WorkOrdersEnabled, 
           'KY' as State, 
          NULL as Address, 
          NULL as City, 
         NULL as Zip, 
          0 as Authorized_Staffing_Union, 
           0 as Authorized_Staffing_Management, 
           0 as Authorized_Staffing_Total, 
           NULL as PermitsOMUserName, 
           NULL as PermitsCapitalUserName, 
           NULL as RSADivisionNumber, 
          10 as StateId, 
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
		   'cd0fd13e50d3475890fc57373e3663cc' as MapId, 
           1 as MarkoutsEditable;

INSERT INTO [dbo].[OperatingCenters]
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
			'Kentucky American Water Company' as CoInfo,
          NULL as CSNum, 
          NULL as FaxNum, 
          NULL as MailAdd, 
          NULL as MailCo, 
          NULL as MailCSZ, 
          'KY3' as OperatingCenterCode, 
          'Southern' as OperatingCenterName, 
          NULL as ServContactNum, 
           1 as WorkOrdersEnabled, 
           'KY' as State, 
          NULL as Address, 
          NULL as City, 
         NULL as Zip, 
          0 as Authorized_Staffing_Union, 
           0 as Authorized_Staffing_Management, 
           0 as Authorized_Staffing_Total, 
           NULL as PermitsOMUserName, 
           NULL as PermitsCapitalUserName, 
           NULL as RSADivisionNumber, 
          10 as StateId, 
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
		   'cd0fd13e50d3475890fc57373e3663cc' as MapId, 
           1 as MarkoutsEditable
GO

");
        }

        public override void Down()
        {
            DeleteOperatingCenter("KY30");
            DeleteOperatingCenter("KY3");
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
