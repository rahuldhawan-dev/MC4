using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180419105153786), Tags("Production")]
    public class AddUpdateOperatingCentersForMC365And362 : Migration
    {
        public override void Up()
        {
            Execute.Sql("update OperatingCenters set OperatingCenterCode = 'VA5' where OperatingCenterId = 77; " +
                        "update OperatingCenters set OperatingCenterCode = 'VA10' where OperatingCenterId = 76; " +
                        "update OperatingCenters set OperatingCenterCode = 'MD2' where OperatingCenterId = 59; " +
                        "update OperatingCenters set OperatingCenterCode = 'WV41' where OperatingCenterId = 78; " +
                        "update OperatingCenters set OperatingCenterCode = 'WV31' where OperatingCenterId = 79; " +
                        "update OperatingCenters set OperatingCenterCode = 'WV22' where OperatingCenterId = 82;");

            Execute.Sql(@"IF NOT Exists(SELECT 1 from OperatingCenters where OperatingCenterCOde = 'CA65')
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
			'California American Water' as CoInfo,
          NULL as CSNum, 
          NULL as FaxNum, 
           '2272 Meadowbrook Ave' as MailAdd, 
          'California American Water' as MailCo, 
           'Merced, CA 95348' as MailCSZ, 
          'CA65' as OperatingCenterCode, 
          'Meadowbrook' as OperatingCenterName, 
          NULL as ServContactNum, 
           1 as WorkOrdersEnabled, 
           'CA' as State, 
           '2272 Meadowbrook Ave' as Address, 
           'Merced' as City, 
         '95348' as Zip, 
          0 as Authorized_Staffing_Union, 
           0 as Authorized_Staffing_Management, 
           0 as Authorized_Staffing_Total, 
           NULL as PermitsOMUserName, 
           NULL as PermitsCapitalUserName, 
           NULL as RSADivisionNumber, 
          5 as StateId, 
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
		   '286278f441e04bf2b1c2ba0763e76227' as MapId, 
           1 as MarkoutsEditable");
        }

        public override void Down()
        {
            Execute.Sql("update OperatingCenters set OperatingCenterCode = 'VA1' where OperatingCenterId = 77; " +
                        "update OperatingCenters set OperatingCenterCode = 'VA2' where OperatingCenterId = 76; " +
                        "update OperatingCenters set OperatingCenterCode = 'MD1' where OperatingCenterId = 59; " +
                        "update OperatingCenters set OperatingCenterCode = 'WV1' where OperatingCenterId = 78; " +
                        "update OperatingCenters set OperatingCenterCode = 'WV2' where OperatingCenterId = 79; " +
                        "update OperatingCenters set OperatingCenterCode = 'WV5' where OperatingCenterId = 82;");
        }
    }
}
