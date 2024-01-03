using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Interconnection
        : IEntityWithCreationTimeTracking, IThingWithNotes, IThingWithDocuments, IThingWithCoordinate
    {
        #region Constants

        public struct StringLengths
        {
            #region Constants

            public const int DEP_DESIGNATION = 30,
                             PROGRAM_INTEREST_NUMBER = 50,
                             ACCOUNT_NUMBER = 50;

            #endregion
        }

        #endregion

        #region Private Members

        private InterconnectionDisplayItem _display;

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        [View("Location")]
        public virtual Coordinate Coordinate { get; set; }

        [DoesNotExport]
        public virtual MapIcon Icon => Coordinate != null ? Coordinate.Icon : null;

        public virtual InterconnectionPurchaseSellTransfer PurchaseSellTransfer { get; set; }

        [View("Operating Status")]
        public virtual InterconnectionOperatingStatus OperatingStatus { get; set; }

        [View("Categorization")]
        public virtual InterconnectionCategory Category { get; set; }

        [View("Delivery Method")]
        public virtual InterconnectionDeliveryMethod DeliveryMethod { get; set; }

        [View("Method of Flow Control")]
        public virtual InterconnectionFlowControlMethod FlowControlMethod { get; set; }

        [View("Interconnect Direction")]
        public virtual InterconnectionDirection Direction { get; set; }

        [View("Interconnect Type")]
        public virtual InterconnectionType Type { get; set; }

        public virtual Facility Facility { get; set; }
        public virtual DateTime CreatedAt { get; set; }

        [StringLength(StringLengths.DEP_DESIGNATION)]
        public virtual string DEPDesignation { get; set; }

        [StringLength(StringLengths.PROGRAM_INTEREST_NUMBER)]
        public virtual string ProgramInterestNumber { get; set; }

        [StringLength(StringLengths.ACCOUNT_NUMBER)]
        public virtual string PurchasedWaterAccountNumber { get; set; }

        [StringLength(StringLengths.ACCOUNT_NUMBER)]
        public virtual string SoldWaterAccountNumber { get; set; }

        public virtual bool? DirectConnection { get; set; }
        public virtual int? InletConnectionSize { get; set; }
        public virtual int? OutletConnectionSize { get; set; }
        public virtual int? InletStaticPressure { get; set; }
        public virtual int? OutletStaticPressure { get; set; }

        [View("Maximum Flow Capacity (MGD)")]
        public virtual float? MaximumFlowCapacity { get; set; }

        [View("Maximum Flow Capacity Stressed Condition (MGD)")]
        public virtual float? MaximumFlowCapacityStressedCondition { get; set; }

        public virtual bool? DistributionPipingRestrictions { get; set; }
        public virtual string WaterQuality { get; set; }

        [Required]
        public virtual bool FluoridatedSupplyReceivingPurveyor { get; set; }

        [Required]
        public virtual bool FluoridatedSupplyDeliveryPurveyor { get; set; }

        [Required]
        public virtual bool ChloramineResidualReceivingPurveyor { get; set; }

        [Required]
        public virtual bool ChloramineResidualDeliveryPurveyor { get; set; }

        [Required]
        public virtual bool CorrosionInhibitorReceivingPurveyor { get; set; }

        [Required]
        public virtual bool CorrosionInhibitorDeliveryPurveyor { get; set; }

        public virtual float? ReversibleCapacity { get; set; }

        [Required]
        public virtual bool AnnualTestRequired { get; set; }

        public virtual bool? Contract { get; set; }
        public virtual float? ContractMaxSummer { get; set; }
        public virtual float? ContractMinSummer { get; set; }
        public virtual float? ContractMaxWinter { get; set; }
        public virtual float? ContractMinWinter { get; set; }
        public virtual IList<Meter> Meters { get; set; }
        public virtual PublicWaterSupply InletPWSID { get; set; }
        public virtual PublicWaterSupply OutletPWSID { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? ContractStartDate { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? ContractEndDate { get; set; }

        #region Notes/Docs

        public virtual IList<Document<Interconnection>> Documents { get; set; }
        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        public virtual IList<Note<Interconnection>> Notes { get; set; }
        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        [DoesNotExport]
        public virtual string TableName => nameof(Interconnection) + "s";

        #endregion

        #region Logical Properties

        public virtual decimal? Latitude => (Coordinate != null) ? Coordinate.Latitude : (decimal?)null;

        public virtual decimal? Longitude => (Coordinate != null) ? Coordinate.Longitude : (decimal?)null;

        public virtual string Display => (_display ?? (_display = new InterconnectionDisplayItem {
            Id = Id,
            FacilityName = Facility?.FacilityName,
            OperatingCenter = Facility?.OperatingCenter,
            FacilityId = Facility?.Id ?? 0
        })).Display;

        [DoesNotExport]
        public virtual string RecordUrl { get; set; }

        #endregion

        #endregion

        #region Constructors

        public Interconnection()
        {
            Meters = new List<Meter>();
        }

        #endregion
    }

    [Serializable]
    public class InterconnectionDisplayItem : DisplayItem<Interconnection>
    {
        [SelectDynamic("FacilityName", Field = "Facility")]
        public string FacilityName { get; set; }

        [SelectDynamic("OperatingCenter", Field = "Facility")]
        public OperatingCenter OperatingCenter { get; set; }

        [SelectDynamic("Id", Field = "Facility")]
        public int FacilityId { get; set; }

        public override string Display =>
            $"Interconnection: {Id} - {FacilityName} - {OperatingCenter.OperatingCenterCode}-{FacilityId}";
    }
}
