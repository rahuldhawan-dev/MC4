using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Migrations;
using MapCall.Common.Model.Migrations._2017;
using MapCall.Common.Model.Migrations._2018;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.ClassExtensions.Int32Extensions;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;
using NetTopologySuite.Geometries;
using NHibernate.Spatial.Type;
using StructureMap.Attributes;

namespace MapCall.Common.Model.Entities
{
    // NOTE: While this theoretically is an IThingWithCoordinate, you need to call ToAssetCoordinate
    //       to get the proper version of it.
    [Serializable]
    public class Hydrant
        : IEntityWithCreationTimeTracking,
            IEntityWithUpdateTracking<User>,
            IThingWithNotes,
            IThingWithDocuments,
            IAsset,
            ISAPEquipment,
            IRetirableWorkOrderAsset,
            IThingWithState,
            IThingWithAssetStatus,
            IThingWithShadow,
            IThingWithTownAndOperatingCenter
    {
        #region Consts

        public struct StringLengths
        {
            public const int HYDRANT_NUMBER = UpdateHydrantsForBug2223.StringLengths.HYD_HYD_NUMBER,
                             CRITICAL_NOTES = UpdateHydrantsForBug2223.StringLengths.HYD_CRITICAL_NOTES,
                             ELEVATION = UpdateHydrantsForBug2223.StringLengths.HYD_ELEVATION,
                             LEGACY_ID = WO200633LegacyIdForHydrantsAndValves.StringLengths.LEGACY_ID,
                             LOCATION = UpdateHydrantsForBug2223.StringLengths.HYD_LOCATION,
                             MAP_PAGE = UpdateHydrantsForBug2223.StringLengths.HYD_MAP_PAGE,
                             STREET_NUMBER = UpdateHydrantsForBug2223.StringLengths.HYD_ST_NUM,
                             VALVE_LOCATION = UpdateHydrantsForBug2223.StringLengths.HYD_VALVE_LOCATION,
                             WORKORDER = ExtendSomeAssetStringFieldLengthsForWO208357.WORK_ORDER_NUMBER_LENGTH,
                             PREMISE_NUMBER = UpdateHydrantsForBug2223.StringLengths.HYD_PREMISE_NUMBER,
                             GISUID = 40;
        }

        public struct Display
        {
            public const string BILLING_IN_SERVICE = "Billing / In Service Date",
                                CRITICAL = "Has Critical Notes",
                                OBJECT_ID = "Object ID",
                                HYDRANT_MANUFACTURER = "Manufacturer",
                                HYDRANT_SIZE = "Hydrant Barrel Diameter",
                                HYDRANT_STATUS = "Status",
                                HYDRANT_BILLING = "Billing",
                                HYDRANT_MAIN_SIZE = "Main Size(inches)",
                                IS_NON_BPUKPI = "Is non BPU/KPI",
                                LATERAL_SIZE = "Lateral Size(inches)",
                                THREAD = "Steamer Thread Type",
                                WORK_ORDER_NUMBER = "WBS #";
        }

        public const string HYDRANT_NUMBER_PATTERN = @"(\w+)(-)(\d+)";
        public static string HYDRANT_NUMBER_PATTERN_ERROR = "Hydrant Number must contain the Hydrant Suffix.";

        private static readonly int[] UNINSPECTABLE_STATUSES = {
            AssetStatus.Indices.CANCELLED, AssetStatus.Indices.REMOVED, AssetStatus.Indices.RETIRED,
            AssetStatus.Indices.INACTIVE
        };

        #endregion

        #region Fields

        [NonSerialized] private IIconSetRepository _iconSetRepository;
        [NonSerialized] private IDateTimeProvider _dateTimeProvider;
        private HydrantDisplayItem _display;

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }

        public virtual Town Town { get; set; }
        public virtual Valve LateralValve { get; set; }
        public virtual Street Street { get; set; }
        public virtual FireDistrict FireDistrict { get; set; }
        public virtual HydrantTagStatus HydrantTagStatus { get; set; }

        [DisplayName(Display.HYDRANT_MANUFACTURER)]
        public virtual HydrantManufacturer HydrantManufacturer { get; set; }

        public virtual HydrantModel HydrantModel { get; set; }
        public virtual User Initiator { get; set; }

        [DisplayName(Display.HYDRANT_STATUS)]
        public virtual AssetStatus Status { get; set; }

        [DisplayName(Display.LATERAL_SIZE)]
        public virtual LateralSize LateralSize { get; set; }

        public virtual Street CrossStreet { get; set; }
        public virtual HydrantDirection OpenDirection { get; set; }
        public virtual Gradient Gradient { get; set; }

        [DisplayName(Display.HYDRANT_SIZE)]
        public virtual HydrantSize HydrantSize { get; set; }

        [View(Description = "This will override Operating Center inspection frequency and/or zone")]
        public virtual int? InspectionFrequency { get; set; }

        public virtual RecurringFrequencyUnit InspectionFrequencyUnit { get; set; }
        
        [View(Description = "This will override Operating Center Painting frequency and/or zone")]
        public virtual int? PaintingFrequency { get; set; }

        public virtual RecurringFrequencyUnit PaintingFrequencyUnit { get; set; }

        public virtual OperatingCenter OperatingCenter { get; set; }

        [DisplayName(Display.HYDRANT_MAIN_SIZE)]
        public virtual HydrantMainSize HydrantMainSize { get; set; }

        [DisplayName(Display.THREAD)]
        public virtual HydrantThreadType HydrantThreadType { get; set; }

        public virtual TownSection TownSection { get; set; }

        [DisplayName(Display.HYDRANT_BILLING)]
        public virtual HydrantBilling HydrantBilling { get; set; }

        public virtual Coordinate Coordinate { get; set; }

        public virtual MainType MainType { get; set; }

        public virtual User UpdatedBy { get; set; }

        [DisplayName(Display.IS_NON_BPUKPI)]
        public virtual bool IsNonBPUKPI { get; set; }

        [View(Display.CRITICAL)]
        public virtual bool Critical { get; set; }

        [Multiline]
        public virtual string CriticalNotes { get; set; }

        public virtual DateTime? DateInstalled { get; set; }

        [View("Date Retired / Removed")]
        public virtual DateTime? DateRetired { get; set; }

        // The DateTested column isn't used anywhere. TODO: Kill it
        public virtual DateTime? DateTested { get; set; }
        public virtual bool IsDeadEndMain { get; set; }
        public virtual decimal? Elevation { get; set; }

        [Required]
        public virtual string HydrantNumber { get; set; }

        public virtual int HydrantSuffix { get; set; }

        [View("Legacy ID")]
        public virtual string LegacyId { get; set; }

        public virtual string Location { get; set; }
        public virtual string MapPage { get; set; }

        public virtual int? Route { get; set; }
        public virtual decimal? Stop { get; set; }
        public virtual string StreetNumber { get; set; }
        public virtual string ValveLocation { get; set; }

        [View(Display.WORK_ORDER_NUMBER)]
        public virtual string WorkOrderNumber { get; set; }
        public virtual int? YearManufactured { get; set; }
        public virtual bool ClowTagged { get; set; }

        [DisplayName(Display.OBJECT_ID)]
        public virtual int? ObjectID { get; set; }

        [DisplayName(Display.BILLING_IN_SERVICE)]
        public virtual DateTime? BillingDate { get; set; }

        public virtual int? BranchLengthFeet { get; set; }
        public virtual int? BranchLengthInches { get; set; }
        public virtual int? DepthBuryFeet { get; set; }
        public virtual int? DepthBuryInches { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime UpdatedAt { get; set; }

        [DisplayName("SAP Equipment ID")]
        public virtual int? SAPEquipmentId { get; set; }

        public virtual string SAPEquipmentNumber => SAPEquipmentId.ToString().PadLeft(18, '0');

        public virtual string GISUID { get; set; }

        [DisplayName("SAP Functional Location")]
        public virtual FunctionalLocation FunctionalLocation { get; set; }

        public virtual WaterSystem WaterSystem { get; set; }
        public virtual Facility Facility { get; set; }
        public virtual string SAPErrorCode { get; set; }

        // Hanging out with us for a while, not editable
        public virtual int? FLRouteNumber { get; set; }
        public virtual int? FLRouteSequence { get; set; }
        public virtual int? Zone { get; set; }
        public virtual int? PaintingZone { get; set; }
        public virtual HydrantType HydrantType { get; set; }
        public virtual HydrantOutletConfiguration HydrantOutletConfiguration { get; set; }
        public virtual HydrantDueInspection HydrantDueInspection { get; set; }
        public virtual HydrantDuePainting HydrantDuePainting { get; set; }
        public virtual IList<WorkOrder> WorkOrders { get; set; }
        public virtual IList<HydrantInspection> HydrantInspections { get; set; }
        public virtual IList<HydrantPainting> Paintings { get; set; }
        public virtual IList<HydrantOutOfService> OutOfServiceRecords { get; set; }
        public virtual IList<HydrantDocument> HydrantDocuments { get; set; }
        public virtual IList<HydrantNote> HydrantNotes { get; set; }

        #endregion

        #region Logical Properties

        //If you want to search by this, move it to a Formula Field instead
        public virtual string PremiseNumber => FireDistrict != null ? FireDistrict.PremiseNumber : string.Empty;

        /// <summary>
        /// NOTE: This is a formula field!
        /// </summary>
        public virtual bool OutOfService { get; protected set; }

        public virtual int InspectionsPerYear { get; protected set; }
        public virtual bool HasWorkOrder { get; set; }
        public virtual bool HasOpenWorkOrder { get; set; }
        public virtual WorkDescription MostRecentOpenWorkOrderWorkDescription { get; set; }
        public virtual WorkDescription[] WorkDescriptions { get; set; }

        /// <summary>
        /// Returns the current HydrantOutOfService record that is still open. If the
        /// hydrant is out of service then it will return null.
        /// </summary>
        public virtual HydrantOutOfService CurrentOpenOutOfServiceRecord
        {
            get
            {
                // Do not order by DateCreated as records can potentially be created out of order!
                // Also be wary that two records could have the same OutOfServiceDate where one
                // record has a BackInServiceDate but the other does not. 
                return OutOfServiceRecords.OrderByDescending(x => x.OutOfServiceDate)
                                          .FirstOrDefault(x => x.BackInServiceDate == null);
            }
        }

        public virtual HydrantInspection LastInspection
        {
            get { return HydrantInspections.OrderByDescending(x => x.DateInspected).FirstOrDefault(); }
        }

        /// <summary>
        /// This is a formula property.
        /// </summary>
        [DisplayName("Last Non-Inspection Date")]
        public virtual DateTime? LastNonInspectionDate { get; set; }

        // TODO: See if this property is still neededd since LastNontINspectionDate doesn't really mean anything anymore.
        public virtual DateTime? ActualLastInspection
        {
            get
            {
                if (LastNonInspectionDate != null && HydrantDueInspection?.LastInspectionDate != null &&
                    LastNonInspectionDate > HydrantDueInspection.LastInspectionDate)
                {
                    return LastNonInspectionDate;
                }

                return HydrantDueInspection?.LastInspectionDate;
            }
        }

        public virtual IList<INoteLink> LinkedNotes => HydrantNotes.Cast<INoteLink>().ToList();

        public virtual IList<IDocumentLink> LinkedDocuments => HydrantDocuments.Cast<IDocumentLink>().ToList();

        [DoesNotExport]
        public virtual string TableName => nameof(Hydrant) + "s";

        [DisplayName("Inspection Frequency")]
        public virtual string InspectionFrequencyDisplay
        {
            get
            {
                if (InspectionFrequencyUnit == null)
                {
                    return null;
                }

                return InspectionFrequency + " " + InspectionFrequencyUnit;
            }
        }

        [DisplayName("Painting Frequency")]
        public virtual string PaintingFrequencyDisplay
        {
            get
            {
                if (PaintingFrequencyUnit == null)
                {
                    return null;
                }

                return PaintingFrequency + " " + PaintingFrequencyUnit;
            }
        }

        [DoesNotExport]
        public virtual MapIcon Icon => Coordinate.Icon;

        [DoesNotExport]
        public virtual bool IsActive => AssetStatus.ACTIVE_STATUSES.Contains(Status.Id);

        [DoesNotExport]
        public virtual bool WorkOrdersEnabled => !AssetStatus.WORK_ORDER_DISABLED_STATUSES.Contains(Status.Id);

        [DoesNotExport]
        public virtual bool CanBeCopied => AssetStatus.CanBeCopiedStatuses.Contains(Status.Id);

        /// <summary>
        /// Returns true if the asset can have inspections added.
        /// </summary>
        [DoesNotExport]
        public virtual bool IsInspectable => !UNINSPECTABLE_STATUSES.Contains(Status.Id);

        /// <summary>
        /// Returns true if the asset can have paintings added.
        /// </summary>
        [DoesNotExport]
        public virtual bool IsPaintable => !UNINSPECTABLE_STATUSES.Contains(Status.Id);

        public virtual bool PaintedToday =>
            _dateTimeProvider.GetCurrentDate().Date == HydrantDuePainting?.LastPaintedAt?.Date;

        [DoesNotExport]
        public virtual string Identifier => HydrantNumber;

        public virtual State State => Town?.State;

        public virtual string DescriptionWithStatus => (_display ?? (_display = new HydrantDisplayItem {
            HydrantNumber = HydrantNumber,
            Status = Status?.Description
        })).Display;

        public virtual string RecordUrl { get; set; }

        #endregion

        #region Injected Properties

        [SetterProperty]
        public virtual IIconSetRepository IconSetRepository
        {
            set => _iconSetRepository = value;
        }

        [SetterProperty]
        public virtual IDateTimeProvider DateTimeProvider
        {
            set => _dateTimeProvider = value;
        }

        #endregion

        #endregion

        #region Constructor

        public Hydrant()
        {
            HydrantNotes = new List<HydrantNote>();
            HydrantDocuments = new List<HydrantDocument>();
            HydrantInspections = new List<HydrantInspection>();
            OutOfServiceRecords = new List<HydrantOutOfService>();
        }

        #endregion

        #region Exposed Methods

        public virtual HydrantAssetCoordinate ToAssetCoordinate()
        {
            // This smells and is a hack just to get icon logic in one spot. 
            var hac = new HydrantAssetCoordinate(_iconSetRepository) {
                IsActive = IsActive,
                IsPublic = HydrantBilling.Id == HydrantBilling.Indices.PUBLIC,
                LastInspection = HydrantDueInspection?.LastInspectionDate,
                Id = Id,
                RequiresInspection = HydrantDueInspection?.RequiresInspection ?? false,
                RequiresPainting = HydrantDuePainting?.RequiresPainting ?? false,
                HasOpenWorkOrder = HasOpenWorkOrder,
                OutOfService = OutOfService
            };

            if (Coordinate != null)
            {
                hac.Latitude = Coordinate.Latitude;
                hac.Longitude = Coordinate.Longitude;
            }

            return hac;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return HydrantNumber;
        }

        #endregion
    }

    [Serializable]
    public class HydrantBilling : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int MUNICIPAL = 1, PUBLIC = 2, COMPANY = 3, PRIVATE = 4;
        }
    }

    [Serializable]
    public class MainType : EntityLookup
    {
        public virtual string SAPCode { get; set; }
    }

    [Serializable]
    public class HydrantThreadType : EntityLookup { }

    [Serializable]
    public class HydrantDirection : ReadOnlyEntityLookup { }

    [Serializable]
    public class HydrantTagStatus : EntityLookup { }

    [Serializable]
    public class HydrantManufacturer : EntityLookup { }

    [Serializable]
    public class HydrantModel : EntityLookup
    {
        public virtual HydrantManufacturer HydrantManufacturer { get; set; }
    }

    /// <summary>
    /// This is a dumb data class needed in the HydrantRepository when it generates 
    /// a hydrant number. This is not an entity.
    /// </summary>
    public class HydrantNumber
    {
        public string FormattedNumber => string.Format("{0}-{1}", Prefix, Suffix);
        public int Suffix { get; set; }
        public string Prefix { get; set; }
    }

    public class HydrantInspectionRequirementHelper
    {
        #region Private Members

        private readonly DateTime _now;

        #endregion

        #region Constructors

        public HydrantInspectionRequirementHelper(IDateTimeProvider dateTimeProvider)
        {
            _now = dateTimeProvider.GetCurrentDate();
        }

        #endregion

        #region Private Methods

        private IEnumerable<(bool IsRequired, string Reason)> VerifyStatus(Hydrant hydrant)
        {
            if (!hydrant.IsActive)
            {
                yield return (false, "Not active");
            }
            else
            {
                yield return (true, "Is Active");
            }
        }

        private bool HydrantHasFrequency(Hydrant hydrant)
        {
            return hydrant.InspectionFrequencyUnit != null && hydrant.InspectionFrequency.HasValue;
        }

        private IEnumerable<(bool IsRequired, string Reason)> VerifyBilling(Hydrant hydrant)
        {
            if (!new int?[] {null, HydrantBilling.Indices.PUBLIC, HydrantBilling.Indices.PRIVATE}.Contains(
                    hydrant.HydrantBilling?.Id))
            {
                var hasFrequency = HydrantHasFrequency(hydrant);
                yield return
                    (hasFrequency,
                        $"Billing is not Public or Private ({hydrant.HydrantBilling}){(hasFrequency ? " but frequency is set" : "")}");
            }
            else
            {
                yield return (true, $"Billing is {hydrant.HydrantBilling.Description}");
            }
        }

        private IEnumerable<(bool IsRequired, string Reason)> VerifyBPUKPI(Hydrant hydrant)
        {
            if (hydrant.IsNonBPUKPI)
            {
                yield return (false, "Is non-BPUKPI");
            }
            else
            {
                yield return (true, "Is not non-BPUKPI");
            }
        }

        private string GetInspectionFrequencyUnitName(int inspectionFrequencyUnitId)
        {
            switch (inspectionFrequencyUnitId)
            {
                case RecurringFrequencyUnit.Indices.YEAR:
                    return "year";
                case RecurringFrequencyUnit.Indices.MONTH:
                    return "month";
                case RecurringFrequencyUnit.Indices.WEEK:
                    return "week";
                case RecurringFrequencyUnit.Indices.DAY:
                    return "day";
                default:
                    throw new ArgumentException(
                        $"Cannot map inspection frequency unit value '{inspectionFrequencyUnitId}'",
                        nameof(inspectionFrequencyUnitId));
            }
        }

        private (bool IsRequired, string Reason) DoVerifyInspectionFrequency(Hydrant hydrant,
            int inspectionFrequencyUnitId, int inspectionFrequency, string type)
        {
            var datePart = GetInspectionFrequencyUnitName(inspectionFrequencyUnitId);

            if (!hydrant.HydrantInspections.Any())
            {
                return (true,
                    $"Has {type} inspection frequency every {inspectionFrequency} {datePart}{(inspectionFrequency.IsOdd() ? "" : "s")}, but has no past inspections");
            }

            int deficit = -1;

            switch (inspectionFrequencyUnitId)
            {
                case RecurringFrequencyUnit.Indices.YEAR:
                    deficit = _now.Year - hydrant.HydrantInspections.Max(i => i.DateInspected).Year;
                    break;
                case RecurringFrequencyUnit.Indices.MONTH:
                    deficit = DateTimeExtensions.MonthsDifference(_now,
                        hydrant.HydrantInspections.Max(i => i.DateInspected));
                    break;
                case RecurringFrequencyUnit.Indices.WEEK:
                    deficit = DateTimeExtensions.WeeksDifference(_now,
                        hydrant.HydrantInspections.Max(i => i.DateInspected));
                    break;
                case RecurringFrequencyUnit.Indices.DAY:
                    deficit = (int)(_now - hydrant.HydrantInspections.Max(i => i.DateInspected)).TotalDays;
                    break;
            }

            return ((deficit >= inspectionFrequency),
                deficit == 0
                    ? $"Has been inspected in the past {inspectionFrequency} {datePart}{(inspectionFrequency.IsOdd() ? "" : "s")} with {type} inspection frequency of {inspectionFrequency} {datePart}{(inspectionFrequency.IsOdd() ? "" : "s")}"
                    : $"Last inspection was {deficit} {datePart}{(deficit.IsOdd() ? "" : "s")} ago with {type} inspection frequency of {inspectionFrequency} {datePart}{(inspectionFrequency.IsOdd() ? "" : "s")}");
        }

        private IEnumerable<(bool IsRequired, string Reason)> VerifyInspectionFrequency(Hydrant hydrant)
        {
            if (!HydrantHasFrequency(hydrant))
            {
                yield break;
            }

            yield return DoVerifyInspectionFrequency(hydrant,
                hydrant.InspectionFrequencyUnit.Id,
                hydrant.InspectionFrequency.Value, "hydrant");
        }

        private IEnumerable<(bool IsRequired, string Reason)> VerifyOperatingCenterFrequencyAndZone(Hydrant hydrant)
        {
            if (HydrantHasFrequency(hydrant))
            {
                // hydrant's local inspectiom frequency has overridden
                yield break;
            }

            if (hydrant.OperatingCenter.ZoneStartYear.HasValue && hydrant.Zone.HasValue)
            {
                var currentZone = (Math.Abs(hydrant.OperatingCenter.ZoneStartYear.Value - _now.Year) %
                                   hydrant.OperatingCenter.HydrantInspectionFrequency) + 1;
                var lastInspectionYear = !hydrant.HydrantInspections.Any()
                    ? 0
                    : hydrant.HydrantInspections.Max(i => i.DateInspected.Year);
                var required = lastInspectionYear < _now.Year && hydrant.Zone == currentZone;
                yield return (required,
                    $"Current Operating Center zone is {currentZone}, hydrant zone is {hydrant.Zone}, last inspection was {lastInspectionYear}");
            }
            else
            {
                yield return DoVerifyInspectionFrequency(hydrant,
                    hydrant.OperatingCenter.HydrantInspectionFrequencyUnit.Id,
                    hydrant.OperatingCenter.HydrantInspectionFrequency, "Operating Center");
            }
        }

        #endregion

        #region Exposed Methods

        public (bool IsRequired, IEnumerable<(bool IsRequired, string Reason)> Reasons) GetStatus(Hydrant hydrant)
        {
            var results = new List<(bool IsRequired, string Reason)>();

            results.AddRange(VerifyStatus(hydrant));
            results.AddRange(VerifyBilling(hydrant));
            results.AddRange(VerifyBPUKPI(hydrant));
            results.AddRange(VerifyInspectionFrequency(hydrant));
            results.AddRange(VerifyOperatingCenterFrequencyAndZone(hydrant));

            return (results.All(r => r.IsRequired), results);
        }

        #endregion
    }
}
