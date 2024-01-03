using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace Contractors.Models.ViewModels
{
    public class MaterialUsedModel : ViewModel<MaterialUsed>
    {
        #region Properties

        [DropDown(DefaultItemLabel = "n/a")]
        [EntityMap, EntityMustExist(typeof(Material))]
        [RequiredWhen(nameof(NonStockDescription), null, ErrorMessage = "Please choose a part number for stock materials.")]
        public virtual int? Material { get; set; }

        [Required, Min(1)]
        public virtual int? Quantity { get; set; }

        [View("Description"), Multiline]
        [RequiredWhen(nameof(Material), null, ErrorMessage = "Non-stock materials must have a description.")]
        public virtual string NonStockDescription { get; set; }

        [DropDown(DefaultItemLabel = "n/a")]
        [EntityMap, EntityMustExist(typeof(StockLocation))]
        [RequiredWhen(nameof(NonStockDescription), null, ErrorMessage = "Please choose a stock location for stock materials.")]
        public virtual int? StockLocation { get; set; }

        #endregion

        #region Constructors

        public MaterialUsedModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateMaterialUsed : MaterialUsedModel
    {
        #region Properties

        [EntityMap, EntityMustExist(typeof(WorkOrder))]
        public virtual int WorkOrder { get; set; }

        #endregion

        #region Constructors

        public CreateMaterialUsed(IContainer container) : base(container) { }

        #endregion
    }

    public class EditMaterialUsed : MaterialUsedModel
    {
        #region Constructors

        public EditMaterialUsed(IContainer container) : base(container) { }

        #endregion
    }
}
 