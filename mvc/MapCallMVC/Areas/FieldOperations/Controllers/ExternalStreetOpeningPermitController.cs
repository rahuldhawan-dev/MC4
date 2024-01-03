using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.ExternalStreetOpeningPermits;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using Permits.Data.Client.Entities;
using Permits.Data.Client.Repositories;
using static MapCall.Common.Controllers.FileController;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    /// <summary>
    /// Controller for the creation of street opening permits via the Permits API.
    /// </summary>
    /// <description>
    ///
    /// HUGE NOTE:
    /// ActionHelper is of no use in this controller, nor is ViewModel. ActionHelper doesn't
    /// allow for swapping out all the repository stuff, which would make it a little
    /// easier to get all the Permits API interactions into the ActionHelper flow. But since
    /// we can't do that without a ton of work, we need to bypass it entirely.
    ///
    /// </description>
    public class ExternalStreetOpeningPermitController : ControllerBaseWithPersistence<StreetOpeningPermit, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        #endregion

        #region Fields

        private readonly IPermitsRepositoryFactory _permitsRepositoryFactory;

        #endregion

        #region Constructors

        public ExternalStreetOpeningPermitController(
            ControllerBaseWithPersistenceArguments<IRepository<StreetOpeningPermit>, StreetOpeningPermit, User> args,
            IPermitsRepositoryFactory permitsRepositoryFactory)
            : base(args)
        {
            _permitsRepositoryFactory = permitsRepositoryFactory;
        }

        #endregion

        #region Private Methods

        private WorkOrder TryGetWorkOrder(int workOrderId)
        {
            return _container.GetInstance<IWorkOrderRepository>().Find(workOrderId);
        }

        private void SaveAndUpdateMapCallPermitWithApiData(StreetOpeningPermit mapcallPermit,
            Permits.Data.Client.Entities.Permit apiPermit)
        {
            mapcallPermit.HasMetDrawingRequirement = apiPermit.HasMetDrawingRequirement;
            mapcallPermit.IsPaidFor = apiPermit.IsPaidFor;

            Repository.Save(mapcallPermit);
        }

        private static string GetPermitUserName(WorkOrder workOrder)
        {
            // All other Permits API usage uses the user's default operating center to get
            // the PermitsUserName. For actually creating permits for a work order, the user
            // submitting it may be accessing a work order in an operating center that's not
            // the same as their default operating center, but they're otherwise allowed to
            // access that work order via roles. This is the only time where we'd use
            // the work order's operating center to grab the PermitsUserName value instead.
            return workOrder.OperatingCenter.PermitsUserName;
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult New(int workOrderId)
        {
            var workOrder = TryGetWorkOrder(workOrderId);

            if (workOrder == null)
            {
                return HttpNotFound();
            }

            var model = new NewExternalStreetOpeningPermitFormOptions();
            model.WorkOrder = workOrder;

            var permitsUserName = GetPermitUserName(workOrder);

            if (string.IsNullOrWhiteSpace(permitsUserName))
            {
                return View("MissingPermitsUserName");
            }

            var stateRepo = _permitsRepositoryFactory.GetRepository<Permits.Data.Client.Entities.State>(permitsUserName);
            var countyRepo = _permitsRepositoryFactory.GetRepository<Permits.Data.Client.Entities.County>(permitsUserName);
            var municipalityRepo = _permitsRepositoryFactory.GetRepository<Permits.Data.Client.Entities.Municipality>(permitsUserName);

            // NOTE: This is the only way to check if there's permit forms available for
            // a state/county/town. Also, it can be slow to respond due to the slowness of the permits QA site.
            // We have zero control over that.

            // NOTE: Is this(or any of the Permits repo methods) returning null and you can't figure out why?
            // See if the repo's HttpClient.IsSiteRunning is false. If it's false, it doesn't error 
            // and just returns null instead.

            // NOTE: No, there's no explanation for why we only use the first result that comes back for
            // each repository method.

            // NOTE: Are you getting a 412 error(or some other non-descript error) back from the server?
            //    - Is the permits username a valid account on permits.mapcall.info?
            //    - Is the test user part of the "New Jersey American Water" company? Our logins will only work for that company.
            //    - Does the account have a payment profile setup on the permits site? 
            //    - Is the test credit card expired?
            
            // NOTE: Are you getting a 404 Not found error? That's what happens when the permits API doesn't
            // have any results. Why it returns a 404 instead of an empty result set is beyond me.
            
            // TODO: This NameValueCollection stuff is awful. Can this be updated to use anonymous objects?
            var states = stateRepo.Search(new NameValueCollection { { "name", workOrder.State.Name } });
            if (states == null)
            {
                // There's no point in doing this check for Counties and Municipalities as well. The
                // odds that the api goes down betwen this request and the other two are slim to none.
                //
                // This check isn't being done in most the other places because they either spit out 
                // an error html string from the permits api, or we get a real exception to throw.
                model.UnableToConnectToApi = true;
            }
            else
            {
                var state = states?.FirstOrDefault();
                if (state != null)
                {
                    if (state.FormId > 0)
                    {
                        model.PermitsApiStateFormId = state.FormId;
                    }

                    var counties = countyRepo.Search(new NameValueCollection { { "name", workOrder.Town.County.Name }, { "stateId", state.Id.ToString() } });
                    var county = counties?.FirstOrDefault();
                    if (county != null)
                    {
                        if (county.FormId > 0)
                        {
                            model.PermitsApiCountyFormId = county.FormId;
                        }

                        var municipalities = municipalityRepo.Search(new NameValueCollection { { "name", workOrder.Town.ShortName }, { "countyId", county.Id.ToString() } });
                        var municipality = municipalities?.FirstOrDefault();
                        if (municipality != null)
                        {
                            if (municipality.FormId > 0)
                            {
                                model.PermitsApiMunicipalityFormId = municipality.FormId;
                            }
                        }
                    }
                }
            }

            // NOTE: ActionHelper will be of no use here.
            return View("New", model);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult NewForPermitForm(NewExternalStreetOpeningPermitForm model)
        {
            if (!ModelState.IsValid)
            {
                return View("ValidationFailures");
            }

            // No need for null check, handled by model validation already.
            var workOrder = TryGetWorkOrder(model.NoApiConflictWorkOrderId.Value);
            model.WorkOrder = workOrder;

            // This returns an html string for the form. Neat.
            // But if there's an error from the server, we'll have no idea.
            var permit = _permitsRepositoryFactory.GetRepository<Permit>(GetPermitUserName(workOrder))
                                                  .New(new NameValueCollection { { "formId", model.FormId.Value.ToString() } });

            model.PermitFormHtml = permit;

            return View("NewForPermitForm", model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Create(NewExternalStreetOpeningPermitForm model)
        {
            if (!ModelState.IsValid)
            {
                return View("ValidationFailures");
            }

            var workOrder = _container.GetInstance<IWorkOrderRepository>().Find(model.NoApiConflictWorkOrderId.Value);

            // NOTE: model.FormId happens to bind to the model because it's sent to us as part of the
            // html string that PermitRepository.New gives us.

            // This returns an html string for the form. Neat.
            var permitRepo = _permitsRepositoryFactory.GetRepository<Permits.Data.Client.Entities.Permit>(GetPermitUserName(workOrder));

            // It's not possible to use a view model with the forms we get/post with the Permits API.
            // They're all dynamic, so we are forced to use the raw postdata for the request. 

            // NOTE: 271 does not handle API errors, so we aren't either for now. As far as I can tell
            // the repo's internal GetResultOrThrow will properly throw an error here.
            var apiPermit = permitRepo.Save(Request.Form);

            // There's no way to do this via the normal ViewModel/ActionHelper route.
            var mapcallPermit = new StreetOpeningPermit {
                WorkOrder = workOrder,
                DateRequested = _container.GetInstance<IDateTimeProvider>().GetCurrentDate(),
                PermitId = apiPermit.Id,
                // This is a non-nullable value, but we don't use it for permits from the permits API for some reason.
                StreetOpeningPermitNumber = string.Empty
            };
            SaveAndUpdateMapCallPermitWithApiData(mapcallPermit, apiPermit);

            if (!apiPermit.HasMetDrawingRequirement)
            {
                return RedirectToAction("EditDrawings", new { id = mapcallPermit.Id });
            }
            else
            {
                return RedirectToAction("BeginPayment", new { id = mapcallPermit.Id });
            }
        }

        #endregion

        #region Drawings

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult EditDrawings(int id)
        {
            var mapcallPermit = Repository.Find(id);
            if (mapcallPermit == null)
            {
                return HttpNotFound();
            }

            var model = new ExternalStreetOpeningPermitUploadDrawingViewModel();
            model.StreetOpeningPermit = id;
            return View("EditDrawings", model);
        }

        // Secure forms are disabled for file uploads. The control in the view
        // is doing all of the work and there's no form tag involved.
        [RequiresSecureForm(false)]
        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult UploadDrawing(UploadModel uploadModel, int permitId)
        {
            var mapcallPermit = Repository.Find(permitId);
            if (mapcallPermit == null)
            {
                return HttpNotFound();
            }

            var permitDrawingRepo = _permitsRepositoryFactory.GetRepository<Drawing>(GetPermitUserName(mapcallPermit.WorkOrder));
            var drawing = permitDrawingRepo.Save(new Drawing {
                FileUpload = new Permits.Data.Client.Entities.AjaxFileUpload {
                    BinaryData = uploadModel.FileUpload.BinaryData,
                    FileName = uploadModel.FileUpload.FileName
                },
                PermitId = mapcallPermit.PermitId.Value
            });

            if (drawing == null)
            {
                // This should never happen, but I don't know why
                // a drawing would ever come back null. 271 only ever used "drawing != null"
                // to indicate success to the file uploader.
                return Json(new {
                    success = false,
                });
            }

            // In 271, the Drawings.asmx service would upload the file and that's it.
            // That would happen automatically without any extra interactions. 
            // The user would then need to click a button to say they were finished
            // uploading the drawing. It's at that point that the MapCall SOP record
            // would be updated. I don't really see a point in doing it that way in MVC.
            // We can just update the permit each time a drawing's uploaded so that the 
            // MapCall record stays in sync.

            var permitRepo = _permitsRepositoryFactory.GetRepository<Permit>(GetPermitUserName(mapcallPermit.WorkOrder));
            var apiPermit = permitRepo.Find(mapcallPermit.PermitId.Value);
            SaveAndUpdateMapCallPermitWithApiData(mapcallPermit, apiPermit);

            return Json(new {
                // This is needed for the drawing process.
                hasMetDrawingRequirement = mapcallPermit.HasMetDrawingRequirement,

                // This is needed for the file upload control.
                success = true,

                // Key is useless here. It's only needed for the file service if the file upload
                // control is being used with FileController. The js is expecting a value back
                // though so we want to give it something. The value is also useless for the client.
                key = Guid.NewGuid(),
                fileName = uploadModel.FileUpload.FileName
            });
        }

        #endregion

        #region Payment

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult BeginPayment(int id)
        {
            var mapcallPermit = Repository.Find(id);
            if (mapcallPermit == null)
            {
                return HttpNotFound();
            }

            if (mapcallPermit.IsPaidFor == true)
            {
                return View("AlreadyPaid");
            }

            // The permits API is not validating whether or not the user met the drawing requirement
            // when getting to the payment step. So we need to do that ourselves.
            if (mapcallPermit.HasMetDrawingRequirement != true)
            {
                return RedirectToAction("EditDrawings", new { id = id });
            }

            var model = new ExternalStreetOpeningPermitBeginPayment();
            model.Id = id;

            var permitPaymentRepo = _permitsRepositoryFactory.GetRepository<Payment>(GetPermitUserName(mapcallPermit.WorkOrder));

            // The repo does not properly throw an exception when the permits API throws an error.
            // Instead, we get a bunch of html back for a YSOD.
            model.PaymentFormHtml = permitPaymentRepo.New(new NameValueCollection {
                { "permitId", mapcallPermit.PermitId.ToString() }
            });

            return View("BeginPayment", model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult CompletePayment(int id)
        {
            var mapcallPermit = Repository.Find(id);
            if (mapcallPermit == null)
            {
                return HttpNotFound();
            }
            
            if (mapcallPermit.IsPaidFor == true)
            {
                return View("AlreadyPaid");
            }

            // The permits API is not validating whether or not the user met the drawing requirement
            // when getting to the payment step. So we need to do that ourselves.
            if (mapcallPermit.HasMetDrawingRequirement != true)
            {
                return RedirectToAction("EditDrawings", new { id = id });
            }

            var permitPaymentRepo = _permitsRepositoryFactory.GetRepository<Payment>(GetPermitUserName(mapcallPermit.WorkOrder));

            permitPaymentRepo.Save(new NameValueCollection {
                { "permitId", mapcallPermit.PermitId.ToString() }
            });

            var permitRepo = _permitsRepositoryFactory.GetRepository<Permit>(GetPermitUserName(mapcallPermit.WorkOrder));
            var apiPermit = permitRepo.Find(mapcallPermit.PermitId.Value);
            SaveAndUpdateMapCallPermitWithApiData(mapcallPermit, apiPermit);

            return View("PaymentComplete");
        }

        #endregion
    }
}
