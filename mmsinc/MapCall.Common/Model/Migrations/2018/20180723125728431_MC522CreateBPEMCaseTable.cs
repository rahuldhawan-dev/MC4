using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180723125728431), Tags("Production")]
    public class MC522CreateBPEMCaseTable : Migration
    {
        public override void Up()
        {
            Create.Table("BusinessProcessExceptionManagementCases")
                  .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
                  .WithColumn("CaseCategory").AsString(4).NotNullable()
                  .WithColumn("CasePriority").AsString(1).Nullable()
                  .WithColumn("AuthorizationGroup").AsString(4).Nullable()
                  .WithColumn("ObjectType").AsString(10).NotNullable()
                  .WithColumn("ObjectKey").AsString(70).NotNullable()
                  .WithColumn("OriginalDateOfClarificationCase").AsString(10).Nullable()
                  .WithColumn("CreationTimeOfClarificationCase").AsString(8).Nullable()
                  .WithColumn("LogicalSystem").AsString(10).Nullable()
                  .WithColumn("CompanyCode").AsString(4).Nullable()
                  .WithColumn("BusinessPartnerNumber").AsString(10).Nullable()
                  .WithColumn("ContractAccountNumber").AsString(12).Nullable()
                  .WithColumn("Premise").AsString(10).Nullable()
                  .WithColumn("ReceivedAt").AsDateTime().Nullable()
                  .WithColumn("SAPErrorCode").AsCustom("ntext").Nullable()
                  .WithColumn("SAPCommunicationError").AsBoolean().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("BusinessProcessExceptionManagementCases");
        }
    }
}
