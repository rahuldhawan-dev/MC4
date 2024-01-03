using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170320102624910), Tags("Production")]
    public class Bug3680 : Migration
    {
        public override void Up()
        {
            Create.Column("UsesValveInspectionFrequency").OnTable("OperatingCenters")
                  .AsBoolean().NotNullable().WithDefaultValue(false);

            Update.Table("OperatingCenters")
                  .Set(new {
                       UsesValveInspectionFrequency = true, LargeValveInspectionFrequency = 5,
                       SmallValveInspectionFrequency = 5
                   }).Where(new {OperatingCenterCode = "SOV"});
            Update.Table("OperatingCenters")
                  .Set(new {
                       UsesValveInspectionFrequency = true, LargeValveInspectionFrequency = 5,
                       SmallValveInspectionFrequency = 5
                   }).Where(new {OperatingCenterCode = "PA61"});

            // There are a lto of SOV valves that are not currently set to 5 years so they need to be.
            Update.Table("Valves").Set(new {InspectionFrequency = 5, InspectionFrequencyUnitId = 4 /* years */})
                  .Where(new {OperatingCenterId = 93});
        }

        public override void Down()
        {
            Delete.Column("UsesValveInspectionFrequency").FromTable("OperatingCenters");
        }
    }
}
