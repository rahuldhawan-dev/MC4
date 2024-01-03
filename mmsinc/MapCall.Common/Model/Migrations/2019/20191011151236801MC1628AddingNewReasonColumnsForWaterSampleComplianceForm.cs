using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20191011151236801)]
    [Tags("Production")]
    public class MC1628AddingNewReasonColumnsForWaterSampleComplianceForm : Migration
    {
        #region Exposed Methods

        public override void Up()
        {
            Alter.Table("WaterSampleComplianceForms")
                 .AddColumn("ContractedLabsSamplesReason").AsString(Int32.MaxValue).Nullable()
                 .AddColumn("CentralLabSamplesReason").AsString(Int32.MaxValue).Nullable()
                 .AddColumn("InternalLabSamplesReason").AsString(Int32.MaxValue).Nullable()
                 .AddColumn("BactiSamplesReason").AsString(Int32.MaxValue).Nullable()
                 .AddColumn("LeadAndCopperSamplesReason").AsString(Int32.MaxValue).Nullable()
                 .AddColumn("WQPSamplesReason").AsString(Int32.MaxValue).Nullable()
                 .AddColumn("SurfaceWaterPlantSamplesReason").AsString(Int32.MaxValue).Nullable()
                 .AddColumn("ChlorineResidualsReason").AsString(Int32.MaxValue).Nullable();
        }

        public override void Down()
        {
            Delete.Column("CentralLabSamplesReason").FromTable("WaterSampleComplianceForms");
            Delete.Column("InternalLabSamplesReason").FromTable("WaterSampleComplianceForms");
            Delete.Column("BactiSamplesReason").FromTable("WaterSampleComplianceForms");
            Delete.Column("LeadAndCopperSamplesReason").FromTable("WaterSampleComplianceForms");
            Delete.Column("WQPSamplesReason").FromTable("WaterSampleComplianceForms");
            Delete.Column("SurfaceWaterPlantSamplesReason").FromTable("WaterSampleComplianceForms");
            Delete.Column("ChlorineResidualsReason").FromTable("WaterSampleComplianceForms");
            Delete.Column("ContractedLabsSamplesReason").FromTable("WaterSampleComplianceForms");
        }

        #endregion
    }
}
