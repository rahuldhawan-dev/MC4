using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services.Reports;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class BPUReportForServicesController : ServiceReportController<SearchBPUReportForServices, BPUReportForServiceReportItem>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Constructors

        public BPUReportForServicesController(ControllerBaseWithPersistenceArguments<IServiceRepository, Service, User> args) : base(args) {}

        #endregion

        #region Methods

        protected override IEnumerable<BPUReportForServiceReportItem> GetResults(SearchBPUReportForServices search)
        {
            return Repository.GetBPUReportForServices(search);
        }

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
            this.AddDropDownData("Year", Repository.GetDistinctYears().OrderByDescending(x => x), x => x, x => x);
        }

        #endregion
    }
}