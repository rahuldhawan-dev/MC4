using FluentMigrator;
using FluentMigrator.Expressions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20210106141842689), Tags("Production")]
    public class MC2827AddAdditionalColumnToEnvironmentalNCRecords : Migration
    {
        public override void Up()
        {
            Alter.Table("EnvironmentalNonComplianceEvents")
                 .AddColumn("CountsAgainstTarget").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("CountsAgainstTarget").FromTable("EnvironmentalNonComplianceEvents");
        }
    }
}
