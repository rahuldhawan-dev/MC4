using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class InterconnectionFlowControlMethod : EntityLookup
    {
        #region Constants

        public new struct StringLengths
        {
            public const int DESCRIPTION = 25;
        }

        #endregion

        #region Properties

        [Required, StringLength(StringLengths.DESCRIPTION)]
        public override string Description { get; set; }

        public virtual IList<Interconnection> Interconnections { get; set; }

        #endregion

        #region Constructors

        public InterconnectionFlowControlMethod()
        {
            Interconnections = new List<Interconnection>();
        }

        #endregion
    }
}
