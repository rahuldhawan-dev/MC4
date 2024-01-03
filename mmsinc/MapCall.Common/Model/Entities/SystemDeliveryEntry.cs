using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SystemDeliveryEntry : IEntity, IThingWithShadow
    {
        #region Properties

        public virtual int Id { get; set; }
        
        [View(FormatStyle.Date)]
        public virtual DateTime WeekOf { get; set; } // For Sys delivery, week starts on Monday. This should always been the Monday of the week the record was created.
        
        /// <summary>
        ///     When set to true, a System Delivery Entry cannot be edited any longer; it must be 'adjusted' instead
        /// </summary>
        public virtual bool? IsValidated { get; set; }

        /// <summary>
        ///     Used for making sure that the value is only true/false and not null.
        /// </summary>
        public virtual bool IsValidatedNotNull => IsValidated ?? false;

        public virtual Employee EnteredBy { get; set; }
        
        public virtual IList<Facility> Facilities { get; set; }

        [View("PWSIDs")]
        public virtual ISet<PublicWaterSupply> PublicWaterSupplies { get; set; }

        [View("WWSIDs")]
        public virtual ISet<WasteWaterSystem> WasteWaterSystems { get; set; }

        public virtual ISet<SystemDeliveryFacilityEntry> FacilityEntries { get; set; }
        
        public virtual IList<SystemDeliveryFacilityEntryAdjustment> Adjustments { get; set; }
        
        public virtual IList<OperatingCenter> OperatingCenters { get; set; }
        
        public virtual User UpdatedBy {get; set; } // Shadow property needed for scheduler
        
        public virtual DateTime UpdatedAt { get; set; } // Shadow property needed for scheduler
        
        public virtual SystemDeliveryType SystemDeliveryType { get; set; }

        public virtual Dictionary<string, Dictionary<string, decimal>> OperatingCenterCategoryTotals => FacilityEntries
           .GroupBy(oc => oc.Facility.OperatingCenter.Description).ToDictionary(g1 => g1.Key,
                g1 => (g1.GroupBy(g2 => g2.SystemDeliveryEntryType.Description)
                         .ToDictionary(g3 => g3.Key, g3 => g3.Sum(x => x.EntryValue))));

        /// <summary>
        ///     This property indicates whether an Entry has been sent to Hyperion. If it has,
        ///     the Entry can only be adjusted by a user with the System Delivery Admin role
        /// </summary>
        public virtual bool IsHyperionFileCreated { get; set; }

        #endregion

        #region Constructor

        public SystemDeliveryEntry()
        {
            FacilityEntries = new HashSet<SystemDeliveryFacilityEntry>();
            Adjustments = new List<SystemDeliveryFacilityEntryAdjustment>();
            Facilities = new List<Facility>();
            PublicWaterSupplies = new HashSet<PublicWaterSupply>();
            WasteWaterSystems = new HashSet<WasteWaterSystem>();
            OperatingCenters = new List<OperatingCenter>();
        }

        #endregion
    }
}
