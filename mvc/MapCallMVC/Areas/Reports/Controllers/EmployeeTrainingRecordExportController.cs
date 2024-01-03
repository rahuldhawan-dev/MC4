using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class EmployeeTrainingRecordExportController : ControllerBaseWithPersistence<IEmployeeRepository, Employee, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsTrainingRecords;

        #endregion

        #region Private Methods

        private void MarkAsExported(IEnumerable<EmployeeTrainingRecordExportItem> results)
        {
            var repo = _container.GetInstance<TrainingRecordRepository>();
            var trainingRecords = results.Select(x => x.TrainingRecord).Distinct();
            foreach (var trainingRecord in trainingRecords)
            {
                trainingRecord.AttendeesExportedDate = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            }
            repo.Save(trainingRecords);
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddDynamicDropDownData<TrainingModule, TrainingModuleDisplayItem>();
            this.AddDropDownData<TrainingRecord>(x => x.GetAllSorted(y => y.Id), x => x.Id, x => x.Id);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Search(SearchEmployeeTrainingRecordExport search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Index(SearchEmployeeTrainingRecordExport search)
        {
            return this.RespondTo(f => {
                search.EnablePaging = false;

                f.View(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => Repository.GetEmployeeTrainingRecordExport(search)
                }));
                f.Excel(() => ActionHelper.DoExcel(search, new ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => Repository.GetEmployeeTrainingRecordExport(search),
                    OnSuccess = () => {
                        MarkAsExported(search.Results);
                        return null; // defer to default.
                    }
                }));
            });
        }

        #endregion

        #region Constructors

        public EmployeeTrainingRecordExportController(ControllerBaseWithPersistenceArguments<IEmployeeRepository, Employee, User> args) : base(args) {}

        #endregion

    }
}