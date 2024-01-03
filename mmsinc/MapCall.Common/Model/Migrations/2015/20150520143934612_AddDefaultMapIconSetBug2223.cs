using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150520143934612), Tags("Production")]
    public class AddDefaultMapIconSetBug2223 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
    declare @defaultIconId int
    set @defaultIconId = (select iconId from MapIcon where iconURL = 'MapIcons/pin_black.png')

    SET IDENTITY_INSERT [IconSets] ON
    insert into IconSets(Id, Name, DefaultIconId) Values(11, 'SingleDefaultIcon', @defaultIconId) 
    SET IDENTITY_INSERT [IconSets] OFF

    insert into MapIconIconSets(IconId, IconSetId) values(@defaultIconId, 11);
            ");
        }

        public override void Down()
        {
            Execute.Sql(@"
                delete from MapIconIconSets where IconSetId = 11
                delete from IconSets where Id = 11
          ");
        }
    }
}
