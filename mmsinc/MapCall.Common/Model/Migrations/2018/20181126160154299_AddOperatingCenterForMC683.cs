using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20181126160154299), Tags("Production")]
    public class AddOperatingCenterForMC683 : Migration
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
'Missouri American Water Lawson' as CoInfo,
'816-580-3217' as CSNum, 
NULL as FaxNum, 
'103 S Pennsylvania Ave' as MailAdd, 
'Missouri American Water' as MailCo, 
'Lawson, MO 64062' as MailCSZ, 
'MO53' as OperatingCenterCode, 
'Lawson' as OperatingCenterName, 
'816-580-3217' as ServContactNum, 
1 as WorkOrdersEnabled, 
'MO' as State, 
'103 S Pennsylvania Ave' as Address, 
'Lawson' as City, 
'64062' as Zip, 
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
'8db238d0c031424ebefaa8456cad78b0' as MapId, 
1 as MarkoutsEditable,
1 as TimeZoneId");
        }

        public override void Down() { }
    }
}
