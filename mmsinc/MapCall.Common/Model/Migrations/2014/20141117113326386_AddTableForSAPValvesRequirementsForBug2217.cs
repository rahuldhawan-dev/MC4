using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20141117113326386), Tags("Production")]
    public class AddTableForSAPValvesRequirementsForBug2217 : Migration
    {
        public const string TABLE_NAME = "ValveSAPRequirements";
        public const int REPORTED_BY = 50, FUNCTIONAL_LOCATION = 50;

        public override void Up()
        {
            Create.Table(TABLE_NAME)
                  .WithIdentityColumn()
                  .WithColumn("NotificationType").AsInt32().Nullable()
                  .WithColumn("Notification").AsInt32().NotNullable()
                  .WithColumn("NotificationDateTime").AsDateTime().Nullable()
                  .WithColumn("SAPEquipmentID").AsInt32().NotNullable()
                  .WithColumn("FunctionalLocation").AsAnsiString(FUNCTIONAL_LOCATION).Nullable()
                  .WithColumn("RequiredStartDateTime").AsDateTime().Nullable()
                  .WithColumn("RequiredEndDateTime").AsDateTime().Nullable()
                  .WithColumn("ReportedBy").AsAnsiString(REPORTED_BY).Nullable()
                  .WithColumn("CreatedAt").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table(TABLE_NAME);
        }
    }
}
