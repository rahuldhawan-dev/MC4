using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits
{
    public class AddEnvironmentalPermitEquipment : AlterEnvironmentalPermitEquipment
    {
        #region Constructors

        public AddEnvironmentalPermitEquipment(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override EnvironmentalPermit MapToEntity(EnvironmentalPermit entity)
        {
            var newEquipment = _container.GetInstance<IEquipmentRepository>().Find(EquipmentId);
            entity.Equipment.Add(newEquipment);
            return entity;
        }

        #endregion
    }
}