using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services.Reports;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("T/D Pending Services Report")]
    public class ServicesTDPendingServicesKPIController : ServiceReportController<SearchServicesTDPendingServicesKPI, TDPendingServicesKPIReportItem>
    {
        public ServicesTDPendingServicesKPIController(ControllerBaseWithPersistenceArguments<IServiceRepository, Service, User> args) : base(args) {}

        protected override IEnumerable<TDPendingServicesKPIReportItem> GetResults(SearchServicesTDPendingServicesKPI search)
        {
            return Repository.GetTDPendingServicesKPI(search);
        }
        
        protected override ActionResult GetExcelResult(SearchServicesTDPendingServicesKPI search)
        {
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            return PartialView("Index", search);
        }
    }
}