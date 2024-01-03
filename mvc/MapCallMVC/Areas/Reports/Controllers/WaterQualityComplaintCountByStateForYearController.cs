using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class WaterQualityComplaintCountByStateForYearController : ControllerBaseWithPersistence<IWaterQualityComplaintRepository, WaterQualityComplaint, User>
    {
        public const RoleModules ROLE = RoleModules.WaterQualityGeneral;

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            ViewData["Year"] = Repository
                              .Where(wqc => wqc.DateComplaintReceived.HasValue)
                              .Select(wqc => wqc.DateComplaintReceived.Value.Year)
                              .Distinct()
                              .ToList()
                              .Select(year => new SelectListItem { Value = year.ToString(), Text = year.ToString() });
            ViewData["State"] = Repository
                               .Where(wqc => wqc.OperatingCenter != null && wqc.OperatingCenter.State != null)
                               .Select(wqc => wqc.OperatingCenter.State.Abbreviation)
                               .Distinct()
                               .ToList()
                               .Select(state => new SelectListItem { Value = state, Text = state });
            return View(new SearchWaterQualityComplaintByStateForYear());
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchWaterQualityComplaintByStateForYear search)
        {
            var args = new ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => {
                    search.EnablePaging = false;
                    var results = Repository.GetByStateForYearReport(search).ToList();
                    search.Count = results.Count;
                    search.Results = results;
                }
            };

            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(search, args));
                f.Excel(() => ActionHelper.DoExcel(search, args));
            });

        }

        public WaterQualityComplaintCountByStateForYearController(ControllerBaseWithPersistenceArguments<IWaterQualityComplaintRepository, WaterQualityComplaint, User> args) : base(args) { }
    }
}
