using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140414111501), Tags("Production")]
    public class RemoveContractorMaterialTableForBug1771 : Migration
    {
        public override void Up()
        {
            Delete.ForeignKey(AddContractorMaterialTableForBug1771
                             .ForeignKeys.FK_CONTRACTORS_MATERIALS_CONTRACTOS_CONTRACTOR_ID)
                  .OnTable(AddContractorMaterialTableForBug1771.Tables.CONTRACTORS_MATERIALS);
            Delete.ForeignKey(AddContractorMaterialTableForBug1771
                             .ForeignKeys.FK_CONTRACTORS_MATERIALS_MATERIALS_MATERIAL_ID)
                  .OnTable(AddContractorMaterialTableForBug1771.Tables.CONTRACTORS_MATERIALS);
            Delete.ForeignKey(AddContractorMaterialTableForBug1771
                             .ForeignKeys.FK_CONTRACTORS_MATERIALS_OPERATING_CENTERS_OPERATING_CENTER_ID)
                  .OnTable(AddContractorMaterialTableForBug1771.Tables.CONTRACTORS_MATERIALS);

            Delete.Table(AddContractorMaterialTableForBug1771.Tables.CONTRACTORS_MATERIALS);
        }

        public override void Down()
        {
            Create.Table(AddContractorMaterialTableForBug1771.Tables.CONTRACTORS_MATERIALS)
                  .WithColumn(AddContractorMaterialTableForBug1771.Columns.CONTRACTOR_MATERIAL_ID).AsInt32()
                  .NotNullable().PrimaryKey().Identity()
                  .WithColumn(AddContractorMaterialTableForBug1771.Columns.CONTRACTOR_ID).AsInt32().NotNullable()
                  .WithColumn(AddContractorMaterialTableForBug1771.Columns.MATERIAL_ID).AsInt32().NotNullable()
                  .WithColumn(AddContractorMaterialTableForBug1771.Columns.OPERATING_CENTER_ID).AsInt32().NotNullable()
                  .WithColumn(AddContractorMaterialTableForBug1771.Columns.YEAR).AsInt32().NotNullable()
                  .WithColumn(AddContractorMaterialTableForBug1771.Columns.COST).AsCurrency().NotNullable();

            Create.ForeignKey(AddContractorMaterialTableForBug1771
                             .ForeignKeys.FK_CONTRACTORS_MATERIALS_CONTRACTOS_CONTRACTOR_ID)
                  .FromTable(AddContractorMaterialTableForBug1771.Tables.CONTRACTORS_MATERIALS)
                  .ForeignColumn(AddContractorMaterialTableForBug1771.Columns.CONTRACTOR_ID)
                  .ToTable(AddContractorMaterialTableForBug1771.Tables.CONTRACTORS)
                  .PrimaryColumn(AddContractorMaterialTableForBug1771.Columns.CONTRACTOR_ID);
            Create.ForeignKey(AddContractorMaterialTableForBug1771
                             .ForeignKeys.FK_CONTRACTORS_MATERIALS_MATERIALS_MATERIAL_ID)
                  .FromTable(AddContractorMaterialTableForBug1771.Tables.CONTRACTORS_MATERIALS)
                  .ForeignColumn(AddContractorMaterialTableForBug1771.Columns.MATERIAL_ID)
                  .ToTable(AddContractorMaterialTableForBug1771.Tables.MATERIALS)
                  .PrimaryColumn(AddContractorMaterialTableForBug1771.Columns.MATERIAL_ID);
            Create.ForeignKey(AddContractorMaterialTableForBug1771
                             .ForeignKeys.FK_CONTRACTORS_MATERIALS_OPERATING_CENTERS_OPERATING_CENTER_ID)
                  .FromTable(AddContractorMaterialTableForBug1771.Tables.CONTRACTORS_MATERIALS)
                  .ForeignColumn(AddContractorMaterialTableForBug1771.Columns.OPERATING_CENTER_ID)
                  .ToTable(AddContractorMaterialTableForBug1771.Tables.OPERATING_CENTERS)
                  .PrimaryColumn(AddContractorMaterialTableForBug1771.Columns.OPERATING_CENTER_ID);

            Create.UniqueConstraint("UQ_ContractorsMaterials").OnTable("ContractorsMaterials")
                  .Columns(new[] {"ContractorID", "MaterialID", "OperatingCenterID", "Year"});
        }
    }
}
