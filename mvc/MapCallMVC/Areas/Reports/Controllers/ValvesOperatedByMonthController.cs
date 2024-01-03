using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("Valve Inspections - All")]
    public class ValvesOperatedByMonthController : ControllerBaseWithPersistence<IValveInspectionRepository, ValveInspection, User>
    {
        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>();
                    this.AddDropDownData("Year", _container.GetInstance<IValveInspectionRepository>().GetDistinctYearsCompleted().OrderByDescending(x => x), x => x, x => x);
                    break;
            }
            base.SetLookupData(action);
        }

        #endregion

        #region Search/Index

        [HttpGet]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchValvesOperatedByMonth>();
        }

        // Why isn't a search set being used or ActionHelper here?
        // The Search model is based on XyzReportItem, 
        // but the results are transformed/kicked over producing results that 
        // are of type XyzReport.
        // TODO: Refactor
        [HttpGet]
        public ActionResult Index(SearchValvesOperatedByMonth search)
        {
            if (search.OperatingCenter == null || search.Year == null)
                return RedirectToAction("Search");

            return this.RespondTo(f => {
                var model = Repository.GetValvesOperatedByMonthReport(search);
                f.View(() => View(model));
                f.Excel(() => this.Excel(model));
            });
        }

        #endregion

        #region Constructors

        public ValvesOperatedByMonthController(
            ControllerBaseWithPersistenceArguments<IValveInspectionRepository, ValveInspection, User> args) : base(args) {}

        #endregion

    }
}