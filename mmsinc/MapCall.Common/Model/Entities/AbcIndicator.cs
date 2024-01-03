using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ABCIndicator : EntityLookup
    {
        public struct Indices
        {
            public const int HIGH = 3, MEDIUM = 2, LOW = 1;
        }

        [Required]
        [StringLength(20)]
        public override string Description { get; set; }
    }
}
