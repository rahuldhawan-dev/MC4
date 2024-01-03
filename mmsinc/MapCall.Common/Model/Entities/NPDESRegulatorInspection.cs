using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class NpdesRegulatorInspection : 
        IEntityWithCreationTimeTracking, 
        IThingWithNotes,
        IThingWithDocuments
    {
        #region Constants

        public struct StringLengths
        {
            public const int REMARKS = 255,
                             SAMPLE_LOCATION = 255;
        }

        public struct ValueRanges
        {
            public const int MIN_HR = 0,
                             MIN_INCHES = 0,
                             MIN_RANGE_PERCENT = 0,
                             MAX_RANGE_PERCENT = 100;
        }

        public struct Display
        {
            public const string ADDITIONAL_EQUIPMENT_NECESSARY = "Additional Equipment Necessary",
                                ARRIVAL_DATE_TIME = "Arrival Date/Time",
                                CALIBRATED_FLOW_METER = "Calibrated Flow Meter",
                                DEPARTURE_DATE_TIME = "Departure Date/Time",
                                DISCHARGE_DURATION = "Discharge Duration (Hr)",
                                DISCHARGE_FLOW = "Discharge Flow (% of pipe)",
                                DISCHARGE_PRESENT = "Discharge Present",
                                DISCHARGE_WEATHER_RELATED = "Weather Related to Discharge",
                                DOWNLOADED_FLOW_METER_DATA = "Downloaded Flow Meter Data",
                                EROSION_PRESENT = "Erosion Present",
                                FLOW_METER_MAINTENANCE = "Flow Meter Maintenance",
                                GATE_MOVING_FREELY = "Gate Moving Freely",
                                HAS_INFILTRATION = "Infiltration from Body of Water",
                                INSPECTION_TYPE = "Inspection Type",
                                INSTALLED_FLOW_METER = "Installed Flow Meter",
                                OTHER_REMARKS = "Other, see remarks",
                                OUTFALL_CONDITION = "Condition of Outfall",
                                PLUME_PRESENT = "Plume Present",
                                RAINFALL_ESTIMATE = "Rainfall Estimate (inches)",
                                REMOVED_FLOW_METER = "Removed Flow Meter",
                                SAMPLES_TAKEN = "Samples Taken",
                                SOLIDS_FLOATABLES_PRESENT = "Solids and Floatables Present";
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual SewerOpening SewerOpening { get; set; }

        public virtual User InspectedBy { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        [View(Display.ARRIVAL_DATE_TIME)]
        public virtual DateTime ArrivalDateTime { get; set; }

        [View(Display.DEPARTURE_DATE_TIME)]
        public virtual DateTime DepartureDateTime { get; set; }

        [View(Display.INSPECTION_TYPE)]
        public virtual NpdesRegulatorInspectionType NpdesRegulatorInspectionType { get; set; }

        [View(Display.HAS_INFILTRATION)]
        public virtual bool HasInfiltration { get; set; }

        public virtual WeatherCondition WeatherCondition { get; set; }

        [View(Display.OUTFALL_CONDITION)]
        public virtual OutfallCondition OutfallCondition { get; set; }

        [View(Display.GATE_MOVING_FREELY)]
        public virtual GateStatusAnswerType GateStatusAnswerType { get; set; }

        public virtual BlockCondition BlockCondition { get; set; }

        [View(Display.DISCHARGE_PRESENT)]
        public virtual bool IsDischargePresent { get; set; }

        public virtual DischargeWeatherRelatedType DischargeWeatherRelatedType { get; set; }

        [View(Display.RAINFALL_ESTIMATE)]
        public virtual float? RainfallEstimate { get; set; }

        [View(Display.DISCHARGE_FLOW)]
        public virtual float? DischargeFlow { get; set; }

        public virtual DischargeCause DischargeCause { get; set; }

        [View(Display.DISCHARGE_DURATION)]
        public virtual float? DischargeDuration { get; set; }

        [View(Display.PLUME_PRESENT)]
        public virtual bool IsPlumePresent { get; set; }

        [View(Display.EROSION_PRESENT)]
        public virtual bool IsErosionPresent { get; set; }

        [View(Display.SOLIDS_FLOATABLES_PRESENT)]
        public virtual bool IsSolidFloatPresent { get; set; }

        [View(Display.ADDITIONAL_EQUIPMENT_NECESSARY)]
        public virtual bool IsAdditionalEquipmentNeeded { get; set; }

        [View(Display.SAMPLES_TAKEN)]
        public virtual bool HasSamplesBeenTaken { get; set; }

        public virtual string SampleLocation { get; set; }

        [View(Display.FLOW_METER_MAINTENANCE)]
        public virtual bool HasFlowMeterMaintenanceBeenPerformed { get; set; }

        [View(Display.DOWNLOADED_FLOW_METER_DATA)]
        public virtual bool HasDownloadedFlowMeterData { get; set; }

        [View(Display.CALIBRATED_FLOW_METER)]
        public virtual bool HasCalibratedFlowMeter { get; set; }

        [View(Display.INSTALLED_FLOW_METER)]
        public virtual bool HasInstalledFlowMeter { get; set; }

        [View(Display.REMOVED_FLOW_METER)]
        public virtual bool HasRemovedFlowMeter { get; set; }

        [View(Display.OTHER_REMARKS)]
        public virtual bool HasFlowMeterBeenMaintainedOther { get; set; }

        [Multiline]
        public virtual string Remarks { get; set; }

        #region Logical Properties

        #region Notes

        public virtual IList<Note<NpdesRegulatorInspection>> Notes { get; set; } = new List<Note<NpdesRegulatorInspection>>();

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        #endregion

        #region Documents

        public virtual IList<Document<NpdesRegulatorInspection>> Documents { get; set; } = new List<Document<NpdesRegulatorInspection>>();

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        #endregion

        [DoesNotExport]
        public virtual string TableName => nameof(NpdesRegulatorInspection) + "s";

        [DoesNotExport]
        public virtual int? SewerOpeningType => SewerOpening?.SewerOpeningType?.Id;

        [DoesNotExport]
        public virtual int? BodyOfWater => SewerOpening?.BodyOfWater?.Id;

        #endregion

        #endregion
    }
}