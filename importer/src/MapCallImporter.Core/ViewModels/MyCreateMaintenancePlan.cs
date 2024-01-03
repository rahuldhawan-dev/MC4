using MapCallMVC.Areas.Production.Models.ViewModels;
using StructureMap;

namespace MapCallImporter.ViewModels
{
    public class MyCreateMaintenancePlan : CreateMaintenancePlanBase
    {
        #region Constructors

        public MyCreateMaintenancePlan(IContainer container) : base(container) { }

        #endregion
    }
}
