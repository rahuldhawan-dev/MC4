using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20141114160701572), Tags("Production")]
    public class AddCanceledToTrainingRecordsForBug2189 : Migration
    {
        #region Constants

        public const string SQL_CLEANUP_MISSING_NOTIFICATION_PURPOSES = @"SET IDENTITY_INSERT NotificationPurposes ON
            IF NOT EXISTS (SELECT 1 FROM NotificationPurposes WHERE NotificationPurposeID = 9) INSERT INTO NotificationPurposes(NotificationPurposeID, ModuleID, Purpose) VALUES(9, 34,'Supervisor Approval')
            IF NOT EXISTS (SELECT 1 FROM NotificationPurposes WHERE NotificationPurposeID = 10) INSERT INTO NotificationPurposes(NotificationPurposeID, ModuleID, Purpose) VALUES(10, 34,'Main Break Entered')
            IF NOT EXISTS (SELECT 1 FROM NotificationPurposes WHERE NotificationPurposeID = 11) INSERT INTO NotificationPurposes(NotificationPurposeID, ModuleID, Purpose) VALUES(11, 6,'Valve')
            IF NOT EXISTS (SELECT 1 FROM NotificationPurposes WHERE NotificationPurposeID = 12) INSERT INTO NotificationPurposes(NotificationPurposeID, ModuleID, Purpose) VALUES(12, 34,'Service Line Renewal Entered')
            IF NOT EXISTS (SELECT 1 FROM NotificationPurposes WHERE NotificationPurposeID = 13) INSERT INTO NotificationPurposes(NotificationPurposeID, ModuleID, Purpose) VALUES(13, 34,'Service Line Installation Entered')
            IF NOT EXISTS (SELECT 1 FROM NotificationPurposes WHERE NotificationPurposeID = 14) INSERT INTO NotificationPurposes(NotificationPurposeID, ModuleID, Purpose) VALUES(14, 55,'AsBuiltImage Coordinate Changed')
            IF NOT EXISTS (SELECT 1 FROM NotificationPurposes WHERE NotificationPurposeID = 15) INSERT INTO NotificationPurposes(NotificationPurposeID, ModuleID, Purpose) VALUES(15, 60,'ProjectsRP New Record')
            IF NOT EXISTS (SELECT 1 FROM NotificationPurposes WHERE NotificationPurposeID = 16) INSERT INTO NotificationPurposes(NotificationPurposeID, ModuleID, Purpose) VALUES(16, 34,'Sewer Overflow Entered')
            IF NOT EXISTS (SELECT 1 FROM NotificationPurposes WHERE NotificationPurposeID = 17) INSERT INTO NotificationPurposes(NotificationPurposeID, ModuleID, Purpose) VALUES(17, 60,'ProjectsRP Completed Record')
            IF NOT EXISTS (SELECT 1 FROM NotificationPurposes WHERE NotificationPurposeID = 18) INSERT INTO NotificationPurposes(NotificationPurposeID, ModuleID, Purpose) VALUES(18, 34,'Job Site Check List')
            IF NOT EXISTS (SELECT 1 FROM NotificationPurposes WHERE NotificationPurposeID = 19) INSERT INTO NotificationPurposes(NotificationPurposeID, ModuleID, Purpose) VALUES(19, 34,'Markout Damage')
            IF NOT EXISTS (SELECT 1 FROM NotificationPurposes WHERE NotificationPurposeID = 20) INSERT INTO NotificationPurposes(NotificationPurposeID, ModuleID, Purpose) VALUES(20, 34,'Equipment Repair')
            IF NOT EXISTS (SELECT 1 FROM NotificationPurposes WHERE NotificationPurposeID = 21) INSERT INTO NotificationPurposes(NotificationPurposeID, ModuleID, Purpose) VALUES(21, 29,'Equipment Record Created')
            IF NOT EXISTS (SELECT 1 FROM NotificationPurposes WHERE NotificationPurposeID = 22) INSERT INTO NotificationPurposes(NotificationPurposeID, ModuleID, Purpose) VALUES(22, 35,'General Liability Claim')
            IF NOT EXISTS (SELECT 1 FROM NotificationPurposes WHERE NotificationPurposeID = 23) INSERT INTO NotificationPurposes(NotificationPurposeID, ModuleID, Purpose) VALUES(23, 65,'Bapp Team Idea')
            IF NOT EXISTS (SELECT 1 FROM NotificationPurposes WHERE NotificationPurposeID = 24) INSERT INTO NotificationPurposes(NotificationPurposeID, ModuleID, Purpose) VALUES(24, 35,'New Lockout Form')
            IF NOT EXISTS (SELECT 1 FROM NotificationPurposes WHERE NotificationPurposeID = 25) INSERT INTO NotificationPurposes(NotificationPurposeID, ModuleID, Purpose) VALUES(25, 35,'Updated Lockout Form')
            IF NOT EXISTS (SELECT 1 FROM NotificationPurposes WHERE NotificationPurposeID = 26) INSERT INTO NotificationPurposes(NotificationPurposeID, ModuleID, Purpose) VALUES(26, 35,'Job Observation')
            IF NOT EXISTS (SELECT 1 FROM NotificationPurposes WHERE NotificationPurposeID = 27) INSERT INTO NotificationPurposes(NotificationPurposeID, ModuleID, Purpose) VALUES(27, 29,'Equipment In Service')
            IF NOT EXISTS (SELECT 1 FROM NotificationPurposes WHERE NotificationPurposeID = 28) INSERT INTO NotificationPurposes(NotificationPurposeID, ModuleID, Purpose) VALUES(28, 70,'CanceledTraining')
            SET IDENTITY_INSERT NotificationPurposes OFF
            ";

        #endregion

        public override void Up()
        {
            Alter.Table("tblTrainingRecords").AddColumn("Canceled").AsBoolean().Nullable();
            Execute.Sql(SQL_CLEANUP_MISSING_NOTIFICATION_PURPOSES);
        }

        public override void Down()
        {
            Delete.Column("Canceled").FromTable("tblTrainingRecords");
            Execute.Sql(
                "DELETE FROM NotificationPurposes WHERE NotificationPurposeID > 8 AND NotificationPurposeID <= 28");
        }
    }
}
