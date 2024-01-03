using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Gradient : ReadOnlyEntityLookup
    {
        #region References

        public virtual IList<Town> Towns { get; set; }

        #endregion

        #region Constructors

        public Gradient()
        {
            Towns = new List<Town>();
        }

        #endregion
    }
}
