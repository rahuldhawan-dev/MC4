using System.Linq;
using MapCall.Common.Model.Entities;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits
{
    public class RemoveEnvironmentalPermitFacility : AlterEnvironmentalPermitFacility
    {
        #region Constructors

        public RemoveEnvironmentalPermitFacility(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override EnvironmentalPermit MapToEntity(EnvironmentalPermit entity)
        {
            var toRemove = entity.Facilities.Single(x => x.Id == FacilityId);
            entity.Facilities.Remove(toRemove);
            return entity;
        }

        #endregion
    }
}