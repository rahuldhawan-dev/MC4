using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Data;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160613141435431), Tags("Production")]
    public class AddTablesEtcForBug2984 : Migration
    {
        public const int
            SERVICE_LINE_PROTECTION = 74;

        public const string
            MODULE_NAME = "Service Line Protection",
            DATA_TYPE = "Service Line Protection Investigation",
            TABLE_NAME = "ServiceLineProtectionInvestigations",
            DOCUMENT_TYPE = "Service Line Protection Investigation";

        public struct StringLengths
        {
            public const int
                CUSTOMER_NAME = 50,
                CUSTOMER_ADDRESS = 120,
                STREET_NUMBER = 10;
        }

        public override void Up()
        {
            #region Role/Module

            Execute.Sql(
                "declare @applicationId int;" +
                "INSERT INTO Applications Values('Service Line Protection');" +
                "select @applicationId = (select top 1 ApplicationId from Applications where Name = 'Service Line Protection');" +
                "set identity_insert Modules on;" +
                "INSERT INTO Modules(ModuleID, ApplicationID, Name) Values(" + SERVICE_LINE_PROTECTION +
                ", @applicationId, '" + MODULE_NAME + "');" +
                "set identity_insert Modules off;");
            Execute.Sql(
                "declare @dataTypeId int;" +
                "insert into [DataType] (Data_Type, Table_Name) values('" + DATA_TYPE + "', '" + TABLE_NAME + "');" +
                "set @dataTypeId = (select @@IDENTITY);" +
                "insert into [DocumentType] (Document_Type, DataTypeID) values('" + DOCUMENT_TYPE + "', @dataTypeId);");

            this.AddNotificationType("Service Line Protection", MODULE_NAME,
                "Service Line Protection Investigation Created");

            #endregion

            #region Tables

            this.CreateLookupTableWithValues("ServiceLineProtectionWorkTypes", "Repair", "Replace");
            this.CreateLookupTableWithValues("ServiceLineProtectionInvestigationStatuses", "Pending Review",
                "Renewal Required", "No Action Required", "Renewal Completed");

            Create.Table("ServiceLineProtectionInvestigations")
                  .WithIdentityColumn()
                  .WithColumn("CustomerName").AsAnsiString(StringLengths.CUSTOMER_NAME).NotNullable()
                  .WithColumn("StreetNumber").AsAnsiString(StringLengths.STREET_NUMBER).NotNullable()
                  .WithForeignKeyColumn("StreetId", "Streets", "StreetID", false)
                  .WithColumn("CustomerAddress2").AsAnsiString(StringLengths.CUSTOMER_ADDRESS).Nullable()
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterID", false)
                  .WithForeignKeyColumn("CustomerCity", "Towns", "TownId", false)
                  .WithColumn("CustomerZip").AsAnsiString(10).NotNullable()
                  .WithColumn("PremiseNumber").AsAnsiString(10).NotNullable()
                  .WithColumn("AccountNumber").AsAnsiString(20).Nullable()
                  .WithColumn("CustomerPhone").AsAnsiString(10).Nullable()
                  .WithColumn("DateInstalled").AsDateTime().Nullable()
                  .WithForeignKeyColumn("CustomerServiceLineMaterialId", "ServiceMaterials", "ServiceMaterialID", false)
                  .WithForeignKeyColumn("CustomerServiceLineSizeId", "ServiceSizes")
                  .WithForeignKeyColumn("ServiceLineProtectionWorkTypeId", "ServiceLineProtectionWorkTypes",
                       nullable: false)
                  .WithForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateID")
                  .WithForeignKeyColumn("ContractorId", "Contractors", "ContractorId", false)
                  .WithColumn("Notes").AsAnsiString(255).Nullable()
                  .WithForeignKeyColumn("ServiceId", "Services")
                  .WithForeignKeyColumn("CompanyServiceLineMaterialId", "ServiceMaterials", "ServiceMaterialID")
                  .WithForeignKeyColumn("CompanyServiceLineSizeId", "ServiceSizes")
                  .WithColumn("RenewalCompleted").AsDateTime().Nullable();

            #endregion
        }

        public override void Down()
        {
            #region Role/Module

            this.DeleteNotificationPurpose("Service Line Protection", MODULE_NAME,
                "Service Line Protection Investigation Created");
            Execute.Sql("Delete Roles where ModuleId = " + SERVICE_LINE_PROTECTION);
            Execute.Sql("Delete Modules where ModuleId = " + SERVICE_LINE_PROTECTION);
            this.DeleteDataType(DATA_TYPE);
            Execute.Sql("DELETE Applications where Name = 'Service Line Protection';");

            #endregion

            #region Table

            Delete.Table("ServiceLineProtectionInvestigations");
            Delete.Table("ServiceLineProtectionWorkTypes");
            Delete.Table("ServiceLineProtectionInvestigationStatuses");

            #endregion
        }
    }
}
