using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class InterconnectionType : EntityLookup
    {
        #region Constants

        public new struct StringLengths
        {
            #region Constants

            public const int DESCRIPTION = 10;

            #endregion
        }

        #endregion

        #region Properties

        [Required, StringLength(StringLengths.DESCRIPTION)]
        public override string Description { get; set; }

        public virtual IList<Interconnection> Interconnections { get; set; }

        #endregion

        #region Constructors

        public InterconnectionType()
        {
            Interconnections = new List<Interconnection>();
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
