using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels.PreJobSafetyBriefs
{
    public class CreateProductionPreJobSafetyBriefFromWorkOrder : CreateProductionPreJobSafetyBriefBase
    {
        #region Properties

        [CheckBoxList]
        public override int[] Employees { get; set; }

        // This should come initially from the querystring of /New
        [Required, Secured, EntityMap, EntityMustExist(typeof(ProductionWorkOrder))]
        public override int? ProductionWorkOrder { get; set; }

        #endregion

        #region Constructors

        public CreateProductionPreJobSafetyBriefFromWorkOrder(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override ProductionPreJobSafetyBrief MapToEntity(ProductionPreJobSafetyBrief entity)
        {
            entity = base.MapToEntity(entity);

            var order = GetProductionWorkOrderForDisplay();
            entity.OperatingCenter = order.OperatingCenter;
            entity.Facility = order.Facility;

            return entity;
        }

        #endregion
    }
}
