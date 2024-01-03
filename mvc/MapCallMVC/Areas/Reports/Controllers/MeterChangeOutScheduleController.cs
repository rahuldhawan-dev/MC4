using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Results;
using MMSINC.Utilities.Pdf;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class MeterChangeOutScheduleController : ControllerBaseWithPersistence<IMeterChangeOutRepository, MeterChangeOut, User>
    {
        #region Consts

        private const RoleModules ROLE = RoleModules.FieldServicesMeterChangeOuts;

        #endregion

        #region Constructor

        public MeterChangeOutScheduleController(ControllerBaseWithPersistenceArguments<IMeterChangeOutRepository, MeterChangeOut, User> args) : base(args) { }

        #endregion

        #region Actions

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index() // NOTE: There's no search parameters for this.
        {
            var results = Repository.GetScheduledReport();
            return this.RespondTo(x => {
                x.View(() => View("Index", results));
                x.Pdf(() => new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), "Pdf", results));
            });
        }

        #endregion
    }
}