using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SpoilRemoval : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual DateTime DateRemoved { get; set; }
        public virtual decimal Quantity { get; set; }

        #endregion

        #region Association Properties

        public virtual SpoilStorageLocation RemovedFrom { get; set; }
        public virtual SpoilFinalProcessingLocation FinalDestination { get; set; }

        #endregion
    }
}
