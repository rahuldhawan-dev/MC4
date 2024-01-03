using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Contractor
        : IEntityLookup, IThingWithNotes, IThingWithDocuments, IEntityWithCreationTimeTracking
    {
        #region Constants

        public struct FormatStrings
        {
            public const string ADDRESS_LINE_ONE = "{0}{1} {2}",
                                CITY_STATE_ZIP = "{0}, {1} {2}";
        }

        public struct Display
        {
            public const string AWR = "Is American Water Resources contractor?",
                                FRAMEWORK_OPERATING_CENTERS = "Framework Operating Centers",
                                FUNCTIONAL_AREAS = "Contractor Functional Areas",
                                IS_BCP_PARTNER = "Is BCP Partner",
                                OPERATING_CENTERS = "Operating Centers",
                                VENDOR = "Vendor Id",
                                WORK_CATEGORIES = "Contractor Work Categories";
        }

        public struct StringLengths
        {
            public const int APARTMENT_NUMBER = 12,
                             HOUSE_NUMBER = 12,
                             NAME = 255,
                             PHONE = 15,
                             ZIP = 12,
                             VENDOR_ID = 50;
        }

        #endregion

        #region Private Members

        private ContractorDisplayItem _display;

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }

        [StringLength(StringLengths.NAME)]
        public virtual string Name { get; set; }

        [StringLength(StringLengths.HOUSE_NUMBER)]
        public virtual string HouseNumber { get; set; }

        [StringLength(StringLengths.APARTMENT_NUMBER)]
        public virtual string ApartmentNumber { get; set; }

        [StringLength(StringLengths.ZIP)]
        public virtual string Zip { get; set; }

        [StringLength(StringLengths.PHONE)]
        public virtual string Phone { get; set; }

        public virtual bool IsUnionShop { get; set; }

        [View(Display.IS_BCP_PARTNER)]
        public virtual bool IsBcpPartner { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual int? QualityControlContactId { get; set; }

        public virtual int? SafetyContactId { get; set; }

        [View(Display.VENDOR), StringLength(StringLengths.VENDOR_ID)]
        public virtual string VendorId { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual bool ContractorsAccess { get; set; }

        [View(Display.AWR)]
        public virtual bool? AWR { get; set; }

        #endregion

        #region References

        public virtual Street Street { get; set; }
        public virtual Town Town { get; set; }
        public virtual State State { get; set; }
        [View(Display.OPERATING_CENTERS)]
        public virtual IList<OperatingCenter> OperatingCenters { get; set; } = new List<OperatingCenter>();

        [View(Display.FRAMEWORK_OPERATING_CENTERS)]
        public virtual IList<OperatingCenter> FrameworkOperatingCenters { get; set; } = new List<OperatingCenter>();
        public virtual IList<Crew> Crews { get; set; } = new List<Crew>();
        public virtual IList<ContractorUser> Users { get; set; } = new List<ContractorUser>();
        public virtual IList<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();
        public virtual IList<ContractorInsurance> Insurances { get; set; } = new List<ContractorInsurance>();
        public virtual IList<ContractorContact> Contacts { get; set; } = new List<ContractorContact>();
        [View(Display.FUNCTIONAL_AREAS)]
        public virtual IList<ContractorFunctionalAreaType> FunctionalAreas { get; set; } = new List<ContractorFunctionalAreaType>();
        [View(Display.WORK_CATEGORIES)]
        public virtual IList<ContractorWorkCategoryType> WorkCategories { get; set; } = new List<ContractorWorkCategoryType>();

        #region Notes

        public virtual IList<Note<Contractor>> Notes { get; set; } = new List<Note<Contractor>>();

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        [DoesNotExport]
        public virtual string TableName => nameof(Contractor) + "s";

        #endregion

        #region Documents

        public virtual IList<Document<Contractor>> Documents { get; set; } = new List<Document<Contractor>>();

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        #endregion

        #endregion

        #region Logical Properties

        public virtual string Description => (_display ?? (_display = new ContractorDisplayItem {
            Id = Id,
            Name = Name
        })).Display;

        public virtual string AddressLineOne =>
            String.Format(FormatStrings.ADDRESS_LINE_ONE, HouseNumber, ApartmentNumber, Street);

        public virtual string CityStateZip => String.Format(FormatStrings.CITY_STATE_ZIP, Town, State, Zip);

        #endregion

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }

    [Serializable]
    public class ContractorDisplayItem : DisplayItem<Contractor>
    {
        public string Name { get; set; }
        public override string Display => Name;
    }
}
