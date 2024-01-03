using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140829135344907), Tags("Production")]
    public class AddUnionLocalMapIcons : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                declare @offset int
                set @offset = (select top 1 Id from MapIconOffsets where Description = 'center')

                insert into [MapIcon] ([iconUrl], [width], [height], [OffsetId]) VALUES('MapIcons/union-AFSCME.png', 80, 33, @offset)
                insert into [MapIcon] ([iconUrl], [width], [height], [OffsetId]) VALUES('MapIcons/union-CUPE.png', 80, 22, @offset)
                insert into [MapIcon] ([iconUrl], [width], [height], [OffsetId]) VALUES('MapIcons/union-HEREU.png', 40, 40, @offset)
                insert into [MapIcon] ([iconUrl], [width], [height], [OffsetId]) VALUES('MapIcons/union-IBEW.png', 40, 40, @offset)
                insert into [MapIcon] ([iconUrl], [width], [height], [OffsetId]) VALUES('MapIcons/union-IBT.png', 30, 40, @offset)
                insert into [MapIcon] ([iconUrl], [width], [height], [OffsetId]) VALUES('MapIcons/union-ICWU.png', 40, 40, @offset)
                insert into [MapIcon] ([iconUrl], [width], [height], [OffsetId]) VALUES('MapIcons/union-IUOE.png', 40, 40, @offset)
                insert into [MapIcon] ([iconUrl], [width], [height], [OffsetId]) VALUES('MapIcons/union-LIUNA.png', 40, 40, @offset)
                insert into [MapIcon] ([iconUrl], [width], [height], [OffsetId]) VALUES('MapIcons/union-PPF.png', 40, 40, @offset)
                insert into [MapIcon] ([iconUrl], [width], [height], [OffsetId]) VALUES('MapIcons/union-SEIU.png', 32, 40, @offset)
                insert into [MapIcon] ([iconUrl], [width], [height], [OffsetId]) VALUES('MapIcons/union-UFCW.png', 80, 21, @offset)
                insert into [MapIcon] ([iconUrl], [width], [height], [OffsetId]) VALUES('MapIcons/union-USW.png', 80, 32, @offset)
");
        }

        public override void Down()
        {
            Execute.Sql("delete from [MapIcon] where [iconUrl] = 'MapIcons/union-AFSCME.png'");
            Execute.Sql("delete from [MapIcon] where [iconUrl] = 'MapIcons/union-CUPE.png'");
            Execute.Sql("delete from [MapIcon] where [iconUrl] = 'MapIcons/union-HEREU.png'");
            Execute.Sql("delete from [MapIcon] where [iconUrl] = 'MapIcons/union-IBEW.png'");
            Execute.Sql("delete from [MapIcon] where [iconUrl] = 'MapIcons/union-IBT.png'");
            Execute.Sql("delete from [MapIcon] where [iconUrl] = 'MapIcons/union-ICWU.png'");
            Execute.Sql("delete from [MapIcon] where [iconUrl] = 'MapIcons/union-IUOE.png'");
            Execute.Sql("delete from [MapIcon] where [iconUrl] = 'MapIcons/union-LIUNA.png'");
            Execute.Sql("delete from [MapIcon] where [iconUrl] = 'MapIcons/union-PPF.png'");
            Execute.Sql("delete from [MapIcon] where [iconUrl] = 'MapIcons/union-SEIU.png'");
            Execute.Sql("delete from [MapIcon] where [iconUrl] = 'MapIcons/union-UFCW.png'");
            Execute.Sql("delete from [MapIcon] where [iconUrl] = 'MapIcons/union-USW.png'");
        }
    }
}
