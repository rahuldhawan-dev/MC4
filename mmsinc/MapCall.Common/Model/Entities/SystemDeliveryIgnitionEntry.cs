using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    /// <summary>
    /// This class represents data received from Ignition. The EntryValue will eventually make its way
    /// into a SystemDeliveryFacilityEntry when a user creates a SystemDeliveryEntry for the EntryDate, FacilityId,
    /// SystemDeliveryType, and SystemDeliveryEntryType.
    /// </summary>
    [Serializable]
    public class SystemDeliveryIgnitionEntry : IEntity
    {
        public virtual int Id { get; set; }
        
        public virtual int FacilityId { get; set; }
        
        public virtual string UnitOfMeasure { get; set; }
        
        public virtual int SystemDeliveryType { get; set; }
        
        public virtual int SystemDeliveryEntryType { get; set; }
        
        public virtual DateTime EntryDate { get; set; }
        
        public virtual string FacilityName { get; set; }
        
        public virtual decimal EntryValue { get; set; }
    }
}
