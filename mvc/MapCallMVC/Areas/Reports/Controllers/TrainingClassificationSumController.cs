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
    public class TrainingClassificationSumController : ControllerBaseWithPersistence<IPositionGroupRepository, PositionGroup, User>
    {
        public const RoleModules ROLE = RoleModules.OperationsTrainingRecords;

        public override void SetLookupData(ControllerAction action)
        {
            this.AddOperatingCenterDropDownData();
            this.AddDropDownData<TrainingModule>(x => x.GetAllSorted(y => y.Title), x => x.Id, x => x.Title);
            this.AddDropDownData<PositionGroupCommonName>();
        }

        public TrainingClassificationSumController(ControllerBaseWithPersistenceArguments<IPositionGroupRepository, PositionGroup, User> args) : base(args) {}

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchTrainingClassificationSum search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchTrainingClassificationSum search)
        {
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => Repository.GetTrainingClassificationSumReport(search)
                }));
                f.Excel(() => ActionHelper.DoExcel(search, new ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => Repository.GetTrainingClassificationSumReport(search)
                }));
            });
        }
    }
}