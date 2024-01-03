using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    [DebuggerDisplay(
        "MarkoutType: {MarkoutType}, DateOfRequest: {DateOfRequest}, ReadyDate: {ReadyDate}, ExpirationDate: {ExpirationDate}")]
    public class Markout : IEntity, IValidatableObject
    {
        #region Constants

        public struct StringLengths
        {
            public const int MARKOUT_NUMBER_MAX_LENGTH = 20,
                             MARKOUT_NUMBER_MIN_LENGTH = 9;
        }

        #endregion

        #region Properties

        #region Table Column Properties

        public virtual int Id { get; set; }
        public virtual string MarkoutNumber { get; set; }

        public virtual string Note { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? DateOfRequest { get; set; }

        [View(FormatStyle.DateTimeWithoutSeconds)]

        public virtual DateTime? ReadyDate { get; set; }

        [View(FormatStyle.DateTimeWithoutSeconds)]
        public virtual DateTime? ExpirationDate { get; set; }

        public virtual MarkoutType MarkoutType { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }

        #endregion

        #endregion

        #region Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
