using MapCall.Common.Model.Entities;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels 
{
    public class CopyMaintenancePlan : CreateMaintenancePlan
    {
        #region Constructors

        public CopyMaintenancePlan(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void Map(MaintenancePlan entity)
        {
            base.Map(entity);
            this.Equipment = null;
            this.IsPlanPaused = false;
            this.IsActive = false;
        }

        #endregion
    }
}
