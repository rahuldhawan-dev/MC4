using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160524150530000), Tags("Production")]
    public class CreateNearMissesTableForBug2943 : Migration
    {
        public const string TABLE_NAME = "NearMisses";

        public struct StringLengths
        {
            public const int INCIDENT_NUMBER = 15,
                             REPORTED_BY = 100,
                             SAFETY_NEAR_MISS = 100,
                             REGULATED = 100,
                             SEVERITY = 100;
        }

        public override void Up()
        {
            Create.Table(TABLE_NAME)
                  .WithIdentityColumn()
                  .WithColumn("IncidentNumber").AsString(StringLengths.INCIDENT_NUMBER).NotNullable()
                  .WithColumn("OccurredAt").AsDateTime().NotNullable()
                  .WithColumn("ReportedBy").AsString(StringLengths.REPORTED_BY).NotNullable()
                  .WithColumn("SafetyNearMiss").AsString(StringLengths.SAFETY_NEAR_MISS).NotNullable()
                  .WithColumn("Regulated").AsString(StringLengths.REGULATED).NotNullable()
                  .WithColumn("Severity").AsString(StringLengths.SEVERITY).NotNullable()
                  .WithColumn("Notes").AsCustom("Text").NotNullable()
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterId");
        }

        public override void Down()
        {
            Delete.Table(TABLE_NAME);
        }
    }
}
