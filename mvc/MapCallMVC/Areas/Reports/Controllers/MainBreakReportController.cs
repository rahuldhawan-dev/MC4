using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Models;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class MainBreakReportController :  ControllerBaseWithPersistence<IWorkOrderRepository, WorkOrder, User>
    {
        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
                    this.AddDropDownData("Year", Repository.GetDistinctYearsCompleted().OrderByDescending(x => x), x => x, x => x);
                    break;
            }
            base.SetLookupData(action);
        }

        #endregion

        #region Constructors

        public MainBreakReportController(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, WorkOrder, User> args) : base(args) { }

        #endregion

        #region Search/Index

        [HttpGet]
        public ActionResult Search(SearchMainBreak search)
        {
            return ActionHelper.DoSearch(search);
        }

        // Why isn't a searchset being used or ActionHelper here?
        // The Search model is based on XZYReportItem, 
        // but the results are transformed/kicked over producing results that 
        // are of type XYZReport.
        // TODO: Refactor
        [HttpGet]
        public ActionResult Index(SearchMainBreak search)
        {
            return this.RespondTo(f => {
                var model = Repository.GetMainBreakServiceLineReport(search);

                f.View(() => View(model));
                f.Excel(() => this.Excel(model));
            });
        }

        #endregion
    }
}
