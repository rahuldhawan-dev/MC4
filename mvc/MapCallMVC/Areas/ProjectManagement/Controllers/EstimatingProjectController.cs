using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.ProjectManagement.Controllers
{
    public class EstimatingProjectController : ControllerBaseWithPersistence<IEstimatingProjectRepository, EstimatingProject, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesEstimatingProjects;

        #endregion

        #region Exposed Methods

        private void SetShowLookupData(EstimatingProject project)
        {
            this.AddDynamicDropDownData<IMaterialRepository, Material, MaterialDisplayItem>(dataGetter: x => x.FindActiveMaterialByOperatingCenter(project.OperatingCenter));
            this.AddDynamicDropDownData<ContractorLaborCost, ContractorLaborCostDisplayItem>(filter: c => c.OperatingCenters.Contains(project.OperatingCenter));
        }

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDropDownData<EstimatingProjectType>("ProjectType");
                    this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>(dataGetter: this.GetUserOperatingCentersFn(ROLE));
                    this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>("Estimator", this.GetUserOperatingCentersEmployeesFn(ROLE));
                    this.AddDropDownData<Material>("Material");
                    break;
                case ControllerAction.New:
                    this.AddDropDownData<EstimatingProjectType>("ProjectType");
                    this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>(dataGetter: this.GetUserOperatingCentersFn(ROLE, RoleActions.Add, extraFilterP: x => x.IsActive));
                    this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>("Estimator", this.GetUserOperatingCentersEmployeesFn(ROLE));
                    this.AddDropDownData<Material>("Material");
                    break;
                case ControllerAction.Edit:
                    this.AddDropDownData<EstimatingProjectType>("ProjectType");
                    this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>(dataGetter: this.GetUserOperatingCentersFn(ROLE, RoleActions.Edit));
                    this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>("Estimator", this.GetUserOperatingCentersEmployeesFn(ROLE));
                    this.AddDropDownData<Material>("Material");
                    break;
                case ControllerAction.Show:
                    this.AddDropDownData<AssetType>("AssetType", x => x.Id, x => x.Description);
                    this.AddDropDownData<CompanyLaborCost>("CompanyLaborCost");
                    this.AddDropDownData<PermitType>("PermitType");
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchEstimatingProject>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, null, project => {
                SetShowLookupData(project);
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchEstimatingProject search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs { RedirectSingleItemToShowView = false}));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateEstimatingProject(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add), RequiresSecureForm]
        public ActionResult Create(CreateEstimatingProject model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditEstimatingProject>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult Update(EditEstimatingProject model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete), RequiresSecureForm]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region AddEstimatingProjectMaterial

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult AddEstimatingProjectMaterial(AddEstimatingProjectMaterial model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region RemoveEstimatingProjectMaterial

        [HttpDelete, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult RemoveEstimatingProjectMaterial(RemoveEstimatingProjectMaterial model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region AddEstimatingProjectOtherCost

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult AddEstimatingProjectOtherCost(AddEstimatingProjectOtherCost model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region RemoveEstimatingProjectOtherCost

        [HttpDelete, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult RemoveEstimatingProjectOtherCost(RemoveEstimatingProjectOtherCost model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region AddEstimatingProjectCompanyLaborCost

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult AddEstimatingProjectCompanyLaborCost(AddEstimatingProjectCompanyLaborCost model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region RemoveEstimatingProjectCompanyLaborCost

        [HttpDelete, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult RemoveEstimatingProjectCompanyLaborCost(RemoveEstimatingProjectCompanyLaborCost model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region AddEstimatingProjectContractorLaborCost

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult AddEstimatingProjectContractorLaborCost(AddEstimatingProjectContractorLaborCost model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region RemoveEstimatingProjectContractorLaborCost

        [HttpDelete, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult RemoveEstimatingProjectContractorLaborCost(RemoveEstimatingProjectContractorLaborCost model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region AddEstimatingProjectPermit

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult AddEstimatingProjectPermit(AddEstimatingProjectPermit model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region RemoveEstimatingProjectPermit

        [HttpDelete, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult RemoveEstimatingProjectPermit(RemoveEstimatingProjectPermit model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        public EstimatingProjectController(ControllerBaseWithPersistenceArguments<IEstimatingProjectRepository, EstimatingProject, User> args) : base(args) {}
    }
}