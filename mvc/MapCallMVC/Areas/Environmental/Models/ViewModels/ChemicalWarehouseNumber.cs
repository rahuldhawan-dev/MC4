using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels
{
    public class ChemicalWarehouseNumberViewModel : ViewModel<ChemicalWarehouseNumber>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter)), Required]
        public int? OperatingCenter { get; set; }
        [Required]
        public string WarehouseNumber { get; set; }

        #endregion

        #region Constructors

        public ChemicalWarehouseNumberViewModel(IContainer container) : base(container) {}

        #endregion
	}

    public class CreateChemicalWarehouseNumber : ChemicalWarehouseNumberViewModel
    {
        #region Constructors

        public CreateChemicalWarehouseNumber(IContainer container) : base(container) {}

        #endregion
	}

    public class EditChemicalWarehouseNumber : ChemicalWarehouseNumberViewModel
    {
        #region Constructors

        public EditChemicalWarehouseNumber(IContainer container) : base(container) {}

        #endregion
	}

    public class SearchChemicalWarehouseNumber : SearchSet<ChemicalWarehouseNumber>
    {
        #region Properties

        public SearchString WarehouseNumber { get; set; }

        #endregion
	}
}