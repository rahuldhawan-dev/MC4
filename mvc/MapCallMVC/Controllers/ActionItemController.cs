using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Controllers
{
    public class ActionItemController : ControllerBaseWithPersistence<ActionItem, User>
    {
        #region Constants

        public const string FRAGMENT_IDENTIFIER = "#ActionItems";
        public const string EDIT_PARTIAL = "_EditForm";
        public string URLForRedirect = "";
        public const string BASE_URL_KEY = "BaseUrl";

        #endregion

        #region Private Methods
        
        private void SetActionItemTypesByTableName(string tableName)
        {
            if (!string.IsNullOrWhiteSpace(tableName))
            {
                this.AddDropDownData<IActionItemTypeRepository, ActionItemType>("Type", r => r.GetByTableName(tableName),
                    t => t.Id, t => t.Description); 
            }
            else
            {
                this.AddDropDownData<IActionItemTypeRepository, ActionItemType>("Type", r => r.GetDefaultList(),
                    t => t.Id, t => t.Description); 
            }
        }

        #endregion
        
        #region New/Create

        [HttpGet]
        public ActionResult New(NewActionItem model)
        {
            ModelState.Clear();
            SetActionItemTypesByTableName(model.TableName);
            return ActionHelper.DoNew(model);
        }

        [HttpPost]
        public ActionResult Create(NewActionItem model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    var actionItem = Repository.Find(model.Id);
                    SendActionItemCreateNotification(actionItem);
                    return RedirectToReferrerOr("Index", "Home", FRAGMENT_IDENTIFIER);
                },
                OnError = () => { return RedirectToReferrerOr("Index", "Home", FRAGMENT_IDENTIFIER);}
            });
        }

        [NonAction]
        // TODO: PLEASE DO NOT ADD NEW CASE STATEMENT HERE, THIS NEEDS TO BE REFACTORED - AARTI 10/28/2021
        public void SendActionItemCreateNotification(ActionItem actionItem)
        {
            switch (actionItem.DataType.TableName)
            {
                case NearMissMap.TABLE_NAME:
                    SendNearMissActionItemCreatedNotification(actionItem);
                    break;
            }
        }
        [NonAction]
        public void SendNearMissActionItemCreatedNotification(ActionItem actionItem)
        {
            var nearMiss = _container.GetInstance<INearMissRepository>()
                                     .Find(actionItem.LinkedId);
            nearMiss.RecordUrl = GetUrlForModel(nearMiss, "Show", "NearMiss", "HealthAndSafety");
            _container.GetInstance<INotificationService>()
                      .Notify(new NotifierArgs {
                           Module = RoleModules.OperationsHealthAndSafety,
                           Purpose = "Near Miss Action Item Created",
                           Data = nearMiss,
                           Address = actionItem.ResponsibleOwner.Email
                       });
        }

        #endregion

        #region Edit/Update

        [HttpGet]
        public ActionResult Edit(EditActionItem model)
        {
            ModelState.Clear();
            return ActionHelper.DoEdit(model.Id, new ActionHelperDoEditArgs<ActionItem, EditActionItem> { 
                InitializeViewModel = (x) => {
                    x.State = model.State;
                    x.ResponsibleOwner = model.ResponsibleOwner;
                    x.Type = model.Type;
                }
            }, onModelFound: item =>
            {
                SetActionItemTypesByTableName(item.DataType.TableName);
            });
        }

        [HttpPost]
        public ActionResult Update(EditActionItem model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => { return Redirect(model.Url); },
                OnError = () => { return Redirect(model.Url);}
            });
        }

        [HttpGet]
        public ActionResult CancelEdit(string url)
        {
            return Redirect(url);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id, new ActionHelperDoDestroyArgs {
                OnSuccess = () => { return RedirectToReferrerOr("Index", "Home", FRAGMENT_IDENTIFIER); },
                OnError = () => { return RedirectToReferrerOr("Index", "Home", FRAGMENT_IDENTIFIER); }
            });
        }

        #endregion
        

        public ActionItemController(ControllerBaseWithPersistenceArguments<IRepository<ActionItem>, ActionItem, User> args) : base(args) { }
    }
}