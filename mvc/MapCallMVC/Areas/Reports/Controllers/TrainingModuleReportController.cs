using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("Training Requirements")]
    public class TrainingModuleReportController : ControllerBaseWithPersistence<IRepository<TrainingModule>, TrainingModule, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = RoleModules.OperationsTrainingModules;

        #endregion

        public TrainingModuleReportController(ControllerBaseWithPersistenceArguments<IRepository<TrainingModule>, TrainingModule, User> args) : base(args) {}

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Index(SearchTrainingModule search)
        {
            return this.RespondTo((formatter) =>
            {
                formatter.View(() => ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                    RedirectSingleItemToShowView = false
                }));
                formatter.Excel(() =>
                {
                    search.EnablePaging = false;
                    var results = Repository.Search(search);
                    return this.Excel(results.Select(tm => new {
                        tm.IsActive, 
                        tm.TrainingModuleCategory,
                        tm.TrainingRequirement,
                        TrainingRequirementID = (tm.TrainingRequirement != null) ? (int?)tm.TrainingRequirement.Id : null,
                        tm.Title,
                        TrainingModuleID = tm.Id,
                        tm.IsActiveInitialTrainingModule,
                        tm.IsActiveRecurringTrainingModule,
                        tm.IsActiveInitialAndRecurringTrainingModule
                    }));
                });
            });
        }
    }
}
