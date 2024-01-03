using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220628135131031)]
    [Tags("Production")]
    public class MC4540_MigrateEquipmentEntriesToFacilityEntries : Migration
    {
        #region Constants

        public struct Tables
        {
            public const string SYSTEM_DELIVERY_FACILITY_ENTRIES = "SystemDeliveryFacilityEntries",
                                SYSTEM_DELIVERY_FACILITY_ENTRY_ADJUSTMENTS = "SystemDeliveryFacilityEntryAdjustments";
        }

        public struct Decimal
        {
            public const int PRECISION = 19,
                             SCALE = 5;
        }

        // Scripts are in the order they'll be executed
        public struct Sql
        {
            public const string SELECT_INTO_TEMP_EQUIPMENT_ENTRIES_TABLE =
                @"SELECT * INTO #TempEquipmentEntries FROM SystemDeliveryEquipmentEntries;";

            // For each day of the week, take the latest adjustment and apply it to it's associated entry.
            public const string APPLY_MONDAY_ADJUSTMENTS_TO_TEMP_EQUIPMENT_ENTRIES =
                @"UPDATE #TempEquipmentEntries
                    SET MondayEntry = (
                        SELECT sdeer.ReversalEntryValue
                        FROM SystemDeliveryEquipmentEntriesReversals sdeer INNER JOIN (
                            SELECT 
                                SystemDeliveryEquipmentEntryId,
                                MAX(DateTimeEntered) AS MaxDate
                            FROM SystemDeliveryEquipmentEntriesReversals
                            GROUP BY SystemDeliveryEquipmentEntryId
                        ) sdeer2 ON sdeer.SystemDeliveryEquipmentEntryId = sdeer2.SystemDeliveryEquipmentEntryId AND 
                                    sdeer.DateTimeEntered = sdeer2.MaxDate    
                            WHERE sdeer.SystemDeliveryEquipmentEntryId = #TempEquipmentEntries.Id AND 
                                  sdeer.DateForReversal = #TempEquipmentEntries.MondayEntryDate
                    )
                    WHERE EXISTS (
                        SELECT SystemDeliveryEquipmentEntryId
                        FROM SystemDeliveryEquipmentEntriesReversals
                        WHERE SystemDeliveryEquipmentEntryId =#TempEquipmentEntries.Id AND 
                              DateForReversal = #TempEquipmentEntries.MondayEntryDate
                );";

            public const string APPLY_TUESDAY_ADJUSTMENTS_TO_TEMP_EQUIPMENT_ENTRIES =
                @"UPDATE #TempEquipmentEntries
                    SET TuesdayEntry = (
                        SELECT sdeer.ReversalEntryValue
                        FROM SystemDeliveryEquipmentEntriesReversals sdeer INNER JOIN (
                            SELECT 
                                SystemDeliveryEquipmentEntryId,
                                MAX(DateTimeEntered) AS MaxDate
                            FROM SystemDeliveryEquipmentEntriesReversals
                            GROUP BY SystemDeliveryEquipmentEntryId
                        ) sdeer2 ON sdeer.SystemDeliveryEquipmentEntryId = sdeer2.SystemDeliveryEquipmentEntryId AND 
                                    sdeer.DateTimeEntered = sdeer2.MaxDate    
                            WHERE sdeer.SystemDeliveryEquipmentEntryId = #TempEquipmentEntries.Id AND 
                                  sdeer.DateForReversal = #TempEquipmentEntries.TuesdayEntryDate
                    )
                    WHERE EXISTS (
                        SELECT SystemDeliveryEquipmentEntryId
                        FROM SystemDeliveryEquipmentEntriesReversals
                        WHERE SystemDeliveryEquipmentEntryId =#TempEquipmentEntries.Id AND 
                              DateForReversal = #TempEquipmentEntries.TuesdayEntryDate
                );";

            public const string APPLY_WEDNESDAY_ADJUSTMENTS_TO_TEMP_EQUIPMENT_ENTRIES =
                @"UPDATE #TempEquipmentEntries
                    SET WednesdayEntry = (
                        SELECT sdeer.ReversalEntryValue
                        FROM SystemDeliveryEquipmentEntriesReversals sdeer INNER JOIN (
                            SELECT 
                                SystemDeliveryEquipmentEntryId,
                                MAX(DateTimeEntered) AS MaxDate
                            FROM SystemDeliveryEquipmentEntriesReversals
                            GROUP BY SystemDeliveryEquipmentEntryId
                        ) sdeer2 ON sdeer.SystemDeliveryEquipmentEntryId = sdeer2.SystemDeliveryEquipmentEntryId AND 
                                    sdeer.DateTimeEntered = sdeer2.MaxDate    
                            WHERE sdeer.SystemDeliveryEquipmentEntryId = #TempEquipmentEntries.Id AND 
                                  sdeer.DateForReversal = #TempEquipmentEntries.WednesdayEntryDate
                    )
                    WHERE EXISTS (
                        SELECT SystemDeliveryEquipmentEntryId
                        FROM SystemDeliveryEquipmentEntriesReversals
                        WHERE SystemDeliveryEquipmentEntryId =#TempEquipmentEntries.Id AND 
                              DateForReversal = #TempEquipmentEntries.WednesdayEntryDate
                );";

            public const string APPLY_THURSDAY_ADJUSTMENTS_TO_TEMP_EQUIPMENT_ENTRIES =
                @"UPDATE #TempEquipmentEntries
                    SET ThursdayEntry = (
                        SELECT sdeer.ReversalEntryValue
                        FROM SystemDeliveryEquipmentEntriesReversals sdeer INNER JOIN (
                            SELECT 
                                SystemDeliveryEquipmentEntryId,
                                MAX(DateTimeEntered) AS MaxDate
                            FROM SystemDeliveryEquipmentEntriesReversals
                            GROUP BY SystemDeliveryEquipmentEntryId
                        ) sdeer2 ON sdeer.SystemDeliveryEquipmentEntryId = sdeer2.SystemDeliveryEquipmentEntryId AND 
                                    sdeer.DateTimeEntered = sdeer2.MaxDate    
                            WHERE sdeer.SystemDeliveryEquipmentEntryId = #TempEquipmentEntries.Id AND 
                                  sdeer.DateForReversal = #TempEquipmentEntries.ThursdayEntryDate
                    )
                    WHERE EXISTS (
                        SELECT SystemDeliveryEquipmentEntryId
                        FROM SystemDeliveryEquipmentEntriesReversals
                        WHERE SystemDeliveryEquipmentEntryId =#TempEquipmentEntries.Id AND 
                              DateForReversal = #TempEquipmentEntries.ThursdayEntryDate
                );";

            public const string APPLY_FRIDAY_ADJUSTMENTS_TO_TEMP_EQUIPMENT_ENTRIES =
                @"UPDATE #TempEquipmentEntries
                    SET FridayEntry = (
                        SELECT sdeer.ReversalEntryValue
                        FROM SystemDeliveryEquipmentEntriesReversals sdeer INNER JOIN (
                            SELECT 
                                SystemDeliveryEquipmentEntryId,
                                MAX(DateTimeEntered) AS MaxDate
                            FROM SystemDeliveryEquipmentEntriesReversals
                            GROUP BY SystemDeliveryEquipmentEntryId
                        ) sdeer2 ON sdeer.SystemDeliveryEquipmentEntryId = sdeer2.SystemDeliveryEquipmentEntryId AND 
                                    sdeer.DateTimeEntered = sdeer2.MaxDate    
                            WHERE sdeer.SystemDeliveryEquipmentEntryId = #TempEquipmentEntries.Id AND 
                                  sdeer.DateForReversal = #TempEquipmentEntries.FridayEntryDate
                    )
                    WHERE EXISTS (
                        SELECT SystemDeliveryEquipmentEntryId
                        FROM SystemDeliveryEquipmentEntriesReversals
                        WHERE SystemDeliveryEquipmentEntryId =#TempEquipmentEntries.Id and 
                              DateForReversal = #TempEquipmentEntries.FridayEntryDate
                );";

            public const string APPLY_SATURDAY_ADJUSTMENTS_TO_TEMP_EQUIPMENT_ENTRIES =
                @"UPDATE #TempEquipmentEntries
                    SET SaturdayEntry = (
                        SELECT sdeer.ReversalEntryValue
                        FROM SystemDeliveryEquipmentEntriesReversals sdeer INNER JOIN (
                            SELECT 
                                SystemDeliveryEquipmentEntryId,
                                MAX(DateTimeEntered) AS MaxDate
                            FROM SystemDeliveryEquipmentEntriesReversals
                            GROUP BY SystemDeliveryEquipmentEntryId
                        ) sdeer2 ON sdeer.SystemDeliveryEquipmentEntryId = sdeer2.SystemDeliveryEquipmentEntryId AND 
                                    sdeer.DateTimeEntered = sdeer2.MaxDate    
                            WHERE sdeer.SystemDeliveryEquipmentEntryId = #TempEquipmentEntries.Id AND 
                                  sdeer.DateForReversal = #TempEquipmentEntries.SaturdayEntryDate
                    )
                    WHERE EXISTS (
                        SELECT SystemDeliveryEquipmentEntryId
                        FROM SystemDeliveryEquipmentEntriesReversals
                        WHERE SystemDeliveryEquipmentEntryId =#TempEquipmentEntries.Id AND 
                              DateForReversal = #TempEquipmentEntries.SaturdayEntryDate
                );";

            public const string APPLY_SUNDAY_ADJUSTMENTS_TO_TEMP_EQUIPMENT_ENTRIES =
                @"UPDATE #TempEquipmentEntries
                    SET SundayEntry = (
                        SELECT sdeer.ReversalEntryValue
                        FROM SystemDeliveryEquipmentEntriesReversals sdeer INNER JOIN (
                            SELECT 
                                SystemDeliveryEquipmentEntryId,
                                MAX(DateTimeEntered) AS MaxDate
                            FROM SystemDeliveryEquipmentEntriesReversals
                            GROUP BY SystemDeliveryEquipmentEntryId
                        ) sdeer2 ON sdeer.SystemDeliveryEquipmentEntryId = sdeer2.SystemDeliveryEquipmentEntryId AND 
                                    sdeer.DateTimeEntered = sdeer2.MaxDate    
                            WHERE sdeer.SystemDeliveryEquipmentEntryId = #TempEquipmentEntries.Id AND 
                                  sdeer.DateForReversal = #TempEquipmentEntries.SundayEntryDate
                    )
                    WHERE EXISTS (
                        SELECT SystemDeliveryEquipmentEntryId
                        FROM SystemDeliveryEquipmentEntriesReversals
                        WHERE SystemDeliveryEquipmentEntryId =#TempEquipmentEntries.Id AND 
                              DateForReversal = #TempEquipmentEntries.SundayEntryDate
                );";
            
            public const string AGGREGATE_EQUIPMENT_ENTRIES_TO_TEMP_FACILITY_ENTRIES =
                @"SELECT
                    SystemDeliveryTypeId,
                    SystemDeliveryEntryId,
                    SystemDeliveryEntryTypeId,
                    EnteredById,
                    FacilityId,
                    SupplierFacilityId,
                    MondayEntryDate,
                    SUM(MondayEntry) AS MondayEntry,                    
                    0 AS IsMondayEntryAnInjection,
                    TuesdayEntryDate,
                    SUM(TuesdayEntry) AS TuesdayEntry,                    
                    0 AS IsTuesdayEntryAnInjection,
                    WednesdayEntryDate,
                    SUM(WednesdayEntry) AS WednesdayEntry,
                    0 AS IsWednesdayEntryAnInjection,                                        
                    ThursdayEntryDate,
                    SUM(ThursdayEntry) AS ThursdayEntry,                    
                    0 AS IsThursdayEntryAnInjection,
                    FridayEntryDate,
                    SUM(FridayEntry) AS FridayEntry,                    
                    0 AS IsFridayEntryAnInjection,
                    SaturdayEntryDate,
                    SUM(SaturdayEntry) AS SaturdayEntry,                    
                    0 AS IsSaturdayEntryAnInjection,
                    SundayEntryDate,
                    SUM(SundayEntry) AS SundayEntry,
                    0 AS IsSundayEntryAnInjection,
                    SUM(MondayEntry + TuesdayEntry + WednesdayEntry + ThursdayEntry + FridayEntry + SaturdayEntry + SundayEntry) AS WeeklyTotal

                INTO #TempFacilityEntries
                FROM #TempEquipmentEntries
                GROUP BY
                    SystemDeliveryTypeId,
                    SystemDeliveryEntryId,
                    SystemDeliveryEntryTypeId,
                    EnteredById,
                    FacilityId, 
                    SupplierFacilityId, 
                    MondayEntryDate, 
                    TuesdayEntryDate, 
                    WednesdayEntryDate, 
                    ThursdayEntryDate, 
                    FridayEntryDate, 
                    SaturdayEntryDate, 
                    SundayEntryDate
                ORDER BY
                    SystemDeliveryEntryId, 
                    SystemDeliveryEntryTypeId;";

            // Moving the facility entries to their new home. The Cross Apply will take the day-specific columns
            // and turn them into rows. Because the equipment entries table structure allowed null values
            // (due to split-week functionality), we need to check for null values - we don't want them
            public const string MIGRATE_AGGREGATED_ENTRIES_TO_FACILITY_ENTRIES =
                @"INSERT INTO SystemDeliveryFacilityEntries (
                    SystemDeliveryTypeId,
                    SystemDeliveryEntryId,
                    SystemDeliveryEntryTypeId,
                    EnteredById,
                    FacilityId,
                    SupplierFacilityId,
                    EntryDate,
                    EntryValue,
                    IsInjection,
                    HasBeenAdjusted)
                SELECT
                    tfe.SystemDeliveryTypeId,
                    tfe.SystemDeliveryEntryId,
                    tfe.SystemDeliveryEntryTypeId,
                    tfe.EnteredById,
                    tfe.FacilityId,
                    tfe.SupplierFacilityId,
                    x.EntryDate,
                    x.EntryValue,
                    x.IsInjection,
                    0
                FROM #TempFacilityEntries tfe
                CROSS APPLY
                    (
                        VALUES
                            (MondayEntryDate, MondayEntry, IsMondayEntryAnInjection),
                            (TuesdayEntryDate, TuesdayEntry, IsTuesdayEntryAnInjection),
                            (WednesdayEntryDate, WednesdayEntry, IsWednesdayEntryAnInjection),
                            (ThursdayEntryDate, ThursdayEntry, IsThursdayEntryAnInjection),
                            (FridayEntryDate, FridayEntry, IsFridayEntryAnInjection),
                            (SaturdayEntryDate, SaturdayEntry, IsSaturdayEntryAnInjection),
                            (SundayEntryDate, SundayEntry, IsSundayEntryAnInjection)
                    ) x (EntryDate, EntryValue, IsInjection)
                WHERE x.EntryDate IS NOT NULL
                AND x.EntryValue IS NOT NULL;";

            // The SystemDeliveryEquipmentEntriesReversals table is really a history table where an entry may
            // have several adjustments. Similar to equipment entries, we'll aggregate the values, group them
            // by facility, and take the latest adjustment
            public const string MIGRATE_EQUIPMENT_ADJUSTMENTS_TO_FACILITY_ADJUSTMENTS =
                @"WITH Adjustments AS (
                    SELECT
                        ROW_NUMBER() OVER(
                            PARTITION BY y.FacilityId, x.DateForReversal, x.SystemDeliveryEntryId ORDER BY x.DateTimeEntered DESC
                        ) AS RowNumber,
                        z.Id AS SystemDeliveryFacilityEntryId,
                        x.SystemDeliveryEntryId,
                        y.FacilityId,
                        x.EnteredById,
                        x.DateForReversal,
                        SUM(x.ReversalEntryValue) OVER(
                            PARTITION BY y.FacilityId, x.DateForReversal, x.SystemDeliveryEntryId
                        ) AS ReversalEntryValue,                        
                        SUM(x.OriginalEntryValue) OVER(
                            PARTITION BY y.FacilityId, x.DateForReversal, x.SystemDeliveryEntryId
                        ) AS OriginalEntryValue,
                        x.DateTimeEntered,
                        x.Comment
                    FROM 
                        SystemDeliveryEquipmentEntriesReversals x 
                        INNER JOIN SystemDeliveryEquipmentEntries y ON 
                            y.Id = x.SystemDeliveryEquipmentEntryId 
                        INNER JOIN SystemDeliveryFacilityEntries z ON 
                            z.SystemDeliveryEntryId = x.SystemDeliveryEntryId AND 
                            z.FacilityId = y.FacilityId AND 
                            z.EntryDate = x.DateForReversal
                )
                INSERT INTO SystemDeliveryFacilityEntryAdjustments (
                    SystemDeliveryFacilityEntryId, 
                    SystemDeliveryEntryId, 
                    FacilityId, 
                    EnteredById, 
                    AdjustedDate, 
                    AdjustedEntryValue, 
                    OriginalEntryValue, 
                    DateTimeEntered, 
                    Comment
                )                
                SELECT 
                    SystemDeliveryFacilityEntryId, 
                    SystemDeliveryEntryId, 
                    FacilityId,
                    EnteredById, 
                    DateForReversal, 
                    ReversalEntryValue, 
                    OriginalEntryValue, 
                    DateTimeEntered, 
                    Comment                 
                FROM Adjustments 
                WHERE Adjustments.ReversalEntryValue != Adjustments.OriginalEntryValue
                AND Adjustments.RowNumber = 1
                ORDER BY SystemDeliveryEntryId, DateForReversal";

            public const string UPDATE_FACILITY_ENTRIES_IF_HAS_BEEN_ADJUSTED = 
                @"UPDATE SystemDeliveryFacilityEntries
                SET HasBeenAdjusted = 1,
                    AdjustmentComment = sdfea.Comment
                FROM 
                    SystemDeliveryFacilityEntries sdfe INNER JOIN
                    SystemDeliveryFacilityEntryAdjustments sdfea ON sdfe.Id = sdfea.SystemDeliveryFacilityEntryId;";
        }

        #endregion

        public override void Up()
        {
            Create.Table(Tables.SYSTEM_DELIVERY_FACILITY_ENTRIES)
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("SystemDeliveryTypeId", "SystemDeliveryTypes", nullable: false)
                  .WithForeignKeyColumn("SystemDeliveryEntryId", "SystemDeliveryEntries", nullable: false)
                  .WithForeignKeyColumn("SystemDeliveryEntryTypeId", "SystemDeliveryEntryTypes", nullable: false)
                  .WithForeignKeyColumn("EnteredById", "tblEmployee", "tblEmployeeId", false)
                  .WithForeignKeyColumn("FacilityId", "tblFacilities", "RecordId", false)
                  .WithForeignKeyColumn("SupplierFacilityId", "tblFacilities", "RecordId")
                  .WithColumn("EntryDate").AsDate().NotNullable()
                  .WithColumn("EntryValue").AsDecimal(Decimal.PRECISION, Decimal.SCALE).NotNullable()
                  .WithColumn("IsInjection").AsBoolean().NotNullable()
                  .WithColumn("HasBeenAdjusted").AsBoolean().NotNullable()
                  .WithColumn("AdjustmentComment")
                  .AsString(100).Nullable();

            Create.Table(Tables.SYSTEM_DELIVERY_FACILITY_ENTRY_ADJUSTMENTS)
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("SystemDeliveryFacilityEntryId", "SystemDeliveryFacilityEntries",
                       nullable: false)
                  .WithForeignKeyColumn("SystemDeliveryEntryId", "SystemDeliveryEntries", nullable: false)
                  .WithForeignKeyColumn("FacilityId", "tblFacilities", "RecordId", false)
                  .WithForeignKeyColumn("EnteredById", "tblEmployee", "tblEmployeeId", false)
                  .WithColumn("AdjustedDate").AsDate().NotNullable()
                  .WithColumn("AdjustedEntryValue").AsDecimal(Decimal.PRECISION, Decimal.SCALE).NotNullable()
                  .WithColumn("OriginalEntryValue").AsDecimal(Decimal.PRECISION, Decimal.SCALE).NotNullable()
                  .WithColumn("DateTimeEntered").AsDateTime().NotNullable()
                  .WithColumn("Comment").AsString(100)
                  .Nullable();

            // Create a temp table to do work so the original data is not disturbed. 
            Execute.Sql(Sql.SELECT_INTO_TEMP_EQUIPMENT_ENTRIES_TABLE);

            // Next, apply adjustments to the temp equipment entries before they're aggregated to the
            // facility level or the connection between adjustments and entries will be lost resulting in incorrect
            // facility totals. Not sure how to do them in one go, so doing day by day.
            Execute.Sql(Sql.APPLY_MONDAY_ADJUSTMENTS_TO_TEMP_EQUIPMENT_ENTRIES);
            Execute.Sql(Sql.APPLY_TUESDAY_ADJUSTMENTS_TO_TEMP_EQUIPMENT_ENTRIES);
            Execute.Sql(Sql.APPLY_WEDNESDAY_ADJUSTMENTS_TO_TEMP_EQUIPMENT_ENTRIES);
            Execute.Sql(Sql.APPLY_THURSDAY_ADJUSTMENTS_TO_TEMP_EQUIPMENT_ENTRIES);
            Execute.Sql(Sql.APPLY_FRIDAY_ADJUSTMENTS_TO_TEMP_EQUIPMENT_ENTRIES);
            Execute.Sql(Sql.APPLY_SATURDAY_ADJUSTMENTS_TO_TEMP_EQUIPMENT_ENTRIES);
            Execute.Sql(Sql.APPLY_SUNDAY_ADJUSTMENTS_TO_TEMP_EQUIPMENT_ENTRIES);

            // Now aggregate the day-specific values by Facility and insert the results into the
            // temp facility entries table.
            Execute.Sql(Sql.AGGREGATE_EQUIPMENT_ENTRIES_TO_TEMP_FACILITY_ENTRIES);

            // Next, migrate the aggregated rows to their final home. In the process,
            // remove the day-specific columns (MondayEntry, TuesdayEntry ...) and turn them into rows
            Execute.Sql(Sql.MIGRATE_AGGREGATED_ENTRIES_TO_FACILITY_ENTRIES);

            // Finally, take all the equipment adjustments, aggregate them, migrate them to the
            // SystemDeliveryFacilityAdjustments table, and associate them to the correct entry
            // in the SystemDeliveryFacilityEntries table.
            Execute.Sql(Sql.MIGRATE_EQUIPMENT_ADJUSTMENTS_TO_FACILITY_ADJUSTMENTS);
            
            // Update HasBeenAdjusted and set AdjustmentComment on faciality entries
            Execute.Sql(Sql.UPDATE_FACILITY_ENTRIES_IF_HAS_BEEN_ADJUSTED);
        }

        public override void Down()
        {
            Delete.Table(Tables.SYSTEM_DELIVERY_FACILITY_ENTRY_ADJUSTMENTS);
            Delete.Table(Tables.SYSTEM_DELIVERY_FACILITY_ENTRIES);
        }
    }
}
