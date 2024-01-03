using System;
using System.Collections.Generic;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class UtilityTransformerKVARating : ReadOnlyEntityLookup
    {
        #region Properties

        public virtual IList<Voltage> Voltages { get; set; }

        #endregion

        #region Constructor

        public UtilityTransformerKVARating()
        {
            Voltages = new List<Voltage>();
        }

        #endregion
    }
}
