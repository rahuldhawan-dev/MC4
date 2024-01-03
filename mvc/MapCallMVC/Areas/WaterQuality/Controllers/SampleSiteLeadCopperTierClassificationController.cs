using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC;
using MMSINC.Controllers;
using System.Linq;
using System.Web.Mvc;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.WaterQuality.Controllers
{
    public class SampleSiteLeadCopperTierClassificationController : ControllerBaseWithPersistence<IRepository<SampleSiteLeadCopperTierClassification>, SampleSiteLeadCopperTierClassification, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.WaterQualityGeneral;

        #endregion

        #region Constructors

        public SampleSiteLeadCopperTierClassificationController(ControllerBaseWithPersistenceArguments<IRepository<SampleSiteLeadCopperTierClassification>, SampleSiteLeadCopperTierClassification, User> args) : base(args) { }

        #endregion  

        #region Cascading Endpoints

        [HttpGet]
        public ActionResult ByState(int stateId)
        {
            return new CascadingActionResult(Repository.Where(x => x.States.Select(y => y.Id).Contains(stateId)), "Description", "Id");
        }

        #endregion
    }
}
