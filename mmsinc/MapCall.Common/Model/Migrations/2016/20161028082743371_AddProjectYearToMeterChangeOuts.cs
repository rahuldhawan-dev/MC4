using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161028082743371), Tags("Production")]
    public class AddProjectYearToMeterChangeOuts : Migration
    {
        public override void Up()
        {
            Alter.Table("MeterChangeOuts").AddColumn("ProjectYear").AsAnsiString(6).Nullable();
        }

        public override void Down()
        {
            Delete.Column("ProjectYear").FromTable("MeterChangeOuts");
        }
    }
}
