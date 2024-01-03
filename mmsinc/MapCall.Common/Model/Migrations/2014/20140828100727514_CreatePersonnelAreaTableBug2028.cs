using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140828100727514), Tags("Production")]
    public class CreatePersonnelAreaTableBug2028 : Migration
    {
        public const string TABLE_NAME = "PersonnelAreas";
        public const int MAX_DESCRIPTION_LENGTH = 30;

        public override void Up()
        {
            Create.Table(TABLE_NAME)
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("Description").AsString(MAX_DESCRIPTION_LENGTH).NotNullable()
                  .WithColumn("PersonnelAreaId").AsInt32().NotNullable()
                  .WithColumn("OperatingCenterId")
                  .AsInt32()
                  .Nullable()
                  .ForeignKey("FK_PersonnelAreas_OperatingCenters_OperatingCenterId", "OperatingCenters",
                       "OperatingCenterId");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_PersonnelAreas_OperatingCenters_OperatingCenterId").OnTable(TABLE_NAME);
            Delete.Table(TABLE_NAME);
        }
    }
}
