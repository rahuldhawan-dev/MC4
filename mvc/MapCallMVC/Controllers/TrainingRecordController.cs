using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Metadata;
using MMSINC.Results;
using MMSINC.Utilities.Pdf;

namespace MapCallMVC.Controllers
{
    public class TrainingRecordController : ControllerBaseWithPersistence<ITrainingRecordRepository, TrainingRecord, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = RoleModules.OperationsTrainingRecords;
        public const string NOTIFICATION_PURPOSE = "Training Record";
        public const string TRAINING_SESSIONS_INCOMPLETE =
            "There are not enough training sessions assigned for the Total Hours required.";
        public const string CANCELED_NOTIFICATION = "CanceledTraining";
        public const string NOT_FOUND = "Training Record with the id '{0}' was not found.";
        public const string SUBJECT = "Canceled Training Course Notification";

        #endregion

        #region Private Methods

        private void SendCreationsMostBodaciousNotification(TrainingRecord model)
        {
            var notifier = _container.GetInstance<INotificationService>();
            model.RecordUrl = GetUrlForModel(model, "Show", "TrainingRecord");
            var args = new NotifierArgs {
                OperatingCenterId = model.ClassLocation.OperatingCenter.Id,
                Module = ROLE_MODULE,
                Purpose = NOTIFICATION_PURPOSE,
                Data = model
            };
            notifier.Notify(args);
        }

        /// <summary>
        /// What about the other notifications? Where are they getting generated from?????
        /// The employee tab creates a form that posts to Notification/Create, the values for that form 
        /// are created in our SecureFormDynamicValues table somehow.
        /// </summary>
        /// <param name="entity"></param>
        private void SendUpdateNotification(TrainingRecord entity)
        {
            entity.RecordUrl = GetUrlForModel(entity, "Show", "TrainingRecord");
            var notifier = _container.GetInstance<INotificationService>();
            foreach (var employee in entity.EmployeesScheduled)
            {
                if (!string.IsNullOrWhiteSpace(employee.Employee.EmailAddress))
                    notifier.Notify(new NotifierArgs {
                        OperatingCenterId = entity.OperatingCenterId,
                        Module = ROLE_MODULE,
                        Purpose = CANCELED_NOTIFICATION,
                        Data = entity,
                        Subject = SUBJECT,
                        Address = employee.Employee.EmailAddress
                    });
                if (employee.Employee.ReportsTo != null && !string.IsNullOrWhiteSpace(employee.Employee.ReportsTo.EmailAddress))
                    notifier.Notify(new NotifierArgs {
                        OperatingCenterId = entity.OperatingCenterId,
                        Module = ROLE_MODULE,
                        Purpose = CANCELED_NOTIFICATION,
                        Data = entity,
                        Subject = SUBJECT,
                        Address = employee.Employee.ReportsTo.EmailAddress
                    });
            }
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            Action allActions = () => {
                this.AddDynamicDropDownData<TrainingContactHoursProgramCoordinator,
                    TrainingContactHoursProgramCoordinatorDisplayItem>(x => x.Id, x => x.Display, "ProgramCoordinator");
                this.AddOperatingCenterDropDownData("InstructorOperatingCenter");
                this.AddOperatingCenterDropDownData("ClassLocationOperatingCenter");
                this.AddDynamicDropDownData<ClassLocation, ClassLocationDisplayItem>(x => x.Id, x => x.Display);
            };

            switch (action)
            {
                case ControllerAction.Search:
                    allActions();
                    this.AddDynamicDropDownData<TrainingModule, TrainingModuleDisplayItem>(dataGetter: x => x.GetAllSorted(y => y.Id));
                    this.AddDropDownData<TrainingRequirement>();
                    break;
                case ControllerAction.New:
                    allActions();
                    this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>("Instructor", r => r.GetActiveEmployeesForSelect());
                    this.AddDropDownData<IEmployeeRepository, Employee>("SecondInstructor", r => r.GetActiveEmployeesForSelect());
                    this.AddDynamicDropDownData<ITrainingModuleRepository, TrainingModule, TrainingModuleDisplayItem>(dataGetter: x => x.GetActiveTrainingModules());
                    break;
                case ControllerAction.Edit:
                    allActions();
                    this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>("Instructor");
                    this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>("SecondInstructor");
                    this.AddDynamicDropDownData<TrainingModule, TrainingModuleDisplayItem>(dataGetter: x => x.GetAllSorted(y => y.Id));
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Search(SearchTrainingRecord search)
        {
            return ActionHelper.DoSearch(search);
        }

        /// <summary>
        /// This violates the show/id format, so would
        /// TrainingRecordCertificate/Show. This this skips the restful crud method check.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Show(int id, int? employeeId = null)
        {
            return this.RespondTo(x => {

                x.View(() => {
                    var model = Repository.Find(id);
                    if (model == null)
                        return HttpNotFound();
                    
                    if (!model.HasEnoughTrainingSessionsHoursForTrainingModule)
                        DisplayNotification(TRAINING_SESSIONS_INCOMPLETE);

                    return View(model);
                });
                x.Pdf(() => {
                    var model = Repository.Find(id);

                    // if we're trying to print a single employee and they aren't valid, 404
                    if (employeeId != null)
                    {
                        var validEmployee = model.LinkedEmployeesEligibleForCertificates.Any(e => e.Employee.Id == employeeId.Value);
                        if (!validEmployee)
                            return HttpNotFound();
                        ViewData["employeeId"] = employeeId;
                    }
                    
                    if (model == null)
                        return HttpNotFound();
                    
                    return new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), "Pdf", model);
                });
            });
        }

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Index(SearchTrainingRecord search)
        {
            return this.RespondTo(formatter => {
                formatter.View(() => ActionHelper.DoIndex(search));

                // TODO bug 2489: This fragment isn't returning correctly when there are zero results.
                // Consider making DoIndex return the index with zero results for .frag requests.
                formatter.Fragment(() => ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs
                {
                    ViewName = "_AjaxIndexLimited",
                    IsPartial = true
                }));
                formatter.Excel(() => ActionHelper.DoExcel(search));
                formatter.Calendar(() => {
                    search.EnablePaging = false;
                    var data = Repository.Search(search)
                                         .SelectMany(r => {
                                              // not sure why this needs to happen here, this should
                                              // be handled by the interceptor
                                              _container.BuildUp(r);
                                              return r.TrainingSessions;
                                          });
                    return this.Calendar(data, s => s.ToCalendarItem(Url), true);
                });
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE_MODULE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateTrainingRecord(_container));
        }

        [HttpPost, RequiresRole(ROLE_MODULE, RoleActions.Add)]
        public ActionResult Create(CreateTrainingRecord model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    SendCreationsMostBodaciousNotification(Repository.Find(model.Id));
                    return null;
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE_MODULE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditTrainingRecord>(id);
        }

        [HttpPost, RequiresRole(ROLE_MODULE, RoleActions.Edit)]
        public ActionResult Update(EditTrainingRecord model)
        {
            // TODO: There shouldn't need to be three different calls to ActionHelper.DoUpdate.
            var original = Repository.Find(model.Id);
            if (original == null)
                return ActionHelper.DoUpdate(model);

            // TODO: Move this to the view model. MapToEntity should set a property on the view model
            // that says the value has changed on the entity.
            if (!original.Canceled.GetValueOrDefault() && model.Canceled)
            {
                return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs
                {
                    NotFound = string.Format(NOT_FOUND, model.Id),
                    OnSuccess = () => {
                        SendUpdateNotification(Repository.Find(model.Id));
                        return null; // defer to default
                    }
                });
            }
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE_MODULE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region Finalize

        [HttpPost, RequiresRole(ROLE_MODULE, RoleActions.Edit)]
        public ActionResult Finalize(FinalizeTrainingRecord model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region AddTrainingSession

        [HttpPost, RequiresRole(ROLE_MODULE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult AddTrainingSession(AddTrainingSession model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnError = () => RedirectToAction("Show", new { id = model.Id })
            });
        }

        #endregion

        #region RemoveTrainingSession

        [HttpDelete, RequiresRole(ROLE_MODULE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult RemoveTrainingSession(RemoveTrainingSession model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Constructors

        public TrainingRecordController(
            ControllerBaseWithPersistenceArguments<ITrainingRecordRepository, TrainingRecord, User> args) : base(args) {}

        #endregion

    }
}
