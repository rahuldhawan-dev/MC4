using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210805075322922), Tags("Production")]
    public class AddArcMobileMapIdToOperatingCentersForMC3501 : Migration
    {
        public override void Up()
        {
            Alter.Table("OperatingCenters").AddColumn("ArcMobileMapId").AsAnsiString(32).Nullable();
            Execute.Sql("UPDATE OperatingCenters SET ArcMobileMapId = MapId;");
            Execute.Sql("UPDATE OperatingCenters SET MapId = 'e8ae5ea9dc304901b35f07d12854a6ba' WHERE MapId = '15fdc279b4234fcb85f455ee70897a9e';");
        }

        public override void Down()
        {
            Execute.Sql("UPDATE OperatingCenters SET MapId = '15fdc279b4234fcb85f455ee70897a9e' WHERE MapId = 'e8ae5ea9dc304901b35f07d12854a6ba'");
            Delete.Column("ArcMobileMapId").FromTable("OperatingCenters");
        }
    }
}