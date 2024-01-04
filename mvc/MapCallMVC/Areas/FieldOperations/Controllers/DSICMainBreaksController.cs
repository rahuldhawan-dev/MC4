using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Models;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Results;
using MMSINC.Utilities;
using System.Linq;
using System.Web.Mvc;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class DSICMainBreaksController : ControllerBaseWithPersistence<IMainBreakRepository, MainBreak, User>
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
            return ActionHelper.DoSearch<SearchDSICMainBreaks>();
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchDSICMainBreaks search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs { MaxResults = MAX_INDEX_RESULTS }));
                formatter.Excel(() => {
                    var results = Repository.Search(search).Select(x => new {
                        x.WorkOrder.OperatingCenter,
                        WorkOrderNumber = x.WorkOrder.Id,
                        x.WorkOrder.WorkDescription,
                        x.WorkOrder.DateReceived,
                        x.WorkOrder.DateCompleted,
                        x.WorkOrder.StreetAddress,
                        x.WorkOrder.Town,
                        Size = x.ServiceSize,
                        x.Depth,
                        Material = x.MainBreakMaterial.Description,
                        FailureType = x.MainFailureType.Description,
                        SoilCondition = x.MainBreakSoilCondition.Description,
                        CustomersAffected = x.WorkOrder.EstimatedCustomerImpact,
                        x.ShutdownTime,
                        x.WorkOrder.AlertIssued,
                        x.WorkOrder.Latitude,
                        x.WorkOrder.Longitude,
                        x.BoilAlertIssued,
                    });

                    return new ExcelResult().AddSheet(results, new MMSINC.Utilities.Excel.ExcelExportSheetArgs { });
                });
            });
        }

        #endregion

        #region Constructors

        public DSICMainBreaksController(ControllerBaseWithPersistenceArguments<IMainBreakRepository, MainBreak, User> args) : base(args) { }

        #endregion
    }
}