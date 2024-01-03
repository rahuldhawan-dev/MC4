using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class TankInspectionType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int ROUTINE = 1,
                             INSPECTION_SITE_OBSERVATION = 2,
                             COMPREHENSIVE = 3,
                             WARRANTY = 4,
                             INSPECTION_SITE_OBSERVATION_DRONE = 5;
        }
    }
}