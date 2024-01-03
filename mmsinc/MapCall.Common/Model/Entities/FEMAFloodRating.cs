using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FEMAFloodRating : IEntity
    {
        #region Structs

        public struct StringLengths
        {
            public const int DESCRIPTION = 50;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        [Required, StringLength(StringLengths.DESCRIPTION)]
        public virtual string Description { get; set; }

        public virtual IList<Facility> Facilities { get; set; }

        #endregion

        #region Constructors

        public FEMAFloodRating()
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
