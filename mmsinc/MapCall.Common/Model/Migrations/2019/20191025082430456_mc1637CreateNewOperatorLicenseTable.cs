using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20191025082430456), Tags("Production")]
    public class mc1637CreateNewOperatorLicenseTable : Migration
    {
        public override void Up()
        {
            // Lookup up table for the License types
            this.CreateLookupTableWithValues("OperatorLicenseTypes", "Water Distribution Licensed Operator",
                "Water Treatment Licensed Operator",
                "Sewer Collections Licensed Operator",
                "Sewer Treatment Licensed Operator",
                "Industrial Waste Treatment Licensed Operator");

            // Main table
            Create.Table("OperatorLicenses")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("EmployeeId", "tblEmployee", "tblEmployeeID", false)
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterId", false)
                  .WithForeignKeyColumn("StateId", "States", "StateId", false)
                  .WithForeignKeyColumn("OperatorLicenseTypeId", "OperatorLicenseTypes", "Id", false)
                  .WithColumn("LicenseLevel").AsAnsiString(4).NotNullable()
                  .WithColumn("LicenseNumber").AsAnsiString(12).NotNullable()
                  .WithColumn("ValidationDate").AsDateTime().NotNullable()
                  .WithColumn("ExpirationDate").AsDateTime().NotNullable();

            // Add document type
            this.AddDataType("OperatorLicenses");

            this.AddDocumentType("Operator License Document", "OperatorLicenses");
        }

        public override void Down()
        {
            this.RemoveDocumentTypeAndAllRelatedDocuments("Operator License Document", "OperatorLicenses");
            this.RemoveDataType("OperatorLicenses");

            Delete.ForeignKeyColumn("OperatorLicenses", "EmployeeId", "tblEmployee", "tblEmployeeID");
            Delete.ForeignKeyColumn("OperatorLicenses", "OperatingCenterId", "OperatingCenters", "OperatingCenterId");
            Delete.ForeignKeyColumn("OperatorLicenses", "StateId", "States", "StateId");
            Delete.ForeignKeyColumn("OperatorLicenses", "OperatorLicenseTypeId", "OperatorLicenseTypes", "Id");

            Delete.Table("OperatorLicenseTypes");
            Delete.Table("OperatorLicenses");
        }
    }
}
