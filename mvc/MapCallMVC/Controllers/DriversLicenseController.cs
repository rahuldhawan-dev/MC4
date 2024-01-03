using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data;

namespace MapCallMVC.Controllers
{
    public class DriversLicenseController : ControllerBaseWithPersistence<IDriversLicenseRepository, DriversLicense, User>
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
                    this.AddDropDownData<DriversLicenseClass>();
                    this.AddDynamicDropDownData<DriversLicenseRestriction, DriversLicenseRestrictionDisplayItem>("Restrictions");
                    this.AddDynamicDropDownData<DriversLicenseEndorsement, DriversLicenseEndorsementDisplayItem>("Endorsements");
                    this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
                    this.AddDropDownData<EmployeeStatus>();
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchDriversLicense search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchDriversLicense search)
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
            return ActionHelper.DoNew(new CreateDriversLicense(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateDriversLicense model)
        {
            return ActionHelper.DoCreate(model);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Renew(int id)
        {
            var entity = Repository.Find(id);
            if (entity == null)
            {
                return HttpNotFound();
            }

            SetLookupData(ControllerAction.New);
            var model = ViewModelFactory.Build<CreateDriversLicense, DriversLicense>(entity);
            model.IssuedDate = null;
            model.RenewalDate = null;
            return View(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditDriversLicense>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditDriversLicense model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        public DriversLicenseController(ControllerBaseWithPersistenceArguments<IDriversLicenseRepository, DriversLicense, User> args) : base(args) {}
    }
}