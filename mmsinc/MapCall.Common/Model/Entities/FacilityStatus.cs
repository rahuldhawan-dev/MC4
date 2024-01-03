using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FacilityStatus : IEntity
    {
        #region Structs

        public struct StringLengths
        {
            public const int DESCRIPTION = 50;
        }

        public struct Indices
        {
            public const int ACTIVE = 159, INACTIVE = 160, PENDING = 161, PENDING_RETIREMENT = 162;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        [Required, StringLength(FEMAFloodRating.StringLengths.DESCRIPTION)]
        public virtual string Description { get; set; }

        public virtual IList<Facility> Facilities { get; set; }

        #endregion

        #region Constructors

        public FacilityStatus()
        {
            Facilities = new List<Facility>();
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }
}
