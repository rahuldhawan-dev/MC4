using System;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    /// <summary>
    /// Represents the most current information available about the <see cref="CustomerSideMaterial"/>,
    /// <see cref="ServiceLineSize"/>, <see cref="ReadingDeviceDirectionalLocation"/>, and
    /// <see cref="ReadingDevicePositionalLocation"/> at a given <see cref="Premise"/> gleaned from
    /// Short Cycle Work Orders and their Completions in the Work1View system. 
    /// </summary>
    [Serializable]
    public class ShortCycleCustomerMaterial : IEntity
    {
        public virtual int Id { get; set; }
        
        public virtual Premise Premise { get; set; }
        
        [View("Assignment Date")]
        public virtual DateTime? AssignmentStart { get; set; }
        
        public virtual ServiceMaterial CustomerSideMaterial { get; set; }
        
        public virtual string ServiceLineSize { get; set; }
        
        public virtual DateTime? TechnicalInspectedOn { get; set; }
        
        [View("Location")]
        public virtual SmallMeterLocation ReadingDevicePositionalLocation { get; set; }
        
        [View("Direction")]
        public virtual MeterDirection ReadingDeviceDirectionalLocation { get; set; }
        
        [View("Work Order Number")]
        public virtual long ShortCycleWorkOrderNumber { get; set; }
    }
}
