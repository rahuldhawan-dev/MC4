using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170309102205383), Tags("Production")]
    public class Bug3656 : Migration
    {
        public override void Up()
        {
            Alter.Table("WaterSamples")
                 .AddColumn("IsInvalid").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("IsInvalid").FromTable("WaterSamples");
        }
    }
}
