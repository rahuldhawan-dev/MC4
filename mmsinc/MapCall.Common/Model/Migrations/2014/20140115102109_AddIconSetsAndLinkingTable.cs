using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140115102109), Tags("Production")]
    public class AddIconSetsAndLinkingTable : Migration
    {
        public struct TableNames
        {
            public const string ICON_SETS = "IconSets",
                                MAP_ICON_ICON_SETS = "MapIconIconSets";
        }

        public struct ColumnNames
        {
            public struct Common
            {
                public const string ID = "Id";
            }

            public struct IconSets
            {
                public const string NAME = "Name",
                                    DEFAULT_ICON_ID = "DefaultIconId";
            }

            public struct MapIconIconSets
            {
                public const string ICON_ID = "IconId",
                                    ICON_SET_ID = "IconSetId";
            }
        }

        public struct ColumnSizes
        {
            public struct IconSets
            {
                public const int NAME = 30;
            }
        }

        public override void Up()
        {
            Create.Table(TableNames.ICON_SETS)
                  .WithColumn(ColumnNames.Common.ID).AsInt32().Identity().NotNullable().PrimaryKey()
                  .WithColumn(ColumnNames.IconSets.NAME).AsAnsiString(ColumnSizes.IconSets.NAME).NotNullable().Unique()
                  .WithColumn(ColumnNames.IconSets.DEFAULT_ICON_ID)
                  .AsInt32()
                  .ForeignKey(
                       String.Format("FK_{0}_{1}_{2}", TableNames.ICON_SETS, "MapIcon",
                           ColumnNames.IconSets.DEFAULT_ICON_ID), "MapIcon", "IconId").NotNullable();
            Create.Table(TableNames.MAP_ICON_ICON_SETS)
                  .WithColumn(ColumnNames.Common.ID).AsInt32().Identity().NotNullable().PrimaryKey()
                  .WithColumn(ColumnNames.MapIconIconSets.ICON_ID)
                  .AsInt32()
                  .ForeignKey(
                       String.Format("FK_{0}_{1}_{2}", TableNames.MAP_ICON_ICON_SETS, "MapIcon",
                           ColumnNames.MapIconIconSets.ICON_ID), "MapIcon", "IconId").NotNullable()
                  .WithColumn(ColumnNames.MapIconIconSets.ICON_SET_ID)
                  .AsInt32()
                  .ForeignKey(
                       String.Format("FK_{0}_{1}_{2}", TableNames.MAP_ICON_ICON_SETS, TableNames.ICON_SETS,
                           ColumnNames.MapIconIconSets.ICON_SET_ID), TableNames.MAP_ICON_ICON_SETS,
                       ColumnNames.Common.ID).NotNullable();

            Execute.Sql(@"UPDATE [MapIcon] SET [iconURL] = 'MapIcons/NCFO.png' WHERE [iconURL] = 'NCFO.png'
UPDATE [MapIcon] SET [iconURL] = 'MapIcons/IUOE.png' WHERE [iconURL] = 'IUOE.png'
UPDATE [MapIcon] SET [iconURL] = 'MapIcons/UWA.png' WHERE [iconURL] = 'UWA.png'
UPDATE [MapIcon] SET [iconURL] = 'MapIcons/pin_red.png' WHERE [iconURL] = 'm_ValR.png'
UPDATE [MapIcon] SET [iconURL] = 'MapIcons/pin_black.png' WHERE [iconURL] = 'm_ValB.png'
UPDATE [MapIcon] SET [iconURL] = 'MapIcons/pin_green.png' WHERE [iconURL] = 'm_valG.png'
UPDATE [MapIcon] SET [iconURL] = 'MapIcons/Smoking_OPs.jpg' WHERE [iconURL] = 'Smoking_OPs_Pic.jpg'
UPDATE [MapIcon] SET [iconURL] = 'MapIcons/pin_blue.gif' WHERE [iconURL] = 'm_ValBl.gif'
UPDATE [MapIcon] SET [iconURL] = 'MapIcons/hydrant_green.png' WHERE [iconURL] = 'm_HydG.png'
UPDATE [MapIcon] SET [iconURL] = 'MapIcons/valve_blue.gif' WHERE [iconURL] = 'm_BOffBl.gif'
UPDATE [MapIcon] SET [iconURL] = 'MapIcons/applications-science.png' WHERE [iconURL] = 'applications-science.png'
UPDATE [MapIcon] SET [iconURL] = 'MapIcons/tap_brass.gif' WHERE [iconURL] = 'tap_brass.gif'
UPDATE [MapIcon] SET [iconURL] = 'MapIcons/pin_purple.gif' WHERE [iconURL] = 'm_valP.gif'
UPDATE [MapIcon] SET [iconURL] = 'MapIcons/x_red.png' WHERE [iconURL] = 'redx.png'
UPDATE [MapIcon] SET [iconURL] = 'MapIcons/e.png' WHERE [iconURL] = 'e.png'
UPDATE [MapIcon] SET [iconURL] = 'MapIcons/hourglass_black.png' WHERE [iconURL] = 'm_MeterBlack.png'
UPDATE [MapIcon] SET [iconURL] = 'MapIcons/hourglass_green.png' WHERE [iconURL] = 'm_MeterGreen.png'
UPDATE [MapIcon] SET [iconURL] = 'MapIcons/antenna_green.png' WHERE [iconURL] = 'icon_MeterRecorderGreen.png'
UPDATE [MapIcon] SET [iconURL] = 'MapIcons/antenna_yellow.png' WHERE [iconURL] = 'icon_MeterRecorderYellow.png'
UPDATE [MapIcon] SET [iconURL] = 'MapIcons/antenna_red.png' WHERE [iconURL] = 'icon_MeterRecorderRed.png'
UPDATE [MapIcon] SET [iconURL] = 'MapIcons/shovel_blue.png' WHERE [iconURL] = 'icons/shovel_blue.png'
UPDATE [MapIcon] SET [iconURL] = 'MapIcons/shovel_yellow.png' WHERE [iconURL] = 'icons/shovel_yellow.png'
UPDATE [MapIcon] SET [iconURL] = 'MapIcons/shovel_green.png' WHERE [iconURL] = 'icons/shovel_green.png'
UPDATE [MapIcon] SET [iconURL] = 'MapIcons/shovel_red.png' WHERE [iconURL] = 'icons/shovel_red.png'");

            Execute.Sql(
                @"INSERT INTO [IconSets] (Name, DefaultIconId) SELECT 'Antennae', IconId FROM [MapIcon] WHERE [iconURL] LIKE 'MapIcons/antenna_green%';
INSERT INTO [MapIconIconSets] (IconId, IconSetId)
SELECT i.IconId, s.Id
FROM
	[MapIcon] AS i
LEFT JOIN
	[IconSets] AS s
ON
	s.Name = 'Antennae'
WHERE
	i.[iconURL] LIKE 'MapIcons/antenna_%';
INSERT INTO [IconSets] (Name, DefaultIconId) SELECT 'Assets', IconId FROM [MapIcon] WHERE [iconURL] LIKE 'MapIcons/hydrant_green%';
INSERT INTO [MapIconIconSets] (IconId, IconSetId)
SELECT i.IconId, s.Id
FROM
	[MapIcon] AS i
LEFT JOIN
	[IconSets] AS s
ON
	s.Name = 'Assets'
WHERE
	i.[iconURL] LIKE 'MapIcons/hydrant_%'
OR
	i.[iconURL] LIKE 'MapIcons/tap_%'
OR
	i.[iconURL] LIKE 'MapIcons/valve_%';
INSERT INTO [IconSets] (Name, DefaultIconId) SELECT TOP 1 'Hourglasses', IconId FROM [MapIcon] WHERE [iconURL] LIKE 'MapIcons/hourglass_black%';
INSERT INTO [MapIconIconSets] (IconId, IconSetId)
SELECT i.IconId, s.Id
FROM
	[MapIcon] AS i
LEFT JOIN
	[IconSets] AS s
ON
	s.Name = 'Hourglasses'
WHERE
	i.[iconURL] LIKE 'MapIcons/hourglass_%';
INSERT INTO [IconSets] (Name, DefaultIconId) SELECT 'Miscellaneous', IconId FROM [MapIcon] WHERE [iconURL] LIKE 'MapIcons/x_red%';
INSERT INTO [MapIconIconSets] (IconId, IconSetId)
SELECT i.IconId, s.Id
FROM
	[MapIcon] AS i
LEFT JOIN
	[IconSets] AS s
ON
	s.Name = 'Miscellaneous'
WHERE
	i.[iconURL] LIKE 'MapIcons/x_red%'
OR
	i.[iconURL] LIKE 'MapIcons/applications-science%'
OR
	i.[iconURL] LIKE 'MapIcons/e.%'
OR
	i.[iconURL] LIKE 'MapIcons/IUOE%'
OR
	i.[iconURL] LIKE 'MapIcons/NCFO%'
OR
	i.[iconURL] LIKE 'MapIcons/Smoking_OPs%'
OR
	i.[iconURL] LIKE 'MapIcons/UWA%';
INSERT INTO [IconSets] (Name, DefaultIconId) SELECT 'Pins', IconId FROM [MapIcon] WHERE [iconURL] LIKE 'MapIcons/pin_black%';
INSERT INTO [MapIconIconSets] (IconId, IconSetId)
SELECT i.IconId, s.Id
FROM
	[MapIcon] AS i
LEFT JOIN
	[IconSets] AS s
ON
	s.Name = 'Pins'
WHERE
	i.[iconURL] LIKE 'MapIcons/pin_%';
INSERT INTO [IconSets] (Name, DefaultIconId) SELECT 'Shovels', IconId FROM [MapIcon] WHERE [iconURL] LIKE 'MapIcons/shovel_blue%';
INSERT INTO [MapIconIconSets] (IconId, IconSetId)
SELECT i.IconId, s.Id
FROM
	[MapIcon] AS i
LEFT JOIN
	[IconSets] AS s
ON
	s.Name = 'Shovels'
WHERE
	i.[iconURL] LIKE 'MapIcons/shovel_%';");
        }

        public override void Down()
        {
            Delete.Table(TableNames.ICON_SETS);
            Delete.Table(TableNames.MAP_ICON_ICON_SETS);

            Execute.Sql(@"UPDATE [MapIcon] SET [iconURL] = 'NCFO.png' WHERE [iconURL] = 'MapIcons/NCFO.png'
UPDATE [MapIcon] SET [iconURL] = 'IUOE.png' WHERE [iconURL] = 'MapIcons/IUOE.png'
UPDATE [MapIcon] SET [iconURL] = 'UWA.png' WHERE [iconURL] = 'MapIcons/UWA.png'
UPDATE [MapIcon] SET [iconURL] = 'm_ValR.png' WHERE [iconURL] = 'MapIcons/pin_red.png'
UPDATE [MapIcon] SET [iconURL] = 'm_ValB.png' WHERE [iconURL] = 'MapIcons/pin_black.png'
UPDATE [MapIcon] SET [iconURL] = 'm_valG.png' WHERE [iconURL] = 'MapIcons/pin_green.png'
UPDATE [MapIcon] SET [iconURL] = 'Smoking_OPs_Pic.jpg' WHERE [iconURL] = 'MapIcons/Smoking_OPs.jpg'
UPDATE [MapIcon] SET [iconURL] = 'm_ValBl.gif' WHERE [iconURL] = 'MapIcons/pin_blue.gif'
UPDATE [MapIcon] SET [iconURL] = 'm_HydG.png' WHERE [iconURL] = 'MapIcons/hydrant_green.png'
UPDATE [MapIcon] SET [iconURL] = 'm_BOffBl.gif' WHERE [iconURL] = 'MapIcons/valve_blue.gif'
UPDATE [MapIcon] SET [iconURL] = 'applications-science.png' WHERE [iconURL] = 'MapIcons/applications-science.png'
UPDATE [MapIcon] SET [iconURL] = 'tap_brass.gif' WHERE [iconURL] = 'MapIcons/tap_brass.gif'
UPDATE [MapIcon] SET [iconURL] = 'm_valP.gif' WHERE [iconURL] = 'MapIcons/pin_purple.gif'
UPDATE [MapIcon] SET [iconURL] = 'redx.png' WHERE [iconURL] = 'MapIcons/x_red.png'
UPDATE [MapIcon] SET [iconURL] = 'e.png' WHERE [iconURL] = 'MapIcons/e.png'
UPDATE [MapIcon] SET [iconURL] = 'm_MeterBlack.png' WHERE [iconURL] = 'MapIcons/hourglass_black.png'
UPDATE [MapIcon] SET [iconURL] = 'm_MeterGreen.png' WHERE [iconURL] = 'MapIcons/hourglass_green.png'
UPDATE [MapIcon] SET [iconURL] = 'icon_MeterRecorderGreen.png' WHERE [iconURL] = 'MapIcons/antenna_green.png'
UPDATE [MapIcon] SET [iconURL] = 'icon_MeterRecorderYellow.png' WHERE [iconURL] = 'MapIcons/antenna_yellow.png'
UPDATE [MapIcon] SET [iconURL] = 'icon_MeterRecorderRed.png' WHERE [iconURL] = 'MapIcons/antenna_red.png'
UPDATE [MapIcon] SET [iconURL] = 'icons/shovel_blue.png' WHERE [iconURL] = 'MapIcons/shovel_blue.png'
UPDATE [MapIcon] SET [iconURL] = 'icons/shovel_yellow.png' WHERE [iconURL] = 'MapIcons/shovel_yellow.png'
UPDATE [MapIcon] SET [iconURL] = 'icons/shovel_green.png' WHERE [iconURL] = 'MapIcons/shovel_green.png'
UPDATE [MapIcon] SET [iconURL] = 'icons/shovel_red.png' WHERE [iconURL] = 'MapIcons/shovel_red.png'");
        }
    }
}
