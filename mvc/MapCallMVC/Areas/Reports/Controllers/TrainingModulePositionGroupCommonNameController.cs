using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Models;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class TrainingModulePositionGroupCommonNameController : ControllerBaseWithPersistence<IPositionGroupCommonNameRepository, PositionGroupCommonName, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = RoleModules.OperationsTrainingModules;

        #endregion

        #region Constructor

        public TrainingModulePositionGroupCommonNameController(ControllerBaseWithPersistenceArguments<IPositionGroupCommonNameRepository, PositionGroupCommonName, User> args) : base(args) { }

        #endregion

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action == ControllerAction.Search)
            {
                var modulesSorted = _container.GetInstance<ITrainingModuleRepository>().GetAllSorted(x => x.Title);
                this.AddDropDownData(modulesSorted, x => x.Id, x => x.Title);

                var pgcnSorted = _container.GetInstance<IPositionGroupCommonNameRepository>().GetAllSorted(x => x.Description);
                this.AddDropDownData("Ids", pgcnSorted, x => x.Id, x => x.Description);
            }
        }

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Index(SearchTrainingModulePositionGroupCommonName search)
        {
            search.EnablePaging = false;

            return this.RespondTo(format => {
                format.View(() => ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => Repository.SearchByTrainingModule(search)
                }));
                format.Excel(() => ActionHelper.DoExcel(search, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => Repository.SearchByTrainingModule(search)
                }));
            });
        }

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchTrainingModulePositionGroupCommonName>();
        }
    }
}