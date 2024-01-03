using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits
{
    public class AddEnvironmentalPermitFacility : AlterEnvironmentalPermitFacility
    {
        #region Constructors

        public AddEnvironmentalPermitFacility(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override EnvironmentalPermit MapToEntity(EnvironmentalPermit entity)
        {
            var newFacility = _container.GetInstance<IFacilityRepository>().Find(FacilityId);
            entity.Facilities.Add(newFacility);
            return entity;
        }

        #endregion
    }
}