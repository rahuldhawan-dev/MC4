using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MapCall.Common.Utility.Notifications;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Results;
using MMSINC.Utilities.Pdf;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class WorkOrderInvoiceController : ControllerBaseWithPersistence<WorkOrderInvoice, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkOrderInvoice;

        public const string SUBMITTED_NOTIFICATION_PURPOSE = "Work Order Invoice Submitted", CANCELED_NOTIFICATION_PURPOSE = "Work Order Invoice Cancelled";

        public const string NOT_FOUND = "The work order invoice you are trying to access does not exist or you do not have access to it.",
            DOES_NOT_MATCH_WORKORDER = "The current invoice has a set of schedule of values that does not match the work order. Please consider deleting this invoice and recreating a new one from the work order.";
        
        #endregion

        #region Private Methods

        private void SendCreationsMostBodaciousNotification(WorkOrderInvoice model, string notificationPurpose)
        {
            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs {
                OperatingCenterId = model.WorkOrder?.OperatingCenter?.Id ?? 0,
                Module = ROLE,
                Purpose = notificationPurpose,
                Data = model
            };

            var pdfResult = new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), "Pdf",
                ViewModelFactory.Build<WorkOrderInvoicePdf, WorkOrderInvoice>(model));
            var pdf = pdfResult.RenderPdfToBytes(ControllerContext);

            args.AddAttachment($"Invoice{model.Id}.pdf", pdf);
            notifier.Notify(args);
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            this.AddDropDownData<WorkOrderInvoiceStatus>();
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchWorkOrderInvoice>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x => {
                x.View(() => {
                    return ActionHelper.DoShow(id, null, entity => {
                        if (!entity.ScheduleOfValuesMatchWorkOrderScheduleOfValues && !entity.SubmittedDate.HasValue)
                        {
                            DisplayNotification(DOES_NOT_MATCH_WORKORDER);
                        }
                        this.AddDropDownData<ScheduleOfValueCategory>(z => z.Where(y => y.ScheduleOfValueType == entity.ScheduleOfValueType));
                    });
                });
                x.Pdf(() => {
                    var model = ViewModelFactory.Build<WorkOrderInvoicePdf, WorkOrderInvoice>(Repository.Find(id));
                    if (model == null)
                        return HttpNotFound();
                    return new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), "Pdf", model);
                });
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchWorkOrderInvoice search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int? id)
        {
            var model = new CreateWorkOrderInvoice(_container);
            if (id.HasValue)
            {
                model.WorkOrder = id;
            }
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateWorkOrderInvoice model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditWorkOrderInvoice>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditWorkOrderInvoice model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    var entity = Repository.Find(model.Id);
                    if (model.SendSubmittedNotificationOnSave)
                        SendCreationsMostBodaciousNotification(entity, SUBMITTED_NOTIFICATION_PURPOSE);
                    if (model.SendCanceledNotificationOnSave)
                        SendCreationsMostBodaciousNotification(entity, CANCELED_NOTIFICATION_PURPOSE);

                    return null;
                }
            });
        }
		
        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region Add/Remove Schedule Of Values

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult AddScheduleOfValue(AddWorkOrderInvoiceScheduleOfValue model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpDelete, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult RemoveScheduleOfValue(RemoveWorkOrderInvoiceScheduleOfValue model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Constructors

        public WorkOrderInvoiceController(ControllerBaseWithPersistenceArguments<IRepository<WorkOrderInvoice>, WorkOrderInvoice, User> args) : base(args) {}

		#endregion
    }
}