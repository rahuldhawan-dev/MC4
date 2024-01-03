using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Controllers
{
    public class HepatitisBVaccinationController : ControllerBaseWithPersistence<IHepatitisBVaccinationRepository, HepatitisBVaccination, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.HumanResourcesEmployeeLimited;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.New:
                case ControllerAction.Edit:
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
                    this.AddDropDownData<HepatitisBVaccineStatus>();
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchHepatitisBVaccination search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchHepatitisBVaccination search)
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
            return ActionHelper.DoNew(new CreateHepatitisBVaccination(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateHepatitisBVaccination model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditHepatitisBVaccination>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditHepatitisBVaccination model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        public HepatitisBVaccinationController(ControllerBaseWithPersistenceArguments<IHepatitisBVaccinationRepository, HepatitisBVaccination, User> args) : base(args) {}
    }
}
