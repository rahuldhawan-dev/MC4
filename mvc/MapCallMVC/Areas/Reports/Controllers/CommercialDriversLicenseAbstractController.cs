using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Models;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("Commercial Drivers License Abstract")]
    public class CommercialDriversLicenseAbstractController : ControllerBaseWithPersistence<IEmployeeRepository, Employee, User>
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
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
                    this.AddDropDownData<EmployeeStatus>("Status");
                    this.AddDynamicDropDownData<PositionGroup, PositionGroupDisplayItem>();
                    this.AddDropDownData<CommercialDriversLicenseProgramStatus>();
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchCommercialDriversLicenseAbstract search)
        {
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(search));
                f.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search)
                        .Select(e => new {
                            e.OperatingCenter,
                            e.CommercialDriversLicenseProgramStatus,
                            e.Status,
                            e.EmployeeId,
                            e.LastName,
                            e.FirstName,
                            e.DriversLicenseIssuedDate,
                            e.DriversLicense,
                            e.IsCDLCompliant
                        });
                    return this.Excel(results);
                });
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchCommercialDriversLicenseAbstract search)
        {
            return ActionHelper.DoSearch(search);
        }

        #endregion

        public CommercialDriversLicenseAbstractController(ControllerBaseWithPersistenceArguments<IEmployeeRepository, Employee, User> args) : base(args) {}
    }
}
