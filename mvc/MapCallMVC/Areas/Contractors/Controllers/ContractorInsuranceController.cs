using MMSINC.Controllers;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data.NHibernate;
using MapCall.Common.Metadata;
using MapCallMVC.Areas.Contractors.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Utilities;
using MMSINC;

namespace MapCallMVC.Areas.Contractors.Controllers
{
    public class ContractorInsuranceController : ControllerBaseWithPersistence<ContractorInsurance, User>
    {
        #region Constants

        public const RoleModules Role = RoleModules.ContractorsGeneral;

        #endregion

        #region Constructors

        public ContractorInsuranceController(ControllerBaseWithPersistenceArguments<IRepository<ContractorInsurance>, ContractorInsurance, User> args) : base(args) { }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(Role)]
        public ActionResult Search(SearchContractorInsurance search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(Role)]
        public ActionResult Index(SearchContractorInsurance search)
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
            return ActionHelper.DoNew(new CreateContractorInsurance(_container));
        }

        [HttpPost, RequiresRole(Role, RoleActions.Add)]
        public ActionResult Create(CreateContractorInsurance model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(Role, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditContractorInsurance>(id);
        }

        [HttpPost, RequiresRole(Role, RoleActions.Edit)]
        public ActionResult Update(EditContractorInsurance model)
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

        #region ByContractorId

        [HttpGet]
        public ActionResult ByContractorId(int contractorId)
        {
            return new CascadingActionResult(Repository.Where(ci => ci.Contractor.Id == contractorId), "PolicyNumber", "Id");
        }

        #endregion
    }
}