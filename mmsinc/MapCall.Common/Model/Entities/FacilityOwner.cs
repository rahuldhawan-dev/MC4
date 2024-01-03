using System;
using System.Collections.Generic;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FacilityOwner : ReadOnlyEntityLookup
    {
        #region Structs

        public struct StringLengths
        {
            public const int DESCRIPTION = 50;
        }
        
        public struct Indices
        {
            public const int AMERICAN_WATER = 157;
        }

        #endregion

        #region Properties

        //public virtual int Id { get; protected set; }
        //[Required, StringLength(StringLengths.DESCRIPTION)]
        //public virtual string Description { get; set; }

        public virtual IList<Facility> Facilities { get; set; }

        #endregion

        #region Constructors

        public FacilityOwner()
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
