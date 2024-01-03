using System.ComponentModel;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("Training Summary")]
    public class OperatingCenterTrainingSummaryController : ControllerBaseWithPersistence<IOperatingCenterRepository, OperatingCenter, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsTrainingRecords;

        #endregion

        #region Constructors

        public OperatingCenterTrainingSummaryController(
            ControllerBaseWithPersistenceArguments<IOperatingCenterRepository, OperatingCenter, User> args) : base(args) {}

        #endregion

        #region Exposed Methods
        
        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddDropDownData<TrainingRequirement>();
            this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
           // this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
        }

        #endregion

        #region Search/Index

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchOperatingCenterTrainingSummary search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchOperatingCenterTrainingSummary search)
        {
            return this.RespondTo((formatter) =>
            {
                search.Results = Repository.GetOperatingCenterTrainingSummaryReportItems(search);
                formatter.View(() => View(search));
                formatter.Excel(() => this.Excel(search.Results));
            });

        }

        #endregion
    }
}