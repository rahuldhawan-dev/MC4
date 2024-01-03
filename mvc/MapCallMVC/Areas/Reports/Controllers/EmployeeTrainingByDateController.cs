using System.ComponentModel;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("Employee Training By Date")]
    public class EmployeeTrainingByDateController : ControllerBaseWithPersistence<IEmployeeRepository, Employee, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsTrainingRecords;

        #endregion

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
            this.AddDropDownData<TrainingRequirement>();
            this.AddDropDownData<PositionGroupCommonName>();
        }

        #region Search

        public EmployeeTrainingByDateController(ControllerBaseWithPersistenceArguments<IEmployeeRepository, Employee, User> args) : base(args) { }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchEmployeeTrainingByDate>();
        }

        #endregion

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchEmployeeTrainingByDate search)
        {
            var args = new ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => Repository.GetTrainingByDate(search)
            };
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(search, args));
                f.Excel(() => ActionHelper.DoExcel(search, args));
            });
        }
    }
}