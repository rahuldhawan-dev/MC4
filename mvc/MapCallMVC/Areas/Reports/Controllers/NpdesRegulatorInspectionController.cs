using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Models;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("NPDES Regulator Inspection")]
    public class NpdesRegulatorInspectionController : ControllerBaseWithPersistence<INpdesRegulatorInspectionRepository, NpdesRegulatorInspection, User>
    {
        #region Constructors

        public NpdesRegulatorInspectionController(ControllerBaseWithPersistenceArguments<INpdesRegulatorInspectionRepository, NpdesRegulatorInspection, User> args) : base(args) {}

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDropDownData("Year", _container.GetInstance<INpdesRegulatorInspectionRepository>().GetDistinctYearsCompleted().OrderByDescending(x => x), x => x, x => x);
                    break;
            }
            base.SetLookupData(action);
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public ActionResult Search(SearchNpdesRegulatorInspection search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet]
        public ActionResult Index(SearchNpdesRegulatorInspection model)
        {
            var args = new ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => Repository.SearchNpdesRegulatorInspectionReport(model)
            };
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(model, args));
                formatter.Excel(() => {
                    model.EnablePaging = false;
                    return ActionHelper.DoExcel(model, args);
                });
            });
        }

        #endregion
    }
}