using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140304151957), Tags("Production")]
    public class AddContractorMaterialTableForBug1771 : Migration
    {
        public struct Tables
        {
            public const string CONTRACTORS = "Contractors",
                                MATERIALS = "Materials",
                                CONTRACTORS_MATERIALS = "ContractorsMaterials",
                                OPERATING_CENTERS = "OperatingCenters";
        }

        public struct Columns
        {
            public const string CONTRACTOR_ID = "ContractorID",
                                MATERIAL_ID = "MaterialID",
                                YEAR = "Year",
                                OPERATING_CENTER_ID = "OperatingCenterID",
                                COST = "Cost",
                                CONTRACTOR_MATERIAL_ID = "ContractorMaterialID";
        }

        public struct ForeignKeys
        {
            public const string FK_CONTRACTORS_MATERIALS_CONTRACTOS_CONTRACTOR_ID =
                                    "FK_ContractorsMaterials_Contractors_ContractorID",
                                FK_CONTRACTORS_MATERIALS_MATERIALS_MATERIAL_ID =
                                    "FK_ContractorsMaterials_Materials_MaterialID",
                                FK_CONTRACTORS_MATERIALS_OPERATING_CENTERS_OPERATING_CENTER_ID =
                                    "FK_ContractorsMaterials_OperatingCenters_OperatingCenterID";
        }

        public override void Up()
        {
            Create.Table(Tables.CONTRACTORS_MATERIALS)
                  .WithColumn(Columns.CONTRACTOR_MATERIAL_ID).AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn(Columns.CONTRACTOR_ID).AsInt32().NotNullable()
                  .WithColumn(Columns.MATERIAL_ID).AsInt32().NotNullable()
                  .WithColumn(Columns.OPERATING_CENTER_ID).AsInt32().NotNullable()
                  .WithColumn(Columns.YEAR).AsInt32().NotNullable()
                  .WithColumn(Columns.COST).AsCurrency().NotNullable();

            Create.ForeignKey(ForeignKeys.FK_CONTRACTORS_MATERIALS_CONTRACTOS_CONTRACTOR_ID)
                  .FromTable(Tables.CONTRACTORS_MATERIALS).ForeignColumn(Columns.CONTRACTOR_ID)
                  .ToTable(Tables.CONTRACTORS).PrimaryColumn(Columns.CONTRACTOR_ID);
            Create.ForeignKey(ForeignKeys.FK_CONTRACTORS_MATERIALS_MATERIALS_MATERIAL_ID)
                  .FromTable(Tables.CONTRACTORS_MATERIALS).ForeignColumn(Columns.MATERIAL_ID)
                  .ToTable(Tables.MATERIALS).PrimaryColumn(Columns.MATERIAL_ID);
            Create.ForeignKey(ForeignKeys.FK_CONTRACTORS_MATERIALS_OPERATING_CENTERS_OPERATING_CENTER_ID)
                  .FromTable(Tables.CONTRACTORS_MATERIALS).ForeignColumn(Columns.OPERATING_CENTER_ID)
                  .ToTable(Tables.OPERATING_CENTERS).PrimaryColumn(Columns.OPERATING_CENTER_ID);
        }

        public override void Down()
        {
            Delete.ForeignKey(ForeignKeys.FK_CONTRACTORS_MATERIALS_CONTRACTOS_CONTRACTOR_ID)
                  .OnTable(Tables.CONTRACTORS_MATERIALS);
            Delete.ForeignKey(ForeignKeys.FK_CONTRACTORS_MATERIALS_MATERIALS_MATERIAL_ID)
                  .OnTable(Tables.CONTRACTORS_MATERIALS);
            Delete.ForeignKey(ForeignKeys.FK_CONTRACTORS_MATERIALS_OPERATING_CENTERS_OPERATING_CENTER_ID)
                  .OnTable(Tables.CONTRACTORS_MATERIALS);

            Delete.Table(Tables.CONTRACTORS_MATERIALS);
        }
    }
}
