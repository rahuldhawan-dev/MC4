using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140807082908326), Tags("Production")]
    public class AddColumnsToEquipmentForBug2019 : Migration
    {
        public override void Up()
        {
            Alter.Table("Equipment")
                 .AddColumn("ArcFlashHierarchy").AsDecimal(18, 5).Nullable()
                 .AddColumn("ArcFlashRating").AsAnsiString(StringLengths.MAX_DEFAULT_VALUE).Nullable();
        }

        public override void Down()
        {
            Delete.Column("ArcFlashRating").FromTable("Equipment");
            Delete.Column("ArcFlashHierarchy").FromTable("Equipment");
        }
    }
}
