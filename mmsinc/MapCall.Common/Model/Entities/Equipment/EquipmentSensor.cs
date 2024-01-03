using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EquipmentSensor : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual Equipment Equipment { get; set; }
        public virtual Sensor Sensor { get; set; }

        #endregion
    }
}
