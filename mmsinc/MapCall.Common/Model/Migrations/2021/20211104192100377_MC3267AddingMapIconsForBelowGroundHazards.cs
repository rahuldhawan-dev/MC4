using System;
using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20211104192100377), Tags("Production")]
    public class MC3267AddingMapIconsForBelowGroundHazards : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("MapIcon").Rows(
                new { iconURL = "MapIcons/belowgroundhazard-green.png", width = 24, height = 24, OffsetId = 5 },
                new { iconURL = "MapIcons/belowgroundhazard-gray.png", width = 24, height = 24, OffsetId = 5 });
            Execute.Sql(@"INSERT INTO [MapIconIconSets] (IconId, IconSetId)
                          SELECT i.IconId, s.Id
                          FROM
                          	  [MapIcon] AS i
                          LEFT JOIN
                          	  [IconSets] AS s
                          ON
                          	  s.Name = 'Assets'
                          WHERE
                          	  i.[iconURL] LIKE 'MapIcons/belowgroundhazard%'
                          OR
                          	  i.[iconURL] LIKE 'MapIcons/belowgroundhazard%'");
        }

        public override void Down()
        {
            Action<string> deleteIcon = (fileName) => {
                Execute.Sql(@"declare @iconId int
                            set @iconId = (select IconId from MapIcon where iconUrl = 'MapIcons/" + fileName + @"')
                            
                            delete from MapIconIconSets where IconId = @iconId and IconSetId = 2
                            delete from MapIcon where IconId = @iconId
                           ");
            };

            deleteIcon("belowgroundhazard-green.png");
            deleteIcon("belowgroundhazard-blue.png");
        }
    }
}

