using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20211021083116160), Tags("Production")]
    public class MC3393AddMapIconForNearMiss : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
            declare @iconId int
            declare @iconSetId int
			set @iconSetId = 14
   
            insert into MapIcon([iconUrl], [height], [width],[OffsetId]) VALUES('MapIcons/nearmiss.png', 24, 24, 5)
            set @iconId = (SELECT @@IDENTITY)

			SET IDENTITY_INSERT [IconSets] ON;
			insert into IconSets([Id], [Name], [DefaultIconId]) VALUES(@iconSetId, 'NearMiss', @iconId)
			SET IDENTITY_INSERT [IconSets] OFF;
          
            insert into MapIconIconSets([IconId], [IconSetId]) VALUES(@iconId, @iconSetId)");
        }

        public override void Down()
        {
            Execute.Sql(@"
            update Coordinates set IconId = 8 where IconId = (select IconId from MapIcon where iconUrl = 'MapIcons/nearmiss.png')
            delete from MapIconIconSets where iconSetId = 14
            delete from IconSets where Id = 14
            delete from MapIcon where iconURL = 'MapIcons/nearmiss.png'
            ");
        }
    }
}