using System.Threading;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140221160112), Tags("Production")]
    public class AddFunctionalLocationSAPEqIDToEquipmentForBug1740 : Migration
    {
        #region Constants

        public struct Tables
        {
            public const string EQUIPMENT = "Equipment",
                                FUNCTIONAL_LOCATIONS = "FunctionalLocations";
        }

        public struct Columns
        {
            public const string SAP_EQUIPMENT_ID = "SAPEquipmentID",
                                FUNCTIONAL_LOCATION_ID = "FunctionalLocationID";
        }

        public struct ForeignKeys
        {
            public const string FK_EQUIPMENT_FUNCTIONAL_LOCATION =
                "FK_Equipment_FunctionalLocations_FunctionalLocationID";
        }

        #endregion

        public override void Up()
        {
            Alter.Table(Tables.EQUIPMENT).AddColumn(Columns.SAP_EQUIPMENT_ID).AsInt32().Nullable();
            Alter.Table(Tables.EQUIPMENT).AddColumn(Columns.FUNCTIONAL_LOCATION_ID).AsInt32().Nullable();
            Create.ForeignKey(ForeignKeys.FK_EQUIPMENT_FUNCTIONAL_LOCATION)
                  .FromTable(Tables.EQUIPMENT).ForeignColumn(Columns.FUNCTIONAL_LOCATION_ID)
                  .ToTable(Tables.FUNCTIONAL_LOCATIONS).PrimaryColumn(Columns.FUNCTIONAL_LOCATION_ID);
        }

        public override void Down()
        {
            Delete.ForeignKey(ForeignKeys.FK_EQUIPMENT_FUNCTIONAL_LOCATION).OnTable(Tables.EQUIPMENT);
            Delete.Column(Columns.FUNCTIONAL_LOCATION_ID).FromTable(Tables.EQUIPMENT);

            Delete.Column(Columns.SAP_EQUIPMENT_ID).FromTable(Tables.EQUIPMENT);
        }
    }
}
