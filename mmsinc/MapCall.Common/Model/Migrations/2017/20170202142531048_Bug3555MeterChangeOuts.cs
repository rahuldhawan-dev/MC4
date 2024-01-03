using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170202142531048), Tags("Production")]
    public class Bug3555MeterChangeOuts : Migration
    {
        public override void Up()
        {
            Create.Column("CutBolts").OnTable("MeterChangeOuts")
                  .AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("CutBolts").FromTable("MeterChangeOuts");
        }
    }
}
