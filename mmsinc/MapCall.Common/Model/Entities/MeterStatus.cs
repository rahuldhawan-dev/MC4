using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MeterStatus : IEntity
    {
        public const int DESCRIPTION_LENGTH = 50;

        public virtual int Id { get; set; }

        [Required, StringLength(DESCRIPTION_LENGTH)]
        public virtual string Description { get; set; }
    }
}
