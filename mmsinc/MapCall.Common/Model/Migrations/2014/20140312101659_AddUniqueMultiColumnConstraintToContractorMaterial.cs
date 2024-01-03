using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140312101659), Tags("Production")]
    public class AddUniqueMultiColumnConstraintToContractorMaterial : Migration
    {
        public override void Up()
        {
            Create.UniqueConstraint("UQ_ContractorsMaterials").OnTable("ContractorsMaterials")
                  .Columns(new[] {"ContractorID", "MaterialID", "OperatingCenterID", "Year"});
        }

        public override void Down()
        {
            Delete.UniqueConstraint("UQ_ContractorsMaterials").FromTable("ContractorsMaterials");
        }
    }
}
