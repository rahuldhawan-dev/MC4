using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161021141408398), Tags("Production")]
    public class AddPurposeForBug3261 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                " IF NOT EXISTS (SELECT 1 FROM WorkOrderPurposes WHERE Description = 'Construction Project') BEGIN" +
                " SET IDENTITY_INSERT [dbo].[WorkOrderPurposes] ON;" +
                " INSERT INTO [WorkOrderPurposes](WorkOrderPurposeID, Description) Values(19, 'Construction Project');" +
                " SET IDENTITY_INSERT [dbo].[WorkOrderPurposes] OFF;" +
                " END");
        }

        public override void Down()
        {
            // noop
        }
    }
}
