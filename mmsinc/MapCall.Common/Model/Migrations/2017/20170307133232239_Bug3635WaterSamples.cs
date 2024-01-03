using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170307133232239), Tags("Production")]
    public class Bug3635WaterSamples : Migration
    {
        public override void Up()
        {
            Alter.Table("WaterSamples")
                 .AddColumn("NonDetect").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("NonDetect").FromTable("WaterSamples");
        }
    }
}
