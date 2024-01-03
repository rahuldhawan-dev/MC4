using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels
{
    public class ChemicalUnitCostViewModel : ViewModel<ChemicalUnitCost>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(Chemical))]
        [Required]
        public virtual int? Chemical { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(ChemicalWarehouseNumber))]
        public virtual int? WarehouseNumber { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(ChemicalVendor))]
        public virtual int? Vendor { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual decimal? PricePerPoundWet { get; set; }
        public virtual string PoNumber { get; set; }
        public virtual int? ChemicalLeadTimeDays { get; set; }
        public virtual string ChemicalOrderingProcess { get; set; }

        #endregion

        #region Constructors

        public ChemicalUnitCostViewModel(IContainer container) : base(container) {}

        #endregion
	}

    public class CreateChemicalUnitCost : ChemicalUnitCostViewModel
    {
        #region Constructors

        public CreateChemicalUnitCost(IContainer container) : base(container) {}

        #endregion
	}

    public class EditChemicalUnitCost : ChemicalUnitCostViewModel
    {
        #region Constructors

        public EditChemicalUnitCost(IContainer container) : base(container) {}

        #endregion
	}

    public class SearchChemicalUnitCost : SearchSet<ChemicalUnitCost>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(Chemical))]
        public int? Chemical { get; set; }
        [SearchAlias("Chemical", "PartNumber")]
        public SearchString PartNumber { get; set; }
        public SearchString PoNumber { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(ChemicalWarehouseNumber))]
        public int? WarehouseNumber { get; set; }
        public DateRange StartDate { get; set; }
        public DateRange EndDate { get; set; }

        #endregion
	}
}