using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221209122809879), Tags("Production")]
    public class MC4834_RenameSewerMainCleaningsColumns : AutoReversingMigration
    {
        public override void Up()
        {
            Rename.Column("FlushWaterVolume").OnTable("SewerMainCleanings").To("GallonsOfWaterUsed");
            Rename
               .Column("FootageOfMainCleaned")
               .OnTable("SewerMainCleanings")
               .To("FootageOfMainInspected");
            Rename.Column("CompletedDate").OnTable("SewerMainCleanings").To("InspectedDate");
        }
    }
}

