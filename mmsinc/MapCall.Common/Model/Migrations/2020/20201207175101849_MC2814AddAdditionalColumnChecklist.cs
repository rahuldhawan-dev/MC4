using FluentMigrator;
using FluentMigrator.Expressions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201207175101849), Tags("Production")]
    public class MC2814AddAdditionalColumnChecklist : Migration
    {
        public override void Up()
        {
            Alter.Table("JobSiteCheckLists")
                 .AddColumn("HaveYouInspectedSlings").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("HaveYouInspectedSlings").FromTable("JobSiteCheckLists");
            ;
        }
    }
}
