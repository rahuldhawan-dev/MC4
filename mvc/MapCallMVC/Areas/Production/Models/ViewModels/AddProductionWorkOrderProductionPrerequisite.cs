using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels {
    public class AddProductionWorkOrderProductionPrerequisite : ViewModel<ProductionWorkOrder>
    {
        #region Properties

        [DropDown, Required, EntityMap(MapDirections.None/* Manually mapped */), EntityMustExist(typeof(ProductionPrerequisite))]
        public int? ProductionPrerequisite { get; set; }

        #endregion

        #region Constructors

        public AddProductionWorkOrderProductionPrerequisite(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override ProductionWorkOrder MapToEntity(ProductionWorkOrder entity)
        {
            var productionWorkOrderProductionPrerequisite = new ProductionWorkOrderProductionPrerequisite {
                ProductionWorkOrder = entity,
                ProductionPrerequisite = _container.GetInstance<IRepository<ProductionPrerequisite>>()
                    .Find(ProductionPrerequisite.Value)
            };
            entity.ProductionWorkOrderProductionPrerequisites.Add(productionWorkOrderProductionPrerequisite);
            return entity;
        }

        #endregion
    }
}