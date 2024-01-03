using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151027103422719), Tags("Production")]
    public class CreateVideosTableBug2666 : Migration
    {
        public const int MAX_SPROUT_VIDEO_ID_LENGTH = 18;
        public const string VIEW_NAME = "VideoView";
        public const string DROP_SQL = "DROP VIEW [" + VIEW_NAME + "];";

        public const string CREATE_SQL = "CREATE VIEW [" + VIEW_NAME + @"] AS
SELECT
  Videos.Id as Id,
  Videos.DataLinkId as LinkedId,
  Videos.DataTypeId,
  Videos.SproutVideoId,
  Videos.Title,
  dt.Table_Name as TableName
FROM
  Videos
INNER JOIN
  DataType dt
ON
  dt.DataTypeId = Videos.DataTypeId;";

        public override void Up()
        {
            Create.Table("Videos")
                  .WithColumn("Id").AsIdColumn()
                  .WithColumn("Title").AsString(256).NotNullable()
                  .WithColumn("SproutVideoId").AsString(MAX_SPROUT_VIDEO_ID_LENGTH).NotNullable()
                  .WithColumn("DataLinkId").AsInt32().NotNullable()
                  .WithColumn("DataTypeId").AsInt32().NotNullable()
                  .ForeignKey("FK_Videos_DataType_DataTypeId", "DataType", "DataTypeId");

            Execute.Sql(CREATE_SQL);
        }

        public override void Down()
        {
            Execute.Sql(DROP_SQL);
            Delete.ForeignKey("FK_Videos_DataType_DataTypeId").OnTable("Videos");
            Delete.Table("Videos");
        }
    }
}
