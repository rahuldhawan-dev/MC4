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
    [DisplayName("Valve Inspections - Required/Visited")]
    public class RequiredValvesOperatedByMonthController : ControllerBaseWithPersistence<IValveInspectionRepository, ValveInspection, User>
    {
        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>(filter: oc => oc.State.Id == State.Indices.NJ);
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
            return ActionHelper.DoSearch<SearchRequiredValvesOperatedByMonth>();
        }

        // Why isn't a search set being used or ActionHelper here?
        // The Search model is based on XzyReportItem, 
        // but the results are transformed/kicked over producing results that 
        // are of type XyzReport.
        // TODO: Refactor
        [HttpGet]
        public ActionResult Index(SearchRequiredValvesOperatedByMonth search)
        {
            // TODO: Validate this properly.
            if (search.OperatingCenter == null || search.Year == null)
                return RedirectToAction("Search");

            return this.RespondTo(f => {
                var model = Repository.GetRequiredValvesOperatedByMonthReport(search);
                f.View(() => View(model));
                f.Excel(() => this.Excel(model));
            });
        }

        #endregion

        #region Constructors

        public RequiredValvesOperatedByMonthController(
            ControllerBaseWithPersistenceArguments<IValveInspectionRepository, ValveInspection, User> args) : base(args) {}

        #endregion

    }
}