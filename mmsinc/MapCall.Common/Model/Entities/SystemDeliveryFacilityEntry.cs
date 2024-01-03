using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    /// <summary>
    /// This class represents a daily entry for a facility. For example:
    /// It's an entry for either Water or Wastewater which is contained in SystemDeliveryType. It will
    /// have a SystemDeliveryEntryType that's appropriate for the SystemDeliveryType, and be associated to a
    /// SystemDeliveryEntry.
    /// 
    /// Each entry is EnteredBy an Employee who will typically create several SystemDeliveryFacilityEntries for
    /// one or more Facilities. Sometimes, like when a SystemDeliveryFacilityEntry has a SystemDeliveryEntryType
    /// of 'Transferred To', the SupplierFacility might have a value. The SupplierFacility value will come from
    /// the Facility's SystemDeliveryEntryType if it has one.
    /// 
    /// Each entry will also contain an EntryDate and EntryValue. IsInjection's are rare and exist in anticipation
    /// for some facilities experiencing drought conditions (CA) to have to "inject" water back into the system.
    /// Whether IsInjection is displayed in a view is determined by the Facility's SystemDeliveryEntryType.
    ///
    /// Additionally, an entry may have some SystemDeliveryFacilityEntryAdjustments if the entry was edited after
    /// the SystemDeliveryEntry IsValidated.
    /// 
    /// Knowing if an entry HasBeenAdjusted, if there were any AdjustmentComments, and what the original entry value was,
    /// is what helps us not have to call down to the SystemDeliveryFacilityEntryAdjustments department for every entry
    /// just to ask, "hey, do you happen to have any adjustments for this SystemDeliveryFacilityEntry?"
    ///
    /// Sometimes there are a few questions we like to ask ourselves, like: Is this entry Being Adjusted, and if so,
    /// what's the AdjustedEntryValue? Or, Is the Facility an Injection Site, and what's the name of that Facility
    /// we Purchased water from?
    /// </summary>
    [Serializable]
    public class SystemDeliveryFacilityEntry : IEntity
    {
        #region Constants

        public struct Labels
        {
            public const string IS_INJECTION = "Injection?";
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual SystemDeliveryType SystemDeliveryType { get; set; }
        
        public virtual SystemDeliveryEntry SystemDeliveryEntry { get; set; }
        
        public virtual SystemDeliveryEntryType SystemDeliveryEntryType { get; set; }
        
        public virtual Employee EnteredBy { get; set; }
        
        public virtual Facility Facility { get; set; }
        
        public virtual Facility SupplierFacility { get; set; }

        [View(DisplayFormat = CommonStringFormats.DATE)]
        public virtual DateTime EntryDate { get; set; }
        
        public virtual decimal EntryValue { get; set; }
        
        public virtual decimal? WeeklyTotal { get; set; }
        
        public virtual bool IsInjection { get; set; }
        
        public virtual bool IsBeingAdjusted { get; set; }
        
        public virtual bool HasBeenAdjusted { get; set; }
        
        public virtual decimal? AdjustedEntryValue { get; set; }
        
        public virtual decimal? OriginalEntryValue { get; set; }
        
        public virtual string AdjustmentComment { get; set; }
        
        public virtual bool IsInjectionSite =>
            Facility?.FacilitySystemDeliveryEntryTypes?.FirstOrDefault(x =>
                          x.IsEnabled == true && x.SystemDeliveryEntryType?.Id == SystemDeliveryEntryType?.Id)
                    ?.IsInjectionSite ?? false;
        
        public virtual string SupplierFacilityName => SupplierFacility?.Description ?? string.Empty;
        
        public virtual string PurchaseSupplierName =>
            SystemDeliveryEntryType?.Id == SystemDeliveryEntryType.Indices.PURCHASED_WATER &&
            Facility.FacilitySystemDeliveryEntryTypes.Any(x =>
                x.SystemDeliveryEntryType?.Id == SystemDeliveryEntryType.Indices.PURCHASED_WATER && x.IsEnabled)
                ? Facility?.FacilitySystemDeliveryEntryTypes?.SingleOrDefault(x =>
                                x.SystemDeliveryEntryType?.Id == SystemDeliveryEntryType.Indices.PURCHASED_WATER &&
                                x.IsEnabled)
                          ?.PurchaseSupplier
                : string.Empty;

        public virtual IList<SystemDeliveryFacilityEntryAdjustment> Adjustments { get; set; } =
            new List<SystemDeliveryFacilityEntryAdjustment>();

        #endregion
    }
}
