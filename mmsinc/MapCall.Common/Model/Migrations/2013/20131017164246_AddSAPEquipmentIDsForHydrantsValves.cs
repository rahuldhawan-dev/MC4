using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131017164246), Tags("Production")]
    public class AddSAPEquipmentIDsForHydrantsValves : Migration
    {
        #region Constants

        public struct Tables
        {
            public const string VALVES = "tblNJAWValves", HYDRANTS = "tblNJAWHydrant";
        }

        public struct Columns
        {
            public const string SAP_EQUIPMENT_ID = "SAPEquipmentID";
        }

        #endregion

        public override void Up()
        {
            Alter.Table(Tables.HYDRANTS).AddColumn(Columns.SAP_EQUIPMENT_ID).AsInt32().Nullable();
            Alter.Table(Tables.VALVES).AddColumn(Columns.SAP_EQUIPMENT_ID).AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.Column(Columns.SAP_EQUIPMENT_ID).FromTable(Tables.HYDRANTS);
            Delete.Column(Columns.SAP_EQUIPMENT_ID).FromTable(Tables.VALVES);
        }
    }
}
