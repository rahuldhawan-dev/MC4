using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels.PreJobSafetyBriefs
{
    public abstract class EditProductionPreJobSafetyBriefBase : ProductionPreJobSafetyBriefViewModelBase
    {
        public EditProductionPreJobSafetyBriefBase(IContainer container) : base(container) { }
    }
}
