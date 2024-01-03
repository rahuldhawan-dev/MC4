using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EventType : IEntity
    {
        #region Consts

        public struct StringLengths
        {
            public const int DESCRIPTION = 50,
                             CREATED_BY = 50;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Description { get; set; }
        public virtual string CreatedBy { get; set; }

        #endregion
    }

    [Serializable]
    public class EventTypeDisplayItem : DisplayItem<EventType>
    {
        public string Description { get; set; }
        public override string Display => $"{Description}";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }
    }
}
