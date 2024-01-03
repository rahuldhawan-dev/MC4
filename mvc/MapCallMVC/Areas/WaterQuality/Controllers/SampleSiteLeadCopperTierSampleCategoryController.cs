using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC;
using MMSINC.Controllers;
using System.Web.Mvc;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.WaterQuality.Controllers
{
    public class SampleSiteLeadCopperTierSampleCategoryController : ControllerBaseWithPersistence<IRepository<SampleSiteLeadCopperTierSampleCategory>, SampleSiteLeadCopperTierSampleCategory, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.WaterQualityGeneral;

        #endregion

        #region Constructors

        public SampleSiteLeadCopperTierSampleCategoryController(ControllerBaseWithPersistenceArguments<IRepository<SampleSiteLeadCopperTierSampleCategory>, SampleSiteLeadCopperTierSampleCategory, User> args) : base(args) { }

        #endregion  

        #region Cascading Endpoints

        [HttpGet]
        public ActionResult BySampleSiteLeadCopperTierClassification(int sampleSiteLeadCopperTierClassificationId)
        {
            return new CascadingActionResult<SampleSiteLeadCopperTierSampleCategory, SampleSiteLeadCopperTierSampleCategoryDisplayItem>(Repository.Where(x => x.TierClassifications.Select(y => y.Id).Contains(sampleSiteLeadCopperTierClassificationId)));
        }

        #endregion
    }
}
