using MapCall.Common.Model.Entities;
using MMSINC.Controllers;
using System.Web.Mvc;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Models;
using MMSINC.ClassExtensions;
using System.ComponentModel;
using MapCall.Common.Metadata;

namespace MapCallMVC.Areas.Reports.Controllers
{
    /// <summary>
    /// This controller will return a report that shows the Operating Center, Town, and Count of all the NpdesRegulators that have an Inspection Due
    ///     in that town. Clicking on the Town link will take you to the Index listing the regulators in that count. This is handled in
    ///     the NpdesRegulatorsDueInspectionController in the Areas/FieldOperations folder
    /// </summary>
    [DisplayName("NPDES Regulators Due Inspection")]
    public class NpdesRegulatorsDueInspectionReportController : ControllerBaseWithPersistence<ISewerOpeningRepository, SewerOpening, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Constructor

        public NpdesRegulatorsDueInspectionReportController(ControllerBaseWithPersistenceArguments<ISewerOpeningRepository, SewerOpening, User> args) : base(args) { }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchNpdesRegulatorsDueInspectionReport>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchNpdesRegulatorsDueInspectionReport model)
        {
            var args = new MMSINC.Utilities.ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => Repository.GetNpdesRegulatorsDueInspectionReport(model)
            };
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(model, args));
                f.Excel(() => ActionHelper.DoExcel(model, args));
            });
        }

        #endregion
    }
}