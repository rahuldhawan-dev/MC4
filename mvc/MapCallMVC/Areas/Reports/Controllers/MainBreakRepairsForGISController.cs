using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Models;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("Main Break Repairs for GIS")]
    public class MainBreakRepairsForGISController : ControllerBaseWithPersistence<IWorkOrderRepository, WorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        #endregion

        #region Private Methods

        private static ReportItem ConvertToObject(WorkOrder order)
        {
            return new ReportItem(order);
        }

        #endregion

        #region Search/Index

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchMainBreakRepairsForGIS search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchMainBreakRepairsForGIS search)
        {
            return this.RespondTo(f => {
                f.View(() => {
                    return ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                        SearchOverrideCallback =  () => Repository.GetMainBreakRepairsForGIS(search)
                    });
                });
                f.Excel(() => {
                    search.EnablePaging = false;
                    return this.Excel(Repository.GetMainBreakRepairsForGIS(search).Select(ConvertToObject), new MMSINC.Utilities.Excel.ExcelExportSheetArgs {
                        SheetName = "Main Break Repairs for GIS",
                        Header = "Main Break Repairs for GIS"
                    });
                });
                f.Json(() => {
                    // TODO: Is this supposed to be here still? Is this in the API? Why isn't this return validation errors? The receiver will not get the exception message.
                    if (search.DateCompleted == null || search.DateCompleted.Operator != RangeOperator.Between ||
                        search.DateCompleted.End == null || search.DateCompleted.End.Value
                            .Subtract(search.DateCompleted.Start.Value).TotalDays > 30)
                    {
                        throw new InvalidOperationException(
                            "DateCompletedTime must be a 'between' search of a month or less.");
                    }
                    search.EnablePaging = false;
                    return Json(new {Data = Repository.GetMainBreakRepairsForGIS(search).Select(ConvertToObject)},
                        JsonRequestBehavior.AllowGet);
                });
            });
        }

        #endregion

        public MainBreakRepairsForGISController(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, WorkOrder, User> args) : base(args) { }

        public class ReportItem
        {
            #region Properties

            public string WorkOrderNumber { get; }
            public string OperatingCenter { get; }
            public string Town { get; }
            public string StreetAddress { get; }
            public bool? UpdatedMobileGIS { get; }
            public string WorkDescription { get; }
            public string LastAssignedTo { get; }
            public DateTime? DateCompleted { get; }
            public string CompletedBy { get; }
            public string SAPWorkOrderNumber { get; }
            public string SAPNotificationNumber { get; }
            public string EstimatedCustomerImpact { get; }
            public string AnticipatedRepairTime { get; }
            public bool? AlertIssued { get; }
            public bool? SignificantTrafficImpact { get; }

            #endregion

            #region Constructors

            public ReportItem(WorkOrder order)
            {
                WorkOrderNumber = order.Id.ToString();
                OperatingCenter = order.OperatingCenter.ToString();
                Town = order.Town.ToString();
                StreetAddress = order.StreetAddress;
                UpdatedMobileGIS = order.UpdatedMobileGIS;
                WorkDescription = order.WorkDescription.ToString();
                LastAssignedTo = order.CurrentCrew?.ToString();
                DateCompleted = order.DateCompleted;
                CompletedBy = order.CompletedBy?.ToString();
                SAPWorkOrderNumber = order.SAPWorkOrderNumber?.ToString();
                SAPNotificationNumber = order.SAPNotificationNumber?.ToString();
                EstimatedCustomerImpact = order.EstimatedCustomerImpact?.ToString();
                AnticipatedRepairTime = order.AnticipatedRepairTime?.ToString();
                AlertIssued = order.AlertIssued;
                SignificantTrafficImpact = order.SignificantTrafficImpact;
            }

            #endregion
        }
    }
}