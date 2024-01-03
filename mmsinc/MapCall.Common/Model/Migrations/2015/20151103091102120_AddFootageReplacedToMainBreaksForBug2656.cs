using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151103091102120), Tags("Production")]
    public class AddFootageReplacedToMainBreaksForBug2656 : Migration
    {
        public override void Up()
        {
            Alter.Table("MainBreaks")
                 .AddColumn("FootageReplaced")
                 .AsInt32()
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column("FootageReplaced").FromTable("MainBreaks");
        }
    }
}
