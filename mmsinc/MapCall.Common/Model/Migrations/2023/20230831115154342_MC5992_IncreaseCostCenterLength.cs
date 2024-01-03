using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230831115154342), Tags("Production")]
    public class MC5992_IncreaseCostCenterLength : Migration
    {
        public const string DROP_INDEX =
                                "DROP INDEX [_dta_index_WorkOrders_19_1888777836__K54_1_2_3_5_6_7_8_9_10_11_13_14_15_16_19_20_23_25_26_27_28_29_30_31_34_35_36_37_38_39_40_] ON [dbo].[WorkOrders]",
                            CREATE_INDEX =
                                @"CREATE NONCLUSTERED INDEX [_dta_index_WorkOrders_19_1888777836__K54_1_2_3_5_6_7_8_9_10_11_13_14_15_16_19_20_23_25_26_27_28_29_30_31_34_35_36_37_38_39_40_] ON [dbo].[WorkOrders]
                                    ([SewerOpeningId] ASC) INCLUDE([WorkOrderID],[OldWorkOrderNumber],[CreatedAt],[DateReceived],[DateStarted],[CustomerName],[StreetNumber],[StreetID],[NearestCrossStreetID],[TownID],[ZipCode],[PhoneNumber],[SecondaryPhoneNumber],[CustomerAccountNumber],[ServiceNumber],[ORCOMServiceOrderNumber],[DateCompleted],[DatePrinted],[DateReportSent],[BackhoeOperator],[ExcavationDate],[DateCompletedPC],[PremiseNumber],[InvoiceNumber],[AssetTypeID],[WorkDescriptionID],[MarkoutRequirementID],[TrafficControlRequired],[StreetOpeningPermitRequired],[ValveID],[HydrantID],[LostWater],[NumberOfOfficersRequired],[Latitude],[Longitude],[OperatingCenterID],[ApprovedOn],[MaterialsApprovedOn],[MaterialsDocID],[CompletedByID],[DistanceFromCrossStreet],[OfficeAssignedOn],[OriginalOrderNumber],[BusinessUnit],[StormCatchID],[SignificantTrafficImpact],[AlertID],[MarkoutToBeCalled],[AccountCharged],[SAPNotificationNumber],[SAPWorkOrderNumber]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]";
        public override void Up()
        {
            Execute.Sql(DROP_INDEX);
            Alter.Column("BusinessUnit").OnTable("WorkOrders").AsString(256).Nullable();
            Execute.Sql(CREATE_INDEX);
        }

        public override void Down()
        {
            // noop, can't go back
        }
    }
}

