using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Permissions;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.Reports.Models;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data;
using StructureMap;
using OperatingCenter = MapCall.Common.Model.Entities.OperatingCenter;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class KPIHydrantInspectionByMonthController : ControllerBaseWithPersistence<IHydrantInspectionRepository, HydrantInspection, User>
    {
        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>();
                    this.AddDropDownData("Year", _container.GetInstance<IHydrantInspectionRepository>().GetDistinctYearsCompleted().OrderByDescending(x => x), x => x, x => x);
                    break;
            }
            base.SetLookupData(action);
        }

        #endregion

        #region Search/Index

        [HttpGet]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchKPIHydrantInspectionsByMonth>();
        }

        // Why isn't a searchset being used or ActionHelper here?
        // The Search model is based on XZYReportItem, 
        // but the results are transformed/kicked over producing results that 
        // are of type XYZReport.
        // TODO: Refactor
        [HttpGet]
        public ActionResult Index(SearchKPIHydrantInspectionsByMonth search)
        {
            return this.RespondTo(f => {
                var model = Repository.GetKPIHydrantsInspectedReport(search);

                f.View(() => View(model));
                f.Excel(() => this.Excel(model));
            });
        }

        #endregion

        #region Constructors

        public KPIHydrantInspectionByMonthController(ControllerBaseWithPersistenceArguments<IHydrantInspectionRepository, HydrantInspection, User> args) : base(args) {}

        #endregion
    }
}