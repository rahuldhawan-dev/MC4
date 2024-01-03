using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161202141705824), Tags("Production")]
    public class AddCriticalColumnsToSewerManholeForBug3313 : Migration
    {
        public struct StringLengths
        {
            public const int HYD_CRITICAL_NOTES = 15;
        }

        public override void Up()
        {
            Alter.Table("SewerManholes")
                 .AddColumn("Critical").AsBoolean().NotNullable().WithDefaultValue(false)
                 .AddColumn("CriticalNotes").AsAnsiString(StringLengths.HYD_CRITICAL_NOTES).Nullable();
        }

        public override void Down()
        {
            Delete.Column("CriticalNotes").FromTable("SewerManholes");
            Delete.Column("Critical").FromTable("SewerManholes");
        }
    }
}
