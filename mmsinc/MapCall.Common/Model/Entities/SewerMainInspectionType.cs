using System;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SewerMainInspectionType : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int ACOUSTIC = 1,
                             CCTV = 2,
                             MAIN_CLEANING_PM = 3,
                             SMOKE_TEST = 4;
        }

        public virtual bool RequiresGrade => new[] { Indices.ACOUSTIC, Indices.CCTV }.Contains(Id);
    }
}
