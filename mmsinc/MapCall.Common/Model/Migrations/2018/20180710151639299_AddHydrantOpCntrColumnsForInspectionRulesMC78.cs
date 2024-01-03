using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180710151639299), Tags("Production")]
    public class AddHydrantOpCntrColumnsForInspectionRulesMC78 : Migration
    {
        public override void Up()
        {
            Alter.Table("OperatingCenters").AddColumn("ZoneStartYear").AsInt32().Nullable();
            Alter.Table("Hydrants").AddColumn("Zone").AsInt32().Nullable();
            Execute.Sql(
                "UPDATE [Hydrants] SET [Zone] = v.ValveZoneId FROM [Hydrants] H JOIN [Valves] V ON V.Id = H.LateralValveId");
        }

        public override void Down()
        {
            Delete.Column("ZoneStartYear").FromTable("OperatingCenters");
            Delete.Column("Zone").FromTable("Hydrant");
        }
    }
}
