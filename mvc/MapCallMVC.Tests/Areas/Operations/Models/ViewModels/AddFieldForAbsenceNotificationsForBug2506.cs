using FluentMigrator;

namespace MapCallMVC.Tests.Areas.Operations.Models.ViewModels
{
    [Migration(20150724155806745), Tags("Production")]
    public class AddFieldForAbsenceNotificationsForBug2506 : Migration
    {
        public const string TABLE_NAME = "AbsenceNotifications";

        public override void Up()
        {
            Alter.Table(TABLE_NAME).AddColumn("ProgressiveDisciplineAdministered").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("ProgressiveDisciplineAdministered").FromTable(TABLE_NAME);
        }
    }
}