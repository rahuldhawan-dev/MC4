using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class JobSiteSafetyAnalysisController : ControllerBaseWithPersistence<JobSiteSafetyAnalysis, User>
    {
        #region Constants

        public const RoleModules ROLE = JobSiteCheckListController.ROLE_MODULE;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddOperatingCenterDropDownData();
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchJobSiteSafetyAnalysis>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchJobSiteSafetyAnalysis search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                    RedirectSingleItemToShowView = false 
                }));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        public JobSiteSafetyAnalysisController(ControllerBaseWithPersistenceArguments<IRepository<JobSiteSafetyAnalysis>, JobSiteSafetyAnalysis, User> args) : base(args) {}
    }
}