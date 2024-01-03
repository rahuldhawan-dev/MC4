using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230901083524815), Tags("Production")]
    public class MC6169_CreateIndexes : Migration
    {
        public const string CREATE_INDEXES = @"
if not exists (select 1 from sysindexes where name = '_dta_index_Valves_5_1031010754__K57_K49_K25_45')
CREATE NONCLUSTERED INDEX [_dta_index_Valves_5_1031010754__K57_K49_K25_45] ON [dbo].[Valves]
(
	[OperatingCenterId] ASC,
	[DateInstalled] ASC,
	[Id] ASC
)
INCLUDE([WorkOrderNumber]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]

if not exists (select 1 from sysindexes where name = '_dta_index_WorkOrders_5_1888777836__K35_K39')

CREATE NONCLUSTERED INDEX [_dta_index_WorkOrders_5_1888777836__K35_K39] ON [dbo].[WorkOrders]
(
	[WorkDescriptionID] ASC,
	[ValveID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
",
                            DROP_INDEXES =
                                "drop index _dta_index_Valves_5_1031010754__K57_K49_K25_45 on valves;drop index _dta_index_WorkOrders_5_1888777836__K35_K39 on valves";
        
        public override void Up()
        {
            Execute.Sql(CREATE_INDEXES);
        }

        public override void Down()
        {
            Execute.Sql(DROP_INDEXES);
        }
    }
}

