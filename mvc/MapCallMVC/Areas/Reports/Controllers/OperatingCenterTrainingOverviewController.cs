using System.ComponentModel;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Results;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("Training Overview")]
    public class OperatingCenterTrainingOverviewController : ControllerBaseWithPersistence<IOperatingCenterRepository, OperatingCenter, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsTrainingRecords;

        #endregion

        #region Constructors

        public OperatingCenterTrainingOverviewController(
            ControllerBaseWithPersistenceArguments<IOperatingCenterRepository, OperatingCenter, User> args) : base(args) {}

        #endregion

        #region Exposed Methods
        
        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddDropDownData<TrainingRequirement>();
            this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
            this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
        }

        #endregion

        #region Search/Index

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchOperatingCenterTrainingOverview search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchOperatingCenterTrainingOverview search)
        {
            return this.RespondTo((formatter) => {
                search.Results = Repository.GetOperatingCenterTrainingOverviewReportItems(search);
                formatter.View(() => View(search));
                formatter.Excel(() => {
                    //this.Excel(results);
                    var result = new ExcelResult();
                    var header = search.IsOSHARequirement.HasValue ? "OSHA Requirement: " + search.IsOSHARequirement : null;
                    result.AddSheet(search.Results, new MMSINC.Utilities.Excel.ExcelExportSheetArgs { Header = header });
                    return result;
                });
            });

        }

        #endregion
    }
}