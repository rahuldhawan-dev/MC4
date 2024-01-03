using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20181129083613221), Tags("Production")]
    public class MC756_IncreaseMarkoutDamageRequestNumberLength : Migration
    {
        public override void Up()
        {
            Alter.Column("RequestNum")
                 .OnTable("MarkoutDamages")
                 .AsString(20).Nullable();
        }

        public override void Down()
        {
            // No down migration as it will cause data truncation.
        }
    }
}
