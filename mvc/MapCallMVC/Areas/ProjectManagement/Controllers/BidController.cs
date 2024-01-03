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
    public class BidController : ControllerBaseWithPersistence<IEstimatingProjectRepository, EstimatingProject, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesEstimatingProjects;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            if (action == ControllerAction.New)
            {
                this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>("Employee",
                    filter: e => e.OperatingCenter != null && e.PhoneWork != null && e.PhoneWork != "" && e.EmailAddress != null && e.EmailAddress != "");
            }
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE)] // This doesn't have the Add action because it's only used for generating a pdf.
        public ActionResult New(int id)
        {
            return ActionHelper.DoNew(new CreateBidForm(_container) {Id = id});
        }

        [HttpPost, RequiresRole(ROLE), RequiresSecureForm] // This doesn't have the Add action because it's only used for generating a pdf.
        public ActionResult Create(CreateBidForm model)
        {
            if (!ModelState.IsValid)
            {
                DisplayModelStateErrors();
                return ActionHelper.DoNew(model);
            }

            var entity = Repository.Find(model.Id);

            if (entity == null)
            {
                return DoHttpNotFound("Not found.");
            }

            var formModel = new BidForm(_container, model);
            formModel.Map(entity);

            return
                this.RespondTo(x =>
                    x.Pdf(() =>
                        model.NoPdf ?
                        View("Pdf", formModel) :
                        new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), "Pdf", formModel)));
        }

        #endregion

        public BidController(ControllerBaseWithPersistenceArguments<IEstimatingProjectRepository, EstimatingProject, User> args) : base(args) {}
    }
}