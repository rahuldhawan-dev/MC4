using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Controllers;
using System.Web.Mvc;
using MMSINC.Data.NHibernate;
using MapCall.Common.Metadata;
using MapCallMVC.Areas.Contractors.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Contractors.Controllers
{
    public class ContractorAgreementController : ControllerBaseWithPersistence<ContractorAgreement, User>
    {
        #region Constants

        public const RoleModules Role = RoleModules.ContractorsAgreements;

        #endregion

        #region Constructors

        public ContractorAgreementController(ControllerBaseWithPersistenceArguments<IRepository<ContractorAgreement>, ContractorAgreement, User> args) : base(args) { }

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            if (action == ControllerAction.New)
            {
                this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>("OperatingCenters", filter: x => x.IsActive);
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(Role)]
        public ActionResult Search(SearchContractorAgreement search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(Role)]
        public ActionResult Index(SearchContractorAgreement search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Fragment(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    IsPartial = true,
                    ViewName = "_Index",
                    OnNoResults = () => PartialView("_NoResults")
                }));
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
            return ActionHelper.DoNew(new CreateContractorAgreement(_container));
        }

        [HttpPost, RequiresRole(Role, RoleActions.Add)]
        public ActionResult Create(CreateContractorAgreement model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(Role, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditContractorAgreement>(id);
        }

        [HttpPost, RequiresRole(Role, RoleActions.Edit)]
        public ActionResult Update(EditContractorAgreement model)
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
    }
}