using MapCallMVC.Areas.Production.Models.ViewModels.PreJobSafetyBriefs;

namespace MapCallMVC.Tests.Areas.Production.ViewModels.PreJobSafetyBriefs
{
    public abstract class EditProductionPreJobSafetyBriefTestBase<TViewModel>
        : ProductionPreJobSafetyBriefViewModelTestBase<TViewModel>
        where TViewModel : EditProductionPreJobSafetyBriefBase { }
}
