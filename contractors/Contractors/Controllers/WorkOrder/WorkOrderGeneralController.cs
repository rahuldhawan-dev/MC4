using System.Web.Mvc;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using NHibernate.Criterion;

namespace Contractors.Controllers.WorkOrder
{
    public class WorkOrderGeneralController : WorkOrderControllerBase<WorkOrderGeneralSearch>
    {
        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            if (action == ControllerAction.Search)
            {
                this.AddDropDownData<MapCall.Common.Model.Repositories.IDocumentTypeRepository, DocumentType>(r => r.GetByTableName(WorkOrderMap.TABLE_NAME),
                    t => t.Id, t => t.Name);
            }
        }

        #endregion

        #region Index

        [HttpGet]
        public ActionResult Index(WorkOrderGeneralSearch search)
        {
            return ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => Repository.SearchGeneralOrders(search)
            });
        }
        
        #endregion

        #region Show

        [HttpGet]
        public ActionResult Show(int id)
        {
            
            return ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                GetEntityOverride = () => Repository.GeneralOrders
                                            .Add(Restrictions.IdEq(id))
                                            .UniqueResult<MapCall.Common.Model.Entities.WorkOrder>()
            }, wo => {
                if (wo.CancelledAt.HasValue)
                    DisplayNotification($"Cancelled On: {wo.CancelledAt}, Reason: {wo.WorkOrderCancellationReason}");
            });
        }

        #endregion

        public WorkOrderGeneralController(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, MapCall.Common.Model.Entities.WorkOrder, ContractorUser> args) : base(args) {}
    }
}
