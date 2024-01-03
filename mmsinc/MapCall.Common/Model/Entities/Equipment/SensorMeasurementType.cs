using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SensorMeasurementType : ReadOnlyEntityLookup
    {
        public struct Descriptions
        {
            public const string KILOWATT = "kw",
                                WATT = "watts",
                                KILOWATT_HOURS = "kwh";
        }
    }
}
