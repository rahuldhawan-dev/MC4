using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Results;
using MMSINC.Utilities.Pdf;

namespace MapCallMVC.Controllers
{
    public class TrainingRecordAttendanceFormController : ControllerBaseWithPersistence<ITrainingRecordRepository, TrainingRecord, User>
    {
        #region Constants

        public const RoleModules ROLE = TrainingRecordController.ROLE_MODULE;

        #endregion

        #region New/Create

        // TODO: This should be Show/id.pdf, not Create. -Ross 3/20/2020
        [HttpGet, RequiresRole(ROLE)] // Add RoleAction not needed for pdf
        public ActionResult Create(int id, bool noPdf = false)
        {
            var entity = Repository.Find(id);

            if (entity == null)
            {
                return DoHttpNotFound("Not found.");
            }

            return this.RespondTo(x => x.Pdf(() => noPdf ? View("Pdf", entity) : new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), "Pdf", entity)));
        }

        #endregion

        public TrainingRecordAttendanceFormController(ControllerBaseWithPersistenceArguments<ITrainingRecordRepository, TrainingRecord, User> args) : base(args) {}
    }
}