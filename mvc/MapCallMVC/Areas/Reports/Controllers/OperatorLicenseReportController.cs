using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels.OperatorLicenses;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("System Operator License Report")]
    public class OperatorLicenseReportController : ControllerBaseWithPersistence<IOperatorLicenseRepository, OperatorLicense, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.HumanResourcesEmployeeLimited;

        #endregion

        #region Constructors

        public OperatorLicenseReportController(ControllerBaseWithPersistenceArguments<IOperatorLicenseRepository, OperatorLicense, User> args) : base(args) { }

        #endregion

        #region Search/Index

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchOperatorLicenseReport search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchOperatorLicenseReport search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => Repository.SearchOperatorLicenseReport(search)
                }));

                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.SearchOperatorLicenseReport(search)
                                            .Select(e => new {
                                                 e.State, 
                                                 e.Employee,
                                                 e.Employee.EmployeeId,
                                                 e.OperatorLicenseType,
                                                 OperatorOfRecord = string.Join(", ", e.PublicWaterSupplies.Select(y => y.PublicWaterSupply)),
                                                 e.LicenseLevel,
                                                 e.LicenseSubLevel,
                                                 e.ExpirationDate,
                                                 e.LicensedOperatorOfRecord
                                             });
                    return this.Excel(results);
                });
            });
        }

        #endregion
    }
}
