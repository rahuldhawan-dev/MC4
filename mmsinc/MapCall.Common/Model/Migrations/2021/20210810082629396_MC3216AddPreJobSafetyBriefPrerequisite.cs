using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210810082629396), Tags("Production")]
    public class MC3216AddPreJobSafetyBriefPrerequisite : Migration
    {
        public override void Up()
        {
            Execute.Sql("SET IDENTITY_INSERT [dbo].[ProductionPrerequisites] ON");
            Insert.IntoTable("ProductionPrerequisites").Row(new { ID = 6, Description = "Pre Job Safety Brief" });
            Execute.Sql("SET IDENTITY_INSERT [dbo].[ProductionPrerequisites] OFF");
        }

        public override void Down()
        {
            Delete.FromTable("ProductionPrerequisites").Row(new { ID = 6 });
        }
    }
}
