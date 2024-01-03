using System.ComponentModel;
using System.Linq;
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

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("Employee NJDEP License Report")]
    public class EmployeeNJDEPLicenseController : ControllerBaseWithPersistence<IEmployeeRepository, Employee, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.HumanResourcesEmployee;

        #endregion

        #region Constructors

        public EmployeeNJDEPLicenseController(ControllerBaseWithPersistenceArguments<IEmployeeRepository, Employee, User> args) : base(args) {}

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            //this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
            this.AddDropDownData<EmployeeDepartment>("Department");
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchEmployeeNJDEPLicense search = null)
        {
            return ActionHelper.DoSearch(search);
        }
        
        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchEmployeeNJDEPLicense search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search).Select(x => new {
                        x.OperatingCenter,
                        x.FullName,
                        x.Department,
                        Category = (x.CurrentPosition != null) ? x.CurrentPosition.Category : string.Empty,
                        x.LicenseWaterTreatment,
                        x.LicenseWaterDistribution,
                        x.LicenseSewerCollection,
                        x.LicenseSewerTreatment,
                        x.LicenseIndustrialDischarge
                    });
                    return this.Excel(results);
                });
            });
        }

        #endregion

    }
}