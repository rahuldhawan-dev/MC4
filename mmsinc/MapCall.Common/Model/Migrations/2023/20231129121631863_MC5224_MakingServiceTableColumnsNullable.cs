using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20231129121631863), Tags("Production")]
    public class MC5224_MakingServiceTableColumnsNullable : Migration
    {
        public override void Up()
        {
            Alter.Column("Agreement").OnTable("Services").AsBoolean().Nullable();
            Alter.Column("BureauOfSafeDrinkingWaterPermitRequired").OnTable("Services").AsBoolean().Nullable();
            Alter.Column("DeveloperServicesDriven").OnTable("Services").AsBoolean().Nullable();
            Alter.Column("MeterSettingRequirement").OnTable("Services").AsBoolean().Nullable();
        }

        // We don't need any alter statement in Down method as
        // it's going to blow if you try and turn the Nullable columns into Not Nullable
        // should even one case occur where there is a nullable entry
        public override void Down() { }
    }
}

