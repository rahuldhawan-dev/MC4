using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SewerOpening
        : IEntityWithChangeTracking<User>,
            IThingWithNotes,
            IThingWithDocuments,
            IAsset,
            IThingWithAssetStatus,
            ISAPEquipment,
            IRetirableWorkOrderAsset,
            IThingWithShadow,
            IThingWithTownAndOperatingCenter
    {
        #region Consts

        public struct StringLengths
        {
            public const int CREATED_BY = 50,
                             CRITICAL_NOTES = 150,
                             DISTANCE_FROM_CROSS_STREET = 255,
                             LOCATION_DESCRIPTION = 100,
                             MAP_PAGE = 50,
                             NPDES_PERMIT_NUMBER = 10,
                             OLD_NUMBER = 50,
                             OPENING_NUMBER = 50,
                             OUTFALL_NUMBER = 10,
                             STREET_NUMBER = 10,
                             TASK_NUMBER = 50;
        }

        public struct Display
        {
            public const string CRITICAL = "Has Critical Notes",
                                NPDES_PERMIT_NUMBER = "NPDES Permit Number",
                                TASK_NUMBER = "WBS #",
                                LAST_NPDES_INSPECTION_DATE = "Last NPDES Regulator Inspection Date";
        }

        #endregion

        #region Fields

        private SewerOpeningDisplayItem _display;

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Town Town { get; set; }

        [View(WasteWaterSystem.DisplayNames.WASTEWATER_SYSTEM)]
        public virtual WasteWaterSystem WasteWaterSystem { get; set; }
        public virtual Street Street { get; set; }

        [DisplayName("Cross Street")]
        public virtual Street IntersectingStreet { get; set; }

        public virtual Coordinate Coordinate { get; set; }

        [DisplayName("Opening Status")]
        public virtual AssetStatus Status { get; set; }

        public virtual SewerOpeningMaterial SewerOpeningMaterial { get; set; }

        [DisplayName("Geo.E Functional Location")]
        public virtual string GeoEFunctionalLocation { get; set; }

        [DisplayName("SAP Functional Location")]
        public virtual FunctionalLocation FunctionalLocation { get; set; }

        public virtual TownSection TownSection { get; set; }

        public virtual string OpeningNumber { get; set; }

        // Here so it appears after OpeningNumber in the export
        public virtual SewerOpeningType SewerOpeningType { get; set; }

        [View(Display.TASK_NUMBER)]
        public virtual string TaskNumber { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? DateInstalled { get; set; }

        [View("Date Retired / Removed")]
        public virtual DateTime? DateRetired { get; set; }

        public virtual string MapPage { get; set; }
        public virtual User CreatedBy { get; set; }

        [DisplayName("Date Added")]
        public virtual DateTime CreatedAt { get; set; }

        public virtual string StreetNumber { get; set; }

        [View("Legacy ID")]
        public virtual string OldNumber { get; set; }

        public virtual string DistanceFromCrossStreet { get; set; }
        public virtual bool? IsEpoxyCoated { get; set; }
        public virtual int? Route { get; set; }
        public virtual int? Stop { get; set; }
        public virtual int OpeningSuffix { get; set; }
        public virtual bool? IsDoghouseOpening { get; set; }

        [DisplayName("SAP Equipment")]
        public virtual int? SAPEquipmentId { get; set; }

        public virtual string SAPEquipmentNumber => SAPEquipmentId.ToString().PadLeft(18, '0');
        public virtual string SAPErrorCode { get; set; }
        public virtual decimal? DepthToInvert { get; set; }
        public virtual decimal? RimElevation { get; set; }

        [View(Description = "This will override Operating Center inspection frequency.")]
        public virtual int? InspectionFrequency { get; set; }

        public virtual RecurringFrequencyUnit InspectionFrequencyUnit { get; set; }
        public virtual DateTime UpdatedAt { get; set; }

        public virtual User UpdatedBy { get; set; }

        [StringLength(StringLengths.OUTFALL_NUMBER)]
        public virtual string OutfallNumber { get; set; }

        [StringLength(StringLengths.LOCATION_DESCRIPTION)]
        public virtual string LocationDescription { get; set; }

        public virtual BodyOfWater BodyOfWater { get; set; }
        
        public virtual IList<SewerOpeningConnection> SewerOpeningConnections =>
            UpstreamSewerOpeningConnections.Union(DownstreamSewerOpeningConnections).ToList();

        public virtual IList<SewerOpeningConnection> UpstreamSewerOpeningConnections { get; set; }
        public virtual IList<SewerOpeningConnection> DownstreamSewerOpeningConnections { get; set; }

        public virtual IList<SewerMainCleaning> SewerMainCleanings =>
            SewerMainCleanings1.Union(SewerMainCleanings2).ToList();

        public virtual IList<SewerMainCleaning> SewerMainCleanings1 { get; set; }
        public virtual IList<SewerMainCleaning> SewerMainCleanings2 { get; set; }

        public virtual IList<SewerOpeningInspection> SewerOpeningInspections { get; set; }

        public virtual IList<NpdesRegulatorInspection> NpdesRegulatorInspections { get; set; }

        public virtual IList<WorkOrder> WorkOrders { get; set; }

        public virtual IList<SmartCoverAlert> SmartCoverAlerts { get; set; }

        [View(Display.CRITICAL)]
        public virtual bool Critical { get; set; }

        [Multiline]
        public virtual string CriticalNotes { get; set; }

        #endregion

        #region Logical Properties

        public virtual string DescriptionWithStatus => (_display ?? (_display = new SewerOpeningDisplayItem {
            OpeningNumber = OpeningNumber,
            Status = Status.Description
        })).Display;

        #region Documents

        public virtual IList<Document<SewerOpening>> Documents { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        #endregion

        #region Notes

        public virtual IList<Note<SewerOpening>> Notes { get; set; }

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        #endregion

        public virtual MapIcon Icon => Coordinate?.Icon;

        [DoesNotExport]
        public virtual string TableName => nameof(SewerOpening) + "s";

        [DoesNotExport]
        public virtual bool IsActive => Status != null && AssetStatus.ACTIVE_STATUSES.Contains(Status.Id);

        [DoesNotExport]
        public virtual bool WorkOrdersEnabled => !AssetStatus.WORK_ORDER_DISABLED_STATUSES.Contains(Status.Id);

        [DoesNotExport]
        public virtual bool CanBeCopied => AssetStatus.CanBeCopiedStatuses.Contains(Status.Id) || AssetStatus.ACTIVE_STATUSES.Contains(Status.Id);

        [DoesNotExport]
        public virtual string Identifier => OpeningNumber;

        public virtual decimal? Latitude => Coordinate?.Latitude;

        public virtual decimal? Longitude => Coordinate?.Longitude;

        public virtual State State => Town.State;

        [DoesNotExport, View(Display.NPDES_PERMIT_NUMBER)]
        public virtual string NpdesPermitNumber => WasteWaterSystem?.PermitNumber;

        [DoesNotExport]
        public virtual string RegulatorNumber => OpeningNumber;

        [DoesNotExport]
        public virtual string RecordUrl { get; set; }

        public virtual DateTime? LastInspectionDate => SewerOpeningInspections
                                                      .OrderByDescending(x => x.DateInspected)
                                                      .Select(x => (DateTime?)x.DateInspected)
                                                      .FirstOrDefault();

        [DisplayName(Display.LAST_NPDES_INSPECTION_DATE)]
        public virtual DateTime? LastNpdesRegulatorInspectionDate => NpdesRegulatorInspections
                                                                    .OrderByDescending(x => x.DepartureDateTime)
                                                                    .Select(x => (DateTime?)x.DepartureDateTime)
                                                                    .FirstOrDefault();
        public virtual DateTime? LastManholeCleaningDate => SewerMainCleanings
                                                           .OrderByDescending(x => x.InspectedDate)
                                                           .Select(x => x.InspectedDate).FirstOrDefault();

        [DoesNotExport]
        public virtual bool IsInactive => Status != null && AssetStatus.INACTIVE_STATUSES.Contains(Status.Id);

        public static readonly int[] UNINSPECTABLE_STATUSES = {
            AssetStatus.Indices.CANCELLED, AssetStatus.Indices.REMOVED, AssetStatus.Indices.RETIRED,
            AssetStatus.Indices.INACTIVE
        };

        /// <summary>
        /// Returns true if the asset can have inspections added.
        /// </summary>
        [DoesNotExport]
        public virtual bool IsInspectable => !UNINSPECTABLE_STATUSES.Contains(Status.Id);

        #endregion

        #endregion

        #region Constructors

        public SewerOpening()
        {
            UpstreamSewerOpeningConnections = new List<SewerOpeningConnection>();
            DownstreamSewerOpeningConnections = new List<SewerOpeningConnection>();
            SewerMainCleanings1 = new List<SewerMainCleaning>();
            SewerMainCleanings2 = new List<SewerMainCleaning>();
            WorkOrders = new List<WorkOrder>();
            SewerOpeningInspections = new List<SewerOpeningInspection>();
            NpdesRegulatorInspections = new List<NpdesRegulatorInspection>();
            Documents = new List<Document<SewerOpening>>();
            Notes = new List<Note<SewerOpening>>();
            SmartCoverAlerts = new List<SmartCoverAlert>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return OpeningNumber;
        }

        #endregion
    }
}
