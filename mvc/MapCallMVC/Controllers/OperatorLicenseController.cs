using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels.OperatorLicenses;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    [DisplayName("System Operator Licenses")]
    public class OperatorLicenseController
        : ControllerBaseWithPersistence<IOperatorLicenseRepository, OperatorLicense, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.HumanResourcesEmployeeLimited;

        #endregion

        #region Lookups

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action == ControllerAction.Show)
            {
                var publicWaterSupplies = ((IQueryable<PublicWaterSupplyDisplayItem>)_container
                       .GetInstance<IRepository<PublicWaterSupply>>()
                       .GetAllSorted()
                       .SelectDynamic<PublicWaterSupply, PublicWaterSupplyDisplayItem>()
                       .Result)
                   .ToList();

                this.AddDropDownData("PublicWaterSupply", publicWaterSupplies, x => x.Id, x => x.Display);

                var wasteWaterSystems = ((IQueryable<WasteWaterSystemDisplayItem>)_container
                       .GetInstance<IRepository<WasteWaterSystem>>()
                       .GetAllSorted()
                       .SelectDynamic<WasteWaterSystem, WasteWaterSystemDisplayItem>()
                       .Result)
                   .ToList();

                this.AddDropDownData("WasteWaterSystem", wasteWaterSystems, x => x.Id, x => x.Display);
            }
        }

        #endregion

        #region Constructors

        public OperatorLicenseController(ControllerBaseWithPersistenceArguments<IOperatorLicenseRepository, OperatorLicense, User> args) : base(args) { }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchOperatorLicense search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchOperatorLicense search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search).Select(x => new {
                        x.Id,
                        x.LicenseLevel,
                        x.LicenseSubLevel,
                        x.LicenseNumber,
                        x.ValidationDate,
                        x.ExpirationDate,
                        x.LicensedOperatorOfRecord,
                        OperatorOfRecord = string.Join(", ", x.PublicWaterSupplies.Select(y => y.PublicWaterSupply)),
                        x.OperatingCenter,
                        x.Employee,
                        x.Employee.EmployeeId,
                        x.State,
                        x.OperatorLicenseType, 
                        x.Expired
                    });
                    return this.Excel(results);
                });
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(_viewModelFactory.Build<CreateOperatorLicense>());
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateOperatorLicense model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<OperatorLicenseViewModel>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(OperatorLicenseViewModel model)
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
        
        #region Add/Remove WasteWaterSystems

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult AddWasteWaterSystem(AddOperatorLicenseWasteWaterSystem model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult RemoveWasteWaterSystem(RemoveOperatorLicenseWasteWaterSystem model)
        {
            return ActionHelper.DoUpdate(model);
        }
        
        #endregion
    }
}
