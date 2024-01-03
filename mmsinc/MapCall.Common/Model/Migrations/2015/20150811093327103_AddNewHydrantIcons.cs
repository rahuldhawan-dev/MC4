using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150811093327103), Tags("Production")]
    public class AddNewHydrantIcons : Migration
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

            setIcon("hydrant-yellow.png");
            setIcon("hydrant-yellowblack.png");
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

            deleteIcon("hydrant-yellow.png");
            deleteIcon("hydrant-yellowblack.png");
        }
    }
}
