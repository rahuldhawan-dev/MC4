using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MapIcon : IEntity, IValidatableObject
    {
        #region Constants

        // TODO: This format isn't valid for MapCall proper.
        public const string URL_FORMAT = "~/Content/images/{0}";

        public struct Indices
        {
            public const int WorkOrder = 67,
                             PREMISE_BLUE_ORANGE = 89,
                             PREMISE_MABLUE = 88,
                             PREMISE_ORANGE = 87,
                             PREMISE_BLUE = 82;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string FileName { get; set; }
        public virtual int Height { get; set; }
        public virtual int Width { get; set; }

        public virtual MapIconOffset Offset { get; set; }

        public virtual IList<IconSet> IconSets { get; set; } = new List<IconSet>();

        #region Logical Properties

        public virtual string Url => string.Format(URL_FORMAT, FileName);

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
