using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Models;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class CompletedWorkOrdersController : ControllerBaseWithPersistence<IWorkOrderRepository, WorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;
        public const int MAX_INDEX_RESULTS = 1000;

        #endregion

        #region Private Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            if (action == ControllerAction.Search)
            {
                this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
            }
        }

        #endregion

        #region Search/Index

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchCompletedWorkOrders>();
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchCompletedWorkOrders search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs { MaxResults = MAX_INDEX_RESULTS }));
                formatter.Excel(() => {
                    var args = new ActionHelperDoIndexArgs();
                    search.EnablePaging = false;
                    args.MaxResults = MAX_INDEX_RESULTS;
                    args.AutofitForExcel = false;
                    args.SearchOverrideCallback = () => Repository.Search(search).Select(x => new {
                        WorkOrderID = x.Id,
                        x.OperatingCenter,
                        x.Town,
                        x.SAPWorkOrderNumber,
                        x.SAPNotificationNumber,
                        x.StreetAddress,
                        x.WorkDescription,
                        x.AssetType,
                        x.AssetId,
                        x.CreatedBy,
                        x.CreatedAt,
                        x.DateReceived,
                        x.RequestedBy,
                        x.RequestingEmployee,
                        x.CompletedBy,
                        x.DateCompleted,
                        OrderProcessTime =
                            $"{x.OrderProcessTime?.Days} days, {x.OrderProcessTime?.Hours}:{x.OrderProcessTime?.Minutes}",
                        x.ApprovedBy,
                        x.ApprovedOn,
                        SupervisorProcessTime =
                            $"{x.SupervisorProcessTime?.Days} days, {x.SupervisorProcessTime?.Hours}:{x.SupervisorProcessTime?.Minutes}",
                        x.HasMaterialsUsed,
                        x.MaterialsApprovedBy,
                        x.MaterialsApprovedOn,
                        StockProcessTime =
                            $"{x.StockProcessTime?.Days} days, {x.StockProcessTime?.Hours}:{x.StockProcessTime?.Minutes}",
                        x.CurrentCrew,
                        x.AccountCharged,
                        x.WorkDescription.AccountingType,
                        x.Notes
                    });
                    return ActionHelper.DoExcel(search, args);
                });
            });
        }

        #endregion

        #region Constructors

        public CompletedWorkOrdersController(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, WorkOrder, User> args) : base(args) { }

        #endregion
    }
}
