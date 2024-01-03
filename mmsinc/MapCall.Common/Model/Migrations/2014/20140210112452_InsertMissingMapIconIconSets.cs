using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140210112452), Tags("Production")]
    public class InsertMissingMapIconIconSets : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"set identity_insert MapIcon on;
                            if not exists (select 1 from mapicon where iconID = 28)
	                            insert into MapIcon(iconID, iconURL,width, height) Values(28, 'MapIcons/shovel_blue.png', 28, 28)
                            if not exists (select 1 from mapicon where iconID = 29)
	                            insert into MapIcon(iconID, iconURL,width, height) Values(29, 'MapIcons/shovel_yellow.png',	28, 28)
                            if not exists (select 1 from mapicon where iconID = 30)
	                            insert into MapIcon(iconID, iconURL,width, height) Values(30 ,'MapIcons/shovel_green.png', 28, 28)
                            if not exists (select 1 from mapicon where iconID = 31)
	                            insert into MapIcon(iconID, iconURL,width, height) Values(31, 'MapIcons/shovel_red.png', 28, 28)
                            if not exists (select 1 from mapicon where iconID = 32)
	                            insert into MapIcon(iconID, iconURL,width, height) Values(32, 'MapIcons/i.png',	24,	24)
                            set identity_insert MapIcon off;

                            set identity_insert IconSets on ;
                            if not exists (select 1 from iconsets where Id = 6)
	                            insert into IconSets(Id, Name, DefaultIconId) Values(6, 'Shovels', 28) 
                            if not exists (select 1 from iconsets where Id = 7)
	                            insert into IconSets(Id, Name, DefaultIconId) Values(7, 'Incidents', 32)
                            set identity_insert IconSets off ;
                            if not exists (Select 1 from MapIconIconSets where IconID = 32 and IconSetID = 7)
	                            insert into MapIconIconSets ([IconId], [IconSetId]) VALUES(32, 7)");
        }

        public override void Down()
        {
            Execute.Sql(@"Delete MapIconIconSets where IconSetID IN (6, 7);
                          Delete IconSets where Id in (6,7);
                          Delete MapIcon where iconId in (28, 29, 30, 31, 32)");
        }
    }
}
