using System;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Metadata;
using MMSINC.Results;
using MMSINC.Utilities.Pdf;

namespace MapCallMVC.Areas.ProjectManagement.Controllers
{
    public class TaskOrderGeneratorController : ControllerBaseWithPersistence<IEstimatingProjectRepository, EstimatingProject, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesEstimatingProjects;
        public const string MODEL_TEMP_DATA_KEY = "MODEL";

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public virtual ActionResult Show(int id)
        {
            var model = (TaskOrderGeneratorForm)TempData[MODEL_TEMP_DATA_KEY];

            if (model == null)
            {
                return DoHttpNotFound("Model not found in TempData; visit Create first.");
            }

            var project = Repository.Find(id);

            if (project == null)
            {
                return DoHttpNotFound(String.Format("Estimating Project with id {0} not found.", id));
            }

            model.Map(project);

            return this.RespondTo(x => {
                x.View(() => View("Show", model));
                x.Pdf(() => new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), "Show", model));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE)] // Read action is fine, this is just a pdf.
        public virtual ActionResult New(TaskOrderGeneratorForm model)
        {
            var id = model.Id;
            var project = Repository.Find(id);
            
            if (project == null)
            {
                return DoHttpNotFound(String.Format("Estimating Project with id {0} not found.", id));
            }

            return View(ViewModelFactory.Build<TaskOrderGeneratorForm, EstimatingProject>(project));
        }

        [HttpPost, RequiresSecureForm, RequiresRole(ROLE)] // Read action is fine, this is just a pdf. Also really doesn't need to require secure form.
        public ActionResult Create(TaskOrderGeneratorForm model)
        {
            // TODO: Just return the pdf immediately instead of storing this in TempData. -Ross 1/28/2020
            TempData[MODEL_TEMP_DATA_KEY] = model;
            return RedirectToAction("Show", new {id = model.Id, ext = "pdf"});
        }

        #endregion

        public TaskOrderGeneratorController(ControllerBaseWithPersistenceArguments<IEstimatingProjectRepository, EstimatingProject, User> args) : base(args) {}
    }
}
