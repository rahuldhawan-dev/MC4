using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCallMVC.Areas.Reports.Models;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Reports.Controllers
{
    // NOTE: No role because the repository methods don't bother with them and this report will become inconsistent
    //       if users get different results based on whether or not they have specific hydrant/valve roles.
    [DisplayName("Inspection Productivity Report")]
    public class InspectionProductivityController : ControllerBaseWithPersistence<IHydrantInspectionRepository, HydrantInspection, User>
    {
        #region Constructor

        // GET: Reports/InspectionProductivity
        public InspectionProductivityController(ControllerBaseWithPersistenceArguments<IHydrantInspectionRepository, HydrantInspection, User> args) : base(args) {}

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>();
                    this.AddEnumDropDownData<InspectionProductivityWeekSpan>("Week");
                    break;
            }
        }

        [HttpGet]
        public ActionResult Index(SearchInspectionProductivity model)
        {
            return ActionHelper.DoIndex(model, new ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => {
                    // Need to ToList() the three results because they're using the same search model and each search would wipe out the previous results.
                    var hydrantResult = Repository.GetInspectionProductivityReport(model).ToList();
                    var blowOffResult = _container.GetInstance<IBlowOffInspectionRepository>().GetInspectionProductivityReport(model).ToList();
                    var valveResult = _container.GetInstance<IValveInspectionRepository>().GetInspectionProductivityReport(model).ToList();

                    // ToList the results because the view hasa to enumerate it the collection about a million times.
                    model.Results = hydrantResult.Concat(blowOffResult).Concat(valveResult).ToList();

                    // Need to eet the count because if valveResult has zero results(but hydrants/blowoffs have > 0 results),
                    // the Count property will have been set to zero and ActionHelper will think there aren't any results.
                    model.Count = model.Results.Count();
                }
            });
        }

        [HttpGet]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchInspectionProductivity>();
        }

        #endregion
    }
}