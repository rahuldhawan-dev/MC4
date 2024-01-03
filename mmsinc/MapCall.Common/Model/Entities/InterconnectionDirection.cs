using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class InterconnectionDirection : EntityLookup
    {
        #region Constants

        public new struct StringLengths
        {
            public const int DESCRIPTION = 20;
        }

        #endregion

        #region Properties

        [Required, StringLength(StringLengths.DESCRIPTION)]
        public override string Description { get; set; }

        public virtual IList<Interconnection> Interconnections { get; set; }

        #endregion

        #region Constructors

        public InterconnectionDirection()
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
