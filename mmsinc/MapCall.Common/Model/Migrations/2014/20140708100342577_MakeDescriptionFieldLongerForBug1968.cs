using FluentMigrator;
using ColumnNames = MapCall.Common.Model.Migrations.CreateTablesForBug1774.ColumnNames;
using TableNames = MapCall.Common.Model.Migrations.CreateTablesForBug1774.TableNames;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140708100342577), Tags("Production")]
    public class MakeDescriptionFieldLongerForBug1968 : Migration
    {
        public const int STRING_LENGTH = 300;

        public override void Up()
        {
            Alter.Column(ColumnNames.Common.DESCRIPTION)
                 .OnTable(TableNames.ESTIMATING_PROJECTS)
                 .AsAnsiString(STRING_LENGTH)
                 .Nullable();
        }

        // no need
        public override void Down() { }
    }
}
