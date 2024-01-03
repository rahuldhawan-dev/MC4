using System;
using FluentMigrator;
using MMSINC.ClassExtensions.IEnumerableExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140626100552228), Tags("Production")]
    public class CreateIconOffsetsForBug1945 : Migration
    {
        public static string[] OFFSETS = {
            "top-left", "top-center", "top-right",
            "left", "center", "right",
            "bottom-left", "bottom-center", "bottom-right"
        };

        public const string TABLE_NAME = "MapIconOffsets";
        public const string FOREIGN_KEY_NAME = "FK_MapIcon_MapIconOffsets_OffsetId";

        public override void Up()
        {
            Create.Table(TABLE_NAME)
                  .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn("Description").AsString(15).NotNullable();

            Alter.Table("MapIcon")
                 .AddColumn("OffsetId").AsInt32().ForeignKey(FOREIGN_KEY_NAME, TABLE_NAME, "Id").Nullable();

            OFFSETS.Each(
                s => Execute.Sql(String.Format("INSERT INTO {0} (Description) VALUES ('{1}');", TABLE_NAME, s)));

            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'bottom-center') WHERE IconURL = 'MapIcons/antenna_green.png';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'bottom-center') WHERE IconURL = 'MapIcons/antenna_red.png';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'bottom-center') WHERE IconURL = 'MapIcons/antenna_yellow.png';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'center') WHERE IconURL = 'MapIcons/applications-science.png';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'center') WHERE IconURL = 'MapIcons/e.png';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'center') WHERE IconURL = 'MapIcons/hourglass_black.png';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'center') WHERE IconURL = 'MapIcons/hourglass_green.png';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'bottom-center') WHERE IconURL = 'MapIcons/hydrant_green.png';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'center') WHERE IconURL = 'MapIcons/i.png';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'center') WHERE IconURL = 'MapIcons/IUOE.png';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'top-left') WHERE IconURL = 'MapIcons/NCFO.png';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'bottom-center') WHERE IconURL = 'MapIcons/pin_black.png';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'bottom-center') WHERE IconURL = 'MapIcons/pin_blue.gif';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'bottom-center') WHERE IconURL = 'MapIcons/pin_green.png';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'bottom-center') WHERE IconURL = 'MapIcons/pin_purple.gif';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'bottom-center') WHERE IconURL = 'MapIcons/pin_red.png';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'bottom-center') WHERE IconURL = 'MapIcons/shovel_blue.png';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'bottom-center') WHERE IconURL = 'MapIcons/shovel_green.png';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'bottom-center') WHERE IconURL = 'MapIcons/shovel_red.png';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'bottom-center') WHERE IconURL = 'MapIcons/shovel_yellow.png';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'top-left') WHERE IconURL = 'MapIcons/Smoking_OPs.jpg';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'center') WHERE IconURL = 'MapIcons/tap_brass.gif';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'center') WHERE IconURL = 'MapIcons/UWA.png';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'bottom-center') WHERE IconURL = 'MapIcons/valve_blue.gif';",
                TABLE_NAME));
            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'center') WHERE IconURL = 'MapIcons/x_red.png';",
                TABLE_NAME));

            Execute.Sql(String.Format(
                "UPDATE MapIcon SET OffsetId = (SELECT Id FROM {0} WHERE Description = 'center') WHERE OffsetId IS NULL;",
                TABLE_NAME));

            Alter.Table("MapIcon").AlterColumn("OffsetId").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.ForeignKey(FOREIGN_KEY_NAME).OnTable("MapIcon");
            Delete.Column("OffsetId").FromTable("MapIcon");
            Delete.Table(TABLE_NAME);
        }
    }
}
