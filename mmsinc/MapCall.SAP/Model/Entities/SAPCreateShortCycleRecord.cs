using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCall.SAP.Model.Entities
{
    [Serializable]
    public class SAPCreateShortCycleRecord : SAPEntity
    {
        #region Properties

        public virtual string MaintenanceActivityType { get; set; }
        public virtual string Installation { get; set; }
        public virtual string BusinessPartnerNumber { get; set; }
        public virtual string ContractAccount { get; set; }
        public virtual string DeviceLocation { get; set; }
        public virtual string EquipmentNumber { get; set; }
        public virtual string SerialNumber { get; set; }
        public virtual string WorkDescription { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string CompanyNumber { get; set; }
        public virtual string ContractorName { get; set; }
        public virtual string ContractorPhone { get; set; }
        public virtual string DayFrom { get; set; }
        public virtual string DayTo { get; set; }
        public virtual string HourAM { get; set; }
        public virtual string HourPM { get; set; }
        public virtual string LetterId { get; set; }
        public virtual string Telephone { get; set; }
        public virtual string FSRId { get; set; }

        #endregion
    }
}
