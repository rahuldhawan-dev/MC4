using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WaterQualityComplaintSampleResult : IEntity, IValidatableObject
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual WaterConstituent WaterConstituent { get; set; }
        public virtual WaterQualityComplaint Complaint { get; set; }

        [Required]
        public virtual DateTime SampleDate { get; set; }

        [Required]
        [StringLength(50)]
        public virtual string SampleValue { get; set; }

        [StringLength(25)]
        public virtual string UnitOfMeasure { get; set; }

        [Required]
        [StringLength(50)]
        public virtual string AnalysisPerformedBy { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
