using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels.SampleSites;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Utilities.Pdf;

namespace MapCallMVC.Areas.WaterQuality.Controllers
{
    public class SampleSiteLeadCopperCertificationController : ControllerBaseWithPersistence<SampleSite, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.WaterQualityGeneral;

        #endregion

        #region Constructor

        public SampleSiteLeadCopperCertificationController(ControllerBaseWithPersistenceArguments<IRepository<SampleSite>, SampleSite, User> args) : base(args) { }

        #endregion

        #region New/Create

        // NOTE: This action only requires the Read role because it's only needed to help populate values for the Show action.
        [HttpGet, RequiresRole(ROLE)]
        public ActionResult New(int id)
        {
            var entity = Repository.Find(id);

            if (entity == null)
            {
                return DoHttpNotFound("Sample site not found.");
            }

            var plans = entity.SamplePlans.OrderBy(x => x.MonitoringPeriodTo);
            this.AddDropDownData<SamplePlan>(plans, x => x.Id, x => $"{x.MonitoringPeriodFrom.ToShortDateString()} - {x.MonitoringPeriodTo.ToShortDateString()}");

            return ActionHelper.DoNew(new CreateSampleSiteLeadCopperCertification(_container) { Id = id });
        }

        // NOTE: Bug 3359 requested that the PDF link should bypass the New form and directly show the pdf if the
        // the SampleSite only has one SamplePlan. There is no use for an HttpPost/Create method here as nothing's
        // being persisted to the database for this. 
        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(CreateSampleSiteLeadCopperCertification model)
        {
            var entity = Repository.Find(model.Id);

            if (entity == null)
            {
                return DoHttpNotFound("Sample site not found.");
            }

            ViewData["SamplePlan"] = _container.GetInstance<IRepository<SamplePlan>>().Find(model.SamplePlan.Value);
            ViewData["ServiceMaterials"] = _container.GetInstance<IRepository<ServiceMaterial>>().GetAll();

            return this.RespondTo(x =>
            {
                x.Pdf(() => new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), "Pdf", entity));
            });
        }

        #endregion
    }
}