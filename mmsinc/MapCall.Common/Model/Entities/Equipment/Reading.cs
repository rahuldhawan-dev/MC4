using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Reading : IEntity, IValidatableObject
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual DateTime DateTimeStamp { get; set; }
        public virtual int? RawData { get; set; }
        public virtual float ScaledData { get; set; }
        public virtual int? CheckSum { get; set; }
        public virtual int? Interpolate { get; set; }

        public virtual Sensor Sensor { get; set; }

        #endregion

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }

    public class ReadingCalculation
    {
        public Sensor Sensor { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }

    public class ReadingCalculationSummary
    {
        /// <summary>
        /// Returns all the Reading instances used for the calculations if they were included
        /// by the repository. This will return null if they were not included.
        /// </summary>
        public IEnumerable<Reading> Readings { get; set; }

        /// <summary>
        /// Returns all the calculations broken up by sensor and date.
        /// </summary>
        public IEnumerable<ReadingCalculation> Calculations { get; set; }

        /// <summary>
        /// Returns all the calculations by date with the totals for all sensors for a date combined.
        /// </summary>
        public IDictionary<DateTime, double> Totals { get; set; }
    }
}
