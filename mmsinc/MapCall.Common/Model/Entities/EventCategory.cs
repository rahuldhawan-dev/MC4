using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EventCategory : EntityLookup
    {
        public struct Indices
        {
            public const int CONTROLLED = 1,
                             UNCONTROLLED = 2;
        }

        [Required]
        [StringLength(50)]
        public override string Description { get; set; }
    }
}
