using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230726140838952), Tags("Production")]
    public class MC5718_AddBusinessUnitToNonRevenueWaterAdjustments : AutoReversingMigration
    {
        public override void Up() =>
            Create.Column("BusinessUnit")
                  .OnTable("NonRevenueWaterAdjustments")
                  .AsString(6)
                  .NotNullable().WithDefaultValue("999999");
    }
}

