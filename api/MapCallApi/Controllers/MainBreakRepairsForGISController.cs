using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallApi.Models;
using MMSINC.Controllers;
using MMSINC.Data;

namespace MapCallApi.Controllers
{
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

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchMainBreakRepairsForGIS search)
        {
            if (search.DateCompleted == null || search.DateCompleted.Operator != RangeOperator.Between ||
                search.DateCompleted.End == null || search.DateCompleted.End.Value
                    .Subtract(search.DateCompleted.Start.Value).TotalDays > 30)
            {
                throw new InvalidOperationException(
                    "DateCompletedTime must be a 'between' search of a month or less.");
            }
            search.EnablePaging = false;
            return new JsonNetResult() { Data = Repository.GetMainBreakRepairsForGIS(search).Select(ConvertToObject) };
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
            public decimal? Latitude { get; set; }
            public decimal? Longitude { get; set; }
            public DateTime CreatedOn { get; set; }

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
                Latitude = order.Latitude;
                Longitude = order.Longitude;
                CreatedOn = order.CreatedAt;
            }

            #endregion
        }
    }
}