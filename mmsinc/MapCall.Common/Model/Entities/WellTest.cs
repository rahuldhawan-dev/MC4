using System;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WellTest : IEntity, IThingWithEmployee
    {
        #region Constants

        public readonly struct Ranges
        {
            public const int
                PUMPING_RATE_MIN = 10,
                PUMPING_RATE_MAX = 10000;

            public const double
                MEASUREMENT_POINT_MIN = 1,
                MEASUREMENT_POINT_MAX = 30;
        }

        public readonly struct Validation
        {
            public const string
                STATIC_WATER_LEVEL_REGEX = @"^(-[1-9]|-[1-9]\d|-100|\d|[1-9]\d{1,2}|1000)(\.[0-9]{2})$",
                STATIC_WATER_LEVEL_ERROR_MSG = "Static Water Level must be between -100.00 and 1000.00, and to the nearest 0.01 foot.",
                PUMPING_WATER_LEVEL_REGEX = @"^(\d|[1-9]\d{1,2}|1000)(\.[0-9]{2})$",
                PUMPING_WATER_LEVEL_ERROR_MSG = "Pumping Water Level must be between 0.00 and 1000.00, and to the nearest 0.01 foot.",
                STATIC_WATER_LEVEL_GREATER_THAN_PUMPING_WATER_LEVEL_MSG =
                    "Static Water Level is greater than Pumping Water Level; please check your entries for accuracy.",
                INVALID_PUMP_DEPTH_MESSAGE = "Pump Depth is invalid; please check Equipment Characteristics for accuracy.",
                INVALID_WELL_DEPTH_MESSAGE = "Well Depth is invalid; please check Equipment Characteristics for accuracy.",
                INVALID_WELL_CAPACITY_RATING_MESSAGE = "Well Capacity Rating is invalid; please check Equipment Characteristics for accuracy.",
                STATIC_WATER_LEVEL_GREATER_THAN_PUMP_DEPTH_MESSAGE =
                    "Static Water Level is greater than Pump Depth; please check your entries and Equipment Characteristics for accuracy.",
                STATIC_WATER_LEVEL_GREATER_THAN_WELL_DEPTH_MESSAGE =
                    "Static Water Level is greater than Well Depth; please check your entries and Equipment Characteristics for accuracy.",
                PUMPING_WATER_LEVEL_GREATER_THAN_PUMP_DEPTH_MESSAGE =
                    "Pumping Water Level is greater than Pump Depth; please check your entries and Equipment Characteristics for accuracy.",
                PUMPING_WATER_LEVEL_GREATER_THAN_WELL_DEPTH_MESSAGE =
                    "Pumping Water Level is greater than Well Depth; please check your entries and Equipment Characteristics for accuracy.",
                PUMPING_RATE_GREATER_THAN_WELL_CAPACITY_RATING_MESSAGE =
                    "Pumping Rate is greater than Well Capacity Rating; please check your entries and Equipment Characteristics for accuracy.";
        }

        public readonly struct DisplayNames
        {
            public readonly struct Equipment
            {
                public const string
                    WELL_DIAMETER_TOP = "Diameter of Well - Top (in)",
                    WELL_DIAMETER_BOTTOM = "Diameter of Well - Bottom (in)",
                    PUMP_DEPTH = "Pump Depth (ft)",
                    WELL_DEPTH = "Well Depth (ft)";
            }

            public readonly struct Form
            {
                public const string
                    PUMPING_RATE = "Pumping Rate After 1 Hour (gpm)",
                    MEASUREMENT_POINT = "Measurement Point (ft-above/below surface grade)",
                    STATIC_WATER_LEVEL = "Static Water Level (ft-bmp)",
                    PUMPING_WATER_LEVEL = "Pumping Water Level After 1 Hour (ft-bmp)",
                    DRAW_DOWN = "Drawdown (ft)",
                    ABOVE_OR_BELOW_GRADE = "Above or Below Grade",
                    SPECIFIC_CAPACITY = "Specific Capacity (gpm/ft)",
                    WELL_CAPACITY_RATING = "Well Capacity Rating (gpm)";
            }
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual ProductionWorkOrder ProductionWorkOrder { get; set; }

        public virtual Equipment Equipment { get; set; }

        public virtual State State => Equipment?.OperatingCenter?.State;

        public virtual OperatingCenter OperatingCenter => Equipment?.OperatingCenter;

        public virtual Facility Facility => Equipment?.Facility;

        public virtual string CompanySubsidiary => Equipment?.Facility?.CompanySubsidiary?.Description;

        public virtual string WaterSystem => Equipment?.Facility?.PublicWaterSupply?.Description;

        public virtual string WellName => Equipment?.Description;

        [View(DisplayNames.Equipment.WELL_DIAMETER_TOP)]
        public virtual string WellDiameterTop => Equipment?.GetCharacteristicValue("DIAMETERTOP");

        [View(DisplayNames.Equipment.WELL_DIAMETER_BOTTOM)]
        public virtual string WellDiameterBottom => Equipment?.GetCharacteristicValue("DIAMETERBOTTOM");

        [View(DisplayNames.Equipment.WELL_DEPTH)]
        public virtual string WellDepth => Equipment?.GetCharacteristicValue("WELLDEPTH");

        [View(DisplayNames.Equipment.PUMP_DEPTH)]
        public virtual string PumpDepth => Equipment?.GetCharacteristicValue("PUMPDEPTH");

        public virtual string MethodOfMeasurement => Equipment?.GetCharacteristicValue("METHOD_OF_MEASUREMENT");

        public virtual string IsWellVaulted => Equipment?.GetCharacteristicValue("WELL_VAULTED");

        public virtual string WellType => Equipment?.GetCharacteristicValue("WELL_TYPE");

        [View(DisplayNames.Form.WELL_CAPACITY_RATING)]
        public virtual string WellCapacityRating => Equipment?.GetCharacteristicValue("WELL_CAPACITY_RATING");

        public virtual Employee Employee { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime DateOfTest { get; set; }

        [View(DisplayNames.Form.PUMPING_RATE)]
        public virtual int PumpingRate { get; set; }

        [View(DisplayNames.Form.MEASUREMENT_POINT, FormatStyle.DecimalMaxTwoDecimalPlacesWithTrailingZeroes)]
        public virtual decimal MeasurementPoint { get; set; }

        [View(DisplayNames.Form.ABOVE_OR_BELOW_GRADE)]
        public virtual WellTestGradeType GradeType { get; set; }

        [View(DisplayNames.Form.STATIC_WATER_LEVEL, FormatStyle.DecimalMaxTwoDecimalPlacesWithTrailingZeroes)]
        public virtual decimal StaticWaterLevel { get; set; }

        [View(DisplayNames.Form.PUMPING_WATER_LEVEL, FormatStyle.DecimalMaxTwoDecimalPlacesWithTrailingZeroes)]
        public virtual decimal PumpingWaterLevel { get; set; }

        [View(DisplayNames.Form.DRAW_DOWN, FormatStyle.DecimalMaxTwoDecimalPlacesWithTrailingZeroes)]
        public virtual decimal DrawDown => PumpingWaterLevel - StaticWaterLevel;

        [View(DisplayNames.Form.SPECIFIC_CAPACITY, FormatStyle.DecimalMaxTwoDecimalPlacesWithTrailingZeroes)]
        public virtual decimal SpecificCapacity => PumpingRate / DrawDown;

        #endregion
    }
}
