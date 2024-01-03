using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201209192227830), Tags("Production")]
    public class MC1776ChangeTextToVarChar : Migration
    {
        private const string EVENTS = "Events", EVENTS_OLD_NAME = "EventDocuments";

        public override void Up()
        {
            Alter.Table(EVENTS).AlterColumn("RootCause").AsAnsiString(int.MaxValue).Nullable();
            Alter.Table(EVENTS).AlterColumn("EventSummary").AsAnsiString(int.MaxValue).Nullable();
            Alter.Table(EVENTS).AlterColumn("ResponseActions").AsAnsiString(int.MaxValue).Nullable();
            Alter.Table(EVENTS_OLD_NAME).AlterColumn("Description").AsAnsiString(int.MaxValue).Nullable();
        }

        public override void Down()
        {
            Alter.Table(EVENTS).AlterColumn("RootCause").AsText().Nullable();
            Alter.Table(EVENTS).AlterColumn("EventSummary").AsText().Nullable();
            Alter.Table(EVENTS).AlterColumn("ResponseActions").AsText().Nullable();
            Alter.Table(EVENTS_OLD_NAME).AlterColumn("Description").AsText().Nullable();
        }
    }
}
