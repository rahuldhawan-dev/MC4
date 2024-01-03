using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140421141814), Tags("Production")]
    public class AddDateFieldsToIncidentsForBug1861 : Migration
    {
        public const string TABLE_NAME = "Incidents";

        public struct ColumnNames
        {
            public const string LOST_TIME_START = "LostTimeStartDate",
                                LOST_TIME_END = "LostTimeEndDate",
                                RESTRICTIVE_DUTY_START = "RestrictiveDutyStartDate",
                                RESTRICTIVE_DUTY_END = "RestrictiveDutyEndDate";
        }

        public override void Up()
        {
            Alter.Table(TABLE_NAME)
                 .AddColumn(ColumnNames.LOST_TIME_START).AsDateTime().Nullable()
                 .AddColumn(ColumnNames.LOST_TIME_END).AsDateTime().Nullable()
                 .AddColumn(ColumnNames.RESTRICTIVE_DUTY_START).AsDateTime().Nullable()
                 .AddColumn(ColumnNames.RESTRICTIVE_DUTY_END).AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column(ColumnNames.LOST_TIME_START).FromTable(TABLE_NAME);
            Delete.Column(ColumnNames.LOST_TIME_END).FromTable(TABLE_NAME);
            Delete.Column(ColumnNames.RESTRICTIVE_DUTY_START).FromTable(TABLE_NAME);
            Delete.Column(ColumnNames.RESTRICTIVE_DUTY_END).FromTable(TABLE_NAME);
        }
    }
}
