using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.ProjectManagement.Controllers
{
    public class CompanyLaborCostController : ControllerBaseWithPersistence<IRepository<CompanyLaborCost>, CompanyLaborCost, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesEstimatingProjects;

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchCompanyLaborCost search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchCompanyLaborCost search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateCompanyLaborCost(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add), RequiresSecureForm]
        public ActionResult Create(CreateCompanyLaborCost model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditCompanyLaborCost>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult Update(EditCompanyLaborCost model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        public CompanyLaborCostController(ControllerBaseWithPersistenceArguments<IRepository<CompanyLaborCost>, CompanyLaborCost, User> args) : base(args) {}
    }
}
