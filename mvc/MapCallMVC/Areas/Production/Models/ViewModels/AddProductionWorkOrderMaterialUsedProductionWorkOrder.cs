using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class AddProductionWorkOrderMaterialUsedProductionWorkOrder : ViewModel<ProductionWorkOrder>
    {
        #region Properties

        [DoesNotAutoMap]
        [ComboBox, EntityMustExist(typeof(Material)), RequiredWhen("NonStockDescription", ComparisonType.EqualTo, null, ErrorMessage = "Either a Material must be selected or a Non Stock Description must be entered.")]
        public int? Material { get; set; }
        
        [Required, DoesNotAutoMap]
        public int? Quantity { get; set; }
        
        [DoesNotAutoMap]
        [StringLength(50), RequiredWhen("Material", ComparisonType.EqualTo, null, ErrorMessage = "Either a Material must be selected or a Non Stock Description must be entered.")]
        public string NonStockDescription { get; set; }

        [DoesNotAutoMap, DropDown, EntityMustExist(typeof(StockLocation))]
        public int? StockLocation { get; set; }
        
        #endregion

        #region Constructors

        public AddProductionWorkOrderMaterialUsedProductionWorkOrder(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override ProductionWorkOrder MapToEntity(ProductionWorkOrder entity)
        {
            var materialUsed = new ProductionWorkOrderMaterialUsed {
                ProductionWorkOrder = entity,
                Quantity = Quantity.Value
            };
            if (!string.IsNullOrWhiteSpace(NonStockDescription))
                materialUsed.NonStockDescription = NonStockDescription;
            if (StockLocation.HasValue)
                materialUsed.StockLocation =
                    _container.GetInstance<IRepository<StockLocation>>().Find(StockLocation.Value);
            if (Material.HasValue)
                    materialUsed.Material = _container.GetInstance<IMaterialRepository>().Find(Material.Value);
            entity.ProductionWorkOrderMaterialUsed.Add(materialUsed);
            return entity;
        }

        #endregion
    }

    public class RemoveProductionWorkOrderMaterialUsedProductionWorkOrder : ViewModel<ProductionWorkOrder>
    {
        #region Properties

        [Required, EntityMustExist(typeof(ProductionWorkOrderMaterialUsed))]
        public int? ProductionWorkOrderMaterialUsed { get; set; }

        #endregion

        #region Constructors

        public RemoveProductionWorkOrderMaterialUsedProductionWorkOrder(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override ProductionWorkOrder MapToEntity(ProductionWorkOrder entity)
        {
            var productionWorkOrderMaterialUsed = _container
                .GetInstance<IRepository<ProductionWorkOrderMaterialUsed>>()
                .Find(ProductionWorkOrderMaterialUsed.Value);
            entity.ProductionWorkOrderMaterialUsed.Remove(productionWorkOrderMaterialUsed);
            return entity;
        }

        #endregion
    }
}
