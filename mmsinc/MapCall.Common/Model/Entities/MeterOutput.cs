using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MeterOutput : IEntity
    {
        public virtual int Id { get; set; }

        [Required]
        public virtual int Outputs { get; set; }
    }
}
