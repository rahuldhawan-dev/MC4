using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161216111038417), Tags("Production")]
    public class MainCrossingsBug3350 : Migration
    {
        public override void Up()
        {
            Alter.Table("MainCrossings")
                 .AddColumn("IsolationValves").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("IsolationValves").FromTable("MainCrossings");
        }
    }
}
