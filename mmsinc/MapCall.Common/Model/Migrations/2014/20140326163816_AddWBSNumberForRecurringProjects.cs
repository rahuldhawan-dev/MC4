using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140326163816), Tags("Production")]
    public class AddWBSNumberForRecurringProjects : Migration
    {
        #region Constants

        public struct TableNames
        {
            public const string RECURRING_PROJECTS = "RPProjects";
        }

        public struct ColumnNames
        {
            public const string WBS_NUMBER = "WBSNumber";
        }

        public struct StringLengths
        {
            public const int WBS_NUMBER = 18;
        }

        #endregion

        public override void Up()
        {
            Alter.Table(TableNames.RECURRING_PROJECTS).AddColumn(ColumnNames.WBS_NUMBER)
                 .AsAnsiString(StringLengths.WBS_NUMBER).Nullable();
        }

        public override void Down()
        {
            Delete.Column(ColumnNames.WBS_NUMBER).FromTable(TableNames.RECURRING_PROJECTS);
        }
    }
}
