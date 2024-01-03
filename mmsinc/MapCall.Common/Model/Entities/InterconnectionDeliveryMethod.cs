using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class InterconnectionDeliveryMethod : EntityLookup
    {
        #region Constants

        public new struct StringLengths
        {
            #region Constants

            public const int DESCRIPTION = 15;

            #endregion
        }

        #endregion

        #region Properties

        [Required, StringLength(StringLengths.DESCRIPTION)]
        public override string Description { get; set; }

        public virtual IList<Interconnection> Interconnections { get; set; }

        #endregion

        #region Constructors

        public InterconnectionDeliveryMethod()
        {
            Interconnections = new List<Interconnection>();
        }

        #endregion
    }
}
