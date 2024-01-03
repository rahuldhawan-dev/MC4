using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MeterDialCount : IEntity
    {
        public virtual int Id { get; set; }

        [Required]
        public virtual int Description { get; set; }
    }
}
