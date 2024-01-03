using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services.Reports;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class ServicesRenewedSummaryController : ServiceReportController<SearchServicesRenewedSummary, ServicesRenewedSummaryReportItem>
    {
        #region Private Methods

        protected override IEnumerable<ServicesRenewedSummaryReportItem> GetResults(SearchServicesRenewedSummary search)
        {
            return Repository.GetServicesRenewedSummary(search);
        }

        #endregion

        #region Constructors

        public ServicesRenewedSummaryController(ControllerBaseWithPersistenceArguments<IServiceRepository, Service, User> args) : base(args) {}

        #endregion
    }
}