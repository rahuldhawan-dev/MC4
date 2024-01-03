using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221129144324927), Tags("Production")]
    public class MC5195AddAndRenameColumns : Migration
    {
        public override void Up()
        {
            Alter.Table("EchoshoreLeakAlerts").AddColumn("FieldInvestigationRecommendedOn").AsDateTime().Nullable();
            Rename.Column("DateCreated").OnTable("EchoshoreLeakAlerts").To("DatePCNCreated");
        }

        public override void Down()
        {
            Rename.Column("DatePCNCreated").OnTable("EchoshoreLeakAlerts").To("DateCreated");
            Delete.Column("FieldInvestigationRecommendedOn").FromTable("EchoshoreLeakAlerts");
        }
    }
}
