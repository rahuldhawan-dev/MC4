using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EnvironmentalPermitType : ReadOnlyEntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int AIR_PERMIT = 1,
                             ALLOCATION_PERMIT = 2,
                             BSDW_PERMIT = 3,
                             BSDW_CONSTRUCTION = 4,
                             CMO_PERMIT = 5,
                             DAM_PERMIT = 6,
                             DPCC = 7,
                             LAND_USE_PERMIT = 8,
                             MASTER_PERMIT = 9,
                             PDES_PERMIT = 10,
                             PHYSICAL_CONNECTION = 18,
                             REMEDIAL_ACTION = 19,
                             RADIOLOGICAL_PERMIT = 11,
                             SANITARY_COLLECTION = 12,
                             STREAM_ENCROACHMENT = 13,
                             TCPA = 14,
                             UNDERGROUND_INJECTION_CONTROL = 15,
                             UST_PERMIT = 16,
                             WASTE_WATER_PERMIT_ = 17;
        }

        #endregion
    }
}
