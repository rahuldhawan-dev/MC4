using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150825163834846), Tags("Production")]
    public class AddFunctionForBug2572 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
CREATE FUNCTION DateAddPlus(@datePart varchar(1), @increment int, @date datetime)
returns datetime
as
begin
	if (@datePart = 'd') return DateAdd(dd, @increment, @date)
	if (@datePart = 'w') return DateAdd(ww, @increment, @date)
	if (@datePart = 'm') return DateAdd(mm, @increment, @date)
	if (@datePart = 'y') return DateAdd(yy, @increment, @date)
	return null
end");
            Execute.Sql("GRANT ALL ON DateAddPlus to McUser;");
        }

        public override void Down()
        {
            Execute.Sql("DROP FUNCTION DateAddPlus");
        }
    }
}
