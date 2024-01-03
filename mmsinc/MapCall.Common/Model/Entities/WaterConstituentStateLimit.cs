using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WaterConstituentStateLimit : IEntity
    {
        public virtual int Id { get; set; }

        [StringLength(255)]
        public virtual string Description { get; set; }

        [StringLength(255)]
        public virtual string Agency { get; set; }

        public virtual float? Min { get; set; }
        public virtual float? Max { get; set; }
        public virtual float? Mcl { get; set; }
        public virtual float? Mclg { get; set; }
        public virtual float? Smcl { get; set; }

        [StringLength(255)]
        public virtual string ActionLimit { get; set; }

        [StringLength(255)]
        public virtual string Regulation { get; set; }

        [Range(0, 9999)]
        public virtual int? StateDEPAnalyteCode { get; set; }

        public virtual WaterConstituent WaterConstituent { get; set; }
        public virtual State State { get; set; }
        public virtual UnitOfWaterSampleMeasure UnitOfMeasure { get; set; }
    }
}
