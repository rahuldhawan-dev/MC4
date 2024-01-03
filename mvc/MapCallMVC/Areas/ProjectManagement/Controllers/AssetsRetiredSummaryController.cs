using System;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Results;
using MMSINC.Utilities.Pdf;

namespace MapCallMVC.Areas.ProjectManagement.Controllers
{
    public class AssetsRetiredSummaryController : ControllerBaseWithPersistence<IEstimatingProjectRepository, EstimatingProject, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesEstimatingProjects;
 
        #endregion

        #region Search/Index/Show

        public AssetsRetiredSummaryController(ControllerBaseWithPersistenceArguments<IEstimatingProjectRepository, EstimatingProject, User> args) : base(args) {}

        [HttpGet, RequiresRole(ROLE)]
        public virtual ActionResult Show(int id)
        {
            var project = Repository.Find(id);

            if (project == null)
            {
                return DoHttpNotFound(String.Format("Estimating Project with id {0} not found.", id));
            }

            var model = ViewModelFactory.Build<AssetsRetiredSummary, EstimatingProject>(project);

            return this.RespondTo(x => {
                x.View(() => View("Show", model));

                x.Pdf(() => new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), "Show", model));
            });
        }

        #endregion
    }
}