using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140414152139), Tags("Production")]
    public class AddFrameworkOperatingCentersForContractorsBug1840 : Migration
    {
        public struct TableNames
        {
            public const string CONTRACTORS = "Contractors",
                                OPERATING_CENTERS = "OperatingCenters",
                                CONTRACTOR_FRAMEWORK_OPERATING_CENTERS = "ContractorsFrameworkOperatingCenters";
        }

        public struct ColumnNames
        {
            public const string CONTRACTOR_ID = "ContractorId",
                                OPERATING_CENTER_ID = "OperatingCenterId";
        }

        public override void Up()
        {
            Create.Table(TableNames.CONTRACTOR_FRAMEWORK_OPERATING_CENTERS)
                  .WithColumn(ColumnNames.CONTRACTOR_ID).AsInt32().NotNullable().ForeignKey(
                       "FK_ContractorsFrameworkOperatingCenters_Contractors_ContractorId", TableNames.CONTRACTORS,
                       ColumnNames.CONTRACTOR_ID)
                  .WithColumn(ColumnNames.OPERATING_CENTER_ID).AsInt32().NotNullable().ForeignKey(
                       "FK_ContractorsFrameworkOperatingCenters_OperatingCenters_OperatingCenterId",
                       TableNames.OPERATING_CENTERS, ColumnNames.OPERATING_CENTER_ID);
        }

        public override void Down()
        {
            Delete.Table(TableNames.CONTRACTOR_FRAMEWORK_OPERATING_CENTERS);
        }
    }
}
