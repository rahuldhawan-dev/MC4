using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SoilConditionsWithinExcavationType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int CAN_BARELY_PENETRATE_SOIL_WITHIN_EXCAVATION = 1,
                             CAN_PENETRATE_SOIL_WITH_MODERATE_PRESSURE = 2,
                             CAN_EASILY_PENETRATE_SOIL = 3;
        }
    }
}
