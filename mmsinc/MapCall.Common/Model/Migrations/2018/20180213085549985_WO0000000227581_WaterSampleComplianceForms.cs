using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180213085549985), Tags("Production")]
    public class WO0000000227581_WaterSampleCompliance : Migration
    {
        public override void Up()
        {
            Create.Table("WaterSampleComplianceForms")
                  .WithIdentityColumn("Id")
                  .WithColumn("PublicWaterSupplyId").AsInt32().NotNullable()
                  .ForeignKey("FK_WaterSampleComplianceForms_PublicWaterSupplies_PublicWaterSupplyId",
                       "PublicWaterSupplies", "Id")
                  .WithColumn("CertifiedByUserId").AsInt32().NotNullable()
                  .ForeignKey("FK_WaterSampleComplianceForms_tblPermissions_CertifiedByUserId", "tblPermissions",
                       "RecId")
                  .WithColumn("DateCertified").AsDateTime().NotNullable()
                  .WithColumn("CertifiedMonth").AsInt32().NotNullable()
                  .WithColumn("CertifiedYear").AsInt32().NotNullable()
                  .WithColumn("NoteText").AsCustom("ntext").Nullable()
                  .WithColumn("CentralLabSamplesHaveBeenCollectedAndReported").AsBoolean().Nullable()
                  .WithColumn("ContractedLabsSamplesHaveBeenCollectedAndReported").AsBoolean().Nullable()
                  .WithColumn("InternalLabsSamplesHaveBeenCollectedAndReported").AsBoolean().Nullable()
                  .WithColumn("BactiSamplesHaveBeenCollectedAndReported").AsBoolean().Nullable()
                  .WithColumn("LeadAndCopperSamplesHaveBeenCollectedAndReported").AsBoolean().Nullable()
                  .WithColumn("WQPSamplesHaveBeenCollectedAndReported").AsBoolean().Nullable()
                  .WithColumn("SurfaceWaterPlantSamplesHaveBeenCollectedAndReported").AsBoolean().Nullable()
                  .WithColumn("ChlorineResidualsHaveBeenCollectedAndReported").AsBoolean().Nullable();

            this.AddDataType("WaterSampleComplianceForms");
            this.AddDocumentType("General", "WaterSampleComplianceForms");
        }

        public override void Down()
        {
            this.RemoveDocumentTypeAndAllRelatedDocuments("General", "WaterSampleComplianceForms");
            this.RemoveDataType("WaterSampleComplianceForms");
            Delete.ForeignKey("FK_WaterSampleComplianceForms_PublicWaterSupplies_PublicWaterSupplyId")
                  .OnTable("WaterSampleComplianceForms");
            Delete.ForeignKey("FK_WaterSampleComplianceForms_tblPermissions_CertifiedByUserId")
                  .OnTable("WaterSampleComplianceForms");
            Delete.Table("WaterSampleComplianceForms");
        }
    }
}
