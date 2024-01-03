using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200916123418791), Tags("Production")]
    public class NormalizeShortCycleWorkOrderSecurityThreatsForMC1803 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "UPDATE ShortCycleWorkOrderSecurityThreats SET PoliceEscort = CASE LTRIM(RTRIM(PoliceEscort)) WHEN 'No' THEN '0' WHEN 'Yes' THEN '1' ELSE NULL END;");

            Alter.Table("ShortCycleWorkOrderSecurityThreats").AlterColumn("PoliceEscort").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Alter.Table("ShortCycleWorkOrderSecurityThreats").AlterColumn("PoliceEscort").AsString(3).Nullable();

            Execute.Sql(
                "UPDATE ShortCycleWorkOrderSecurityThreats SET PoliceEscort = CASE LTRIM(RTRIM(PoliceEscort)) WHEN '0' THEN 'No' WHEN '1' THEN 'Yes' ELSE NULL END;");
        }
    }
}
