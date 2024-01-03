using System;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.ViewModels
{
    public class ProductionWorkOrderEquipmentNotification
    {
        public string ProductionWorkOrderUrl { get; set; }
        public int ProductionWorkOrderId { get; set; }
        public string ProductionWorkDescription { get; set; }
        public CorrectiveOrderProblemCode CorrectiveOrderProblemCode { get; set; }
        public string AsLeftConditionComment { get; set; }
        public ProductionWorkOrder RoutineWorkOrder { get; set; }
        public string FacilityUrl { get; set; }
        public string FacilityName { get; set; }
        public string EquipmentUrl { get; set; }
        public Equipment Equipment { get; set; }
        public string RoutineWorkOrderUrl { get; set; }
        public DateTime? DateReceived { get; set; }
    }
}
