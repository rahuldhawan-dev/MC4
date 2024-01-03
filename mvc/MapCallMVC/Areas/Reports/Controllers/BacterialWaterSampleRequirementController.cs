using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Models;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class BacterialWaterSampleRequirementController : ControllerBaseWithPersistence<IBacterialWaterSampleRepository, BacterialWaterSample, User>
    {
        #region Constructors

        public BacterialWaterSampleRequirementController(ControllerBaseWithPersistenceArguments<IBacterialWaterSampleRepository, BacterialWaterSample, User> args) : base(args) {}

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
            this.AddDropDownData<BacterialSampleType>();
            this.AddDropDownData("Year", _container.GetInstance<IBacterialWaterSampleRepository>().GetDistinctYears().OrderByDescending(x => x), x => x, x => x);
        }

        #endregion

        [HttpGet, RequiresRole(RoleModules.WaterQualityGeneral)]
        public ActionResult Search(SearchBacterialWaterSampleRequirement search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(RoleModules.WaterQualityGeneral)]
        public ActionResult Index(SearchBacterialWaterSampleRequirement search)
        {
            // ActionHelper doesn't currently support search results with different types than the generic param of ISearchSet.
            return this.RespondTo(f => {
                search.EnablePaging = false;
                var results = Repository.GetBacterialWaterSampleRequirementsReport(search);
                f.View(() => View("Index", results));
                f.Excel(() => this.Excel(results));
            });
        }
    }
}