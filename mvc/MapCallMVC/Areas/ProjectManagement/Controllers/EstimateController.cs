using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Metadata;
using MMSINC.Results;
using MMSINC.Utilities.Pdf;

namespace MapCallMVC.Areas.ProjectManagement.Controllers
{
    public class EstimateController : ControllerBaseWithPersistence<IEstimatingProjectRepository, EstimatingProject, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesEstimatingProjects;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            this.AddDropDownData<IUserRepository, User>("AssignedTo", e => e.Where(x => x.HasAccess).OrderBy(x => x.FullName), x => x.FullName, x => x.FullName);
        }

        #endregion


        #region New/Create

        // NOTE: This action only requires the Read role because it's part of Show.
        [HttpGet, RequiresRole(ROLE)]
        public ActionResult New(int id)
        {
            return ActionHelper.DoNew(new CreateEstimateForm(_container) {Id = id});
        }

        // NOTE: This action only requires the Read role because it's part of Show.
        [HttpPost, RequiresRole(ROLE), RequiresSecureForm] // It really does not require a secure form for any reason.
        public ActionResult Create(CreateEstimateForm model)
        {
            // Note: don't use EstimateForm as the model. The model binder makes a mess of things.

            var entity = Repository.Find(model.Id);

            if (entity == null)
            {
                return DoHttpNotFound("Not found.");
            }

            var formModel = ViewModelFactory.Build<EstimateForm, EstimatingProject>(entity);
            formModel.Throwaways = model;

            return this.RespondTo(x =>
            {
                x.Pdf(() => new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), "Pdf", formModel));
            });
        }

        #endregion

        public EstimateController(ControllerBaseWithPersistenceArguments<IEstimatingProjectRepository, EstimatingProject, User> args) : base(args) {}
    }
}