using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class InterconnectionPurchaseSellTransfer : EntityLookup
    {
        #region Constants

        public struct StringLenghts
        {
            #region Constants

            public const int DESCRIPTION = 8;

            #endregion
        }

        #endregion

        #region Properties

        [Required, StringLength(StringLenghts.DESCRIPTION)]
        public override string Description { get; set; }

        public virtual IList<Interconnection> Interconnections { get; set; }

        #endregion

        #region Constructors

        public InterconnectionPurchaseSellTransfer()
        {
            Interconnections = new List<Interconnection>();
        }

        #endregion
    }
}
