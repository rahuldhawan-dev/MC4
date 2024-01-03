using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200515105015013), Tags("Production")]
    public class MC2278MakeProductionWorkDescriptionOrderTypeNotNullable : Migration
    {
        public override void Up()
        {
            Alter.Column("OrderTypeId").OnTable("ProductionWorkDescriptions").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Alter.Column("OrderTypeId").OnTable("ProductionWorkDescriptions").AsInt32().Nullable();
        }
    }
}
