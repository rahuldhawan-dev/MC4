using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    /// <summary>
    /// Any time an Edit needs to be made to a SystemDeliveryFacilityEntry after it's SystemDeliveryEntry
    /// IsValidated, we need to create a SystemDeliveryFacilityEntryAdjustment. While creating the Adjustment, we
    /// need to capture some other useful information, like: What Facility is this for, which Employee was
    /// it EnteredBy, what was the AdjustedDate and AdjustedEntryValue, what was the OriginalEntryValue that we're
    /// now adjusting, what's the current Date and Time I'm entering this adjustment, and do I feel like leaving
    /// any Comments that would be helpful in explaining why I'm making an adjustment.
    /// </summary>
    [Serializable]
    public class SystemDeliveryFacilityEntryAdjustment : IEntity
    {
        #region Constants

        public struct StringLengths
        {
            public const int MAX_COMMENT = 100;
        }

        #endregion

        #region Properties
        
        public virtual int Id { get; set; }
        
        public virtual SystemDeliveryFacilityEntry SystemDeliveryFacilityEntry { get; set; }
        
        public virtual SystemDeliveryEntry SystemDeliveryEntry { get; set; }
        
        public virtual Facility Facility { get; set; }

        public virtual Employee EnteredBy { get; set; }
        
        [View(FormatStyle.Date)]
        public virtual DateTime AdjustedDate { get; set; }
        
        [DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_MAX_THREE_DECIMAL_PLACES)]
        public virtual decimal AdjustedEntryValue { get; set; }
        
        [DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_MAX_THREE_DECIMAL_PLACES)]
        public virtual decimal OriginalEntryValue { get; set; }
        
        public virtual DateTime DateTimeEntered { get; set; }
        
        public virtual string Comment { get; set; }

        #endregion
    }
}

