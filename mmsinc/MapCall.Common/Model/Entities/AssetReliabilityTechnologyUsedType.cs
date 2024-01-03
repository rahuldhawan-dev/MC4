using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class AssetReliabilityTechnologyUsedType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int INFRARED_THERMOGRAPHY = 1,
                             VIBRATION_ANALYSIS = 2,
                             MOTOR_WINDING_ANALYSIS_INSULATION_RESISTANCE = 3,
                             VISUAL_INSPECTION = 4,
                             AIRBORNE_ULTRASOUND = 5,
                             LASER_ALIGNMENT = 6,
                             EARTH_GROUND_TESTING = 7,
                             ELECTRICAL_TESTING = 8,
                             WIRE_TO_WATER_PUMP_PERFORMANCE = 9,
                             MOTION_AMPLIFICATION = 11,
                             PROTECTIVE_RELAY_TESTING = 12,
                             OTHER = 13,
                             BATTERY_TESTING = 14,
                             DYNAMIC_MOTOR_TESTING_ESA = 15,
                             MICRO_OHMMETER_TESTING = 16;
        }
    }
}
