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
    public class ChemicalInventoryTransactionViewModel : ViewModel<ChemicalInventoryTransaction>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(ChemicalStorage))]
        [Required]
        public virtual int? Storage { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(ChemicalDelivery))]
        public virtual int? Delivery { get; set; }
        public virtual DateTime? Date { get; set; }
        public virtual int? QuantityGallons { get; set; }
        public virtual int? QuantityPounds { get; set; }
        public virtual string InventoryRecordType { get; set; }

        #endregion

        #region Constructors

        public ChemicalInventoryTransactionViewModel(IContainer container) : base(container) {}

        #endregion
	}

    public class CreateChemicalInventoryTransaction : ChemicalInventoryTransactionViewModel
    {
        #region Constructors

        public CreateChemicalInventoryTransaction(IContainer container) : base(container) {}

        #endregion
	}

    public class EditChemicalInventoryTransaction : ChemicalInventoryTransactionViewModel
    {
        #region Constructors

        public EditChemicalInventoryTransaction(IContainer container) : base(container) {}

        #endregion
	}

    public class SearchChemicalInventoryTransaction : SearchSet<ChemicalInventoryTransaction>
    {
        #region Properties

        public SearchString InventoryRecordType { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(ChemicalDelivery))]
        public int? Delivery { get; set; }
        [DropDown, EntityMap(MapDirections.None), EntityMustExist(typeof(Chemical))]
        [SearchAlias("Storage", "Chemical.Id")]
        public int? Chemical { get; set; }
        public DateRange Date { get; set; }

        #endregion
	}
}