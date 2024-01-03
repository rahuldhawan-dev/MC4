using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FacilitySystemDeliveryEntryType : IEntity
    {
        public virtual int Id { get; set; }
        
        public virtual Facility Facility { get; set; }
        
        public virtual Facility SupplierFacility { get; set; }
        
        public virtual string PurchaseSupplier { get; set;}
        
        public virtual SystemDeliveryEntryType SystemDeliveryEntryType { get; set; }
         
        public virtual bool IsEnabled { get; set; }
        
        public virtual decimal? MinimumValue { get; set; }
        
        public virtual decimal? MaximumValue { get; set; }
        
        public virtual int? BusinessUnit { get; set;}
        
        public virtual bool IsInjectionSite { get; set; }

        public virtual bool IsAutomationEnabled { get; set; }
    }
}
