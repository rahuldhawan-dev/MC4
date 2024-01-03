using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services.Reports;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("Services Retired")]
    public class ServicesRetiredController : ServiceReportController<SearchServicesRetired, ServicesRetiredReportItem>
    {
        #region Constructors

        public ServicesRetiredController(ControllerBaseWithPersistenceArguments<IServiceRepository, Service, User> args) : base(args) {}

        #endregion
        
        #region Private Methods

        protected override IEnumerable<ServicesRetiredReportItem> GetResults(SearchServicesRetired search)
        {
            return Repository.GetServicesRetired(search);
        }
        
        #endregion

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddDropDownData("YearRetired", Repository.GetDistinctYearsRetired().OrderByDescending(x => x), x => x, x => x);
        }
    }
}