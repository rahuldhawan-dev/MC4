using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160419142008601), Tags("Production")]
    public class CreateTableForBug2881 : Migration
    {
        public override void Up()
        {
            Create.Table("PDESSystems")
                  .WithColumn("Id").AsIdColumn()
                  .WithColumn("OperatingCenterId").AsInt32().NotNullable()
                  .ForeignKey("FK_PDESSystems_OperatingCenters_OperatingCenterId", "OperatingCenters",
                       "OperatingCenterId")
                  .WithColumn("WasteWaterSystemName").AsString(50)
                  .WithColumn("PDESPermitNumber")
                  .AsString(50) // This is supposed to match EnvironmentalPermit.PermitNumber
                  .WithColumn("GravityLength").AsInt32().NotNullable()
                  .WithColumn("ForceLength").AsInt32().NotNullable()
                  .WithColumn("NumberOfLiftStations").AsInt32().NotNullable()
                  .WithColumn("TreatmentDescription").AsString(255).NotNullable()
                  .WithColumn("NumberOfCustomers").AsInt32().NotNullable()
                  .WithColumn("PeakFlowMGD").AsInt32().NotNullable()
                  .WithColumn("IsCombinedSewerSystem").AsBoolean().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("PDESSystems");
        }
    }
}
