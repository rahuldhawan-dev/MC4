using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PolicyPractice : ReadOnlyEntityLookup, IValidatableObject
    {
        //public virtual int Id { get; set; } 
        //[StringLength(255)]
        //public virtual string Description { get; set; } 
        public virtual string Summary { get; set; }

        /* THESE MIGHT ALL MATTER AT SOME POINT:
        public virtual Facility Facility { get; set; } 
        public virtual int? RegulationId { get; set; } 
        public virtual int? EquipmentId { get; set; } 
        public virtual DateTime? DateApproved { get; set; } 
        public virtual DateTime? DateIssued { get; set; } 
        [StringLength(50)]
        public virtual string Revision { get; set; } 
        public virtual string ReviewFrequencyDays { get; set; } 
        public virtual bool? PsmTcpa { get; set; } 
        public virtual bool? Dpcc { get; set; } 
        public virtual bool? Osha { get; set; } 
        public virtual bool? Company { get; set; } 
        public virtual bool? Sox { get; set; } 
        */
    }
}
