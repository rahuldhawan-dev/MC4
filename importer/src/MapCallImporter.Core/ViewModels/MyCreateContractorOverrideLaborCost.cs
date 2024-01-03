using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using StructureMap;

namespace MapCallImporter.ViewModels
{
    public class MyCreateContractorOverrideLaborCost : BaseContractorOverrideLaborCostViewModel
    {
        #region Constructors

        public MyCreateContractorOverrideLaborCost(IContainer container) : base(container) { }

        #endregion
    }
}
