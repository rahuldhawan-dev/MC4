using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220722015443683), Tags("Production")]
    public class MC3000CreatingAwiaCompliance : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("AwiaComplianceCertificationTypes", "Emergency Response Plan", "Risk and Resilience Assessment");
            Create.Table("AwiaCompliances")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("StateId", "States", "stateID", false)
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterID", false)
                  .WithForeignKeyColumn("CertificationTypeId", "AwiaComplianceCertificationTypes", "Id", false)
                  .WithForeignKeyColumn("EnteredById", "tblPermissions", "RecID", false)
                  .WithForeignKeyColumn("CertifiedById", "tblPermissions", "RecID", false)
                  .WithColumn("DateSubmitted").AsDateTime2().NotNullable()
                  .WithColumn("DateAccepted").AsDateTime2().NotNullable()
                  .WithColumn("RecertificationDue").AsDateTime2().NotNullable();

            Create.Table("AwiaCompliancePublicWaterSupplies")
                  .WithColumn("AwiaComplianceId").AsInt32().NotNullable()
                  .ForeignKey("FK_AwiaCompliancePublicWaterSupplies_AwiaCompliances_AwiaComplianceId", "AwiaCompliances", "Id")
                  .WithColumn("PublicWaterSupplyId").AsInt32().NotNullable()
                  .ForeignKey("FK_AwiaCompliancePublicWaterSupplies_PublicWaterSupplies_PublicWaterSupplyId", "PublicWaterSupplies", "Id");

            this.AddDataType("AwiaCompliances");
            this.AddDocumentType("Copy of the receipt of the USEPA", "AwiaCompliances");
            this.AddDocumentType("Other", "AwiaCompliances");
        }

        public override void Down()
        {
            this.RemoveDocumentTypeAndAllRelatedDocuments("Copy of the receipt of the USEPA", "AwiaCompliances");
            this.RemoveDocumentTypeAndAllRelatedDocuments("Other", "AwiaCompliances");
            this.RemoveDataType("AwiaCompliances");
            Delete.Table("AwiaCompliancePublicWaterSupplies");
            Delete.Table("AwiaCompliances");
            Delete.Table("AwiaComplianceCertificationTypes");
        }
    }
}

