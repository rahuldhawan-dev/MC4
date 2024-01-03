using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180912124738675), Tags("Production")]
    public class TrimWorkOrderOnShortCycleWorkOrders : Migration
    {
        public override void Up()
        {
            Delete.Index("UIX_ShortCycleWorkOrders_WorkOrder").OnTable("ShortCycleWorkOrders");
            Execute.Sql(
                "IF EXISTS (Select 1 from sysindexes where Name = 'IX_ShortCycleWorkOrders_Id_WorkOrder') DROP INDEX [IX_ShortCycleWorkOrders_Id_WorkOrder] ON [dbo].[ShortCycleWorkOrders]");
            Execute.Sql(
                "IF EXISTS (Select 1 from sysindexes where Name = 'ShortCycleWorkOrders_WorkOrder') DROP INDEX [ShortCycleWorkOrders_WorkOrder] ON [dbo].[ShortCycleWorkOrders]");
            Alter.Column("WorkOrder").OnTable("ShortCycleWorkOrders").AsAnsiString(12)
                 .Unique("UIX_ShortCycleWorkOrders_WorkOrder").Nullable();
            Execute.Sql(
                @"CREATE NONCLUSTERED INDEX IX_ShortCycleWorkOrders_Id_WorkOrder ON ShortCycleWorkOrders (Id, WorkOrder)");
            Execute.Sql(
                "CREATE NONCLUSTERED INDEX [ShortCycleWorkOrders_WorkOrder] ON [dbo].[ShortCycleWorkOrders](	[WorkOrder] ASC)INCLUDE ([Id], [SAPCommunicationError], [FunctionalLocation], [NotificationNumber], [Priority], [BackReportingType], [CompanyCode], [Premise], [Status], [WBSElement], [PlanningPlant], [MATCode], [MATCodeDescription], [OperationText], [FSRId], [FSRName], [CustomerNumber], [NextReplacementYear], [ServiceType], [MeterSerialNumber], [ManufacturerSerialNumber], [IsCustomerEnrolledForEmail], [IsUpdate], [AssignmentStart], [AssignmentEnd], [OrderType], [OperationId], [WorkCenter], [CustomerAccount], [NormalDuration ], [NormalDurationUnit ], [PlannerGroup], [ActiveMQStatus], [PhoneAhead], [CustomerAtHome], [OperationNumber], [ModifiedBy], [TimeModified], [ReasonCode], [DueDate], [DueTime], [LiabilityIndicator], [AppointmentStart], [AppointmentEnd], [CrewSize], [ReceivedAt]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]");
        }

        public override void Down() { }
    }
}
