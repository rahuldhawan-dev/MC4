using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140626150451169), Tags("Production")]
    public class AddIndexesStatisticsForBug1966 : Migration
    {
        #region Constants

        public const string CREATE_INDEXES_STATISTICS =
                                @"IF NOT EXISTS (SELECT 1 from sysindexes where Name = '_dta_index_WorkOrders_14_1888777836__K35_K48_K23_K1')
                BEGIN

                CREATE NONCLUSTERED INDEX [_dta_index_WorkOrders_14_1888777836__K35_K48_K23_K1] ON [dbo].[WorkOrders]
                (
	                [WorkDescriptionID] ASC,
	                [OperatingCenterID] ASC,
	                [DateCompleted] ASC,
	                [WorkOrderID] ASC
                )WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
                END
              IF NOT EXISTS (SELECT 1 from sysindexes where name = '_dta_stat_1888777836_1_48_23_35')
                BEGIN
	                CREATE STATISTICS [_dta_stat_1888777836_1_48_23_35] ON [dbo].[WorkOrders]([WorkOrderID], [OperatingCenterID], [DateCompleted], [WorkDescriptionID])
                END

              IF NOT EXISTS (SELECT 1 from sysindexes where name = '_dta_stat_720773675_2_19')
                BEGIN
	                CREATE STATISTICS [_dta_stat_720773675_2_19] ON [dbo].[Restorations]([WorkOrderID], [FinalRestorationDate])
                END",
                            ROLLBACK_INDEXES_STATISTIC_S = @"
                DROP INDEX [_dta_index_WorkOrders_14_1888777836__K35_K48_K23_K1] ON [dbo].[WorkOrders]
                DROP STATISTICS [WorkOrders].[_dta_stat_1888777836_1_48_23_35] 
                DROP STATISTICS [Restorations].[_dta_stat_720773675_2_19]
            ";

        #endregion

        public override void Up()
        {
            Execute.Sql(CREATE_INDEXES_STATISTICS);
        }

        public override void Down()
        {
            Execute.Sql(ROLLBACK_INDEXES_STATISTIC_S);
        }
    }
}
