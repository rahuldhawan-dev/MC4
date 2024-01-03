using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150519082225008), Tags("Production")]
    public class CreateHydrantValveBlowoffMapIconsBug2223 : Migration
    {
        public override void Up()
        {
            Action<string> setIcon = (fileName) => {
                Execute.Sql(@"
    declare @offsetId int
    set @offsetId = (select Id from MapIconOffsets where Description = 'center')
 
    insert into MapIcon(iconURL, width, height, offsetId) values('MapIcons/" + fileName + @"', 24, 24, @offsetId)
    declare @iconId int; set @iconId = (select @@IDENTITY)

    declare @iconSetId int
    set @iconSetId = (select Id from IconSets where Name = 'Assets')

    insert into MapIconIconSets (IconId, IconSetId) values(@iconId, @iconSetId)
");
            };

            setIcon("hydrant-green.png");
            setIcon("hydrant-blue.png");
            setIcon("hydrant-red.png");
            setIcon("hydrant-greenblack.png");
            setIcon("hydrant-gray.png");
            setIcon("hydrant-redblack.png");

            setIcon("valve-green.png");
            setIcon("valve-blue.png");
            setIcon("valve-red.png");
            setIcon("valve-greenblack.png");
            setIcon("valve-gray.png");
            setIcon("valve-redblack.png");

            setIcon("blowoff-green.png");
            setIcon("blowoff-blue.png");
            setIcon("blowoff-red.png");
            setIcon("blowoff-greenblack.png");
            setIcon("blowoff-gray.png");
            setIcon("blowoff-redblack.png");
        }

        public override void Down()
        {
            Action<string> deleteIcon = (fileName) => {
                Execute.Sql(@"
    declare @iconId int
    set @iconId = (select IconId from MapIcon where iconUrl = 'MapIcons/" + fileName + @"')

    delete from MapIconIconSets where IconId = @iconId
    delete from MapIcon where IconId = @iconId
");
            };

            deleteIcon("hydrant-green.png");
            deleteIcon("hydrant-blue.png");
            deleteIcon("hydrant-red.png");
            deleteIcon("hydrant-greenblack.png");
            deleteIcon("hydrant-gray.png");
            deleteIcon("hydrant-redblack.png");

            deleteIcon("valve-green.png");
            deleteIcon("valve-blue.png");
            deleteIcon("valve-red.png");
            deleteIcon("valve-greenblack.png");
            deleteIcon("valve-gray.png");
            deleteIcon("valve-redblack.png");

            deleteIcon("blowoff-green.png");
            deleteIcon("blowoff-blue.png");
            deleteIcon("blowoff-red.png");
            deleteIcon("blowoff-greenblack.png");
            deleteIcon("blowoff-gray.png");
            deleteIcon("blowoff-redblack.png");
        }
    }
}
