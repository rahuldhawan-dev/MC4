using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Validation;

namespace MMSINC.Data
{
    public class RequiredDateRange : DateRange
    {
        #region Properties

        [RequiredWhen("Operator", RangeOperator.Between)]
        public override DateTime? Start { get; set; }

        [Required]
        public override DateTime? End { get; set; }

        public override bool IsValid
        {
            get
            {
                if (End == null)
                    return false;
                return base.IsValid;
            }
        }

        #endregion
    }
}
