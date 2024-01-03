using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels
{
    public class ChemicalDeliveryViewModel : ViewModel<ChemicalDelivery>
    {
        #region Properties

        [DropDown("Environmental", "ChemicalStorage", "ByChemical", DependsOn = "Chemical"), EntityMap, EntityMustExist(typeof(ChemicalStorage))]
        [Required]
        public virtual int? Storage { get; set; }
        [DropDown("Environmental", "ChemicalUnitCost", "ByChemical", DependsOn = "Chemical"), EntityMap, EntityMustExist(typeof(ChemicalUnitCost))]
        public virtual int? UnitCost { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(Chemical))]
        [Required]
        public virtual int? Chemical { get; set; }
        public virtual DateTime? DateOrdered { get; set; }
        public virtual DateTime? ScheduledDeliveryDate { get; set; }
        public virtual DateTime? ActualDeliveryDate { get; set; }
        public virtual string ConfirmationInformation { get; set; }
        public virtual string ReceiptNumberJde { get; set; }
        public virtual string BatchNumberJde { get; set; }
        public virtual int? EstimatedDeliveryQuantityGallons { get; set; }
        public virtual int? ActualDeliveryQuantityGallons { get; set; }
        public virtual int? EstimatedDeliveryQuantityPounds { get; set; }
        public virtual int? ActualDeliveryQuantityPounds { get; set; }
        public virtual string DeliveryTicketNumber { get; set; }
        public virtual string DeliveryInstructions { get; set; }
        public virtual bool SplitFacilityDelivery { get; set; }
        public virtual string SecurityInformation { get; set; }

        #endregion

        #region Constructors

        public ChemicalDeliveryViewModel(IContainer container) : base(container) {}

        #endregion
	}

    public class CreateChemicalDelivery : ChemicalDeliveryViewModel
    {
        #region Constructors

        public CreateChemicalDelivery(IContainer container) : base(container) {}

        #endregion
	}

    public class EditChemicalDelivery : ChemicalDeliveryViewModel
    {
        #region Constructors

        public EditChemicalDelivery(IContainer container) : base(container) {}

        #endregion
	}

    public class SearchChemicalDelivery : SearchSet<ChemicalDelivery>
    {
        #region Properties

        public SearchString ReceiptNumberJde { get; set; }
        public SearchString DeliveryTicketNumber { get; set; }
        [DropDown, EntityMap(MapDirections.None), EntityMustExist(typeof(OperatingCenter)), SearchAlias("Facility", "OperatingCenter.Id")]
        public int? OperatingCenter { get; set; } 
        [DropDown, EntityMap(MapDirections.None), EntityMustExist(typeof(ChemicalVendor)), SearchAlias("UnitCost", "Vendor.Id")]
        public int? Vendor { get; set; }
        [SearchAlias("Chemical", "PartNumber")]
        public SearchString ChemicalPartNumber { get; set; }
        [View("Delivery Date")]
        public DateRange ActualDeliveryDate { get; set; }

        #endregion
	}
}