using System;
using System.Linq;
using System.Web.Mvc;
using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Metadata;
using MMSINC.Results;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using MMSINC.Utilities.Sorting;
using StructureMap;

namespace Contractors.Controllers
{
    public class MeterChangeOutController : ControllerBaseWithValidation<IMeterChangeOutRepository, MeterChangeOut>
    {
        #region Constructor

        public MeterChangeOutController(ControllerBaseWithPersistenceArguments<IMeterChangeOutRepository, MeterChangeOut, ContractorUser> args) : base(args) { }

        #endregion

        #region Private Methods

        public override void SetLookupData(ControllerAction action)
        {
            if (action == ControllerAction.Edit)
            {
                this.AddDropDownData<IContractorMeterCrewRepository, ContractorMeterCrew>("CalledInByContractorMeterCrew", x => x.GetAll().Where(y => y.IsActive).OrderBy(y => y.Description), x => x.Id, x => x.Description);
                this.AddDropDownData<IContractorMeterCrewRepository, ContractorMeterCrew>("AssignedContractorMeterCrew", x => x.GetAll().Where(y => y.IsActive).OrderBy(y => y.Description), x => x.Id, x => x.Description);
            }
            if (action == ControllerAction.Search)
            {
                this.AddDropDownData<ServiceSize>("MeterSize", r => r.GetAllSorted(x => x.Size), x => x.Id, x => x.ServiceSizeDescription);
            }

        }

        #endregion

        #region Actions

        [HttpGet]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch(new SearchMeterChangeOut());
        }

        [HttpGet]
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

                    return new PdfResult(
                        _container.GetInstance<IHtmlToPdfConverter>(),
                        "PdfIndex", results);
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
            var crew = (search.CalledInByContractorMeterCrew.HasValue) ? "-" + _container.GetInstance<IContractorMeterCrewRepository>().Find(search.CalledInByContractorMeterCrew.Value).Description : "";
            return new ExcelResult($"{now.Year}-{now.Month}-{now.Day}{crew}-{now.AddHours(-1):tt}").AddSheet(results.Select(x => x.ToExcelRecord()));
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x => {
                x.View(() => ActionHelper.DoShow(id));
                x.Pdf(() => ActionHelper.DoPdf(id));
            });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditMeterChangeOut>(id);
        }

        [HttpPost]
        public ActionResult Update(EditMeterChangeOut model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpGet, NoCache]
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