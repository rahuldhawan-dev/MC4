using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Migrations;
using MapCall.Common.Model.Migrations._2017;
using MapCall.Common.Model.Migrations._2018;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;
using StructureMap.Attributes;

namespace MapCall.Common.Model.Entities
{
    // NOTE: While this theoretically is an IThingWithCoordinate, you need to call ToAssetCoordinate
    //       to get the proper version of it.
    [Serializable]
    public class Valve
        : IThingWithNotes,
            IThingWithDocuments,
            IEntityLookup,
            IAsset,
            IThingWithAssetStatus,
            ISAPEquipment,
            IRetirableWorkOrderAsset,
            IThingWithState,
            IThingWithShadow,
            IEntityWithUpdateTracking<User>,
            IThingWithTownAndOperatingCenter,
            IEntityWithCreationTimeTracking
    {
        #region Consts

        public struct StringLengths
        {
            public const int VALVE_NUMBER = UpdateValvesForBug2224.StringLengths.VAL_VAL_NUM,
                             VALVE_LOCATION = UpdateValvesForBug2224.StringLengths.VAL_VAL_LOC,
                             CRITICAL_NOTES = UpdateValvesForBug2224.StringLengths.VAL_CRITICAL_NOTES,
                             INSPECTION_FREQUENCY = UpdateValvesForBug2224.StringLengths.VAL_INSP_FREQ,
                             LEGACY_ID = WO200633LegacyIdForHydrantsAndValves.StringLengths.LEGACY_ID,
                             MAP_PAGE = UpdateValvesForBug2224.StringLengths.VAL_MAP_PAGE,
                             SKETCH_NUMBER = UpdateValvesForBug2224.StringLengths.VAL_SKETCH_NUM,
                             STREET_NUMBER = UpdateValvesForBug2224.StringLengths.VAL_ST_NUM,
                             WORK_ORDER_NUMBER = ExtendSomeAssetStringFieldLengthsForWO208357.WORK_ORDER_NUMBER_LENGTH,
                             GISUID = 40;
        }

        public struct Display
        {
            public const string CRITICAL = "Has Critical Notes", 
                                OBJECT_ID = "Object ID",
                                TRAFFIC = "Traffic Control Required",
                                VALVE_BILLING = "Billing Information",
                                STREET = "Street Name",
                                ROUTE = "Route #",
                                SKETCH_NUMBER = "Sketch #",
                                BPUKPI = "Non BPU/KPI",
                                VALVE_SIZE = "Valve Size (in)",
                                TURNS = "Number of Turns",
                                WORK_ORDER_NUMBER = "WBS #",
                                IS_LARGE_VALVE = "Size Range",
                                SIZE_RANGE_LARGE_VALVE = ">= 12",
                                SIZE_RANGE_SMALL_VALVE = "< 12",
                                SIZE_RANGE_NULL = "N/A";
        }

        public const string VALVE_NUMBER_PATTERN = @"(\w+)(-)(\d+)";
        public static string VALVE_NUMBER_PATTERN_ERROR = "Valve Number must contain the Valve Suffix.";

        private static readonly int[] UNINSPECTABLE_STATUSES = {
            AssetStatus.Indices.CANCELLED, AssetStatus.Indices.REMOVED, AssetStatus.Indices.RETIRED,
            AssetStatus.Indices.INACTIVE
        };

        #endregion

        #region Fields

        [NonSerialized] private IIconSetRepository _iconSetRepository;

        private ValveDisplayItem _display;

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Town Town { get; set; }

        [DisplayName(Display.STREET)]
        public virtual Street Street { get; set; }

        [DisplayName(Display.VALVE_BILLING)]
        public virtual ValveBilling ValveBilling { get; set; }

        public virtual Street CrossStreet { get; set; }
        public virtual RecurringFrequencyUnit InspectionFrequencyUnit { get; set; }
        public virtual ValveNormalPosition NormalPosition { get; set; }
        public virtual ValveOpenDirection OpenDirection { get; set; }
        public virtual TownSection TownSection { get; set; }
        public virtual MainType MainType { get; set; }
        public virtual ValveControl ValveControls { get; set; }
        public virtual ValveManufacturer ValveMake { get; set; }
        public virtual ValveType ValveType { get; set; }

        [DisplayName(Display.VALVE_SIZE)]
        public virtual ValveSize ValveSize { get; set; }

        public virtual AssetStatus Status { get; set; }
        public virtual User Initiator { get; set; }
        public virtual ValveZone ValveZone { get; set; }
        public virtual Coordinate Coordinate { get; set; }

        [DisplayName("SAP Functional Location")]
        public virtual FunctionalLocation FunctionalLocation { get; set; }

        public virtual WaterSystem WaterSystem { get; set; }
        public virtual Facility Facility { get; set; }
        public virtual User UpdatedBy { get; set; }

        // TODO: This property should be renamed to IsNonBPUKPI to match Hydrants.
        //       Also because the property name doesn't agree with the display name.
        [DisplayName(Display.BPUKPI)]
        public virtual bool BPUKPI { get; set; }

        [View(Display.CRITICAL)]
        public virtual bool Critical { get; set; }

        [Multiline]
        public virtual string CriticalNotes { get; set; }

        [View("Date Retired / Removed")]
        public virtual DateTime? DateRetired { get; set; }

        public virtual DateTime? DateTested { get; set; }
        public virtual decimal? Elevation { get; set; }

        public virtual string GISUID { get; set; }
        public virtual int? InspectionFrequency { get; set; }

        [View("Legacy ID")]
        public virtual string LegacyId { get; set; }

        public virtual string MapPage { get; set; }

        [DisplayName(Display.OBJECT_ID)]
        public virtual int? ObjectID { get; set; }

        [DisplayName(Display.ROUTE)]
        public virtual int? Route { get; set; }

        public virtual decimal? Stop { get; set; }

        [DisplayName(Display.SKETCH_NUMBER)]
        public virtual string SketchNumber { get; set; }

        public virtual string StreetNumber { get; set; }

        [DisplayName(Display.TRAFFIC)]
        public virtual bool Traffic { get; set; }

        [DisplayName(Display.TURNS)]
        [View(Description = "Click the Help Icon on the top right of this page for more information.")]
        public virtual decimal? Turns { get; set; }

        public virtual string ValveLocation { get; set; }
        public virtual Gradient Gradient { get; set; }
        public virtual string ValveNumber { get; set; }
        public virtual int ValveSuffix { get; set; }

        [DisplayName(Display.WORK_ORDER_NUMBER)]
        public virtual string WorkOrderNumber { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        //public virtual int? ImageActionID { get; set; }
        public virtual DateTime? DateInstalled { get; set; }
        public virtual DateTime UpdatedAt { get; set; }
        public virtual int? SAPEquipmentId { get; set; }
        public virtual string SAPEquipmentNumber => SAPEquipmentId.ToString().PadLeft(18, '0');
        public virtual string SAPErrorCode { get; set; }
        public virtual bool ControlsCrossing { get; set; }

        [View(DisplayName = "Depth to Nut Feet")]
        public virtual int? DepthFeet { get; set; }

        [View(DisplayName = "Depth to Nut Inches")]
        public virtual int? DepthInches { get; set; }

        public virtual bool HasImages { get; set; }

        /// <summary>
        /// This is a formula property.
        /// </summary>
        /// <remarks>
        /// it's dumb and confusing because valves that control "blow offs" 
        /// don't get blow off inspections, but valves that control "blow off with flushing" 
        /// are the ones that get blow off inspections - Alex 4/22/2015
        /// </remarks>
        public virtual bool CanHaveBlowOffInspections { get; protected set; }

        public virtual IList<Hydrant> LateralHydrants { get; set; }
        public virtual IList<ValveImage> ValveImages { get; set; }
        public virtual IList<ValveInspection> ValveInspections { get; set; }
        public virtual IList<BlowOffInspection> BlowOffInspections { get; set; }
        public virtual IList<WorkOrder> WorkOrders { get; set; }
        public virtual IList<MainCrossing> MainCrossings { get; set; }

        public virtual string Description => ToString();

        #endregion

        #region References

        public virtual IList<ValveDocument> ValveDocuments { get; set; }
        public virtual IList<ValveNote> ValveNotes { get; set; }

        #endregion

        #region Logical Properties

        public virtual IList<INoteLink> LinkedNotes => ValveNotes.Cast<INoteLink>().ToList();

        public virtual IList<IDocumentLink> LinkedDocuments => ValveDocuments.Cast<IDocumentLink>().ToList();

        [DoesNotExport]
        public virtual string TableName => nameof(Valve) + "s";

        [DoesNotExport]
        public virtual ValveInspection LastInspection
        {
            get { return ValveInspections.OrderByDescending(x => x.DateInspected).FirstOrDefault(); }
        }

        [DoesNotExport]
        public virtual BlowOffInspection LastBlowOffInspection
        {
            get { return BlowOffInspections.OrderByDescending(x => x.DateInspected).FirstOrDefault(); }
        }

        /// <summary>
        /// This is a formula property.
        /// </summary>
        public virtual DateTime? LastInspectionDate { get; set; }

        /// <summary>
        /// This is a formula property.
        /// </summary>
        public virtual DateTime? LastNonInspectionDate { get; set; }

        /// <summary>
        /// This is a formula property
        /// </summary>
        public virtual DateTime? LastBlowOffInspectionDate { get; set; }

        /// <summary>
        /// This is a formula property
        /// </summary>
        public virtual DateTime? LastBlowOffNonInspectionDate { get; set; }

        /// <summary>
        /// This is a formula property.
        /// </summary>
        [BoolFormat(Valve.Display.SIZE_RANGE_LARGE_VALVE, Valve.Display.SIZE_RANGE_SMALL_VALVE),
         DisplayName(Display.IS_LARGE_VALVE)]
        public virtual bool IsLargeValve { get; set; }

        /// <summary>
        /// Formula Property
        /// </summary>
        public virtual bool RequiresInspection { get; set; }

        /// <summary>
        /// Formula Property
        /// </summary>
        public virtual bool RequiresBlowOffInspection { get; set; }

        /// <summary>
        /// Formula Property
        /// </summary>
        public virtual bool? InNormalPosition { get; set; }

        public virtual ValveNormalPosition PositionLeft { get; set; }

        [DoesNotExport]
        public virtual ValveImage DefaultValveImage
        {
            get
            {
                if (ValveImages.Any(x => x.IsDefaultImageForValve))
                    return ValveImages.OrderByDescending(x => x.Id).First(x => x.IsDefaultImageForValve);
                if (ValveImages.Any())
                    return ValveImages.OrderByDescending(x => x.Id).First();
                return null;
            }
        }

        public virtual decimal MinimumRequiredTurns
        {
            get
            {
                if (Turns != null)
                {
                    return (Turns.Value >= 1)
                        ? Math.Ceiling(Turns.Value * .2m)
                        : Turns.Value;
                }

                return 0;
            }
        }

        /// <summary>
        /// This is a formula property.
        /// </summary>
        [DoesNotExport] // This is a lazy loaded property and slows down exporting significantly.
        public virtual bool HasOpenWorkOrder { get; set; }

        public virtual WorkDescription MostRecentOpenWorkOrderWorkDescription { get; set; }

        [DoesNotExport]
        public virtual MapIcon Icon => Coordinate?.Icon;

        [DoesNotExport]
        public virtual bool IsActive => AssetStatus.ACTIVE_STATUSES.Contains(Status.Id);

        [DoesNotExport]
        public virtual bool WorkOrdersEnabled => !AssetStatus.WORK_ORDER_DISABLED_STATUSES.Contains(Status.Id);

        /// <summary>
        /// Returns true if the asset can have valve/blowoff inspections added.
        /// </summary>
        [DoesNotExport]
        public virtual bool IsInspectable => !UNINSPECTABLE_STATUSES.Contains(Status.Id);

        [DoesNotExport]
        public virtual string Identifier => ValveNumber;

        public virtual decimal? Latitude => Coordinate?.Latitude;
        public virtual decimal? Longitude => Coordinate?.Longitude;

        [DoesNotExport]
        public virtual bool CanBeCopied =>
            AssetStatus.CanBeCopiedStatuses.Contains(Status.Id) && Turns.HasValue && ValveSize != null;

        public virtual State State => Town.State;

        public virtual string DescriptionWithStatus => (_display ?? (_display = new ValveDisplayItem {
            ValveNumber = ValveNumber,
            Status = Status.Description
        })).Display;

        public virtual string RecordUrl { get; set; }

        #endregion

        #region Injected Properties

        [SetterProperty]
        public virtual IIconSetRepository IconSetRepository
        {
            set => _iconSetRepository = value;
        }
        
        #endregion

        #endregion

        #region Constructor

        public Valve()
        {
            ValveImages = new List<ValveImage>();
            ValveInspections = new List<ValveInspection>();
            BlowOffInspections = new List<BlowOffInspection>();
            MainCrossings = new List<MainCrossing>();
            ValveNotes = new List<ValveNote>();
            ValveDocuments = new List<ValveDocument>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return ValveNumber;
        }

        /// <summary>
        /// When selecting valve assets to display on the map, you may want to display both
        /// Valves and Blow Offs(valves with valve controls blow off with flushing). 
        /// If you want to return all your coordinates as just Valve coordinates use the 
        /// valvesOnly property.
        /// </summary>
        /// <param name="valvesOnly">Return all the assets coordinates as valve coordinates.</param>
        /// <returns></returns>
        public virtual AssetCoordinate ToAssetCoordinate(bool valvesOnly = false)
        {
            var showingBlowOffsWithFlushing = CanHaveBlowOffInspections && !valvesOnly;
            // This smells and is a hack just to get icon logic in one spot. 
            var ac = (showingBlowOffsWithFlushing)
                ? (AssetCoordinate)new BlowOffAssetCoordinate(_iconSetRepository)
                : new ValveAssetCoordinate(_iconSetRepository);
            ac.IsActive = IsActive;
            ac.IsPublic = ValveBilling.Id == ValveBilling.Indices.PUBLIC;
            ac.HasOpenWorkOrder = HasOpenWorkOrder;

            if (showingBlowOffsWithFlushing)
            {
                // NOTE: These are both nullable and the AssetCoordinate properties
                //       SHOULD be set to null if it comes up.
                ac.LastInspection = LastBlowOffInspectionDate;
                ac.RequiresInspection = RequiresBlowOffInspection;
            }
            else
            {
                ac.LastInspection = LastInspectionDate;
                ac.RequiresInspection = RequiresInspection;

                // NormalPosition related things are only done for non-blowoff valves.
                if (NormalPosition != null)
                {
                    ac.InNormalPosition = InNormalPosition;
                    ac.NormalPosition = NormalPosition;
                }
                else
                {
                    // bug 3677, if a valve does not have a NormalPosition then they are considered to be in normal position

                    // The formula used by Valve.InNormalPosition returns null when NormalPosition is null
                    // or when there are currently no inspections for the valve. Setting this to null, rather
                    // than true, to be consistent with what the formula returns. 
                    // Also ValveRepository.GetValveAssetCoordinates sets this to null since the AssetCoordinate
                    // property is nullable, unlike the property on Valve.
                    ac.InNormalPosition = null;
                }
            }

            ac.Id = Id;

            if (Coordinate != null)
            {
                ac.Latitude = Coordinate.Latitude;
                ac.Longitude = Coordinate.Longitude;
            }

            return ac;
        }

        #endregion
    }

    [Serializable]
    public class ValveBilling : ReadOnlyEntityLookup
    {
        public const string PUBLIC = "Public",
                            MUNICIPAL = "Municipal",
                            O_AND_M = "O & M",
                            COMPANY = "Company",
                            PRIVATE = "Private";

        public struct Indices
        {
            public const int MUNICIPAL = 1, O_AND_M = 2, PUBLIC = 3, COMPANY = 4, PRIVATE = 5;
        }
    }

    [Serializable]
    public class ValveWorkOrderRequest : EntityLookup { }

    [Serializable]
    public class ValveControl : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int BLOW_OFF = 2, BLOW_OFF_WITH_FLUSHING = 3, HYDRANT = 9, MAIN = 13;
        }

        public const string BLOW_OFF_WITH_FLUSHING = "BLOW OFF WITH FLUSHING",
                            BLOW_OFF = "BLOW OFF";
    }

    [Serializable]
    public class ValveManufacturer : EntityLookup { }

    [Serializable]
    public class ValveSize : IEntityLookup
    {
        public virtual int Id { get; set; }
        public virtual decimal Size { get; set; }
        public virtual string SizeRange { get; set; }
        public virtual string Description => new ValveSizeDisplayItem {Size = Size}.Display;

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    public class ValveSizeDisplayItem : DisplayItem<ValveSize>
    {
        public decimal Size { get; set; }
        public override string Display => string.Format(CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS, Size);
    }

    [Serializable]
    public class ValveType : EntityLookup
    {
        public struct Indices
        {
            public const int BALL = 1, BUTTERFLY = 2, CHECK = 4, GATE = 5, TAPPING = 11;
        }

        public static readonly int[] VALVE_TYPES_REQUIRING_TURNS =
            {Indices.BALL, Indices.BUTTERFLY, Indices.GATE, Indices.TAPPING};

        public static string BALL = "Ball",
                             BUTTERFLY = "Butterfly",
                             CHECK = "Check",
                             GATE = "Gate",
                             TAPPING = "Tapping";

        public virtual string SAPCode { get; set; }
    }

    [Serializable]
    public class ValveZone : EntityLookup { }

    /// <summary>
    /// This is a dumb data class needed in the ValveRepository when it generates 
    /// a valve number. This is not an entity.
    /// </summary>
    public class ValveNumber
    {
        public string FormattedNumber => string.Format("{0}-{1}", Prefix, Suffix);
        public int Suffix { get; set; }
        public string Prefix { get; set; }
    }
}
