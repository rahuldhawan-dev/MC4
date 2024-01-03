using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.ProjectManagement.Controllers
{
    public class ContractorOverrideLaborCostController : ControllerBaseWithPersistence<ContractorOverrideLaborCost, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesEstimatingProjects;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>(dataGetter: this.GetUserOperatingCentersFn(ROLE, action.ToRoleAction()));
            this.AddDynamicDropDownData<Contractor, ContractorDisplayItem>();

            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDynamicDropDownData<ContractorLaborCost, ContractorLaborCostDisplayItem>();

                    break;
            }

        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchContractorOverrideLaborCost model)
        {
            return ActionHelper.DoSearch(model);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchContractorOverrideLaborCost model)
        {
            return ActionHelper.DoIndex(model);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region New/Create

        [HttpGet]
        [RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateContractorOverrideLaborCost(_container));
        }

        [HttpPost, RequiresSecureForm]
        [RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateContractorOverrideLaborCost model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet]
        [RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditContractorOverrideLaborCost>(id);
        }

        [HttpPost, RequiresSecureForm]
        [RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditContractorOverrideLaborCost model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresSecureForm, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        public ContractorOverrideLaborCostController(ControllerBaseWithPersistenceArguments<IRepository<ContractorOverrideLaborCost>, ContractorOverrideLaborCost, User> args) : base(args) {}
    }
}
