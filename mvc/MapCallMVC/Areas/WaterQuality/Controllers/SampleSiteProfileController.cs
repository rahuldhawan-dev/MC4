using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Controllers;
using System.Web.Mvc;
using MMSINC.Data.NHibernate;
using MMSINC;

namespace MapCallMVC.Areas.WaterQuality.Controllers
{
    public class SampleSiteProfileController : ControllerBaseWithPersistence<IRepository<SampleSiteProfile>, SampleSiteProfile, User>
    {
        public SampleSiteProfileController(ControllerBaseWithPersistenceArguments<IRepository<SampleSiteProfile>, SampleSiteProfile, User> args) :
            base(args) { }

        [HttpGet]
        public ActionResult ByPublicWaterSupply(int publicWaterSupplyId)
        {
            return new CascadingActionResult<SampleSiteProfile, SampleSiteProfileDisplayItem>(
                Repository.Where(x => x.PublicWaterSupply.Id == publicWaterSupplyId)
                          .OrderBy(x => x.Number));
        }
    }
}