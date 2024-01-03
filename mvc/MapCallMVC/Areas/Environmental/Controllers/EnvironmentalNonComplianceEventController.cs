using System;
using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Utility.Notifications;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalNonComplianceEvents;

namespace MapCallMVC.Areas.Environmental.Controllers
{
    public class EnvironmentalNonComplianceEventController : ControllerBaseWithPersistence<IEnvironmentalNonComplianceEventRepository, EnvironmentalNonComplianceEvent, User>
    {
        #region Consts

        public const RoleModules ROLE = RoleModules.EnvironmentalGeneral;
        public const string ENVIRONMENTAL_NON_COMPLIANCE_EVENT_CREATED_NOTIFICATION_PURPOSE = "Environmental NonCompliance Event Created";
        public const string ENVIRONMENTAL_NON_COMPLIANCE_ACTION_ITEM_ASSIGNED_NOTIFICATION_PURPOSE = "Environmental NonCompliance Action Item Assigned";
        public const string ENVIRONMENTAL_NON_COMPLIANCE_ACTION_ITEM_ASSIGNED_NOTIFICATION_HELP_LINK = "https://mapcall.awapps.com/Modules/mvc/HelpTopic/Show/271";

        #endregion

        #region Constructor

        public EnvironmentalNonComplianceEventController(ControllerBaseWithPersistenceArguments<IEnvironmentalNonComplianceEventRepository, EnvironmentalNonComplianceEvent, User> args) : base(args) { }

        #endregion

        #region Private Methods

        private void SendEnvironmentalNonComplianceEventCreatedNotification(EnvironmentalNonComplianceEvent model)
        {
            var notifier = _container.GetInstance<INotificationService>();

            notifier.Notify(new NotifierArgs
            {
                OperatingCenterId = model.OperatingCenter.Id,
                Module = ROLE,
                Purpose = ENVIRONMENTAL_NON_COMPLIANCE_EVENT_CREATED_NOTIFICATION_PURPOSE,
                Data = new EnvironmentalNonComplianceEventCreatedNotification
                {
                    CreatedByFullName = AuthenticationService.CurrentUser.FullName,
                    EnvironmentalNonComplianceEvent = model,
                    RecordUrl = GetUrlForModel(model, "Show", "EnvironmentalNonComplianceEvent", "Environmental")
                }
            });
        }

        private void SendEnvironmentalNonComplianceActionItemAssignedNotification(EnvironmentalNonComplianceEventActionItem model, EnvironmentalNonComplianceEvent modelEvent = null)
        {
            var notifier = _container.GetInstance<INotificationService>();

            if (modelEvent != null)
            {
                notifier.Notify(new NotifierArgs
                {
                    Module = ROLE,
                    Purpose = ENVIRONMENTAL_NON_COMPLIANCE_ACTION_ITEM_ASSIGNED_NOTIFICATION_PURPOSE,
                    Address = model.ResponsibleOwner.Email,
                    Data = new EnvironmentalNonComplianceActionItemAssignedNotification {
                        AssignedToFullName = model.ResponsibleOwner.FullName,
                        EnvironmentalNonComplianceEvent = modelEvent,
                        EnvironmentalNonComplianceEventActionItem = model,
                        RecordUrl = GetUrlForModel(modelEvent, "Show", "EnvironmentalNonComplianceEvent", "Environmental"),
                        HelpUrl = ENVIRONMENTAL_NON_COMPLIANCE_ACTION_ITEM_ASSIGNED_NOTIFICATION_HELP_LINK
                    }
                });
            }
            else
            {
                notifier.Notify(new NotifierArgs {
                    Module = ROLE,
                    Purpose = ENVIRONMENTAL_NON_COMPLIANCE_ACTION_ITEM_ASSIGNED_NOTIFICATION_PURPOSE,
                    Address = model.ResponsibleOwner.Email,
                    Data = new EnvironmentalNonComplianceActionItemAssignedNotification {
                        AssignedToFullName = model.ResponsibleOwner.FullName,
                        EnvironmentalNonComplianceEvent = null,
                        EnvironmentalNonComplianceEventActionItem = model,
                        RecordUrl = GetUrlForModel(model, "Show", "EnvironmentalNonComplianceEventActionItem", "Environmental"),
                        HelpUrl = ENVIRONMENTAL_NON_COMPLIANCE_ACTION_ITEM_ASSIGNED_NOTIFICATION_HELP_LINK
                    }
                });
            }
        }

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action == ControllerAction.Show)
            {
                this.AddDropDownData<EnvironmentalNonComplianceEventActionItemType>("Type");
            }
        }

        #region Search/Index/Show

        [HttpGet]
        [RequiresRole(ROLE)]
        public ActionResult Search(SearchEnvironmentalNonComplianceEvent search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet]
        [RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet]
        [RequiresRole(ROLE)]
        public ActionResult Index(SearchEnvironmentalNonComplianceEvent search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(ViewModelFactory.Build<CreateEnvironmentalNonComplianceEvent>());
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateEnvironmentalNonComplianceEvent model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () =>
                {
                    var entity = Repository.Find(model.Id);
                    SendEnvironmentalNonComplianceEventCreatedNotification(entity);
                    return RedirectToAction("Show", "EnvironmentalNonComplianceEvent",
                        new { area = "Environmental", id = model.Id });
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditEnvironmentalNonComplianceEvent>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditEnvironmentalNonComplianceEvent model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult AddEnvironmentalNonComplianceEventActionItem(CreateEnvironmentalNonComplianceEventActionItem model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs() {
                OnSuccess = () => {
                    var entity = Repository.Find(model.Id);
                    var entityActionItem = entity.ActionItems.OrderBy(x => x.Id).Last();
                    SendEnvironmentalNonComplianceActionItemAssignedNotification(entityActionItem, entity);
                    return RedirectToAction("Show", "EnvironmentalNonComplianceEvent",
                        new { area = "Environmental", id = entity.Id });
                }
            });
        }

        #endregion
    }
}
