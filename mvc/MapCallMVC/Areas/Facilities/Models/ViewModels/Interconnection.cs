using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Metadata;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using MapCall.Common.Model.Entities;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class InterconnectionViewModel : ViewModel<Interconnection>
    {
        #region Properties

        [StringLength(Interconnection.StringLengths.DEP_DESIGNATION)]
        public virtual string DEPDesignation { get; set; }

        [DropDown, Required]
        [EntityMustExist(typeof(InterconnectionCategory))]
        [EntityMap]
        public int? Category { get; set; }

        [DropDown, Required]
        [EntityMustExist(typeof(InterconnectionOperatingStatus))]
        [EntityMap]
        public virtual int? OperatingStatus { get; set; }

        [DropDown, Required]
        [EntityMustExist(typeof(InterconnectionDirection))]
        [EntityMap]
        public virtual int? Direction { get; set; }

        [DropDown, Required]
        [EntityMustExist(typeof(InterconnectionType))]
        [EntityMap]
        public virtual int? Type { get; set; }

        [DropDown(ControllerViewDataKey = "PublicWaterSupply")]
        [EntityMustExist(typeof(PublicWaterSupply))]
        [EntityMap]
        public virtual int? InletPWSID { get; set; }

        [DropDown(ControllerViewDataKey = "PublicWaterSupply")]
        [EntityMustExist(typeof(PublicWaterSupply))]
        [EntityMap]
        public virtual int? OutletPWSID { get; set; }

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

        [Required, DropDown]
        [EntityMustExist(typeof(InterconnectionDeliveryMethod))]
        [EntityMap]
        public virtual int? DeliveryMethod { get; set; }
        public virtual int? InletConnectionSize { get; set; }
        public virtual int? OutletConnectionSize { get; set; }
        public virtual int? InletStaticPressure { get; set; }
        public virtual int? OutletStaticPressure { get; set; }
        public virtual float? MaximumFlowCapacity { get; set; }
        public virtual float? MaximumFlowCapacityStressedCondition { get; set; }

        [Required, DropDown]
        [EntityMustExist(typeof(InterconnectionFlowControlMethod))]
        [EntityMap]
        public virtual int? FlowControlMethod { get; set; }

        public virtual float? ReversibleCapacity { get; set; }

        [Coordinate]
        [View("Location")]
        [EntityMustExist(typeof(Coordinate))]
        [EntityMap("Coordinate")]
        public virtual int? CoordinateId { get; set; }

        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }

        #endregion

        #region Constructors

        public InterconnectionViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class CreateInterconnection : InterconnectionViewModel
    {
        #region Properties

        [DoesNotAutoMap("Used for cascading only")]
        [DropDown, SearchAlias("Facility", "f", "OperatingCenter.Id")]
        [Required] // Required because Facility is required and cascades off of this field.
        public int? OperatingCenter { get; set; }

        [DropDown("", "Facility", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operaeting center above")]
        [Required, EntityMustExist(typeof(Facility)), EntityMap]
        public virtual int? Facility { get; set; }

        #endregion

        #region Constructors

        public CreateInterconnection(IContainer container) : base(container) {}

        #endregion
    }

    public class EditInterconnection : InterconnectionViewModel
    {
        [SearchAlias("Facility", "f", "OperatingCenter.Id")]
        [Required] // Required because Facility is required and cascades off of this field.
        [DropDown, EntityMap("Facility.OperatingCenter", MMSINC.Utilities.ObjectMapping.MapDirections.ToViewModel)]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Facility", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        [Required, EntityMustExist(typeof(Facility)), EntityMap]
        public virtual int? Facility { get; set; }

        #region Constructors

        public EditInterconnection(IContainer container) : base(container) {}

        #endregion
    }

    public class SearchInterconnection : SearchSet<Interconnection>
    {
        #region Properties

        [DropDown, SearchAlias("Facility", "f", "OperatingCenter.Id")]
        public int? OperatingCenter { get; set; }
        [DropDown("", "Facility", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        public int? Facility { get; set; }
        [View("Categorization"), DropDown, EntityMap, EntityMustExist(typeof(InterconnectionCategory))]
        public int? Category { get; set; }
        
        [DropDown, EntityMap, EntityMustExist(typeof(InterconnectionOperatingStatus))]
        public int? OperatingStatus { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(InterconnectionDirection))]
        public int? Direction { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(InterconnectionType))]
        public int? Type { get; set; }


        public DateRange ContractStartDate { get; set; }
        public DateRange ContractEndDate { get; set; }

        #endregion
    }
}
