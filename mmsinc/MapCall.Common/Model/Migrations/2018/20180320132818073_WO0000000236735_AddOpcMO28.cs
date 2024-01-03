using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180320132818073), Tags("Production")]
    public class WO0000000236735 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "INSERT INTO OperatingCenters(CoInfo, MailAdd, MailCo, MailCSZ, OperatingCenterCode, OperatingCenterName, WorkOrdersEnabled, State, Address, " +
                "City, Zip, Authorized_Staffing_Union, Authorized_Staffing_Management, Authorized_Staffing_Total, StateId, IsActive, HydrantInspectionFrequency, " +
                "HydrantInspectionFrequencyUnitId, LargeValveInspectionFrequency, LargeValveInspectionFrequencyUnitId, SmallValveInspectionFrequency, SmallValveInspectionFrequencyUnitId, " +
                "HasWorkOrderInvoicing, SAPEnabled, IsContractedOperations, SAPWorkOrdersEnabled, UsesValveInspectionFrequency, MapId, MarkoutsEditable)" +
                "VALUES('Missouri American Water Company', '2802 Brookview Ave', 'Missouri American Water', 'Sedalia', 'MO28', 'Maplewood', " +
                "1,	'MO', '2802 Brookview Ave', 'Sedalia', 65301, 0, 0, 0, 12, 1, 1, 4, 2, 4, 4, 4, 0, 1, 0, 1, 1, '8db238d0c031424ebefaa8456cad78b0', 1)");
        }

        public override void Down()
        {
            Execute.Sql("Delete from OperatingCenters where OperatingCenterCode = 'MO28'");
        }
    }
}
