using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200706142046211), Tags("Production")]
    public class ChangeCallIdToLongForMC2428 : Migration
    {
        public const string VIEW_NAME = "ShortCycleAssignmentsMinimumIdByCallId";
        public const string DROP_VIEW = "DROP VIEW [" + VIEW_NAME + "]";

        public const string CREATE_VIEW = "CREATE VIEW [" + VIEW_NAME + @"] AS
        	SELECT 
		        Min(Id) as Id
	        FROM 
		        ShortCycleAssignments
	        GROUP BY CallId";

        public override void Up()
        {
            Alter.Table("ShortCycleAssignments").AlterColumn("CallId").AsInt64().NotNullable();
            Execute.Sql(CREATE_VIEW);
        }

        public override void Down()
        {
            Alter.Table("ShortCycleAssignments").AlterColumn("CallId")
                 .AsAnsiString().NotNullable();
            Execute.Sql(DROP_VIEW);
        }
    }
}
