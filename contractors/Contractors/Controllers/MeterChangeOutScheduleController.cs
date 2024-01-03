using System.Web.Mvc;
using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Results;
using MMSINC.Utilities.Pdf;

namespace Contractors.Controllers
{
    public class MeterChangeOutScheduleController : ControllerBaseWithValidation<IMeterChangeOutRepository, MeterChangeOut>
    {
        #region Constructor

        public MeterChangeOutScheduleController(ControllerBaseWithPersistenceArguments<IMeterChangeOutRepository, MeterChangeOut, ContractorUser> args) : base(args) { }

        #endregion

        #region Private Methods

        #endregion

        #region Actions

        [HttpGet]
        public ActionResult Index() // NOTE: There's no search parameters for this.
        {
            var results = Repository.GetScheduledReport();
            return this.RespondTo(x => {
                x.View(() => View("Index", results));
                x.Pdf(() =>
                    new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(),
                        "Pdf", results));
            });
        }

        #endregion
    }
}