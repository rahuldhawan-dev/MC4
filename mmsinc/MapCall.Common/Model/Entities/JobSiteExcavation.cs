using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class JobSiteExcavation : IEntity, IValidatableObject
    {
        #region Consts

        public struct StringLengths
        {
            public const int CREATED_BY = AddCreatedByToJobSiteExcavationsTable.MAX_CREATEDBY;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual JobSiteCheckList JobSiteCheckList { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS)]
        public virtual decimal WidthInFeet { get; set; } // FEET

        [DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS)]
        public virtual decimal LengthInFeet { get; set; } // FEET

        [DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS)]
        public virtual decimal DepthInInches { get; set; } // INCHES

        [DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITHOUT_SECONDS)]
        public virtual DateTime ExcavationDate { get; set; }

        public virtual JobSiteExcavationLocationType LocationType { get; set; }
        public virtual JobSiteExcavationSoilType SoilType { get; set; }

        #endregion

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }
    }
}
