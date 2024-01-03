using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220525104221393), Tags("Production")]
    public class MC4037AlterTaskGroupsAddPlanTypeColumn : Migration
    {
        public override void Up()
        {
            Alter.Table("TaskGroups").AddColumn("Type").AsAnsiString(50).Nullable();
        }

        public override void Down()
        {
            Delete.Column("Type").FromTable("TaskGroups");
        }
    }
}

