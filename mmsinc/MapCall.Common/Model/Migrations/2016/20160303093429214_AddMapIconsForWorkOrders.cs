using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160303093429214), Tags("Production")]
    public class AddMapIconsForWorkOrders : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"dbcc checkident ('mapicon', reseed, 66)
                        dbcc checkident ('iconsets', reseed, 11)
                        insert into MapIcon Values('MapIcons/construction_green.png', 32,27,4)
                        insert into MapIcon Values('MapIcons/construction_red.png', 32,27,4)
                        insert into MapIcon Values('MapIcons/construction_yellow.png', 32,27,4)
                        insert into MapIcon Values('MapIcons/construction_gray.png', 32,27,4)
                        insert into IconSets SELECT 'WorkOrders', (Select iconId from MapIcon where iconURL = 'MapIcons/construction_green.png')
                        Insert Into MapIconIconSets Values(70,12)
                        Insert Into MapIconIconSets Values(69,12)
                        Insert Into MapIconIconSets Values(68,12)
                        Insert Into MapIconIconSets Values(67,12)
                        ");
        }

        public override void Down()
        {
            Execute.Sql("DELETE FROM MapIconIconSets WHERE IconSetID = 12;" +
                        "DELETE FROM IconSets WHERE Id = 12;" +
                        "DELETE FROM MapIcon WHERE IconID in (67, 68, 69, 70);");
        }
    }
}
