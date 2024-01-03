using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Controllers;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Results;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.ProjectManagement.Controllers
{
    [ActionHelperViewVirtualPathFormat("~/Areas/ProjectManagement/Views/ContractorLaborCost/{0}.cshtml")]
    public class ContractorLaborCostController : EntityLookupControllerBase<IContractorLaborCostRepository, ContractorLaborCost, ContractorLaborCostViewModel>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesEstimatingProjects;

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Show:
                    this.AddOperatingCenterDropDownData();
                    break;
            }
        }

        #endregion

        #region FindByStockNumberUnitOrDescription

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult FindByStockNumberUnitOrDescription(string partial)
        {
            return new AutoCompleteResult(Repository.FindByStockNumberUnitOrDescription(partial), "Id", "Description");
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult FindByOperatingCenterId(int operatingCenterId)
        {
            return new CascadingActionResult(Repository.FindByOperatingCenterId(operatingCenterId), "Description", "Id");
        }

        #endregion

        #region Add/Remove Operating Center

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult AddOperatingCenter(int id, int operatingCenterId)
        {
            return ActionHelper.DoUpdate(ViewModelFactory.BuildWithOverrides<AddOperatingCenterContractorLaborCost, ContractorLaborCost>(Repository.Find(id), new {
                OperatingCenterId = operatingCenterId
            }));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult RemoveOperatingCenter(int id, int operatingCenterId)
        {
            return ActionHelper.DoUpdate(ViewModelFactory.BuildWithOverrides<RemoveOperatingCenterContractorLaborCost, ContractorLaborCost>(Repository.Find(id), new {
                OperatingCenterId = operatingCenterId
            }));
        }

        #endregion

        public ContractorLaborCostController(ControllerBaseWithPersistenceArguments<IContractorLaborCostRepository, ContractorLaborCost, User> args) : base(args) {}
    }
}