using System.Collections.Generic;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services.Reports;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class ServicesCompletedByCategoryController : ServiceReportController<SearchServicesCompletedByCategory, ServicesCompletedByCategoryReportItem>
    {
        public ServicesCompletedByCategoryController(ControllerBaseWithPersistenceArguments<IServiceRepository, Service, User> args) : base(args) {}
        
        protected override IEnumerable<ServicesCompletedByCategoryReportItem> GetResults(SearchServicesCompletedByCategory search)
        {
            return Repository.GetServicesCompletedByCategory(search);
        }

        protected override ActionResult GetExcelResult(SearchServicesCompletedByCategory search)
        {
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            return PartialView("Index", search);
        }
    }
}