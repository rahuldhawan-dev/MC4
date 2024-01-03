using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151215102317219), Tags("Production")]
    public class AddGetStartOfDayFunctionForBug2728 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"CREATE FUNCTION GetStartOfDay()
RETURNS DateTime
AS
BEGIN
	RETURN dateadd(day, datediff(day, 0, getdate()), 0);
END;");
        }

        public override void Down()
        {
            Execute.Sql("DROP FUNCTION GetStartOfDay;");
        }
    }
}
