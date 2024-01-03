using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230720071616108), Tags("Production")]
    public class MC5626_AddNpdesRegulatorInspectionTableAndLookups : Migration
    {
        private struct Sql
        {
            public const string
                SQL_1 = @"
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '003',[LocationDescription] = 'WWTP Bypass 003' WHERE Id =82084
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '004', [LocationDescription] = 'Wells Street' WHERE Id =76836
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '005', [LocationDescription] = 'Love Place' WHERE Id =82761
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '006', [LocationDescription] = 'Gardener Ave' WHERE Id =82143
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '007', [LocationDescription] = 'Philo Street' WHERE Id =82146
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '008', [LocationDescription] = 'Hawk Street' WHERE Id =78524
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '009', [LocationDescription] = 'Meade Street' WHERE Id =82756
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '011', [LocationDescription] = 'Von Storch Avenue' WHERE Id =82763
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '012', [LocationDescription] = 'Grove Street' WHERE Id =82765
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '013', [LocationDescription] = '24"" Poplar Street' WHERE Id =82767
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '014', [LocationDescription] = '90"" Poplar Street' WHERE Id =77022
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '015', [LocationDescription] = 'Gordon Avenue' WHERE Id =77187
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '016', [LocationDescription] = 'Pettibone Street' WHERE Id =82760
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '017', [LocationDescription] = 'Vine Street' WHERE Id =77019
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '018', [LocationDescription] = 'Love Road' WHERE Id =77207
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '019', [LocationDescription] = 'Linden Street' WHERE Id =84903
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '020', [LocationDescription] = 'East Lackawanna Avenue' WHERE Id =77002
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '021', [LocationDescription] = 'West Scranton' WHERE Id =82860
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '022', [LocationDescription] = 'Washburn Street' WHERE Id =83399
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '023', [LocationDescription] = 'Luzerne Street' WHERE Id =77106
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '024', [LocationDescription] = 'Hickory Street' WHERE Id =77016
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 519,[OutfallNumber] = '025', [LocationDescription] = 'Willow Street' WHERE Id =82716
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '026', [LocationDescription] = 'West Elm Street' WHERE Id =77158
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '027', [LocationDescription] = 'South Washington Avenue' WHERE Id =77102
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '028', [LocationDescription] = 'Fig Street' WHERE Id =82365
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '029', [LocationDescription] = 'Genet Street' WHERE Id =77012
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 519,[OutfallNumber] = '030', [LocationDescription] = 'Prescott Ave' WHERE Id =78634
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '031', [LocationDescription] = 'Leggetts Creek' WHERE Id =76841
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '032', [LocationDescription] = 'Watkins Street' WHERE Id =76840
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '033', [LocationDescription] = 'West Parker Street' WHERE Id =82233
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '034', [LocationDescription] = 'East Parker Street' WHERE Id =76618
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '035', [LocationDescription] = 'Sanderson Ave' WHERE Id =82620
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '036', [LocationDescription] = 'Tioga Avenue' WHERE Id =83578
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '037', [LocationDescription] = 'Brown Street' WHERE Id =76947
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '038', [LocationDescription] = 'Wurtz Avenue' WHERE Id =82174
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '040', [LocationDescription] = 'West Market Street' WHERE Id =82351
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '043', [LocationDescription] = 'Olive Street' WHERE Id =79890
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '044', [LocationDescription] = 'East Scranton Street' WHERE Id =77444
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '045', [LocationDescription] = 'Emmett Street' WHERE Id =83608
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '047', [LocationDescription] = 'Broadway Street' WHERE Id =77610
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '048', [LocationDescription] = 'Alder Street' WHERE Id =77103
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 519,[OutfallNumber] = '049', [LocationDescription] = 'River Street' WHERE Id =76959
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 519,[OutfallNumber] = '050', [LocationDescription] = 'Schimpff Court' WHERE Id =82491
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '051', [LocationDescription] = 'Birch Street' WHERE Id =82495
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '052', [LocationDescription] = 'Wyoming Avenue' WHERE Id =76623
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '053', [LocationDescription] = 'Cedar Avenue' WHERE Id =81167
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '055', [LocationDescription] = 'Drinker Place' WHERE Id =82390
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '056', [LocationDescription] = 'Boulevard Avenue' WHERE Id =82389
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '057', [LocationDescription] = 'Richmont Street' WHERE Id =82486
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '058', [LocationDescription] = 'Grandview Street' WHERE Id =77252
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '059', [LocationDescription] = 'Woodlawn Street' WHERE Id =82557
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '060', [LocationDescription] = 'Park Street' WHERE Id =83562
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '061', [LocationDescription] = 'Morel Street' WHERE Id =83564
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '062', [LocationDescription] = 'Fisk Street' WHERE Id =82589
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '063', [LocationDescription] = 'Olyphant South' WHERE Id =83597
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '065', [LocationDescription] = 'Drinker Street' WHERE Id =88451
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 519,[OutfallNumber] = '066', [LocationDescription] = 'Burke Street' WHERE Id =87803
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 522,[OutfallNumber] = '067', [LocationDescription] = 'Keyser Avenue' WHERE Id =82641
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '068', [LocationDescription] = 'South Sixth Avenue' WHERE Id =77001
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '069', [LocationDescription] = 'Crane Street' WHERE Id =77163
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 519,[OutfallNumber] = '070', [LocationDescription] = 'Sand Street' WHERE Id =86709
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 519,[OutfallNumber] = '071', [LocationDescription] = 'Lake Street' WHERE Id =86435
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 525,[OutfallNumber] = '072', [LocationDescription] = 'Leggett Street Regulator Chamber' WHERE Id =82089
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 519,[OutfallNumber] = '073', [LocationDescription] = 'Front Street CSO' WHERE Id =76996
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 521,[OutfallNumber] = '074', [LocationDescription] = 'Marion Street CSO' WHERE Id =83404
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 521,[OutfallNumber] = '075', [LocationDescription] = 'Capouse Avenue CSO' WHERE Id =78939
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 521,[OutfallNumber] = '076', [LocationDescription] = 'Sanderson and Marion Street CSO' WHERE Id =83559
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '077', [LocationDescription] = 'Middle Street Pumping Station' WHERE Id =77180
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '078', [LocationDescription] = 'Lackawanna River' WHERE Id =76466
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 519,[OutfallNumber] = '079', [LocationDescription] = 'Myrtle Street Pumping Station' WHERE Id =83954
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 522,[OutfallNumber] = '080', [LocationDescription] = 'Keyser Valley Pumping Station' WHERE Id =75326
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '081', [LocationDescription] = 'Pittston Avenue' WHERE Id =82091
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '082', [LocationDescription] = 'Locust Street' WHERE Id =78286
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '083', [LocationDescription] = 'McNichols' WHERE Id =78249
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '084', [LocationDescription] = '600 East Elm Street' WHERE Id =75221
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '085', [LocationDescription] = '600 West Elm Street' WHERE Id =82118
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 517,[OutfallNumber] = '086', [LocationDescription] = 'Cedar/Maple' WHERE Id =79963
UPDATE [dbo].[SewerOpenings] SET [BodyOfWaterId] = 525,[OutfallNumber] = '087', [LocationDescription] = 'Leggetts/Kelly' WHERE Id =77920
UPDATE [dbo].[SewerOpenings] SET [SewerOpeningTypeId] = 6 WHERE [BodyOfWaterId] IS NOT NULL
GO";
        }

        #region Consts

        private const string BLOCK_CONDITIONS = "BlockConditions",
                             SEWER_OPENINGS = "SewerOpenings",
                             OUTFALL_CONDITIONS = "OutfallConditions",
                             WEATHER_CONDITIONS = "WeatherConditions",
                             DISCHARGE_CAUSES = "DischargeCauses",
                             DISCHARGE_WEATHER_RELATED_TYPES = "DischargeWeatherRelatedTypes",
                             TBL_PERMISSIONS = "tblPermissions",
                             NPDES_REGULATOR_INSPECTION_TYPES = "NPDESRegulatorInspectionTypes",
                             NPDES_REGULATOR_INSPECTIONS = "NPDESRegulatorInspections",
                             NPDES_REGULATOR_INSPECTION_DOCUMENT = "NPDES Regulator Inspection Document";

        #endregion

        public override void Up()
        {
            this.CreateLookupTableWithValues(NPDES_REGULATOR_INSPECTION_TYPES, "Standard", "Rain Event");
            this.CreateLookupTableWithValues(BLOCK_CONDITIONS, "In", "Out", "Stuck", "Floating", "Missing", "In Sewer Main");
            this.CreateLookupTableWithValues(OUTFALL_CONDITIONS, "Good", "Fair", "Poor", "Plugged");
            this.CreateLookupTableWithValues(WEATHER_CONDITIONS, "Dry", "Raining", "Snowing");
            this.CreateLookupTableWithValues(DISCHARGE_CAUSES, "Rain", "Runoff after Rain", "Snow", "Runoff after Snow", "Line Blockage", "Excessive Flow", "Other");
            this.CreateLookupTableWithValues(DISCHARGE_WEATHER_RELATED_TYPES, "Dry", "Wet");
            this.Create.Table(NPDES_REGULATOR_INSPECTIONS)
                .WithIdentityColumn()
                .WithForeignKeyColumn("SewerOpeningId", SEWER_OPENINGS, nullable: false)
                .WithForeignKeyColumn("InspectionTypeId", NPDES_REGULATOR_INSPECTION_TYPES,
                     nullable: true)
                .WithForeignKeyColumn("InspectedById", TBL_PERMISSIONS, "RecID", 
                     nullable: true)
                .WithForeignKeyColumn("BlockConditionId", BLOCK_CONDITIONS, nullable: true)
                .WithForeignKeyColumn("OutfallConditionId", OUTFALL_CONDITIONS, nullable: true)
                .WithForeignKeyColumn("WeatherConditionId", WEATHER_CONDITIONS, nullable: true)
                .WithForeignKeyColumn("DischargeCauseId", DISCHARGE_CAUSES, nullable: true)
                .WithForeignKeyColumn("DischargeWeatherRelatedTypeId", DISCHARGE_WEATHER_RELATED_TYPES, nullable: true)
                .WithForeignKeyColumn("CreatedById", TBL_PERMISSIONS, "RecId", nullable: true)
                .WithColumn("ArrivalDateTime").AsDateTime().NotNullable()
                .WithColumn("DepartureDateTime").AsDateTime().NotNullable()
                .WithColumn("CreatedAt").AsDateTime().NotNullable()
                .WithColumn("HasInfiltration").AsBoolean().NotNullable()
                .WithColumn("IsGateMovingFreely").AsBoolean().Nullable()
                .WithColumn("IsDischargePresent").AsBoolean().NotNullable()
                .WithColumn("RainfallEstimate").AsDecimal().NotNullable()
                .WithColumn("DischargeFlow").AsDecimal().NotNullable()
                .WithColumn("DischargeDuration").AsDecimal().NotNullable()
                .WithColumn("IsPlumePresent").AsBoolean().NotNullable()
                .WithColumn("IsErosionPresent").AsBoolean().NotNullable()
                .WithColumn("IsSolidFloatPresent").AsBoolean().NotNullable()
                .WithColumn("IsAdditionalEquipmentNeeded").AsBoolean().NotNullable()
                .WithColumn("HasSamplesBeenTaken").AsBoolean().NotNullable()
                .WithColumn("SampleLocation").AsString().Nullable()
                .WithColumn("HasFlowMeterMaintenanceBeenPerformed").AsBoolean().NotNullable()
                .WithColumn("HasDownloadedFlowMeterData").AsBoolean().NotNullable()
                .WithColumn("HasCalibratedFlowMeter").AsBoolean().NotNullable()
                .WithColumn("HasInstalledFlowMeter").AsBoolean().NotNullable()
                .WithColumn("HasRemovedFlowMeter").AsBoolean().NotNullable()
                .WithColumn("HasFlowMeterBeenMaintainedOther").AsBoolean().NotNullable()
                .WithColumn("Remarks").AsString().Nullable();

            Alter.Table(SEWER_OPENINGS).AddColumn("BodyOfWaterId").AsInt32().Nullable();
            Alter.Table(SEWER_OPENINGS).AddColumn("OutfallNumber").AsString().Nullable();
            Alter.Table(SEWER_OPENINGS).AddColumn("LocationDescription").AsString().Nullable();
            Execute.Sql(Sql.SQL_1);

            this.AddDataType(NPDES_REGULATOR_INSPECTIONS);
            this.AddDocumentType(NPDES_REGULATOR_INSPECTION_DOCUMENT, NPDES_REGULATOR_INSPECTIONS);
        }

        public override void Down()
        {
            this.RemoveDocumentTypeAndAllRelatedDocuments(NPDES_REGULATOR_INSPECTION_DOCUMENT, NPDES_REGULATOR_INSPECTIONS);
            this.RemoveDataType(NPDES_REGULATOR_INSPECTIONS);

            Delete.Column("BodyOfWaterId").FromTable(SEWER_OPENINGS);
            Delete.Column("OutfallNumber").FromTable(SEWER_OPENINGS);
            Delete.Column("LocationDescription").FromTable(SEWER_OPENINGS);

            Delete.Table(NPDES_REGULATOR_INSPECTIONS);
            Delete.Table(NPDES_REGULATOR_INSPECTION_TYPES);
            Delete.Table(BLOCK_CONDITIONS);
            Delete.Table(DISCHARGE_WEATHER_RELATED_TYPES);
            Delete.Table(OUTFALL_CONDITIONS);
            Delete.Table(WEATHER_CONDITIONS);
            Delete.Table(DISCHARGE_CAUSES);
        }
    }
}
