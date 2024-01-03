using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Department : IEntityLookup
    {
        #region Structs

        public struct Indices
        {
            public const int T_AND_D = 1, CFS = 2, PRODUCTION = 3, WASTE_WATER = 18;
        }

        public static readonly int[] SAP_DEPARTMENTS = {Indices.T_AND_D, Indices.PRODUCTION, Indices.WASTE_WATER};

        public struct StringLengths
        {
            public const int DESCRIPTION = 50, CODE = 2;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        [Required, StringLength(StringLengths.DESCRIPTION)]
        public virtual string Description { get; set; }

        [StringLength(StringLengths.CODE)]
        public virtual string Code { get; set; }

        public virtual IList<Facility> Facilities { get; set; }
        public virtual IList<BusinessUnit> BusinessUnits { get; set; }

        #endregion

        #region Constructors

        public Department()
        {
            Facilities = new List<Facility>();
            BusinessUnits = new List<BusinessUnit>();
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
