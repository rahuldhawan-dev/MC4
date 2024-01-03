using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180727095204243), Tags("Production")]
    public class AddNY4OperatingCenterForMC545 : Migration
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
           ,[MarkoutsEditable]
    ,[TimeZoneId])
     SELECT
  'New York American Water Upstate' as CoInfo,
          NULL as CSNum, 
          NULL as FaxNum, 
           NULL as MailAdd, 
          'New York American Water' as MailCo, 
           NULL as MailCSZ, 
          'NY4' as OperatingCenterCode, 
          'Upstate' as OperatingCenterName, 
          NULL as ServContactNum, 
           1 as WorkOrdersEnabled, 
           'NY' as State, 
           NULL as Address, 
           NULL as City, 
         NULL as Zip, 
          0 as Authorized_Staffing_Union, 
           0 as Authorized_Staffing_Management, 
           0 as Authorized_Staffing_Total, 
           NULL as PermitsOMUserName, 
           NULL as PermitsCapitalUserName, 
           NULL as RSADivisionNumber, 
          2 as StateId, 
           1 as IsActive, 
         NULL as StateRegionId, 
           1 as HydrantInspectionFrequency, 
           4 as HydrantInspectionFrequencyUnitId, 
           5 as LargeValveInspectionFrequency, 
         4 as LargeValveInspectionFrequencyUnitId, 
           5 as SmallValveInspectionFrequency, 
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
           1 as MarkoutsEditable,
   1 as TimeZoneId");
        }

        public override void Down()
        {
            DeleteOperatingCenter("NY4");
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
