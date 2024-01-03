using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using CreateLockoutForm = MapCallMVC.Models.ViewModels.CreateLockoutForm;

namespace MapCallMVC.Areas.HealthAndSafety.Controllers
{
    public class LockoutFormController : ControllerBaseWithPersistence<ILockoutFormRepository, LockoutForm, User>
    {
        #region Constants

        public const RoleModules ROLE = LockoutFormRepository.ROLE;
        public const string CREATE_NOTIFICATION_PURPOSE = "New Lockout Form",
                            UPDATE_NOTIFICATION_PURPOSE = "Updated Lockout Form";
        public const string SHEET_QUESTIONS_ANSWERS = "QuestionsAnswers";

        #endregion

        #region Private Methods

        // Notification
        private void SendCreationsMostBodaciousNotification(LockoutForm model)
        {
            model.RecordUrl = GetUrlForModel(model, "Show", "LockoutForm", "HealthAndSafety");
            if (model.ProductionWorkOrder != null)
            {
                model.RecordUrlWorkOrder =
                    GetUrlForModel(model.ProductionWorkOrder, "Show", "ProductionWorkOrder", "Production");
            }

            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs {
                OperatingCenterId = model.OperatingCenter.Id,
                Module = ROLE,
                Purpose = CREATE_NOTIFICATION_PURPOSE,
                Data = model
            };

            notifier.Notify(args);
        }

        private void SendUpdatesMostBodaciousNotification(LockoutForm model)
        {
            model.RecordUrl = GetUrlForModel(model, "Show", "LockoutForm", "HealthAndSafety");
            if (model.ProductionWorkOrder != null)
            {
                model.RecordUrlWorkOrder =
                    GetUrlForModel(model.ProductionWorkOrder, "Show", "ProductionWorkOrder", "Production");
            }
            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs
            {
                OperatingCenterId = model.OperatingCenter.Id,
                Module = ROLE,
                Purpose = UPDATE_NOTIFICATION_PURPOSE,
                Data = model
            };

            notifier.Notify(args);
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            Action allActions = () =>
            {
                var currentUser = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;

                this.AddDropDownData<LockoutReason>();
            };

            switch (action)
            {
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Add, extraFilterP: x => x.IsActive);
                    this.AddDropDownData<LockoutDeviceLocation>("IsolationPoint",
                        r => r.Where(l => l.IsActive), 
                        l => l.Id,
                        l => l.Description);
                    allActions();
                    break;
                case ControllerAction.Edit:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Edit);
                    this.AddDropDownData<LockoutDeviceLocation>("IsolationPoint");
                    allActions();
                    break;
                case ControllerAction.Search:
                    this.AddDropDownData<LockoutDeviceLocation>("IsolationPoint");
                    allActions();
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchLockoutForm search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x => {
                x.View(() => ActionHelper.DoShow(id));
                x.Fragment(() => ActionHelper.DoShow(id, new ActionHelperDoShowArgs {
                    IsPartial = true,
                    ViewName = "_ShowPopup"
                }));
                x.Map(() => {
                    var model = Repository.Find(id);
                    if (model == null)
                        return HttpNotFound();
                    return _container.With((IEnumerable<IThingWithCoordinate>)new [] {model}).GetInstance<MapResultWithCoordinates>();
                });
                x.Pdf(() => ActionHelper.DoPdf(id));
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchLockoutForm search)
        {
            return this.RespondTo((formatter) =>
            {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search);
                    
                    return this.Excel(results)
                               .AddSheet(results.SelectMany(z => z.LockoutFormAnswers.Select(y => new { y.LockoutForm.Id, y.LockoutFormQuestion.Question, y.Answer, y.Comments})),
                                    new MMSINC.Utilities.Excel.ExcelExportSheetArgs { SheetName = SHEET_QUESTIONS_ANSWERS });
                });

                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, search));
            });
        }

        #endregion

        #region New/Create

        [ActionBarVisible(false)]
        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateLockoutForm(_container));
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult NewFromProductionWorkOrder(int id)
        {
            var order = _container.GetInstance<IRepository<ProductionWorkOrder>>().Find(id);
            if (order == null)
            {
                return HttpNotFound($"Production Work Order with #{id} could not be found.");
            }

            var model = new CreateLockoutForm(_container);
            model.SetValuesFromProductionWorkOrder(order);
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateLockoutForm model)
        {
            // IF THE LOCKOUTANSWERS IS NULL HERE, YOU NEED TO ENSURE THAT ALL YOUR QUESTION/ANSWERS
            // ARE BEING LOADED ONTO THE FORM. YOU MUST HAVE A [0].
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    SendCreationsMostBodaciousNotification(Repository.Find(model.Id));
                    return null;
                },
                OnError = () => {
                    model.SetDefaults();
                    return null;
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditLockoutForm>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditLockoutForm model)
        {
            // TODO: Checking whether or not ReturnedToServiceDateTime was changed to a not-null
            // value should probably be done in the view model as a property, not here.
            var entity = Repository.Find(model.Id);
            var returnedToServiceWasNull = false;
            if (entity != null)
                returnedToServiceWasNull = entity.ReturnedToServiceDateTime == null;

            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    if (returnedToServiceWasNull && model.ReturnedToServiceDateTime != null)
                    {
                        entity = Repository.Find(model.Id);  // is this necessary?
                        SendUpdatesMostBodaciousNotification(entity);
                    }
                    return null;
                }
            });
        }

        #endregion

        #region Copy

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Copy(int id)
        {
            var form = Repository.Find(id);

            if (form == null)
            {
                return DoHttpNotFound($"Could not find form with id {id}.");
            }

            var model = _viewModelFactory.Build<CopyLockoutForm, LockoutForm>(form);
            return ActionHelper.DoNew((CreateLockoutForm)model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        public LockoutFormController(ControllerBaseWithPersistenceArguments<ILockoutFormRepository, LockoutForm, User> args) : base(args) {}
    }
}
