using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20211022121857727), Tags("Production")]
    public class MC3865AddingDateCompletedToNearMiss : Migration
    {
        public override void Up()
        {
            Alter.Table("NearMisses")
                 .AddColumn("DateCompleted").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("DateCompleted").FromTable("NearMisses");
        }
    }
}

