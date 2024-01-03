using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190807114238643), Tags("Production")]
    public class MC1551AddColumnForArcFlashStudyRequired : Migration
    {
        public override void Up()
        {
            Alter.Table("tblFacilities").AddColumn("ArcFlashStudyRequired").AsBoolean().WithDefaultValue(false)
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column("ArcFlashStudyRequired").FromTable("tblFacilities");
        }
    }
}
