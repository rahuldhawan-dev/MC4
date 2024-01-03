using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20231105154753684), Tags("Production")]
    public class MC5521_UpdateWorkDescriptionInactive : Migration
    {
        public override void Up()
        {
            Execute.Sql("UPDATE [dbo].[WorkDescriptions] SET [IsActive] = 0 WHERE [WorkDescriptionID] = 327");
        }

        public override void Down()
        {
            Execute.Sql("UPDATE [dbo].[WorkDescriptions] SET [IsActive] = 1 WHERE [WorkDescriptionID] = 327");
        }
    }
}

