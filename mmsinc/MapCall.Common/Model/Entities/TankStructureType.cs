using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class TankStructureType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int ROUTINE_STANDARD = 1,
                             OBSERVATION_SITE_OBSERVATION = 2,
                             COMPREHENSIVE = 3,
                             WARRANTY = 4;
        }
    }
}
