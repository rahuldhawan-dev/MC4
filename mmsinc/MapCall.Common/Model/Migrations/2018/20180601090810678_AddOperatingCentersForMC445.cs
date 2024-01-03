using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180601090810678), Tags("Production")]
    public class AddOperatingCentersForMC445 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                INSERT INTO [dbo].[OperatingCenters]
                   ([CoInfo]
                   ,[MailAdd]
                   ,[MailCo]
                   ,[MailCSZ]
                   ,[OperatingCenterCode]
                   ,[OperatingCenterName]
                   ,[WorkOrdersEnabled]
                   ,[State]
                   ,[Address]
                   ,[City]
                   ,[Zip]
                   ,[Authorized_Staffing_Union]
                   ,[Authorized_Staffing_Management]
                   ,[Authorized_Staffing_Total]
                   ,[StateId]
                   ,[IsActive]
                   ,[HydrantInspectionFrequency]
                   ,[HydrantInspectionFrequencyUnitId]
                   ,[LargeValveInspectionFrequency]
                   ,[LargeValveInspectionFrequencyUnitId]
                   ,[SmallValveInspectionFrequency]
                   ,[SmallValveInspectionFrequencyUnitId]
                   ,[HasWorkOrderInvoicing]
                   ,[SAPEnabled]
                   ,[IsContractedOperations]
                   ,[SAPWorkOrdersEnabled]
                   ,[UsesValveInspectionFrequency]
                   ,[MapId]
                   ,[MarkoutsEditable])
                select
                   'West Virginia American Water' as CoInfo, 
                   '1060 Riverside Dr' as MailAdd, 
                   'West Virginia American Water' as MailCo,
                   'Gassaway, WV 26624'as MailCSZ, 
                   'WV12' as OperatingCenterCode, 
                   'Gassaway' as OperatingCenterName, 
                   1 as WorkOrdersEnabled, 
                   'WV' as State, 
                   '1060 Riverside Dr' as Address, 
                   'Gassaway' as City, 
                   '26624' as Zip, 
                   0 as Authorized_Staffing_Union, 
                   0 as Authorized_Staffing_Management, 
                   0 as Authorized_Staffing_Total, 
                   15 as StateId, 
                   1 as IsActive, 
                   1 as HydrantInspectionFrequency, 
                   4 as HydrantInspectionFrequencyUnitId, 
                   2 as LargeValveInspectionFrequency, 
                   4 as LargeValveInspectionFrequencyUnitId, 
                   4 as SmallValveInspectionFrequency, 
                   4 as SmallValveInspectionFrequencyUnitId, 
                   0 as HasWorkOrderInvoicing, 
                   1 as SAPEnabled, 
                   0 as IsContractedOperations, 
                   1 as SAPWorkOrdersEnabled, 
                   1 as UsesValveInspectionFrequency, 
                   '3da4d0459e3746b1a95a43fe8f9f5daf' as MapId, 
                   1 as MarkoutsEditable

                INSERT INTO [dbo].[OperatingCenters]
                   ([CoInfo]
                   ,[MailCo]
                   ,[MailCSZ]
                   ,[OperatingCenterCode]
                   ,[OperatingCenterName]
                   ,[WorkOrdersEnabled]
                   ,[State]
                   ,[City]
                   ,[Zip]
                   ,[Authorized_Staffing_Union]
                   ,[Authorized_Staffing_Management]
                   ,[Authorized_Staffing_Total]
                   ,[StateId]
                   ,[IsActive]
                   ,[HydrantInspectionFrequency]
                   ,[HydrantInspectionFrequencyUnitId]
                   ,[LargeValveInspectionFrequency]
                   ,[LargeValveInspectionFrequencyUnitId]
                   ,[SmallValveInspectionFrequency]
                   ,[SmallValveInspectionFrequencyUnitId]
                   ,[HasWorkOrderInvoicing]
                   ,[SAPEnabled]
                   ,[IsContractedOperations]
                   ,[SAPWorkOrdersEnabled]
                   ,[UsesValveInspectionFrequency]
                   ,[MapId]
                   ,[MarkoutsEditable])
                 select
                   'West Virginia American Water' as CoInfo, 
                   'West Virginia American Water' as MailCo,
                   'Webster Springs, WV 26288'as MailCSZ, 
                   'WV14' as OperatingCenterCode, 
                   'Webster Springs' as OperatingCenterName, 
                   1 as WorkOrdersEnabled, 
                   'WV' as State, 
                   NULL as City, 
                   '26288' as Zip, 
                   0 as Authorized_Staffing_Union, 
                   0 as Authorized_Staffing_Management, 
                   0 as Authorized_Staffing_Total, 
                   15 as StateId, 
                   1 as IsActive, 
                   1 as HydrantInspectionFrequency, 
                   4 as HydrantInspectionFrequencyUnitId, 
                   2 as LargeValveInspectionFrequency, 
                   4 as LargeValveInspectionFrequencyUnitId, 
                   4 as SmallValveInspectionFrequency, 
                   4 as SmallValveInspectionFrequencyUnitId, 
                   0 as HasWorkOrderInvoicing, 
                   1 as SAPEnabled, 
                   0 as IsContractedOperations, 
                   1 as SAPWorkOrdersEnabled, 
                   1 as UsesValveInspectionFrequency, 
                   '3da4d0459e3746b1a95a43fe8f9f5daf' as MapId, 
                   1 as MarkoutsEditable

                INSERT INTO [dbo].[OperatingCenters]
                   ([CoInfo]
                   ,[MailCo]
                   ,[OperatingCenterCode]
                   ,[OperatingCenterName]
                   ,[WorkOrdersEnabled]
                   ,[State]
                   ,[Authorized_Staffing_Union]
                   ,[Authorized_Staffing_Management]
                   ,[Authorized_Staffing_Total]
                   ,[StateId]
                   ,[IsActive]
                   ,[HydrantInspectionFrequency]
                   ,[HydrantInspectionFrequencyUnitId]
                   ,[LargeValveInspectionFrequency]
                   ,[LargeValveInspectionFrequencyUnitId]
                   ,[SmallValveInspectionFrequency]
                   ,[SmallValveInspectionFrequencyUnitId]
                   ,[HasWorkOrderInvoicing]
                   ,[SAPEnabled]
                   ,[IsContractedOperations]
                   ,[SAPWorkOrdersEnabled]
                   ,[UsesValveInspectionFrequency]
                   ,[MapId]
                   ,[MarkoutsEditable])
                 select
                   'West Virginia American Water' as CoInfo, 
                   'West Virginia American Water' as MailCo,
                   'WV24' as OperatingCenterCode, 
                   'Bluestone' as OperatingCenterName, 
                   1 as WorkOrdersEnabled, 
                   'WV' as State, 
                   0 as Authorized_Staffing_Union, 
                   0 as Authorized_Staffing_Management, 
                   0 as Authorized_Staffing_Total, 
                   15 as StateId, 
                   1 as IsActive, 
                   1 as HydrantInspectionFrequency, 
                   4 as HydrantInspectionFrequencyUnitId, 
                   2 as LargeValveInspectionFrequency, 
                   4 as LargeValveInspectionFrequencyUnitId, 
                   4 as SmallValveInspectionFrequency, 
                   4 as SmallValveInspectionFrequencyUnitId, 
                   0 as HasWorkOrderInvoicing, 
                   1 as SAPEnabled, 
                   0 as IsContractedOperations, 
                   1 as SAPWorkOrdersEnabled, 
                   1 as UsesValveInspectionFrequency, 
                   '3da4d0459e3746b1a95a43fe8f9f5daf' as MapId, 
                   1 as MarkoutsEditable

                INSERT INTO [dbo].[OperatingCenters]
                   ([CoInfo]
                   ,[MailCo]
                   ,[MailCSZ]
                   ,[OperatingCenterCode]
                   ,[OperatingCenterName]
                   ,[WorkOrdersEnabled]
                   ,[State]
                   ,[City]
                   ,[Zip]
                   ,[Authorized_Staffing_Union]
                   ,[Authorized_Staffing_Management]
                   ,[Authorized_Staffing_Total]
                   ,[StateId]
                   ,[IsActive]
                   ,[HydrantInspectionFrequency]
                   ,[HydrantInspectionFrequencyUnitId]
                   ,[LargeValveInspectionFrequency]
                   ,[LargeValveInspectionFrequencyUnitId]
                   ,[SmallValveInspectionFrequency]
                   ,[SmallValveInspectionFrequencyUnitId]
                   ,[HasWorkOrderInvoicing]
                   ,[SAPEnabled]
                   ,[IsContractedOperations]
                   ,[SAPWorkOrdersEnabled]
                   ,[UsesValveInspectionFrequency]
                   ,[MapId]
                   ,[MarkoutsEditable])
                 select
                   'West Virginia American Water' as CoInfo, 
                   'West Virginia American Water' as MailCo,
                   'Montgomery, WV 25136'as MailCSZ, 
                   'WV25' as OperatingCenterCode, 
                   'Montgomery' as OperatingCenterName, 
                   1 as WorkOrdersEnabled, 
                   'WV' as State, 
                   'Montgomery' as City, 
                   '25136' as Zip, 
                   0 as Authorized_Staffing_Union, 
                   0 as Authorized_Staffing_Management, 
                   0 as Authorized_Staffing_Total, 
                   15 as StateId, 
                   1 as IsActive, 
                   1 as HydrantInspectionFrequency, 
                   4 as HydrantInspectionFrequencyUnitId, 
                   2 as LargeValveInspectionFrequency, 
                   4 as LargeValveInspectionFrequencyUnitId, 
                   4 as SmallValveInspectionFrequency, 
                   4 as SmallValveInspectionFrequencyUnitId, 
                   0 as HasWorkOrderInvoicing, 
                   1 as SAPEnabled, 
                   0 as IsContractedOperations, 
                   1 as SAPWorkOrdersEnabled, 
                   1 as UsesValveInspectionFrequency, 
                   '3da4d0459e3746b1a95a43fe8f9f5daf' as MapId, 
                   1 as MarkoutsEditable

                INSERT INTO [dbo].[OperatingCenters]
                   ([CoInfo]
                   ,[MailCo]
                   ,[OperatingCenterCode]
                   ,[OperatingCenterName]
                   ,[WorkOrdersEnabled]
                   ,[State]
                   ,[Authorized_Staffing_Union]
                   ,[Authorized_Staffing_Management]
                   ,[Authorized_Staffing_Total]
                   ,[StateId]
                   ,[IsActive]
                   ,[HydrantInspectionFrequency]
                   ,[HydrantInspectionFrequencyUnitId]
                   ,[LargeValveInspectionFrequency]
                   ,[LargeValveInspectionFrequencyUnitId]
                   ,[SmallValveInspectionFrequency]
                   ,[SmallValveInspectionFrequencyUnitId]
                   ,[HasWorkOrderInvoicing]
                   ,[SAPEnabled]
                   ,[IsContractedOperations]
                   ,[SAPWorkOrdersEnabled]
                   ,[UsesValveInspectionFrequency]
                   ,[MapId]
                   ,[MarkoutsEditable])
                 select
                   'West Virginia American Water' as CoInfo, 
                   'West Virginia American Water' as MailCo,
                   'WV46' as OperatingCenterCode, 
                   'South Central' as OperatingCenterName, 
                   1 as WorkOrdersEnabled, 
                   'WV' as State, 
                   0 as Authorized_Staffing_Union, 
                   0 as Authorized_Staffing_Management, 
                   0 as Authorized_Staffing_Total, 
                   15 as StateId, 
                   1 as IsActive, 
                   1 as HydrantInspectionFrequency, 
                   4 as HydrantInspectionFrequencyUnitId, 
                   2 as LargeValveInspectionFrequency, 
                   4 as LargeValveInspectionFrequencyUnitId, 
                   4 as SmallValveInspectionFrequency, 
                   4 as SmallValveInspectionFrequencyUnitId, 
                   0 as HasWorkOrderInvoicing, 
                   1 as SAPEnabled, 
                   0 as IsContractedOperations, 
                   1 as SAPWorkOrdersEnabled, 
                   1 as UsesValveInspectionFrequency, 
                   '3da4d0459e3746b1a95a43fe8f9f5daf' as MapId, 
                   1 as MarkoutsEditable

                 INSERT INTO [dbo].[OperatingCenters]
                   ([CoInfo]
                   ,[MailCo]
                   ,[MailCSZ]
                   ,[OperatingCenterCode]
                   ,[OperatingCenterName]
                   ,[WorkOrdersEnabled]
                   ,[State]
                   ,[City]
                   ,[Zip]
                   ,[Authorized_Staffing_Union]
                   ,[Authorized_Staffing_Management]
                   ,[Authorized_Staffing_Total]
                   ,[StateId]
                   ,[IsActive]
                   ,[HydrantInspectionFrequency]
                   ,[HydrantInspectionFrequencyUnitId]
                   ,[LargeValveInspectionFrequency]
                   ,[LargeValveInspectionFrequencyUnitId]
                   ,[SmallValveInspectionFrequency]
                   ,[SmallValveInspectionFrequencyUnitId]
                   ,[HasWorkOrderInvoicing]
                   ,[SAPEnabled]
                   ,[IsContractedOperations]
                   ,[SAPWorkOrdersEnabled]
                   ,[UsesValveInspectionFrequency]
                   ,[MapId]
                   ,[MarkoutsEditable])
                 select
                   'West Virginia American Water' as CoInfo, 
                   'West Virginia American Water' as MailCo,
                   'Salt Rock, WV 25559'as MailCSZ, 
                   'WV47' as OperatingCenterCode, 
                   'Salt Rock' as OperatingCenterName, 
                   1 as WorkOrdersEnabled, 
                   'WV' as State, 
                   'Salt Rock' as City, 
                   '25559' as Zip, 
                   0 as Authorized_Staffing_Union, 
                   0 as Authorized_Staffing_Management, 
                   0 as Authorized_Staffing_Total, 
                   15 as StateId, 
                   1 as IsActive, 
                   1 as HydrantInspectionFrequency, 
                   4 as HydrantInspectionFrequencyUnitId, 
                   2 as LargeValveInspectionFrequency, 
                   4 as LargeValveInspectionFrequencyUnitId, 
                   4 as SmallValveInspectionFrequency, 
                   4 as SmallValveInspectionFrequencyUnitId, 
                   0 as HasWorkOrderInvoicing, 
                   1 as SAPEnabled, 
                   0 as IsContractedOperations, 
                   1 as SAPWorkOrdersEnabled, 
                   1 as UsesValveInspectionFrequency, 
                   '3da4d0459e3746b1a95a43fe8f9f5daf' as MapId, 
                   1 as MarkoutsEditable");
        }

        public override void Down()
        {
            DeleteOperatingCenter("WV12");
            DeleteOperatingCenter("WV14");
            DeleteOperatingCenter("WV24");
            DeleteOperatingCenter("WV25");
            DeleteOperatingCenter("WV46");
            DeleteOperatingCenter("WV47");
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
