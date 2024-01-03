using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Models;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class CovidStatusReportController : ControllerBaseWithPersistence<ICovidIssueRepository, CovidIssue, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.HumanResourcesCovid;

        #endregion

        #region Constructors

        public CovidStatusReportController(ControllerBaseWithPersistenceArguments<ICovidIssueRepository, CovidIssue, User> args) : base(args) { }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchCovidStatusReport search)
        {
            return ActionHelper.DoSearch(search);
        }
        
        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchCovidStatusReport search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search)
                                            .Select(e => new {
                                                 e.Id,
                                                 e.Employee,
                                                 e.State,
                                                 e.Employee.EmployeeId,
                                                 e.OperatingCenter,
                                                 e.PersonnelArea,
                                                 e.SubmissionDate,
                                                 e.SubmissionStatus,
                                                 e.QuarantineStatus,
                                                 e.StartDate,
                                                 e.EstimatedReleaseDate,
                                                 e.ReleaseDate,
                                                 e.ReleaseReason,
                                                 e.RequestType,
                                                 e.TotalDays
                                             });
                    return this.Excel(results);
                });
            });
        }

        #endregion
    }
}
