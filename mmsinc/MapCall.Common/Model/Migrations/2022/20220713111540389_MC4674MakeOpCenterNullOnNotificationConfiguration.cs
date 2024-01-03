using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220713111540389), Tags("Production")]
    public class MC4674MakeOpCenterNullOnNotificationConfiguration : Migration
    {
        public override void Up()
        {
            Alter.Column("OperatingCenterId")
                 .OnTable("NotificationConfigurations")
                 .AsInt32().Nullable();
        }

        public override void Down()
        {
            // no rollback. Data loss would occur.
        }
    }
}

