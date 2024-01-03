using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161027135657891), Tags("Production")]
    public class SAPStuffBug3293 : Migration
    {
        private const int VALVE_TYPE_BYPASS = 3,
                          VALVE_TYPE_GATE = 5;

        #region Private methods

        private void AddSAPColumns(string tableName, bool includeSAPNotificationNumber)
        {
            Alter.Table(tableName).AddColumn("SAPErrorCode").AsCustom("varchar(max)").Nullable();
            if (includeSAPNotificationNumber)
            {
                Alter.Table(tableName).AddColumn("SAPNotificationNumber").AsString(12).Nullable();
            }
        }

        private void RemoveSAPColumns(string tableName, bool includeSAPNotificationNumber)
        {
            Delete.Column("SAPErrorCode").FromTable(tableName);
            if (includeSAPNotificationNumber)
            {
                Delete.Column("SAPNotificationNumber").FromTable(tableName);
            }
        }

        #endregion

        public override void Up()
        {
            Alter.Table("OperatingCenters")
                 .AddColumn("SAPEnabled").AsBoolean().NotNullable().WithDefaultValue(false);

            // ValveType "BY-PASS" is being removed, any valves using it currently need to be set to "GATE".
            Update.Table("Valves").Set(new {ValveTypeId = VALVE_TYPE_GATE})
                  .Where(new {ValveTypeId = VALVE_TYPE_BYPASS});
            Delete.FromTable("ValveTypes").Row(new {Id = VALVE_TYPE_BYPASS});

            AddSAPColumns("Valves", false);
            AddSAPColumns("ValveInspections", true);
            AddSAPColumns("Hydrants", false);
            AddSAPColumns("HydrantInspections", true);
            AddSAPColumns("BlowoffInspections", true);
            AddSAPColumns("SewerMainCleanings", true);
            AddSAPColumns("WorkOrders", false);

            Alter.Table("WorkDescriptions").AddColumn("MaintenanceActivityType").AsString(3).Nullable();

            Alter.Table("Towns")
                 .AddColumn("MainSAPEquipmentId").AsInt32().Nullable()
                 .AddColumn("MainSAPFunctionalLocationId").AsInt32().Nullable()
                 .ForeignKey("FK_Towns_FunctionalLocations_MainSAPFunctionalLocationId", "FunctionalLocations",
                      "FunctionalLocationId");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Towns_FunctionalLocations_MainSAPFunctionalLocationId").OnTable("Towns");
            Delete.Column("MainSAPFunctionalLocationId").FromTable("Towns");
            Delete.Column("MainSAPEquipmentId").FromTable("Towns");

            Delete.Column("MaintenanceActivityType").FromTable("WorkDescriptions");

            RemoveSAPColumns("Valves", false);
            RemoveSAPColumns("ValveInspections", true);
            RemoveSAPColumns("Hydrants", false);
            RemoveSAPColumns("HydrantInspections", true);
            RemoveSAPColumns("BlowoffInspections", true);
            RemoveSAPColumns("SewerMainCleanings", true);
            RemoveSAPColumns("WorkOrders", false);

            // Add back BY-PASS ValveType with the correct id.
            this.EnableIdentityInsert("ValveTypes");
            Insert.IntoTable("ValveTypes").Row(new {Id = VALVE_TYPE_BYPASS, Description = "BY-PASS"});
            this.DisableIdentityInsert("ValveTypes");

            Delete.Column("SAPEnabled").FromTable("OperatingCenters");
        }
    }
}
