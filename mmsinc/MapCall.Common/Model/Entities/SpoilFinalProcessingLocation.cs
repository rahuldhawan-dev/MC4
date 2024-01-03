using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SpoilFinalProcessingLocation : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }

        [Required]
        [StringLength(30)]
        public virtual string Name { get; set; }

        #endregion

        #region Associated Properties

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Town Town { get; set; }
        public virtual Street Street { get; set; }
        public virtual IList<SpoilRemoval> SpoilRemovals { get; set; }

        #endregion

        #region Constructor

        public SpoilFinalProcessingLocation()
        {
            SpoilRemovals = new List<SpoilRemoval>();
        }

        #endregion
    }
}
