using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.ServiceLineProtection.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.ServiceLineProtection.Controllers
{
    public class ServiceLineProtectionInvestigationController : ControllerBaseWithPersistence<IServiceLineProtectionInvestigationRepository, ServiceLineProtectionInvestigation, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ServiceLineProtection;
        public const string CREATE_NOTIFICATION = "Service Line Protection Investigation Created";

        #endregion

        #region Private Methods

        private void SendNotification(ServiceLineProtectionInvestigation entity, string purpose)
        {
            entity.RecordUrl = GetUrlForModel(entity, "Show", "ServiceLineProtectionInvestigation", "ServiceLineProtection");
            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs {
                OperatingCenterId = entity.OperatingCenter.Id,
                Module = ROLE,
                Purpose = purpose,
                Data = entity
            };
            notifier.Notify(args);
        }

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddDropDownData<IContractorRepository, Contractor>("Contractor", x => x.GetAwrContractorsForDropDown(), x => x.Id, x => x.Description);
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);

                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchServiceLineProtectionInvestigation>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(formatter => {
                formatter.View(() => ActionHelper.DoShow(id));
                formatter.Fragment(() => ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                    ViewName = "_ShowPopup",
                    IsPartial = true
                }));
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchServiceLineProtectionInvestigation search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search);
                    return this.Excel(results);
                });
                formatter.Map(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search);
                    return _container.With(results).GetInstance<MapResult>();
                });
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateServiceLineProtectionInvestigation(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateServiceLineProtectionInvestigation model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    var entity = Repository.Find(model.Id);
                    SendNotification(entity, CREATE_NOTIFICATION);
                    return null;
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditServiceLineProtectionInvestigation>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditServiceLineProtectionInvestigation model)
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

		#region Constructors

        public ServiceLineProtectionInvestigationController(ControllerBaseWithPersistenceArguments<IServiceLineProtectionInvestigationRepository, ServiceLineProtectionInvestigation, User> args) : base(args) {}

		#endregion
    }
}