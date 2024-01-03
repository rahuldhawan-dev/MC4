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
    public class BactiSamplesChlorineHighLowController : ControllerBaseWithPersistence<IBacterialWaterSampleRepository, BacterialWaterSample, User>
    {
        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDynamicDropDownData<PublicWaterSupply, PublicWaterSupplyDisplayItem>();
                    this.AddDropDownData<ITownRepository, Town>("Town", t => t.GetAllSorted(), t => t.Id, t => t.ShortName);
                    break;
            }
            base.SetLookupData(action);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(RoleModules.WaterQualityGeneral)]
        public ActionResult Search(SearchBactiSamplesChlorineHighLow search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(RoleModules.WaterQualityGeneral)]
        public ActionResult Index(SearchBactiSamplesChlorineHighLow search)
        {
            // TODO: ActionHelper doesn't know how to deal with searches that have a 
            // result type different from the search type.
            var result = Repository.SearchBactiSamplesChlorineHighLowReport(search);
            return this.RespondTo(f =>
            {
                f.View(() => View(result));
                f.Excel(() => this.Excel(result));
            });
        }

        #endregion

        public BactiSamplesChlorineHighLowController(ControllerBaseWithPersistenceArguments<IBacterialWaterSampleRepository, BacterialWaterSample, User> args) : base(args) { }
    }
}
