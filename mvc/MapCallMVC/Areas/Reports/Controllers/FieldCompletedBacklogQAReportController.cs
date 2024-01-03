using System.ComponentModel;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Models;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("Field Completed Backlog - Quality Assurance")]
    public class FieldCompletedBacklogQAReportController : ControllerBaseWithPersistence<IWorkOrderRepository, WorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        #endregion

        #region Constructor

        public FieldCompletedBacklogQAReportController(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, WorkOrder, User> args) : base(args) { }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownData();
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchFieldCompletedBacklogQAReport>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchFieldCompletedBacklogQAReport model)
        {
            model.EnablePaging = false;
            var args = new ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => { model.Results = Repository.GetFieldCompletedBacklogQAReport(model); }
            };
            return this.RespondTo((f) =>
            {
                f.View(() => ActionHelper.DoIndex(model, args));
                f.Excel(() => ActionHelper.DoExcel(model, args));
            });
        }

        #endregion
    }
}