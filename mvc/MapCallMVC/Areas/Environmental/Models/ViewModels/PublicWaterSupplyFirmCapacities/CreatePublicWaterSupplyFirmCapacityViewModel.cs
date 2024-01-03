using StructureMap;
using MapCall.Common.Model.Entities;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.PublicWaterSupplyFirmCapacities
{
    public class CreatePublicWaterSupplyFirmCapacityViewModel : PublicWaterSupplyFirmCapacityViewModel
    {
        #region Constructors

        public CreatePublicWaterSupplyFirmCapacityViewModel(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override PublicWaterSupplyFirmCapacity MapToEntity(PublicWaterSupplyFirmCapacity entity)
        {
            entity = base.MapToEntity(entity);
            entity.PublicWaterSupply.CurrentPublicWaterSupplyFirmCapacity = entity;
            return entity;
        }

        #endregion
    }
}
