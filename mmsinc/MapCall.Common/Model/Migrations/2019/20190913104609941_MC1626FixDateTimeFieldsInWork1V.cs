using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190913104609941), Tags("Production")]
    public class MC1626FixDateTimeFieldsInWork1V : Migration
    {
        #region Constants

        //TABLES
        public const string ORDERS = "ShortCycleWorkOrders",
                            COMPLETIONS = "ShortCycleWorkOrderCompletions",
                            UPDATES = "ShortCycleWorkOrderStatusUpdates",
                            TIME_CONFIRMATIONS = "ShortCycleWorkOrderTimeConfirmations";

        //UPDATE
        public const string UPDATE_DATE_TIME_WITHOUT_SEPARATORS =
            @"UPDATE {0} SET
                {1}Proper = left({1}, 4) + '-' + 
                substring({1}, 5, 2) + '-' + 
                substring({1}, 7, 2) + ' ' + 
                substring({1}, 9, 2) + ':' + 
                substring({1}, 11, 2) + ':' + 
                replace(substring({1}, 13, 2), '89', '59')
            WHERE
                isDate(left({1}, 4) + '-' + substring({1}, 5, 2) + '-' + substring({1}, 7, 2) + ' ' + substring({1}, 9, 2) + ':' + substring({1}, 11, 2) + ':' + replace(substring({1}, 13, 2), '89', '00')) = 1
                and
                {1} is not null";

        public const string UPDATE_DATE_TIME_WITH_EXISTING_SEPARATORS =
            "UPDATE {0} SET {1}Proper = Convert(DateTime, left({1}, 10) + ' ' + substring({1},11, 8), 120) WHERE isNull({1}, '') <> ''";

        public const string CONSOLIDATE_DATE_TIME =
            @"UPDATE {0} 
                SET {1}DateTime = (left({1}Date, 4) + '-' + substring({1}Date, 5, 2) + '-' + substring({1}Date, 7, 2) + ' ' + substring({1}Time, 1, 2) + ':' + substring({1}Time, 3, 2) + ':' + substring({1}Time, 5, 2))
                WHERE {1}Date is not null
                AND isDate(left({1}Date, 4) + '-' + substring({1}Date, 5, 2) + '-' + substring({1}Date, 7, 2) + ' ' + substring({1}Time, 1, 2) + ':' + substring({1}Time, 3, 2) + ':' + substring({1}Time, 5, 2)) = 1";

        public const string CONSOLIDATE_DATE_TIME_WITH_TIME_SEPARATORS =
            @"UPDATE {0} 
                SET {1}DateTime = (left({1}Date, 4) + '-' + substring({1}Date, 5, 2) + '-' + substring({1}Date, 7, 2) + ' ' + {1}Time)
                WHERE {1}Date is not null
                AND isDate(left({1}Date, 4) + '-' + substring({1}Date, 5, 2) + '-' + substring({1}Date, 7, 2) + ' ' + {1}Time) = 1";

        public const string UPDATE_DATE =
            "update {0} SET {1}Proper = convert(datetime, left({1}, 4)+'-'+substring({1}, 5,2)+'-'+substring({1}, 7,2)) WHERE isNull({1},'') <> ''";

        //ROLLBACK
        public const string ROLLBACK_DATE_TIME =
            "UPDATE {0} SET {1}Old = Replace(Replace(Replace(Convert(VARCHAR, {1}, 120),'-',''), ':', ''), ' ', '')";

        public const string ROLLBACK_DATE_TIME_WITH_SEPARATORS =
            "UPDATE {0} SET {1}Old = Replace(Convert(VARCHAR, {1}, 120),' ','')";

        public const string ROLLBACK_DATE =
            "update {0} SET {1}Old = Replace(convert(varchar, {1}, 23), '-','') WHERE {1} is not null";

        public const string SPLIT_DATE =
            "UPDATE {0} SET {1}Date = Replace(Convert(VARCHAR, {1}DateTime, 23), '-', '') WHERE {1}DateTime IS NOT NULL";

        public const string SPLIT_TIME_WITHOUT_SEPARATORS =
            "UPDATE {0} SET {1}Time = Replace(Convert(VARCHAR, {1}DateTime, 24), ':', '') WHERE {1}DateTime IS NOT NULL";

        public const string SPLIT_TIME_WITH_SEPARATORS =
            "UPDATE {0} SET {1}Time = Convert(VARCHAR, {1}DateTime, 24) WHERE {1}DateTime IS NOT NULL";

        //DATA CLEANUP
        public const string CLEANUP_DATES =
            "UPDATE ShortCycleWorkOrders SET AppointmentEnd = LEFT(appointmentStart, 8) + SUBSTRING(appointmentEnd, 9, 10) WHERE AppointmentEnd like '00000000%'";

        //INDEXES
        public const string UP_RETURN_INDEXES =
            @"CREATE NONCLUSTERED INDEX [ShortCycleWorkOrders_WorkOrder] ON [dbo].[ShortCycleWorkOrders]([WorkOrder] ASC)
        INCLUDE([Id],
            [SAPCommunicationError],
            [FunctionalLocation],
            [NotificationNumber],
            [Priority],
            [BackReportingType],
            [CompanyCode],
            [Premise],
            [Status],
            [WBSElement],
            [PlanningPlant],
            [MATCode],
            [MATCodeDescription],
            [OperationText],
            [FSRId],
            [FSRName],
            [CustomerNumber],
            [NextReplacementYear],
            [ServiceType],
            [MeterSerialNumber],
            [ManufacturerSerialNumber],
            [IsCustomerEnrolledForEmail],
            [IsUpdate],
            [AssignmentStart],
            [AssignmentEnd],
            [OrderType],
            [OperationId],
            [WorkCenter],
            [CustomerAccount],
            [NormalDuration],
            [NormalDurationUnit],
            [PlannerGroup],
            [ActiveMQStatus],
            [PhoneAhead],
            [CustomerAtHome],
            [OperationNumber],
            [ModifiedBy],
            [TimeModified],
            [ReasonCode],
            [DueDateTime],
            [LiabilityIndicator],
            [AppointmentStart],
            [AppointmentEnd],
            [CrewSize],
            [ReceivedAt]) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON[PRIMARY];
CREATE NONCLUSTERED INDEX [_dta_index_ShortCycleWorkOrderStatusUpdates_5_110779602__K9_K5_K3_1_2_6_7_8_10_11_12_13_14_15] ON [dbo].[ShortCycleWorkOrderStatusUpdates]
(
	[StatusNumber] ASC,
	[WorkOrderNumber] ASC,
	[SAPCommunicationError] ASC
)
INCLUDE ( 	[Id],	[ShortCycleWorkOrderId],	[OperationNumber],	[AssignmentStart],	[AssignmentFinish],	[AssignedEngineer],	[DispatcherId],	[EngineerId],	[ReceivedAt],	[ItemTimeStamp],	[StatusNonNumber]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
";

        public const string DOWN_RETURN_INDEXES =
            @"CREATE NONCLUSTERED INDEX [ShortCycleWorkOrders_WorkOrder] ON [dbo].[ShortCycleWorkOrders]([WorkOrder] ASC)
        INCLUDE([Id],
            [SAPCommunicationError],
            [FunctionalLocation],
            [NotificationNumber],
            [Priority],
            [BackReportingType],
            [CompanyCode],
            [Premise],
            [Status],
            [WBSElement],
            [PlanningPlant],
            [MATCode],
            [MATCodeDescription],
            [OperationText],
            [FSRId],
            [FSRName],
            [CustomerNumber],
            [NextReplacementYear],
            [ServiceType],
            [MeterSerialNumber],
            [ManufacturerSerialNumber],
            [IsCustomerEnrolledForEmail],
            [IsUpdate],
            [AssignmentStart],
            [AssignmentEnd],
            [OrderType],
            [OperationId],
            [WorkCenter],
            [CustomerAccount],
            [NormalDuration],
            [NormalDurationUnit],
            [PlannerGroup],
            [ActiveMQStatus],
            [PhoneAhead],
            [CustomerAtHome],
            [OperationNumber],
            [ModifiedBy],
            [TimeModified],
            [ReasonCode],
            [DueDate],
            [DueTime],
            [LiabilityIndicator],
            [AppointmentStart],
            [AppointmentEnd],
            [CrewSize],
            [ReceivedAt]) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON[PRIMARY];
CREATE NONCLUSTERED INDEX [_dta_index_ShortCycleWorkOrderStatusUpdates_5_110779602__K9_K5_K3_1_2_6_7_8_10_11_12_13_14_15] ON [dbo].[ShortCycleWorkOrderStatusUpdates]
(
	[StatusNumber] ASC,
	[WorkOrderNumber] ASC,
	[SAPCommunicationError] ASC
)
INCLUDE ( 	[Id],	[ShortCycleWorkOrderId],	[OperationNumber],	[AssignmentStart],	[AssignmentFinish],	[AssignedEngineer],	[DispatcherId],	[EngineerId],	[ReceivedAt],	[ItemTimeStamp],	[StatusNonNumber]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
";

        #endregion

        #region Private Methods

        private void ModifyDateTimeField(string table, string column, string sql)
        {
            Alter.Table(table).AddColumn($"{column}Proper").AsDateTime().Nullable();
            Execute.Sql(string.Format(sql, table, column));
            Delete.Column(column).FromTable(table);
            Rename.Column($"{column}Proper").OnTable(table).To(column);
        }

        private void RollbackDateTimeField(string table, string column, string sql)
        {
            Alter.Table(table).AddColumn($"{column}Old").AsAnsiString(20).Nullable();
            Execute.Sql(string.Format(sql, table, column));
            Delete.Column(column).FromTable(table);
            Rename.Column($"{column}Old").OnTable(table).To(column);
        }

        private void RollbackDateField(string table, string column, string sql, int columnLength)
        {
            Alter.Table(table).AddColumn($"{column}Old").AsAnsiString(columnLength).Nullable();
            Execute.Sql(string.Format(sql, table, column));
            Delete.Column(column).FromTable(table);
            Rename.Column($"{column}Old").OnTable(table).To(column);
        }

        private void ConsolidateDateTimeField(string table, string column, string sql)
        {
            Alter.Table(table).AddColumn($"{column}DateTime").AsDateTime().Nullable();
            Execute.Sql(string.Format(sql, table, column));
            Delete.Column($"{column}Date").FromTable(table);
            Delete.Column($"{column}Time").FromTable(table);
        }

        private void SplitConsolidatedField(string table, string column, string sqlDate, string sqlTime)
        {
            Alter.Table(table).AddColumn($"{column}Date").AsAnsiString(8).Nullable();
            Alter.Table(table).AddColumn($"{column}Time").AsAnsiString(8).Nullable();
            Execute.Sql(string.Format(sqlDate, table, column));
            Execute.Sql(string.Format(sqlTime, table, column));
            Delete.Column($"{column}DateTime").FromTable(table);
        }

        #endregion

        public override void Up()
        {
            // Kill Indexes
            Execute.Sql("DROP INDEX [ShortCycleWorkOrders_WorkOrder] ON [dbo].[ShortCycleWorkOrders]");
            Execute.Sql(
                "DROP INDEX [_dta_index_ShortCycleWorkOrderStatusUpdates_5_110779602__K9_K5_K3_1_2_6_7_8_10_11_12_13_14_15] ON [dbo].[ShortCycleWorkOrderStatusUpdates]");

            //Cleanup some data
            Execute.Sql(CLEANUP_DATES);

            // Orders
            ModifyDateTimeField(ORDERS, "AssignmentStart", UPDATE_DATE_TIME_WITHOUT_SEPARATORS);
            ModifyDateTimeField(ORDERS, "AssignmentEnd", UPDATE_DATE_TIME_WITHOUT_SEPARATORS);
            ModifyDateTimeField(ORDERS, "AppointmentStart", UPDATE_DATE_TIME_WITHOUT_SEPARATORS);
            ModifyDateTimeField(ORDERS, "AppointmentEnd", UPDATE_DATE_TIME_WITHOUT_SEPARATORS);
            ConsolidateDateTimeField(ORDERS, "Due", CONSOLIDATE_DATE_TIME);

            //// Status Updates
            ModifyDateTimeField(UPDATES, "ItemTimeStamp", UPDATE_DATE_TIME_WITH_EXISTING_SEPARATORS);
            ModifyDateTimeField(UPDATES, "AssignmentStart", UPDATE_DATE_TIME_WITH_EXISTING_SEPARATORS);
            ModifyDateTimeField(UPDATES, "AssignmentFinish", UPDATE_DATE_TIME_WITH_EXISTING_SEPARATORS);

            //// Completions
            ModifyDateTimeField(COMPLETIONS, "TechnicalInspectedOn", UPDATE_DATE);

            // TimeConfirmations
            ModifyDateTimeField(TIME_CONFIRMATIONS, "DateCompleted", UPDATE_DATE);
            ConsolidateDateTimeField(TIME_CONFIRMATIONS, "WorkStart", CONSOLIDATE_DATE_TIME_WITH_TIME_SEPARATORS);
            ConsolidateDateTimeField(TIME_CONFIRMATIONS, "WorkFinish", CONSOLIDATE_DATE_TIME_WITH_TIME_SEPARATORS);

            Execute.Sql(UP_RETURN_INDEXES);
        }

        public override void Down()
        {
            // Kill Indexes
            Execute.Sql("DROP INDEX [ShortCycleWorkOrders_WorkOrder] ON [dbo].[ShortCycleWorkOrders]");
            Execute.Sql(
                "DROP INDEX [_dta_index_ShortCycleWorkOrderStatusUpdates_5_110779602__K9_K5_K3_1_2_6_7_8_10_11_12_13_14_15] ON [dbo].[ShortCycleWorkOrderStatusUpdates]");

            // Orders
            RollbackDateTimeField(ORDERS, "AssignmentStart", ROLLBACK_DATE_TIME);
            RollbackDateTimeField(ORDERS, "AssignmentEnd", ROLLBACK_DATE_TIME);
            RollbackDateTimeField(ORDERS, "AppointmentStart", ROLLBACK_DATE_TIME);
            RollbackDateTimeField(ORDERS, "AppointmentEnd", ROLLBACK_DATE_TIME);
            SplitConsolidatedField(ORDERS, "Due", SPLIT_DATE, SPLIT_TIME_WITHOUT_SEPARATORS);

            //Updates
            RollbackDateTimeField(UPDATES, "ItemTimeStamp", ROLLBACK_DATE_TIME_WITH_SEPARATORS);
            RollbackDateTimeField(UPDATES, "AssignmentStart", ROLLBACK_DATE_TIME_WITH_SEPARATORS);
            RollbackDateTimeField(UPDATES, "AssignmentFinish", ROLLBACK_DATE_TIME_WITH_SEPARATORS);

            //// Completions
            //// TechnicalInspectedOn
            RollbackDateField(COMPLETIONS, "TechnicalInspectedOn", ROLLBACK_DATE, 12);

            // Time Confirmations
            RollbackDateField(TIME_CONFIRMATIONS, "DateCompleted", ROLLBACK_DATE, 8);
            SplitConsolidatedField(TIME_CONFIRMATIONS, "WorkStart", SPLIT_DATE, SPLIT_TIME_WITH_SEPARATORS);
            SplitConsolidatedField(TIME_CONFIRMATIONS, "WorkFinish", SPLIT_DATE, SPLIT_TIME_WITH_SEPARATORS);

            Execute.Sql(DOWN_RETURN_INDEXES);
        }
    }
}
