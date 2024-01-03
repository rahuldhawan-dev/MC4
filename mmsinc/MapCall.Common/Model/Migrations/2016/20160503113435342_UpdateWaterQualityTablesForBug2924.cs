using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160503113435342), Tags("Production")]
    public class UpdateWaterQualityTablesForBug2924 : Migration
    {
        #region Constants

        public struct TableNamesOld
        {
            public const string WATER_CONSTITUENTS = "tblWQWaterConstituents",
                                SAMPLE_ID_MATRIX = "tblWQSampleIDMatrix",
                                SAMPLE_RESULTS = "tblWQSampleResults";
        }

        public struct TableNames
        {
            public const string WATER_CONSTITUENTS = "WaterConstituents",
                                SAMPLE_ID_MATRIX = "SampleIDMatrices",
                                SAMPLE_RESULTS = "WaterSamples";
        }

        public struct ColumnNames
        {
            public const string
                WC_ID = "Id",
                WC_ACTION_LIMIT = "ActionLimit",
                WC_SAMPLE_CONTAINER_SIZE_ML_S = "SampleContainerSize",
                WC_HOLDING_TIME = "HoldingTime",
                WC_PRESERVATIVE_QUENCHING_AGENT = "PreservativeQuenchingAgent",
                WC_TAT_BELLVILE_DAYS = "TATBellvilleDays",
                SR_ID = "Id",
                SR_DATE_ADDED = "CreatedOn",
                SR_SAMPLE_DATE = "SampleDate",
                SR_COLLECTED_BY = "CollectedBy",
                SR_ANALYSIS_PERFORMED_BY = "AnalysisPerformedBy",
                SR_SAMPLE_ID = "SampleMatrixId",
                SR_SAMPLE_VALUE = "SampleValue",
                SM_SAMPLE_ID = "Id",
                SM_ROUTINE_SAMPLE = "RoutineSample",
                SM_SAMPLE_SITE_ID = "SampleSiteId",
                SM_WATER_CONSTITUENT_ID = "WaterConstituentId",
                SM_PROCESS_STAGE = "ProcessStage",
                SM_PROCESS_STAGE_SEQUENCE = "ProcessStageSequence",
                SM_PARAMETER_SEQUENCE = "ParameterSequence",
                SM_SAMPLE_PURPOSE = "SamplePurpose",
                SM_PROCESS_REASON_FOR_SAMPLE = "ProcessReasonForSample",
                SM_PERFORMED_BY = "PerformedBy",
                SM_WEEKLY_FREQUENCY = "WeeklyFrequency",
                SM_DATA_STORAGE_LOCATION = "DataStorageLocation",
                SM_METHOD_INSTRUMENT_LABORATORY = "MethodInstrumentLaboratory",
                SM_TAT_BELLVILLE_LAB_HRS = "TATBellvilleLabHrs",
                SM_BELLEVILLE_SAMPLE_ID = "BellevilleSampleID",
                SM_INTERFERENCE_BY = "InterferenceBy",
                SM_DATA_STORAGE_LOCATION_ON_LINE_INSTRUMENT = "DataStorageLocationOnLineInstrument",
                SM_I_HISTORIAN_SIGNAL_ID_ON_LINE_INSTRUMENT = "IHistorianSignalIDOnLineInstrument",
                SM_COMPLIANCE_REQ = "ComplianceReq",
                SM_PROCESS_TARGET = "ProcessTarget",
                SM_TRIGGER_PHASE_1 = "TriggerPhase1",
                SM_ACTION_PHASE_1 = "ActionPhase1",
                SM_TRIGGER_PHASE_2 = "TriggerPhase2",
                SM_ACTION_PHASE_2 = "ActionPhase2",
                SM_SCADA_NOTES = "SCADANotes";
        }

        public struct ColumnNamesOld
        {
            public const string
                WC_ID = "WaterConstituent_ID",
                WC_ACTION_LIMIT = "Action_Limit",
                WC_SAMPLE_CONTAINER_SIZE_ML_S = "Sample_ContainerSize_ML",
                WC_HOLDING_TIME = "Holding_Time_Hrs",
                WC_PRESERVATIVE_QUENCHING_AGENT = "Preservative_Quenching_Agent",
                WC_TAT_BELLVILE_DAYS = "TAT_Bellvile_Days",
                SR_ID = "Sample_Result_ID",
                SR_DATE_ADDED = "DTM_DataEntered",
                SR_SAMPLE_DATE = "Sample_Date",
                SR_COLLECTED_BY = "Collected_By",
                SR_ANALYSIS_PERFORMED_BY = "Analysis_Performed_By",
                SR_SAMPLE_ID = "Sample_ID",
                SR_SAMPLE_VALUE = "Sample_Value",
                SM_SAMPLE_ID = "Sample_ID",
                SM_ROUTINE_SAMPLE = "Routine_Sample",
                SM_SAMPLE_SITE_ID = "SampleSiteID",
                SM_WATER_CONSTITUENT_ID = "WaterConstituent_ID",
                SM_PROCESS_STAGE = "Process_Stage",
                SM_PROCESS_STAGE_SEQUENCE = "ProcessStage_Sequence",
                SM_PARAMETER_SEQUENCE = "Parameter_Sequence",
                SM_SAMPLE_PURPOSE = "Sample_Purpose",
                SM_PROCESS_REASON_FOR_SAMPLE = "Process_Reason_For_Sample",
                SM_PERFORMED_BY = "Performed_by",
                SM_WEEKLY_FREQUENCY = "Weekly_Frequency",
                SM_DATA_STORAGE_LOCATION = "Data_Storage_Location",
                SM_METHOD_INSTRUMENT_LABORATORY = "Method_Instrument_Laboratory",
                SM_TAT_BELLVILLE_LAB_HRS = "TAT_BellvilleLab_Hrs",
                SM_BELLEVILLE_SAMPLE_ID = "Belleville_Sample_ID",
                SM_INTERFERENCE_BY = "Interference_By",
                SM_DATA_STORAGE_LOCATION_ON_LINE_INSTRUMENT = "Data_Storage_Location_OnLine_Instrument",
                SM_I_HISTORIAN_SIGNAL_ID_ON_LINE_INSTRUMENT = "IHistorian_SignalID_OnLine_Instrument",
                SM_COMPLIANCE_REQ = "Compliance_Req",
                SM_PROCESS_TARGET = "Process_Target",
                SM_TRIGGER_PHASE_1 = "Trigger_Phase_1",
                SM_ACTION_PHASE_1 = "Action_Phase_1",
                SM_TRIGGER_PHASE_2 = "Trigger_Phase_2",
                SM_ACTION_PHASE_2 = "Action_Phase_2",
                SM_SCADA_NOTES = "SCADA_Notes";
        }

        #endregion

        public override void Up()
        {
            #region WaterConstituents

            Rename.Table(TableNamesOld.WATER_CONSTITUENTS).To(TableNames.WATER_CONSTITUENTS);
            Rename.Column(ColumnNamesOld.WC_ID).OnTable(TableNames.WATER_CONSTITUENTS).To(ColumnNames.WC_ID);
            Rename.Column(ColumnNamesOld.WC_ACTION_LIMIT).OnTable(TableNames.WATER_CONSTITUENTS)
                  .To(ColumnNames.WC_ACTION_LIMIT);
            Rename.Column(ColumnNamesOld.WC_SAMPLE_CONTAINER_SIZE_ML_S).OnTable(TableNames.WATER_CONSTITUENTS)
                  .To(ColumnNames.WC_SAMPLE_CONTAINER_SIZE_ML_S);
            Rename.Column(ColumnNamesOld.WC_HOLDING_TIME).OnTable(TableNames.WATER_CONSTITUENTS)
                  .To(ColumnNames.WC_HOLDING_TIME);
            Rename.Column(ColumnNamesOld.WC_PRESERVATIVE_QUENCHING_AGENT).OnTable(TableNames.WATER_CONSTITUENTS)
                  .To(ColumnNames.WC_PRESERVATIVE_QUENCHING_AGENT);
            Rename.Column(ColumnNamesOld.WC_TAT_BELLVILE_DAYS).OnTable(TableNames.WATER_CONSTITUENTS)
                  .To(ColumnNames.WC_TAT_BELLVILE_DAYS);

            #endregion

            #region Sample Results

            Rename.Table(TableNamesOld.SAMPLE_RESULTS).To(TableNames.SAMPLE_RESULTS);
            Rename.Column(ColumnNamesOld.SR_ID).OnTable(TableNames.SAMPLE_RESULTS).To(ColumnNames.SR_ID);
            Rename.Column(ColumnNamesOld.SR_DATE_ADDED).OnTable(TableNames.SAMPLE_RESULTS)
                  .To(ColumnNames.SR_DATE_ADDED);
            Rename.Column(ColumnNamesOld.SR_SAMPLE_DATE).OnTable(TableNames.SAMPLE_RESULTS)
                  .To(ColumnNames.SR_SAMPLE_DATE);
            Rename.Column(ColumnNamesOld.SR_COLLECTED_BY).OnTable(TableNames.SAMPLE_RESULTS)
                  .To(ColumnNames.SR_COLLECTED_BY);
            Rename.Column(ColumnNamesOld.SR_ANALYSIS_PERFORMED_BY).OnTable(TableNames.SAMPLE_RESULTS)
                  .To(ColumnNames.SR_ANALYSIS_PERFORMED_BY);
            Rename.Column(ColumnNamesOld.SR_SAMPLE_ID).OnTable(TableNames.SAMPLE_RESULTS).To(ColumnNames.SR_SAMPLE_ID);
            Rename.Column(ColumnNamesOld.SR_SAMPLE_VALUE).OnTable(TableNames.SAMPLE_RESULTS)
                  .To(ColumnNames.SR_SAMPLE_VALUE);

            #endregion

            #region Sample Matrix

            Rename.Table(TableNamesOld.SAMPLE_ID_MATRIX).To(TableNames.SAMPLE_ID_MATRIX);
            Rename.Column(ColumnNamesOld.SM_SAMPLE_ID).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNames.SM_SAMPLE_ID);
            Rename.Column(ColumnNamesOld.SM_ROUTINE_SAMPLE).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNames.SM_ROUTINE_SAMPLE);
            Rename.Column(ColumnNamesOld.SM_SAMPLE_SITE_ID).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNames.SM_SAMPLE_SITE_ID);
            Rename.Column(ColumnNamesOld.SM_WATER_CONSTITUENT_ID).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNames.SM_WATER_CONSTITUENT_ID);
            Rename.Column(ColumnNamesOld.SM_PROCESS_STAGE).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNames.SM_PROCESS_STAGE);
            Rename.Column(ColumnNamesOld.SM_PROCESS_STAGE_SEQUENCE).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNames.SM_PROCESS_STAGE_SEQUENCE);
            Rename.Column(ColumnNamesOld.SM_PARAMETER_SEQUENCE).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNames.SM_PARAMETER_SEQUENCE);
            Rename.Column(ColumnNamesOld.SM_SAMPLE_PURPOSE).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNames.SM_SAMPLE_PURPOSE);
            Rename.Column(ColumnNamesOld.SM_PROCESS_REASON_FOR_SAMPLE).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNames.SM_PROCESS_REASON_FOR_SAMPLE);
            Rename.Column(ColumnNamesOld.SM_PERFORMED_BY).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNames.SM_PERFORMED_BY);
            Rename.Column(ColumnNamesOld.SM_WEEKLY_FREQUENCY).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNames.SM_WEEKLY_FREQUENCY);
            Rename.Column(ColumnNamesOld.SM_DATA_STORAGE_LOCATION).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNames.SM_DATA_STORAGE_LOCATION);
            Rename.Column(ColumnNamesOld.SM_METHOD_INSTRUMENT_LABORATORY).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNames.SM_METHOD_INSTRUMENT_LABORATORY);
            Rename.Column(ColumnNamesOld.SM_TAT_BELLVILLE_LAB_HRS).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNames.SM_TAT_BELLVILLE_LAB_HRS);
            Rename.Column(ColumnNamesOld.SM_BELLEVILLE_SAMPLE_ID).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNames.SM_BELLEVILLE_SAMPLE_ID);
            Rename.Column(ColumnNamesOld.SM_INTERFERENCE_BY).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNames.SM_INTERFERENCE_BY);
            Rename.Column(ColumnNamesOld.SM_DATA_STORAGE_LOCATION_ON_LINE_INSTRUMENT)
                  .OnTable(TableNames.SAMPLE_ID_MATRIX).To(ColumnNames.SM_DATA_STORAGE_LOCATION_ON_LINE_INSTRUMENT);
            Rename.Column(ColumnNamesOld.SM_I_HISTORIAN_SIGNAL_ID_ON_LINE_INSTRUMENT)
                  .OnTable(TableNames.SAMPLE_ID_MATRIX).To(ColumnNames.SM_I_HISTORIAN_SIGNAL_ID_ON_LINE_INSTRUMENT);
            Rename.Column(ColumnNamesOld.SM_COMPLIANCE_REQ).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNames.SM_COMPLIANCE_REQ);
            Rename.Column(ColumnNamesOld.SM_PROCESS_TARGET).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNames.SM_PROCESS_TARGET);
            Rename.Column(ColumnNamesOld.SM_TRIGGER_PHASE_1).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNames.SM_TRIGGER_PHASE_1);
            Rename.Column(ColumnNamesOld.SM_ACTION_PHASE_1).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNames.SM_ACTION_PHASE_1);
            Rename.Column(ColumnNamesOld.SM_TRIGGER_PHASE_2).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNames.SM_TRIGGER_PHASE_2);
            Rename.Column(ColumnNamesOld.SM_ACTION_PHASE_2).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNames.SM_ACTION_PHASE_2);
            Rename.Column(ColumnNamesOld.SM_SCADA_NOTES).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNames.SM_SCADA_NOTES);

            Execute.Sql("update DataType set Table_Name = 'WaterSamples' where Table_Name = 'tblWQSampleResults'" +
                        "update DataType set Table_Name = 'WaterConstituents' where Table_Name = 'tblWQWaterConstituents'" +
                        "update DataType set Table_Name = 'SampleIDMatrices' where Table_Name = 'tblWQSampleIDMatrix';" +
                        "update tblWQSample_Sites set LeadCopperSite = 0 where LeadCopperSite is null;" +
                        "INSERT Into DocumentType Values('Document',39)");

            #endregion
        }

        public override void Down()
        {
            Execute.Sql("update DataType set Table_Name = 'tblWQSampleResults' where Table_Name = 'WaterSamples'" +
                        "update DataType set Table_Name = 'tblWQWaterConstituents' where Table_Name = 'WaterConstituents'" +
                        "update DataType set Table_Name = 'tblWQSampleIDMatrix' where Table_Name = 'SampleIDMatrices';" +
                        "DELETE FROM DocumentType Where Document_Type = 'Document' and DataTypeID = 39;");

            #region WaterConstituents

            Rename.Column(ColumnNames.WC_ID).OnTable(TableNames.WATER_CONSTITUENTS).To(ColumnNamesOld.WC_ID);
            Rename.Column(ColumnNames.WC_ACTION_LIMIT).OnTable(TableNames.WATER_CONSTITUENTS)
                  .To(ColumnNamesOld.WC_ACTION_LIMIT);
            Rename.Column(ColumnNames.WC_SAMPLE_CONTAINER_SIZE_ML_S).OnTable(TableNames.WATER_CONSTITUENTS)
                  .To(ColumnNamesOld.WC_SAMPLE_CONTAINER_SIZE_ML_S);
            Rename.Column(ColumnNames.WC_HOLDING_TIME).OnTable(TableNames.WATER_CONSTITUENTS)
                  .To(ColumnNamesOld.WC_HOLDING_TIME);
            Rename.Column(ColumnNames.WC_PRESERVATIVE_QUENCHING_AGENT).OnTable(TableNames.WATER_CONSTITUENTS)
                  .To(ColumnNamesOld.WC_PRESERVATIVE_QUENCHING_AGENT);
            Rename.Column(ColumnNames.WC_TAT_BELLVILE_DAYS).OnTable(TableNames.WATER_CONSTITUENTS)
                  .To(ColumnNamesOld.WC_TAT_BELLVILE_DAYS);
            Rename.Table(TableNames.WATER_CONSTITUENTS).To(TableNamesOld.WATER_CONSTITUENTS);

            #endregion

            #region Sample Results

            Rename.Column(ColumnNames.SR_ID).OnTable(TableNames.SAMPLE_RESULTS).To(ColumnNamesOld.SR_ID);
            Rename.Column(ColumnNames.SR_DATE_ADDED).OnTable(TableNames.SAMPLE_RESULTS)
                  .To(ColumnNamesOld.SR_DATE_ADDED);
            Rename.Column(ColumnNames.SR_SAMPLE_DATE).OnTable(TableNames.SAMPLE_RESULTS)
                  .To(ColumnNamesOld.SR_SAMPLE_DATE);
            Rename.Column(ColumnNames.SR_COLLECTED_BY).OnTable(TableNames.SAMPLE_RESULTS)
                  .To(ColumnNamesOld.SR_COLLECTED_BY);
            Rename.Column(ColumnNames.SR_ANALYSIS_PERFORMED_BY).OnTable(TableNames.SAMPLE_RESULTS)
                  .To(ColumnNamesOld.SR_ANALYSIS_PERFORMED_BY);
            Rename.Column(ColumnNames.SR_SAMPLE_ID).OnTable(TableNames.SAMPLE_RESULTS).To(ColumnNamesOld.SR_SAMPLE_ID);
            Rename.Column(ColumnNames.SR_SAMPLE_VALUE).OnTable(TableNames.SAMPLE_RESULTS)
                  .To(ColumnNamesOld.SR_SAMPLE_VALUE);
            Rename.Table(TableNames.SAMPLE_RESULTS).To(TableNamesOld.SAMPLE_RESULTS);

            #endregion

            #region Sample Matrix

            Rename.Column(ColumnNames.SM_SAMPLE_ID).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_SAMPLE_ID);
            Rename.Column(ColumnNames.SM_ROUTINE_SAMPLE).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_ROUTINE_SAMPLE);
            Rename.Column(ColumnNames.SM_SAMPLE_SITE_ID).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_SAMPLE_SITE_ID);
            Rename.Column(ColumnNames.SM_WATER_CONSTITUENT_ID).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_WATER_CONSTITUENT_ID);
            Rename.Column(ColumnNames.SM_PROCESS_STAGE).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_PROCESS_STAGE);
            Rename.Column(ColumnNames.SM_PROCESS_STAGE_SEQUENCE).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_PROCESS_STAGE_SEQUENCE);
            Rename.Column(ColumnNames.SM_PARAMETER_SEQUENCE).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_PARAMETER_SEQUENCE);
            Rename.Column(ColumnNames.SM_SAMPLE_PURPOSE).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_SAMPLE_PURPOSE);
            Rename.Column(ColumnNames.SM_PROCESS_REASON_FOR_SAMPLE).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_PROCESS_REASON_FOR_SAMPLE);
            Rename.Column(ColumnNames.SM_PERFORMED_BY).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_PERFORMED_BY);
            Rename.Column(ColumnNames.SM_WEEKLY_FREQUENCY).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_WEEKLY_FREQUENCY);
            Rename.Column(ColumnNames.SM_DATA_STORAGE_LOCATION).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_DATA_STORAGE_LOCATION);
            Rename.Column(ColumnNames.SM_METHOD_INSTRUMENT_LABORATORY).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_METHOD_INSTRUMENT_LABORATORY);
            Rename.Column(ColumnNames.SM_TAT_BELLVILLE_LAB_HRS).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_TAT_BELLVILLE_LAB_HRS);
            Rename.Column(ColumnNames.SM_BELLEVILLE_SAMPLE_ID).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_BELLEVILLE_SAMPLE_ID);
            Rename.Column(ColumnNames.SM_INTERFERENCE_BY).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_INTERFERENCE_BY);
            Rename.Column(ColumnNames.SM_DATA_STORAGE_LOCATION_ON_LINE_INSTRUMENT).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_DATA_STORAGE_LOCATION_ON_LINE_INSTRUMENT);
            Rename.Column(ColumnNames.SM_I_HISTORIAN_SIGNAL_ID_ON_LINE_INSTRUMENT).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_I_HISTORIAN_SIGNAL_ID_ON_LINE_INSTRUMENT);
            Rename.Column(ColumnNames.SM_COMPLIANCE_REQ).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_COMPLIANCE_REQ);
            Rename.Column(ColumnNames.SM_PROCESS_TARGET).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_PROCESS_TARGET);
            Rename.Column(ColumnNames.SM_TRIGGER_PHASE_1).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_TRIGGER_PHASE_1);
            Rename.Column(ColumnNames.SM_ACTION_PHASE_1).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_ACTION_PHASE_1);
            Rename.Column(ColumnNames.SM_TRIGGER_PHASE_2).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_TRIGGER_PHASE_2);
            Rename.Column(ColumnNames.SM_ACTION_PHASE_2).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_ACTION_PHASE_2);
            Rename.Column(ColumnNames.SM_SCADA_NOTES).OnTable(TableNames.SAMPLE_ID_MATRIX)
                  .To(ColumnNamesOld.SM_SCADA_NOTES);
            Rename.Table(TableNames.SAMPLE_ID_MATRIX).To(TableNamesOld.SAMPLE_ID_MATRIX);

            #endregion
        }
    }
}
