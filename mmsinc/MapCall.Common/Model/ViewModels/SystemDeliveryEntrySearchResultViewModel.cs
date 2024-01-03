using MapCall.Common.Model.Entities;
using MMSINC.Metadata;
using MMSINC.Utilities;
using System;
using System.Collections.Generic;

namespace MapCall.Common.Model.ViewModels
{
    public class SystemDeliveryEntrySearchResultViewModel
    {
        #region Properties

        public int Id { get; set; }
        [View("Date", DisplayFormat = CommonStringFormats.DATE)]
        public DateTime EntryDate { get; set; }
        public string OperatingCenter { get; set; }
        public string SystemDeliveryType { get; set; }
        public string Facility { get; set; }
        public string LegacyIdSd { get; set; }
        public string PublicWaterSupply { get; set; }
        public int? BusinessUnit { get; set; }
        public string PurchaseSupplier { get; set; }
        public string SystemDeliveryEntryType { get; set; }
        public string Adjustment { get; set; }
        public decimal OriginalEntry { get; set; }
        public decimal Value { get; set; }
        public string IsValidated { get; set; }
        public string IsInjection { get; set; }
        [View("Employee")]
        public string EnteredBy { get; set; }
        public string Comment { get; set; }
        
        public string IsHyperionFileCreated { get; set; }

        #endregion
    }

    public class SystemDeliveryEntryFileDumpViewModel
    {
        #region Properties

        public int SystemDeliveryEntryId { get; set; }

        public string Year { get; set; }
        public string Month { get; set; }
        public string BusinessUnit { get; set; }
        public string FacilityName { get; set; }
        public string TotalValue { get; set; }
        public string EntryDescription { get; set; }
        public string SystemDeliveryDescription { get; set; }
        public string AsPostedDescription { get; set; }

        #endregion
    }

    public class SystemDeliveryEntryNotification
    {
        #region Properties

        public string RecordUrl { get; set; }
        public SystemDeliveryEntry Entity { get; set; }
        public OperatingCenter OperatingCenter { get; set; }

        #endregion
    }

    public class SystemDeliveryEntryDueNotification
    {
        public OperatingCenter OperatingCenter { get; set; }
        public Facility Facility { get; set; }
    }

    public class FacilitySystemDeliveryHistoryViewModel
    {
        public struct Display
        {
            public const string WEEK_OF = "Week Of",
                                VALUE = "Value (Thousand of Gallons)";
        }
        public string EntryType { get; set; }
        
        [View(DisplayFormat = CommonStringFormats.DATE, DisplayName = Display.WEEK_OF)]
        public DateTime Date { get; set; }
        
        [View(DisplayName = Display.VALUE)]
        public decimal Value { get; set; }
    }
}
