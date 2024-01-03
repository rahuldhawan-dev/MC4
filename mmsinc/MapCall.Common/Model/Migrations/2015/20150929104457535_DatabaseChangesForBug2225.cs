using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150929104457535), Tags("Production")]
    public class DatabaseChangesForBug2225 : Migration
    {
        private const string SEWER_MANHOLES = "SewerManholes",
                             SEWER_MANHOLE_CONNECTIONS = "SewerManholeConnections",
                             SEWER_MAIN_CLEANINGS = "SewerMainCleanings";

        public struct StringLengths
        {
            public const int CREATED_BY = 50, STREET_NUMBER = 10;
        }

        public struct Sql
        {
            public const string
                DROP_STATISTICS =
                    "DROP STATISTICS [dbo].[SewerMainCleanings].[STAT_CleaningScheduleID_Manhole2];" +
                    "DROP STATISTICS [dbo].[SewerMainCleanings].[STAT_CodeTypeID_Manhole2];" +
                    "DROP STATISTICS [dbo].[SewerMainCleanings].[STAT_ConditionofManhole2ID_ConditionofManhole1ID];" +
                    "DROP STATISTICS [dbo].[SewerMainCleanings].[STAT_HydrantUserID_Manhole2];" +
                    "DROP STATISTICS [dbo].[SewerMainCleanings].[STAT_IntersectingStreetID_StreetID_Manhole2];" +
                    "DROP STATISTICS [dbo].[SewerMainCleanings].[STAT_Manhole1FrameAndCoverID_Manhole2];" +
                    "DROP STATISTICS [dbo].[SewerMainCleanings].[STAT_OperatingCenterID_Manhole2];" +
                    "DROP STATISTICS [dbo].[SewerMainCleanings].[STAT_PayTypeID_Manhole2];" +
                    "DROP STATISTICS [dbo].[SewerMainCleanings].[STAT_TownID_Manhole2];",
                CREATE_STATISTICS =
                    "CREATE STATISTICS [STAT_CleaningScheduleID_Manhole2] ON [dbo].[SewerMainCleanings]([CleaningScheduleID], [Manhole2], [SewerMainCleaningID]);" +
                    "CREATE STATISTICS [STAT_CodeTypeID_Manhole2] ON [dbo].[SewerMainCleanings]([CodeTypeID], [Manhole2], [SewerMainCleaningID]);" +
                    "CREATE STATISTICS [STAT_ConditionofManhole2ID_ConditionofManhole1ID] ON [dbo].[SewerMainCleanings]([ConditionofManhole2ID], [ConditionofManhole1ID], [Manhole2], [SewerMainCleaningID]);" +
                    "CREATE STATISTICS [STAT_HydrantUserID_Manhole2] ON [dbo].[SewerMainCleanings]([HydrantUsedID], [Manhole2], [SewerMainCleaningID]);" +
                    "CREATE STATISTICS [STAT_IntersectingStreetID_StreetID_Manhole2] ON [dbo].[SewerMainCleanings]([IntersectingStreetID], [StreetID], [Manhole2], [SewerMainCleaningID]);" +
                    "CREATE STATISTICS [STAT_Manhole1FrameAndCoverID_Manhole2] ON [dbo].[SewerMainCleanings]([Manhole1FrameAndCoverID], [Manhole2], [SewerMainCleaningID]);" +
                    "CREATE STATISTICS [STAT_OperatingCenterID_Manhole2] ON [dbo].[SewerMainCleanings]([OperatingCenterID], [Manhole2], [SewerMainCleaningID]);" +
                    "CREATE STATISTICS [STAT_PayTypeID_Manhole2] ON [dbo].[SewerMainCleanings]([PayTypeID], [Manhole2], [SewerMainCleaningID]);" +
                    "CREATE STATISTICS [STAT_TownID_Manhole2] ON [dbo].[SewerMainCleanings]([TownID], [Manhole2], [SewerMainCleaningID])",
                CREATE_INDEXES =
                    @"CREATE NONCLUSTERED INDEX [_dta_index_WorkOrders_19_1888777836__K54_1_2_3_5_6_7_8_9_10_11_13_14_15_16_19_20_23_25_26_27_28_29_30_31_34_35_36_37_38_39_40_] ON [dbo].[WorkOrders] ([SewerManholeID] ASC)
INCLUDE (  [WorkOrderID],[OldWorkOrderNumber], [CreatedOn], [DateReceived], [DateStarted], [CustomerName], [StreetNumber], [StreetID], [NearestCrossStreetID], [TownID],
 [ZipCode], [PhoneNumber], [SecondaryPhoneNumber], [CustomerAccountNumber], [ServiceNumber], [ORCOMServiceOrderNumber], [DateCompleted], [DatePrinted], [DateReportSent],
 [BackhoeOperator], [ExcavationDate], [DateCompletedPC], [PremiseNumber], [InvoiceNumber], [AssetTypeID], [WorkDescriptionID], [MarkoutRequirementID], [TrafficControlRequired],
 [StreetOpeningPermitRequired], [ValveID], [HydrantID], [LostWater], [NumberOfOfficersRequired], [Latitude], [Longitude], [OperatingCenterID], [ApprovedOn], [MaterialsApprovedOn],
 [MaterialsDocID], [CompletedByID], [DistanceFromCrossStreet], [OfficeAssignedOn], [OriginalOrderNumber], [BusinessUnit],
 [StormCatchID], [SignificantTrafficImpact], [AlertID], [MarkoutToBeCalled], [AccountCharged], [SAPNotificationNumber], [SAPWorkOrderNumber]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
CREATE NONCLUSTERED INDEX [_dta_index_Note_19_2075922517__K4_K5] ON [dbo].[Note](  [DataLinkID] ASC,  [DataTypeID] ASC )WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
CREATE STATISTICS [_dta_stat_2075922517_5_4] ON [dbo].[Note]([DataTypeID], [DataLinkID])
CREATE NONCLUSTERED INDEX [_dta_index_SewerMainCleanings_19_1225823479__K39_1_2_3_4_5_6_7_8_9_13_14_16_17_18_20_21_22_24_25_26_28_35_37_40_41_42_44_45_46_] ON [dbo].[SewerMainCleanings]
( [Manhole1] ASC)
INCLUDE (  [SewerMainCleaningID], [OperatingCenterID], [Date], [StreetID], [IntersectingStreetID], [TownID], [PayTypeID], [CodeTypeID], [RootCutter],
 [FlushWaterVolume], [HydrantUsedID], [FootageofMainCleaned], [ManholeCatchbasin1], [ConditionofManhole1ID], [Manhole1FrameAndCoverID], [ManholeCatchbasin2], [ConditionofManhole2ID],
 [Manhole2FrameAndCoverID], [MapPage], [NumberOfEmployees], [Overflow], [CleaningScheduleID], [OverflowManhole1], [Manhole2], [OverflowManhole2], [IntersectingStreet2],
 [CreatedBy], [CreatedOn], [CompletedDate], [SewerOverflowId]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
CREATE NONCLUSTERED INDEX [_dta_index_SewerMainCleanings_19_1225823479__K41_1_2_3_4_5_6_7_8_9_13_14_16_17_18_20_21_22_24_25_26_28_35_37_39_40_42_44_45_46_] ON [dbo].[SewerMainCleanings]
( [Manhole2] ASC)
INCLUDE (  [SewerMainCleaningID], [OperatingCenterID], [Date], [StreetID], [IntersectingStreetID], [TownID], [PayTypeID], [CodeTypeID], [RootCutter],
 [FlushWaterVolume], [HydrantUsedID], [FootageofMainCleaned], [ManholeCatchbasin1], [ConditionofManhole1ID], [Manhole1FrameAndCoverID], [ManholeCatchbasin2], [ConditionofManhole2ID],
 [Manhole2FrameAndCoverID], [MapPage], [NumberOfEmployees], [Overflow], [CleaningScheduleID], [Manhole1], [OverflowManhole1], [OverflowManhole2], [IntersectingStreet2],
 [CreatedBy], [CreatedOn], [CompletedDate], [SewerOverflowId]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]",
                DROP_INDEXES =
                    @"DROP INDEX [_dta_index_WorkOrders_19_1888777836__K54_1_2_3_5_6_7_8_9_10_11_13_14_15_16_19_20_23_25_26_27_28_29_30_31_34_35_36_37_38_39_40_] ON [dbo].[WorkOrders]
DROP INDEX [_dta_index_Note_19_2075922517__K4_K5] ON [dbo].[Note]
DROP STATISTICS [dbo].[Note].[_dta_stat_2075922517_5_4]
DROP INDEX [_dta_index_SewerMainCleanings_19_1225823479__K39_1_2_3_4_5_6_7_8_9_13_14_16_17_18_20_21_22_24_25_26_28_35_37_40_41_42_44_45_46_] ON [dbo].[SewerMainCleanings]
DROP INDEX [_dta_index_SewerMainCleanings_19_1225823479__K41_1_2_3_4_5_6_7_8_9_13_14_16_17_18_20_21_22_24_25_26_28_35_37_39_40_42_44_45_46_] ON [dbo].[SewerMainCleanings]
";
        }

        public override void Up()
        {
            this.DeleteIndexIfItExists(SEWER_MANHOLES, "IDX_SewerManholes_TownID_ManholeNumber");
            this.DeleteStatisticIfItExits(SEWER_MANHOLES, "_dta_stat_2025826329_1_4");
            Alter.Column("Description").OnTable("AssetStatuses").AsString(50).NotNullable();
            Alter.Column("ManholeSuffix").OnTable(SEWER_MANHOLES).AsInt32().NotNullable();
            Alter.Column("ManholeNumber").OnTable(SEWER_MANHOLES).AsString(50).NotNullable();

            Alter.Table(SEWER_MANHOLES).AddColumn("StreetNumber").AsAnsiString(StringLengths.STREET_NUMBER).Nullable();

            Alter.Table(SEWER_MANHOLE_CONNECTIONS)
                 .AlterColumn("InspFreq").AsInt32().Nullable();

            Alter.Table(SEWER_MANHOLE_CONNECTIONS)
                 .AddColumn("InspectionFrequencyUnitId").AsInt32().Nullable()
                 .ForeignKey("FK_SewerManholes_RecurringFrequencyUnits_InspectionFrequencyUnitId",
                      "RecurringFrequencyUnits", "Id");

            Alter.Table(SEWER_MANHOLE_CONNECTIONS)
                 .AddColumn("Stop").AsInt32().Nullable();

            Execute.Sql(
                "UPDATE SewerManholeConnections SET Stop = cast(right(str(route, 8, 4), 4) as int) where Route is not null");
            Execute.Sql(
                "UPDATE SewerManholeConnections SET Route = cast(left(str(route, 8, 4), 3) as int) where Route is not null");

            Alter.Column("Route").OnTable(SEWER_MANHOLE_CONNECTIONS).AsInt32().Nullable();

            Execute.Sql(
                "update SewerManholeConnections set CreatedBy = (select recID from tblPermissions where username = createdBy);");
            Alter.Table(SEWER_MANHOLE_CONNECTIONS).AlterForeignKeyColumn("CreatedBy", "tblPermissions", "RecId");

            // There's a "None" value that I'm ignoring. It doesn't appear to be used anywhere so it can just stick
            // with being null.
            Execute.Sql(
                "UPDATE [SewerManholeConnections] SET InspectionFrequencyUnitId = (select top 1 Id from RecurringFrequencyUnits where Description = 'Day') WHERE InspFreqUnit = 'D'");
            Execute.Sql(
                "UPDATE [SewerManholeConnections] SET InspectionFrequencyUnitId = (select top 1 Id from RecurringFrequencyUnits where Description = 'Year') WHERE InspFreqUnit = 'Y'");

            Execute.Sql("update SewerManholes set CreatedBy = 'mcadmin' where CreatedBy = 'import';" +
                        "update SewerManholes set CreatedBy = (select recID from tblPermissions where username = createdBy);");

            Alter.Table("SewerManholes").AlterForeignKeyColumn("CreatedBy", "tblPermissions", "RecId");

            Delete.Column("InspFreqUnit").FromTable(SEWER_MANHOLE_CONNECTIONS);

            Execute.Sql(
                "update SewerMainCleanings set Manhole1 = null where Manhole1 is not null and Manhole1 not in (select SewerManholeID from SewerManholes);" +
                "update SewerMainCleanings set Manhole2 = null where Manhole2 is not null and Manhole2 not in (select SewerManholeID from SewerManholes);");

            Execute.Sql(Sql.DROP_STATISTICS);
            Alter.Table(SEWER_MAIN_CLEANINGS).AlterForeignKeyColumn("HydrantUsedId", "Hydrants");
            Alter.Column("NumberofEmployees").OnTable(SEWER_MAIN_CLEANINGS).AsInt32().Nullable();
            Alter.Column("Manhole1").OnTable(SEWER_MAIN_CLEANINGS).AsInt32().Nullable();
            Alter.Table(SEWER_MAIN_CLEANINGS).AlterForeignKeyColumn("Manhole1", "SewerManholes", "SewerManholeID");
            Alter.Column("Manhole2").OnTable(SEWER_MAIN_CLEANINGS).AsInt32().Nullable();
            Alter.Table(SEWER_MAIN_CLEANINGS).AlterForeignKeyColumn("Manhole2", "SewerManholes", "SewerManholeID");
            Alter.Column("FlushWaterVolume").OnTable(SEWER_MAIN_CLEANINGS).AsInt32().Nullable();

            Execute.Sql("update SewerMainCleanings set CreatedBy = 'mcadmin' where CreatedBy = 'import';" +
                        "update SewerMainCleanings set CreatedBy = (select recID from tblPermissions where username = createdBy);");

            Alter.Table(SEWER_MAIN_CLEANINGS).AlterForeignKeyColumn("CreatedBy", "tblPermissions", "RecId");

            Alter.Table(SEWER_MAIN_CLEANINGS)
                 .AddForeignKeyColumn("SewerOverflowId", "SewerOverflows", "SewerOverflowID");
            Execute.Sql(
                "Update SewerMainCleanings set SewerOverflowId = (Select Top 1 SewerOverflowID from SewerOverflows where SewerOverflows.SewerMainCleaningID = SewerMainCleanings.SewerMainCleaningId);");

            Delete.Column("SewerMainCleaningId").FromTable("SewerOverflows");
            Execute.Sql(Sql.CREATE_INDEXES);

            Delete.Column("OldWorkOrderID").FromTable(SEWER_MAIN_CLEANINGS);

            Execute.Sql(@"
update SewerMainCleanings set PayTypeID = null where PayTypeId = (Select PayTypeID FROM PayTypes where PayType in ('Root 24', 'VideoType'))
DELETE FROM PayTypes where PayType in ('Root 24', 'Video Pipe')
select * from PayTypes

update PayTypes set PayType = 'UNPLANNED-STRAIGHT TIME' where PayTypeID = 1 --	Non-Scheduled-Straight Time
update PayTypes set PayType = 'PLANNED-STRAIGHT TIME' where PayTypeID = 2 --	Scheduled-Straight Time
update PayTypes set PayType = 'PLANNED-OT' where PayTypeID = 3 --	Scheduled-OT
update PayTypes set PayType = 'UNPLANNED-OT' where PayTypeID = 5 --	Non-Scheduled-OT
Update PayTypes set PayType = upper(payType)

Update SewerMainCleanings set CodeTypeID = (Select CodeTypeID from CodeTypes where codeType = 'CLEANING FOR TV') where CodeTypeID in  (Select CodeTypeID from CodeTypes where codeType in ('VIDEO PIPE','CLEAN & TELEVISE'))
UPDATE CodeTypes set CodeType = 'CLEANING FOR VIDEO' where CodeType = 'CLEANING FOR TV'
DELETE FROM CodeTypes where CodeType In ('VIDEO PIPE','CLEAN & TELEVISE')
UPdate CodeTypes set CodeType  = upper(codetype)
UPDATE ManholeConditions SET ManholeCondition = UPPER(ManholeCondition)
UPDATE ManholeFrameAndCovers SET ManholeFrameAndCover = UPPER(ManholeFrameAndCover)
update SewerMainCleanings SET IntersectingSTreetID = null where IntersectingSTreetID = 39242
");
        }

        public override void Down()
        {
            Execute.Sql(@"
INSERT INTO PayTypes Values('Root 24')
INSERT INTO PayTypes Values('Video Pipe')
update PayTypes set PayType = 'Non-Scheduled-Straight Time' where PayTypeID = 1 --	Non-Scheduled-Straight Time
update PayTypes set PayType = 'Scheduled-Straight Time' where PayTypeID = 2 --	Scheduled-Straight Time
update PayTypes set PayType = 'Scheduled-OT' where PayTypeID = 3 --	Scheduled-OT
update PayTypes set PayType = 'Non-Scheduled-OT' where PayTypeID = 5 --	Non-Scheduled-OT
INSERT INTO CodeTypes Values('VIDEO PIPE')
INSERT INTO CodeTypes Values('CLEAN & TELEVISE')
UPDATE CodeTypes set CodeType = 'CLEANING FOR TV' where CodeType = 'CLEANING FOR VIDEO'");
            Alter.Table(SEWER_MAIN_CLEANINGS).AddColumn("OldWorkOrderID").AsInt32().Nullable();
            Execute.Sql(Sql.DROP_INDEXES);
            Alter.Table("SewerOverflows").AddColumn("SewerMainCleaningId").AsInt32().Nullable();
            Execute.Sql(
                "Update SewerOverflows SET SewerMainCleaningId = (Select SewerMainCleaningID from SewerMainCleanings WHERE SewerMainCleanings.SewerOverflowID = SewerOverFLows.SewerOverflowId)");
            Delete.ForeignKeyColumn(SEWER_MAIN_CLEANINGS, "SewerOverflowId", "SewerOverflows", "SewerOverflowId");

            Delete.ForeignKey(Utilities.CreateForeignKeyName(SEWER_MAIN_CLEANINGS, "tblPermissions", "CreatedBy"))
                  .OnTable(SEWER_MAIN_CLEANINGS);
            Alter.Table(SEWER_MAIN_CLEANINGS).AlterColumn("CreatedBy").AsAnsiString(StringLengths.CREATED_BY)
                 .Nullable();
            Execute.Sql(
                "Update SewerMainCleanings set CreatedBy = (select userName from tblPermissions where RecID = createdBy)");

            Alter.Column("FlushWaterVolume").OnTable(SEWER_MAIN_CLEANINGS).AsCustom("float").Nullable();
            Alter.Column("NumberOfEmployees").OnTable(SEWER_MAIN_CLEANINGS).AsCustom("float").Nullable();
            Delete.ForeignKey(Utilities.CreateForeignKeyName(SEWER_MAIN_CLEANINGS, "SewerManholes", "Manhole1"))
                  .OnTable(SEWER_MAIN_CLEANINGS);
            Alter.Column("Manhole1").OnTable(SEWER_MAIN_CLEANINGS).AsAnsiString(255).Nullable();
            Delete.ForeignKey(Utilities.CreateForeignKeyName(SEWER_MAIN_CLEANINGS, "SewerManholes", "Manhole2"))
                  .OnTable(SEWER_MAIN_CLEANINGS);
            Alter.Column("Manhole2").OnTable(SEWER_MAIN_CLEANINGS).AsAnsiString(255).Nullable();
            Delete.ForeignKey(Utilities.CreateForeignKeyName(SEWER_MAIN_CLEANINGS, "Hydrants", "HydrantUsedId"))
                  .OnTable(SEWER_MAIN_CLEANINGS);
            Execute.Sql(Sql.CREATE_STATISTICS);

            Delete.ForeignKey(Utilities.CreateForeignKeyName(SEWER_MANHOLES, "tblPermissions", "CreatedBy"))
                  .OnTable(SEWER_MANHOLES);
            Delete.ForeignKey(Utilities.CreateForeignKeyName(SEWER_MANHOLE_CONNECTIONS, "tblPermissions", "CreatedBy"))
                  .OnTable(SEWER_MANHOLE_CONNECTIONS);
            Alter.Table(SEWER_MANHOLES).AlterColumn("CreatedBy").AsAnsiString(StringLengths.CREATED_BY).Nullable();
            Alter.Table(SEWER_MANHOLE_CONNECTIONS).AlterColumn("CreatedBy").AsAnsiString(StringLengths.CREATED_BY)
                 .Nullable();
            Execute.Sql(
                "Update SewerManholes set CreatedBy = (select userName from tblPermissions where RecID = createdBy)");
            Execute.Sql(
                "Update SewerManholeConnections set CreatedBy = (select userName from tblPermissions where RecID = createdBy)");

            Alter.Column("Route").OnTable(SEWER_MANHOLE_CONNECTIONS).AsFloat().Nullable();
            Execute.Sql(
                "UPDATE SewerManholeConnections SET Route = Cast(Cast(Route as varchar) + '.' + Right('0000' + cast(Stop as varchar), 4) as float) WHERE Route IS NOT NULL");
            Delete.Column("Stop").FromTable(SEWER_MANHOLE_CONNECTIONS);
            Delete.Column("StreetNumber").FromTable(SEWER_MANHOLES);
            Alter.Table(SEWER_MANHOLE_CONNECTIONS)
                 .AddColumn("InspFreqUnit").AsString(4).Nullable();

            Execute.Sql("UPDATE [SewerManholeConnections] SET InspFreqUnit = 'D' WHERE InspectionFrequencyUnitId = 1");
            Execute.Sql("UPDATE [SewerManholeConnections] SET InspFreqUnit = 'Y' WHERE InspectionFrequencyUnitId = 4");

            Delete.ForeignKey("FK_SewerManholes_RecurringFrequencyUnits_InspectionFrequencyUnitId")
                  .OnTable(SEWER_MANHOLE_CONNECTIONS);

            Delete.Column("InspectionFrequencyUnitId").FromTable(SEWER_MANHOLE_CONNECTIONS);

            Alter.Table(SEWER_MANHOLE_CONNECTIONS)
                 .AlterColumn("InspFreq").AsFloat().Nullable();

            Alter.Column("ManholeNumber").OnTable(SEWER_MANHOLES).AsString(50).Nullable();
            Alter.Column("ManholeSuffix").OnTable(SEWER_MANHOLES).AsInt32().Nullable();
            Alter.Column("Description").OnTable("AssetStatuses").AsString(50).Nullable();
        }
    }
}
