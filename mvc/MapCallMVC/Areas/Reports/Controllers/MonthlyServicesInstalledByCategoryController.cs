using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services.Reports;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class MonthlyServicesInstalledByCategoryController : ControllerBaseWithPersistence<IServiceRepository, Service, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Constructors

        public MonthlyServicesInstalledByCategoryController(ControllerBaseWithPersistenceArguments<IServiceRepository, Service, User> args) : base(args) {}

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
            this.AddDropDownData("Year", Repository.GetDistinctYears().OrderByDescending(x => x), x => x, x => x);
        }

        #endregion

        #region Search/Index

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchMonthlyServicesInstalledByCategory search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchMonthlyServicesInstalledByCategory search)
        {
            return this.RespondTo((formatter) => {
                search.EnablePaging = false;
                var results = Repository.GetMonthlyServicesInstalledByCategoryReport(search);

                formatter.View(() => View("Index", results));
                formatter.Excel(() => this.Excel(results));
            });
        }

        #endregion
    }
}