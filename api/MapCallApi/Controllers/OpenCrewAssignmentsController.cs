using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallApi.Models.ShortCycleWorkOrders;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallApi.Controllers
{
    public class OpenCrewAssignmentsController : ControllerBaseWithPersistence<ICrewAssignmentRepository, CrewAssignment, User>
    {
        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        public OpenCrewAssignmentsController(
            ControllerBaseWithPersistenceArguments<ICrewAssignmentRepository, CrewAssignment, User> args) : base(args)
        {
        }

        [RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchOpenCrewAssignments search)
        {
            if (search.Hours == 0) // What if it's less than 0?
                search.Hours = 48;
            return Json(
                Repository.OpenCompanyForcesCrewAssignments(search.Hours).Select(ca => new {
                        WorkOrderID = ca.WorkOrder.Id, 
                        ca.WorkOrder.SAPWorkOrderNumber,
                        DateStarted = string.Format(CommonStringFormats.DATETIME24_WITHOUT_SECONDS, ca.DateStarted),
                        ca.WorkOrder.Latitude,
                        ca.WorkOrder.Longitude,
                        WorkDescription = ca.WorkOrder.WorkDescription.Description,
                        CrewName = $"{ca.Crew.OperatingCenter?.OperatingCenterCode} {ca.Crew.OperatingCenter?.OperatingCenterName} {ca.Crew.Description}",
                        StartedBy = ca.StartedBy?.FullName,
                        StartedByPhone = $"Work: {ca.StartedBy?.Employee?.PhoneWork} Cell: {ca.StartedBy?.Employee?.PhoneCellular}"
                    }), JsonRequestBehavior.AllowGet);
        }
    }
}