using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using DataAnnotationsExtensions;
using MMSINC.Data.NHibernate;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class NpdesRegulatorInspectionViewModel : ViewModel<NpdesRegulatorInspection>
    {
        #region Properties

        // This gets set by the page via js.
        [DoesNotAutoMap]
        public bool IsMapPopup { get; set; }

        [DoesNotAutoMap]
        public NpdesRegulatorInspection Display =>
            _container.GetInstance<IRepository<NpdesRegulatorInspection>>().Find(Id);

        [DoesNotAutoMap]
        public SewerOpening SewerOpeningDisplay =>
            _container.GetInstance<IRepository<SewerOpening>>().Find(SewerOpening);

        [Required, DropDown, EntityMap, EntityMustExist(typeof(SewerOpening))]
        public int SewerOpening { get; set; }

        public string InspectedBy { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITHOUT_SECONDS, ApplyFormatInEditMode = true)]
        [Required, DateTimePicker, View(NpdesRegulatorInspection.Display.ARRIVAL_DATE_TIME)]
        public DateTime? ArrivalDateTime { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITHOUT_SECONDS, ApplyFormatInEditMode = true)]
        [DateTimePicker]
        public DateTime? DepartureDateTime { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(NpdesRegulatorInspectionType))]
        public int? NpdesRegulatorInspectionType { get; set; }

        [Required, View(NpdesRegulatorInspection.Display.HAS_INFILTRATION)]
        public bool? HasInfiltration { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(WeatherCondition))]
        public int? WeatherCondition { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(OutfallCondition))]
        [View(NpdesRegulatorInspection.Display.OUTFALL_CONDITION)]
        public int? OutfallCondition { get; set; }

        [Required, View(NpdesRegulatorInspection.Display.GATE_MOVING_FREELY)]
        [DropDown, EntityMap, EntityMustExist(typeof(GateStatusAnswerType))]
        public int? GateStatusAnswerType { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(BlockCondition))]
        public int? BlockCondition { get; set; }

        [Required, View(NpdesRegulatorInspection.Display.DISCHARGE_PRESENT)]
        public bool? IsDischargePresent { get; set; }

        [RequiredWhen(nameof(IsDischargePresent), ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true, ErrorMessage = "Wet weather related is required.")]
        [DropDown, EntityMap, EntityMustExist(typeof(DischargeWeatherRelatedType))]
        [View(NpdesRegulatorInspection.Display.DISCHARGE_WEATHER_RELATED)]
        public int? DischargeWeatherRelatedType { get; set; }

        [RequiredWhen(nameof(IsDischargePresent), ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        [View(NpdesRegulatorInspection.Display.RAINFALL_ESTIMATE), Min(NpdesRegulatorInspection.ValueRanges.MIN_INCHES)]
        public float? RainfallEstimate { get; set; }

        [DoesNotAutoMap]
        public int? BodyOfWater { get; set; }

        [RequiredWhen(nameof(IsDischargePresent), ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        [View(NpdesRegulatorInspection.Display.DISCHARGE_FLOW)]
        [Range(NpdesRegulatorInspection.ValueRanges.MIN_RANGE_PERCENT, NpdesRegulatorInspection.ValueRanges.MAX_RANGE_PERCENT)]
        public float? DischargeFlow { get; set; }

        [RequiredWhen(nameof(IsDischargePresent), ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        [DropDown, EntityMap, EntityMustExist(typeof(DischargeCause))]
        public int? DischargeCause { get; set; }

        [RequiredWhen(nameof(IsDischargePresent), ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        [View(NpdesRegulatorInspection.Display.DISCHARGE_DURATION), Min(NpdesRegulatorInspection.ValueRanges.MIN_HR)]
        public float? DischargeDuration { get; set; }

        [RequiredWhen(nameof(IsDischargePresent), ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        [View(NpdesRegulatorInspection.Display.PLUME_PRESENT)]
        public bool? IsPlumePresent { get; set; }

        [RequiredWhen(nameof(IsDischargePresent), ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        [View(NpdesRegulatorInspection.Display.EROSION_PRESENT)]
        public bool? IsErosionPresent { get; set; }

        [RequiredWhen(nameof(IsDischargePresent), ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        [View(NpdesRegulatorInspection.Display.SOLIDS_FLOATABLES_PRESENT)]
        public bool? IsSolidFloatPresent { get; set; }

        [RequiredWhen(nameof(IsDischargePresent), ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        [View(NpdesRegulatorInspection.Display.ADDITIONAL_EQUIPMENT_NECESSARY)]
        public bool? IsAdditionalEquipmentNeeded { get; set; }

        [RequiredWhen(nameof(IsDischargePresent), ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        [View(NpdesRegulatorInspection.Display.SAMPLES_TAKEN)]
        public bool? HasSamplesBeenTaken { get; set; }

        [RequiredWhen(nameof(HasSamplesBeenTaken), ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        [StringLength(NpdesRegulatorInspection.StringLengths.SAMPLE_LOCATION)]
        public string SampleLocation { get; set; }

        [View(NpdesRegulatorInspection.Display.FLOW_METER_MAINTENANCE)]
        public bool? HasFlowMeterMaintenanceBeenPerformed { get; set; }

        [View(NpdesRegulatorInspection.Display.DOWNLOADED_FLOW_METER_DATA)]
        public bool? HasDownloadedFlowMeterData { get; set; }

        [View(NpdesRegulatorInspection.Display.CALIBRATED_FLOW_METER)]
        public bool? HasCalibratedFlowMeter { get; set; }

        [View(NpdesRegulatorInspection.Display.INSTALLED_FLOW_METER)]
        public bool? HasInstalledFlowMeter { get; set; }

        [View(NpdesRegulatorInspection.Display.REMOVED_FLOW_METER)]
        public bool? HasRemovedFlowMeter { get; set; }

        [View(NpdesRegulatorInspection.Display.OTHER_REMARKS)]
        public bool? HasFlowMeterBeenMaintainedOther { get; set; }

        [StringLength(NpdesRegulatorInspection.StringLengths.REMARKS)]
        public string Remarks { get; set; }

        #endregion

        #region Constructor

        public NpdesRegulatorInspectionViewModel(IContainer container) : base(container) { }

        #endregion
    }
}
