using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Controllers;
using MMSINC.Utilities;
using System.Web.Mvc;
using MapCallMVC.ClassExtensions;
using System.ComponentModel;
using System.Linq;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MMSINC.ClassExtensions;
using MMSINC.Data.NHibernate;
using MapCall.SAP.Model.Repositories;
using MapCall.SAP.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderStockToIssue;
using MapCall.Common.Helpers;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    [DisplayName("Work Order Stock to Issue")]
    public class WorkOrderStockToIssueController : SapSyncronizedControllerBaseWithPersisence<IWorkOrderRepository, WorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        public const int MAX_RESULTS = 1000;

        #endregion

        #region Constructor

        public WorkOrderStockToIssueController(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, WorkOrder, User> args) : base(args) { }

        #endregion

        #region Private Methods

        protected override void UpdateEntityForSap(WorkOrder entity)
        {
            // NOTE: This controller *only* updates SAP for the APPROVE action.

            AddAuditLogEntry("SAPUpdate", entity.Id, "SAPUpdateStart", "Start Update", entity.MaterialsDocID);
            entity.SAPWorkOrderStep = _container.GetInstance<IRepository<SAPWorkOrderStep>>().Find(SAPWorkOrderStep.Indices.APPROVE_GOODS);
            var sapRepo = _container.GetInstance<ISAPWorkOrderRepository>();
            var sapGoodsIssue = new SAPGoodsIssue(entity);
            var goodsIssued = sapRepo.Approve(sapGoodsIssue);
            foreach (var item in goodsIssued.Items)
            {
                AddAuditLogEntry("SAPUpdate", entity.Id, "MaterialDocId", "CalledApproveGoods", item.MaterialDocument);
                AddAuditLogEntry("SAPUpdate", entity.Id, "SAPErrorCode", "CalledApproveGoods", item.Status);
                if (string.IsNullOrWhiteSpace(item.MaterialDocument))
                {
                    entity.MaterialPostingDate = null;
                    entity.MaterialsApprovedOn = null;
                    entity.MaterialsApprovedBy = null;
                }
            }

            // NOTE: This maps things to the entity, and then the base controller method
            // that calls UpdateEntityForSap ultimately saves the entity to the database.
            goodsIssued.MapToWorkOrder(entity);

            AddAuditLogEntry("SAPUpdate", entity.Id, "SAPUpdateEnd", "End Update", entity.MaterialsDocID);
        }

        private void AddAuditLogEntry(string auditEntryType, int entityId, string fieldName, string oldValue, string newValue)
        {
            var auditRepo = _container.GetInstance<IAuditLogEntryRepository>();
            auditRepo.Save(new AuditLogEntry {
                AuditEntryType = auditEntryType,
                EntityId = entityId,
                EntityName = nameof(WorkOrder),
                FieldName = fieldName,
                OldValue = oldValue,
                NewValue = newValue,
                Timestamp = _container.GetInstance<IDateTimeProvider>().GetCurrentDate(),
                User = AuthenticationService.CurrentUser
            });
        }

        #endregion

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action == ControllerAction.Search)
            {
                this.AddOperatingCenterDropDownDataForRoleAndAction(RoleModules.FieldServicesWorkManagement,
                    extraFilterP: oc => oc.WorkOrdersEnabled);
            }
        }

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch(new SearchWorkOrderStockToIssue());
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Index(SearchWorkOrderStockToIssue search)
        {
            return ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                MaxResults = MAX_RESULTS,
                SearchOverrideCallback = () => Repository.GetStockToIssueWorkOrders(search)
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(formatter => {
                formatter.View(() => ActionHelper.DoShow(id, new ActionHelperDoShowArgs {
                    GetEntityOverride = () => Repository.FindStockToIssueWorkOrder(id)
                }));
                formatter.Fragment(() => ActionHelper.DoShow(id, new ActionHelperDoShowArgs {
                    IsPartial = true,
                    ViewName = "../WorkOrder/_ShowPopup",
                    GetEntityOverride = () => Repository.FindStockToIssueWorkOrder(id)
                }));
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, () => {
                    var search = new SearchWorkOrderStockToIssue {
                        Id = id
                    };
                    return Repository.GetStockToIssueWorkOrders(search);
                }));
            });
        }

        #endregion

        #region Approval

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Approve(ApproveWorkOrderStockToIssue model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                GetEntityOverride = () => Repository.FindStockToIssueWorkOrder(model.Id),
                OnError = () => RedirectToAction("Show", new { id = model.Id }),
                OnSuccess = () => {
                    var entity = Repository.Find(model.Id);

                    // In 271, there are about a hundred different safety checks that 
                    // abort the SAP update if there aren't any SAP-linked materials used.
                    if (entity.IsSAPUpdatableWorkOrder && entity.MaterialsUsed.Any(x => x.Material != null))
                    {
                        UpdateSAP(model.Id, ROLE);
                    }

                    // Approve can redirect back to the approval show page since approved
                    // work orders are searchable here.
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        #endregion

        #region Edit

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator), ActionBarVisible(false)]
        public void Edit(int id) { }

        #endregion
    }
}
