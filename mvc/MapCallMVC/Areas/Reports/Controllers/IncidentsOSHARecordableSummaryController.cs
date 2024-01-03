using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Results;
using MMSINC.Utilities.Pdf;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class IncidentsOSHARecordableSummaryController : ControllerBaseWithPersistence<IIncidentRepository, Incident, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsIncidents;

        #endregion

        #region Constructors

        public IncidentsOSHARecordableSummaryController(ControllerBaseWithPersistenceArguments<IIncidentRepository, Incident, User> args) : base(args) { }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Read);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchIncidentOSHARecordableSummary search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchIncidentOSHARecordableSummary search)
        {
            search.EnablePaging = false;

            return ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => Repository.SearchOSHA(search),
                OnSuccess = () => new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), "Pdf", search),
                OnNoResults = () => new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), "Pdf", search),
            });
        }

        #endregion

    }
}