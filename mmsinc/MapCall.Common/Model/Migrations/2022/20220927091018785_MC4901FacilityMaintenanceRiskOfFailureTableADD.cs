using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221019164920160), Tags("Production")]
    public class MC4901FacilityMaintenanceRiskOfFailureTableADD : Migration
    {
        public struct TableNames
        {
            public const string FACILITY_TABLE = "tblFacilities",
                                FACILITYRISKSCORE_TABLE = "FacilityMaintenanceRiskOfFailures";
        }
        public override void Up()
        {
            Create.Table(TableNames.FACILITYRISKSCORE_TABLE)
                  .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn("RiskScore").AsInt32()
                  .WithColumn("Description").AsString();
            Insert.IntoTable(TableNames.FACILITYRISKSCORE_TABLE).Row(new { RiskScore = 1, Description = "1 - Low Risk Control" });
            Insert.IntoTable(TableNames.FACILITYRISKSCORE_TABLE).Row(new { RiskScore = 2, Description = "2 - Low-Moderate Risk" });
            Insert.IntoTable(TableNames.FACILITYRISKSCORE_TABLE).Row(new { RiskScore = 3, Description = "3 - Moderate Risk" });
            Insert.IntoTable(TableNames.FACILITYRISKSCORE_TABLE).Row(new { RiskScore = 4, Description = "4 - Moderate-High Risk" });
            Insert.IntoTable(TableNames.FACILITYRISKSCORE_TABLE).Row(new { RiskScore = 6, Description = "6 - High Risk" });
            Insert.IntoTable(TableNames.FACILITYRISKSCORE_TABLE).Row(new { RiskScore = 9, Description = "9 - High-Critical Risk" });
            Alter.Table(TableNames.FACILITY_TABLE).AddForeignKeyColumn("MaintenanceRiskOfFailureId", TableNames.FACILITYRISKSCORE_TABLE, "Id", true);
            Execute.Sql(@"UPDATE tblFacilities SET MaintenanceRiskOfFailureId = MaintenanceRiskOfFailure.Id 
                        FROM(SELECT Id, RiskScore FROM FacilityMaintenanceRiskOfFailures) 
                        AS MaintenanceRiskOfFailure WHERE MaintenanceRiskOfFailure.RiskScore = tblFacilities.RiskOfFailureScore");

            Delete.Column("RiskOfFailureScore").FromTable(TableNames.FACILITY_TABLE);
        }

        public override void Down()
        {
            Create.Column("RiskOfFailureScore").OnTable(TableNames.FACILITY_TABLE).AsInt32().Nullable();
            Execute.Sql(@"UPDATE tblFacilities SET RiskOfFailureScore = RiskOfFailureScore.RiskScore 
                        FROM(SELECT Id, RiskScore FROM FacilityMaintenanceRiskOfFailures)
                        AS RiskOfFailureScore WHERE RiskOfFailureScore.Id = tblFacilities.MaintenanceRiskOfFailureId");
            this.DeleteForeignKeyColumn(TableNames.FACILITY_TABLE, "MaintenanceRiskOfFailureId", TableNames.FACILITYRISKSCORE_TABLE);
            Delete.Table(TableNames.FACILITYRISKSCORE_TABLE);
        }
    }
}

