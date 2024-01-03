using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ProductionWorkOrderMeasurementPointValue : IEntity
    {
        public virtual int Id { get; set; }
        public virtual ProductionWorkOrder ProductionWorkOrder { get; set; }
        public virtual MeasurementPointEquipmentType MeasurementPointEquipmentType { get; set; }
        public virtual Equipment Equipment { get; set; }
        public virtual string Value { get; set; }
        public virtual int? MeasurementDocId { get; set; }
    }
}
