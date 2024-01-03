using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180626115120586), Tags("Production")]
    public class MC404BacterialWaterSampleFlushTimeField : Migration
    {
        public override void Up()
        {
            Create.Column("FlushTimeMinutes").OnTable("BacterialWaterSamples")
                  .AsDecimal(6, 2).Nullable();
        }

        public override void Down()
        {
            Delete.Column("FlushTimeMinutes").FromTable("BacterialWaterSamples");
        }
    }
}
