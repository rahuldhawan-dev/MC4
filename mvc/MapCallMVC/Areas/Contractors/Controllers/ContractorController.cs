using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Contractors.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using System.Web.Mvc;

namespace MapCallMVC.Areas.Contractors.Controllers
{
    public class ContractorController : ControllerBaseWithPersistence<IContractorRepository, Contractor, User>
    {
        #region Constants

        public const RoleModules Role = RoleModules.ContractorsGeneral;

        #endregion

        #region Constructors

        public ContractorController(ControllerBaseWithPersistenceArguments<IContractorRepository, Contractor, User> args) : base(args) { }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            this.AddDynamicDropDownData<ContractorContactType, ContractorContactTypeDisplayItem>("ContactType");
            switch (action)
            {
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(Role, RoleActions.Add, "OperatingCenters", extraFilterP: x => x.IsActive);
                    this.AddOperatingCenterDropDownDataForRoleAndAction(Role, RoleActions.Add, "FrameworkOperatingCenters", extraFilterP: x => x.IsActive);
                    break;
            }
        }

        [HttpGet]
        public ActionResult GetFrameworkContractorsByOperatingCenter(int operatingCenterId)
        {
            return new CascadingActionResult(Repository.GetFrameworkContractorsByOperatingCenterId(operatingCenterId), "Description", "Id")
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult ByOperatingCenterId(int operatingCenterId)
        {
            return new CascadingActionResult(Repository.GetByOperatingCenterId(operatingCenterId), "Description", "Id")
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult ActiveContractorsByOperatingCenterId(int operatingCenterId)
        {
            return new CascadingActionResult(Repository.GetActiveContractorsByOperatingCenterId(operatingCenterId), "Description", "Id");
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(Role)]
        public ActionResult Search(SearchContractor search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(Role)]
        public ActionResult Index(SearchContractor search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        [HttpGet, RequiresRole(Role)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(Role, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateContractor(_container));
        }

        [HttpPost, RequiresRole(Role, RoleActions.Add)]
        public ActionResult Create(CreateContractor model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(Role, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditContractor>(id);
        }

        [HttpPost, RequiresRole(Role, RoleActions.Edit)]
        public ActionResult Update(EditContractor model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(Role, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region ContractorContact

        [HttpPost, RequiresRole(Role, RoleActions.Edit)]
        public ActionResult CreateContractorContact(CreateContractorContact model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpDelete, RequiresRole(Role, RoleActions.Edit)]
        public ActionResult DestroyContractorContact(DestroyContractorContact model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion
    }
}