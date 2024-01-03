using System.ComponentModel;
using System.Linq;
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
    [DisplayName("Required Employee Training")]
    public class EmployeeTrainingController : ControllerBaseWithPersistence<IEmployeeRepository, Employee, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsTrainingRecords;

        #endregion

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            this.AddOperatingCenterDropDownDataForRoleAndAction(RoleModules.OperationsTrainingRecords);
            this.AddDropDownData<TrainingRequirement>();
            this.AddDropDownData<PositionGroupCommonName>();
        }

        #region Search

        public EmployeeTrainingController(ControllerBaseWithPersistenceArguments<IEmployeeRepository, Employee, User> args) : base(args) {}

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchEmployeeTraining search)
        {
            return ActionHelper.DoSearch(search);
        }

        #endregion

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchEmployeeTraining search)
        {
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => Repository.GetRecentTraining(search)
                }));
                // If there are no results for a fragment, we dont' want to return the search which is
                // what DoIndex does.
                f.Fragment(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    ViewName = "_Index",
                    IsPartial = true,
                    SearchOverrideCallback = () => Repository.GetRecentTraining(search),
                    OnNoResults = () => PartialView("_NoResults")
                }));
                f.Excel(() => ActionHelper.DoExcel(search, new ActionHelperDoIndexArgs
                {
                    SearchOverrideCallback = () => Repository.GetRecentTraining(search)
                }));
            });
        }
    }
}