
using StructureMap;

namespace MapCallMVC.Areas.ProjectManagement.Models.ViewModels
{
    public class CreateEstimatingProjectOtherCost : EstimatingProjectOtherCostViewModel
    {
        #region Constructors

        public CreateEstimatingProjectOtherCost(IContainer container) : base(container) {}

        #endregion
    }
}