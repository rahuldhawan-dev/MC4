using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ProductionPrerequisite : EntityLookup
    {
        public struct Indices
        {
            public const int HAS_LOCKOUT_REQUIREMENT = 1,
                             IS_CONFINED_SPACE = 2,
                             JOB_SAFETY_CHECKLIST = 3,
                             AIR_PERMIT = 4,
                             HOT_WORK = 5,
                             PRE_JOB_SAFETY_BRIEF = 6,
                             RED_TAG_PERMIT = 7;
        }

        [ScriptIgnore]
        public virtual IList<Equipment> Equipment { get; set; }

        public ProductionPrerequisite()
        {
            Equipment = new List<Equipment>();
        }
    }
}
