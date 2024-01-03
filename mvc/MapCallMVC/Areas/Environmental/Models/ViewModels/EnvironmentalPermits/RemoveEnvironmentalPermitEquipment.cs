using MapCall.Common.Model.Entities;
using System.Linq;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits
{
    public class RemoveEnvironmentalPermitEquipment : AlterEnvironmentalPermitEquipment
    {
        #region Constructors

        public RemoveEnvironmentalPermitEquipment(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override EnvironmentalPermit MapToEntity(EnvironmentalPermit entity)
        {
            var toRemove = entity.Equipment.Single(x => x.Id == EquipmentId);
            entity.Equipment.Remove(toRemove);
            return entity;
        }

        #endregion
    }
}