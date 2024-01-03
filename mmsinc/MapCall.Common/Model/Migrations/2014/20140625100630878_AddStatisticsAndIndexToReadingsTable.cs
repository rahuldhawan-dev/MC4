using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140625100630878), Tags("Production")]
    public class AddStatisticsAndIndexToReadingsTable : Migration
    {
        public override void Up()
        {
            Execute.Sql("CREATE STATISTICS [_dta_stat_1871397786_2_1] ON [Readings]([DateTimeStamp], [SensorID])");
            Execute.Sql(@"CREATE NONCLUSTERED INDEX [_dta_index_Readings_9_1871397786__K1_K2_4] ON [dbo].[Readings]
                        (
	                        [SensorID] ASC,
	                        [DateTimeStamp] ASC
                        )
                        INCLUDE ([ScaledData]) ON [PRIMARY]
                        ");
        }

        public override void Down()
        {
            Execute.Sql("DROP STATISTICS [Readings].[_dta_stat_1871397786_2_1]");
            Delete.Index("_dta_index_Readings_9_1871397786__K1_K2_4").OnTable("Readings");
        }
    }
}
