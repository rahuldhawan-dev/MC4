using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalNonComplianceEvents;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Environmental.Controllers
{
    public class EnvironmentalNonComplianceEventActionItemController : ControllerBaseWithPersistence<MapCall.Common.Model.Entities.EnvironmentalNonComplianceEventActionItem, User>
    {
        public const RoleModules ROLE = RoleModules.EnvironmentalGeneral;

        public EnvironmentalNonComplianceEventActionItemController(ControllerBaseWithPersistenceArguments<IRepository<MapCall.Common.Model.Entities.EnvironmentalNonComplianceEventActionItem>, MapCall.Common.Model.Entities.EnvironmentalNonComplianceEventActionItem, User> args) : base(args) { }

        #region Private Methods

        private void SendEnvironmentalNonComplianceActionItemAssignedNotification(EnvironmentalNonComplianceEvent model, EnvironmentalNonComplianceEventActionItem actionItem)
        {
            var notifier = _container.GetInstance<INotificationService>();

            notifier.Notify(new NotifierArgs {
                Module = ROLE,
                Purpose = EnvironmentalNonComplianceEventController.ENVIRONMENTAL_NON_COMPLIANCE_ACTION_ITEM_ASSIGNED_NOTIFICATION_PURPOSE,
                Address = actionItem.ResponsibleOwner.Email,
                Data = new EnvironmentalNonComplianceActionItemAssignedNotification {
                    AssignedToFullName = AuthenticationService.CurrentUser.FullName,
                    EnvironmentalNonComplianceEvent = model,
                    EnvironmentalNonComplianceEventActionItem = actionItem,
                    RecordUrl = GetUrlForModel(model, "Show", "EnvironmentalNonComplianceEventActionItem", "Environmental"),
                    HelpUrl = EnvironmentalNonComplianceEventController.ENVIRONMENTAL_NON_COMPLIANCE_ACTION_ITEM_ASSIGNED_NOTIFICATION_HELP_LINK
                }
            });
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchEnvironmentalNonComplianceEventActionItem search)
        {
            return ActionHelper.DoSearch(search);
        }
        
        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchEnvironmentalNonComplianceEventActionItem search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));

                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search).Select(e => new {
                        e.Id,
                        EnvironmentalNonComplianceEventId = e.EnvironmentalNonComplianceEvent.Id,
                        e.EnvironmentalNonComplianceEvent.State,
                        e.EnvironmentalNonComplianceEvent.OperatingCenter,
                        e.Type,
                        ResponsibleOwner = e.ResponsibleOwner?.FullName,
                        e.NotListedType,
                        e.ActionItem,
                        e.TargetedCompletionDate,
                        e.DateCompleted
                    });

                    return this.Excel(results);
                });
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditEnvironmentalNonComplianceEventActionItem>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditEnvironmentalNonComplianceEventActionItem model)
        {
            var repoModel = _container.GetInstance<IRepository<EnvironmentalNonComplianceEventActionItem>>();
            var initialModel = repoModel.Find(model.Id);
            int? initialResponsibleOwner = initialModel?.ResponsibleOwner?.Id;
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () =>
                {
                    var repoEvent = _container.GetInstance<IRepository<EnvironmentalNonComplianceEvent>>();
                    var modelEvent = repoEvent.Find(model.EnvironmentalNonComplianceEventId ?? 0);

                    if (initialResponsibleOwner != null && initialResponsibleOwner != model.ResponsibleOwner)
                    {
                        var repo = _container.GetInstance<IRepository<EnvironmentalNonComplianceEventActionItem>>();
                        var actionItem = repo.Find(model.Id);
                        SendEnvironmentalNonComplianceActionItemAssignedNotification(modelEvent, actionItem);
                    }
                    return RedirectToAction("Show", "EnvironmentalNonComplianceEvent",
                        new { area = "Environmental", id = model.EnvironmentalNonComplianceEventId });
                }
            });
        }

        #endregion
    }
}