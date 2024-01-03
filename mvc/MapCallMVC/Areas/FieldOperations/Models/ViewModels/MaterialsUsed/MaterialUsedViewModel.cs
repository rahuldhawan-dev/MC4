using MMSINC.Data;
using MapCall.Common.Model.Entities;
using StructureMap;
using MMSINC.Validation;
using System.ComponentModel.DataAnnotations;
using MMSINC.Metadata;
using DataAnnotationsExtensions;
using MMSINC.Utilities.ObjectMapping;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.MaterialsUsed
{
    public class MaterialUsedViewModel : ViewModel<MaterialUsed>
    {
        #region Constants

        public const string MATERIAL_ERROR_MESSAGE = "Please choose a part number for stock materials.",
                            NON_STOCK_DESCRIPTION_ERROR_MESSAGE = "Non-stock materials must have a description.",
                            STOCK_LOCATION_ERROR_MESSAGE = "Please choose a stock location for stock materials.";

        #endregion

        #region Constructors

        public MaterialUsedViewModel(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [Required, EntityMap, EntityMustExist(typeof(WorkOrder))]
        public int? WorkOrder { get; set; }

        [RequiredWhen(nameof(NonStockDescription), null, ErrorMessage = MATERIAL_ERROR_MESSAGE)]
        [EntityMap, EntityMustExist(typeof(Material)), DropDown]
        public int? Material { get; set; }

        [Required, Min(1)]
        public int? Quantity { get; set; }
        
        [RequiredWhen(nameof(Material), null, ErrorMessage = NON_STOCK_DESCRIPTION_ERROR_MESSAGE)]
        [View("Description"), Multiline]
        public string NonStockDescription { get; set; }
        
        [RequiredWhen(nameof(NonStockDescription), null, ErrorMessage = STOCK_LOCATION_ERROR_MESSAGE)]
        [EntityMap, EntityMustExist(typeof(StockLocation)), DropDown]
        public int? StockLocation { get; set; }

        [DoesNotAutoMap]
        public int? OperatingCenter { get; set; }

        #endregion
    }
}