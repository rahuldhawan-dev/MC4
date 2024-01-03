using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201204150637772), Tags("Production")]
    public class AddThreeBoolsToWorkDescription : Migration
    {
        public override void Up()
        {
            Alter.Table("WorkDescriptions").AddColumn("MarkoutRequired").AsBoolean().Nullable()
                 .AddColumn("MaterialsRequired").AsBoolean().Nullable()
                 .AddColumn("JobSiteCheckListRequired").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("MarkoutRequired").FromTable("WorkDescriptions");
            Delete.Column("MaterialsRequired").FromTable("WorkDescriptions");
            Delete.Column("JobSiteCheckListRequired").FromTable("WorkDescriptions");
        }
    }
}
