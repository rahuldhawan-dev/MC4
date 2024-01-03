using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class TrainingTotalHoursController : ControllerBaseWithPersistence<ITrainingRecordRepository, TrainingRecord, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsTrainingRecords;

        #endregion

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            this.AddDropDownData<TrainingModule>(x => x.GetAllSorted(y => y.Title), x => x.Id, x => x.Title);
            this.AddDropDownData<PositionGroupCommonName>();
            this.AddOperatingCenterDropDownData();
        }

        public TrainingTotalHoursController(ControllerBaseWithPersistenceArguments<ITrainingRecordRepository, TrainingRecord, User> args) : base(args) {}

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchTrainingTotalHours search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchTrainingTotalHours search)
        {
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => Repository.GetTotalTrainingHours(search)
                }));
                f.Excel(() => ActionHelper.DoExcel(search, new ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => Repository.GetTotalTrainingHours(search)
                }));
            });
        }
    }
}