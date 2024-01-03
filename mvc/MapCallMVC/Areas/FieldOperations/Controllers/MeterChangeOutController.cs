using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Results;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using MMSINC.Utilities.Sorting;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class MeterChangeOutController : ControllerBaseWithPersistence<IMeterChangeOutRepository, MeterChangeOut, User>
    {
        #region Consts

        private const RoleModules ROLE = RoleModules.FieldServicesMeterChangeOuts;

        #endregion

        #region Constructor

        public MeterChangeOutController(ControllerBaseWithPersistenceArguments<IMeterChangeOutRepository, MeterChangeOut, User> args) : base(args) { }

        #endregion

        #region Private Methods

        public override void SetLookupData(ControllerAction action)
        {
            if (action == ControllerAction.Edit)
            {
                this.AddDropDownData<ContractorMeterCrew>("CalledInByContractorMeterCrew", x => x.Where(y => y.IsActive).OrderBy(y => y.Description), x => x.Id, x => x.Description);
                this.AddDropDownData<ContractorMeterCrew>("AssignedContractorMeterCrew", x => x.Where(y => y.IsActive).OrderBy(y => y.Description), x => x.Id, x => x.Description);
            }
        }

        #endregion

        #region Actions

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch(new SearchMeterChangeOut());
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchMeterChangeOut search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Pdf(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search);

                    // Bug 3618: They want different sorting based on whether or not they're searching
                    // for scheduled vs unscheduled meter change outs. 

                    if (!search.IsUnscheduledSearch)
                    {
                        // Bug 3618: They asked for this sorting.
                        results = results.OrderBy(x => x.AssignedContractorMeterCrew?.ToString())
                                     .ThenBy(x => x.DateScheduled)
                                     .ThenBy(x => x.MeterScheduleTime?.Description)
                                     .ThenBy(x => x.ServiceCity?.ToString())
                                     .ThenBy(x => x.ServiceStreet)
                                     // Bug 3405 specifically complained that these aren't sorting correctly. This is due to the
                                     // column being a string. When they inevitably import something that isn't an integer we can
                                     // fix this again.
                                     .ThenBy(x => x.ServiceStreetNumber, new AlphaNumericComparer()).ToList(); ;
                    }
                    else
                    {
                        // Bug 3504: They asked for this sorting.
                        results = results.OrderBy(x => x.ServiceCity?.ToString())
                                         .ThenBy(x => x.ServiceStreet)
                                         // Bug 3405 specifically complained that these aren't sorting correctly. This is due to the
                                         // column being a string. When they inevitably import something that isn't an integer we can
                                         // fix this again.
                                         .ThenBy(x => x.ServiceStreetNumber, new AlphaNumericComparer()).ToList();
                    }

                    return new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), "PdfIndex", results);
                });
                formatter.Excel(() => {
                    return SpecialExport(search);
                });
            });
        }

        private ActionResult SpecialExport(SearchMeterChangeOut search)
        {
            search.EnablePaging = false;
            var results = Repository.Search(search).ToList();
            if (search.MarkAdvantexExport.HasValue)
            {
                foreach (var record in results)
                {
                    record.ClickAdvantexUpdated = search.MarkAdvantexExport.Value;
                }
                Repository.Save(results);
            }
            var now = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            var crew = (search.CalledInByContractorMeterCrew.HasValue) ? "-" + _container.GetInstance<IRepository<ContractorMeterCrew>>().Find(search.CalledInByContractorMeterCrew.Value).Description : "";
            return new ExcelResult($"{now.Year}-{now.Month}-{now.Day}{crew}-{now.AddHours(-1):tt}").AddSheet(results.Select(x => x.ToExcelRecord()));
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x => {
                x.View(() => ActionHelper.DoShow(id));
                x.Pdf(() => ActionHelper.DoPdf(id));
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditMeterChangeOut>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditMeterChangeOut model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpGet, RequiresRole(ROLE), NoCache]
        public ActionResult ValidateNewSerialNumber(int id, string newSerialNumber)
        {
            // Note that while this is "validation" it isn't
            // supposed to prevent the duplicate from being entered. Why? Ask Ralph/Freddy.

            return Json(new
            {
                isUnique = Repository.IsNewSerialNumberUnique(id, newSerialNumber)
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}