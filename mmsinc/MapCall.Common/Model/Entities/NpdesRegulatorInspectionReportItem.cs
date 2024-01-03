using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    public class NpdesRegulatorInspectionReportItem
    {
        #region Constants

        public readonly struct DisplayNames
        {
            public const string INSPECTION_ID = "Inspection Id",
                                WASTE_WATER_SYSTEM = "WWSID",
                                PERMIT_NUMBER = "Permit Number",
                                LOCATION_DESCRIPTION = "Location Description",
                                OUTFALL_NUMBER = "Outfall Number",
                                DEPARTURE_DATE_INSPECTED = "Departure Date Inspected",
                                INSPECTED_BY = "Inspected By",
                                INSPECTION_TYPE = "Inspection Type",
                                BLOCK_CONDITION = "Block Condition",
                                DISCHARGE_PRESENT = "Discharge Present",
                                WEATHER_RELATED = "Weather Related",
                                RAINFALL_ESTIMATE = "Rainfall Estimate",
                                BODY_OF_WATER = "Body Of Water",
                                DISCHARGE_FLOW = "Discharge Flow",
                                DISCHARGE_CAUSE = "Discharge Cause",
                                DISCHARGE_DURATION = "Discharge Duration";
        }

        #endregion

        #region Properties

        [View(DisplayNames.INSPECTION_ID)]
        public int InspectionId { get; set; }

        public string Town { get; set; }

        [View(DisplayNames.WASTE_WATER_SYSTEM)]
        public WasteWaterSystem WasteWaterSystem { get; set; }

        [View(DisplayNames.PERMIT_NUMBER)]
        public string PermitNumber { get; set; }

        [View(DisplayNames.LOCATION_DESCRIPTION)]
        public string LocationDescription { get; set; }

        [View(DisplayNames.OUTFALL_NUMBER)]
        public string OutfallNumber { get; set; }

        [View(DisplayNames.DEPARTURE_DATE_INSPECTED)]
        public DateTime DepartureDateTime { get; set; }

        [View(DisplayNames.INSPECTED_BY)]
        public User InspectedBy { get; set; }

        [View(DisplayNames.INSPECTION_TYPE)]
        public NpdesRegulatorInspectionType InspectionType { get; set; }

        [View(DisplayNames.BLOCK_CONDITION)]
        public BlockCondition BlockCondition { get; set; }

        [View(DisplayNames.DISCHARGE_PRESENT)]
        public bool DischargePresent { get; set; }

        [View(DisplayNames.WEATHER_RELATED)]
        public WeatherCondition WeatherRelated { get; set; }

        [View(DisplayNames.RAINFALL_ESTIMATE)]
        public float RainfallEstimate { get; set; }

        [View(DisplayNames.BODY_OF_WATER)]
        public BodyOfWater BodyOfWater { get; set; }

        [View(DisplayNames.DISCHARGE_FLOW)]
        public float DischargeFlow { get; set; }

        [View(DisplayNames.DISCHARGE_CAUSE)]
        public DischargeCause DischargeCause { get; set; }

        [View(DisplayNames.DISCHARGE_DURATION)]
        public float DischargeDuration { get; set; }

        public string Remarks { get; set; }

        #endregion
    }

    public interface ISearchNpdesRegulatorInspectionReport : ISearchSet<NpdesRegulatorInspectionReportItem>
    {
        [SearchAlias("OperatingCenter", "Id")]
        int? OperatingCenter { get; set; }

        [SearchAlias("Town", "Id")]
        int? Town { get; set; }

        [Search(CanMap = false)]
        int? Year { get; set; }

        [Search(CanMap = false)]
        DateRange DepartureDateTime { get; set; }
    }
}