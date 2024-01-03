using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class MeterChangeOutCompletionsController : ControllerBaseWithPersistence<IMeterChangeOutRepository, MeterChangeOut, User>
    {
        #region Consts

        private const RoleModules ROLE = RoleModules.FieldServicesMeterChangeOuts;

        #endregion

        #region Constructors

        public MeterChangeOutCompletionsController(
            ControllerBaseWithPersistenceArguments<IMeterChangeOutRepository, MeterChangeOut, User> args) : base(args) {}

        #endregion

        #region Private Methods

        private IEnumerable<MeterChangeOutExcelSummaryRecord> WeeklyExport(SearchMeterChangeOutCompletions search)
        {
            var newSearch = new SearchMeterChangeOut
            {
                CalledInByContractorMeterCrew = search.CalledInByContractorMeterCrew,
                DateStatusChanged = search.DateStatusChanged,
                MeterChangeOutStatus = search.MeterChangeOutStatus,
                OperatingCenter = search.OperatingCenter,
                EnablePaging = false
            };
            var weekly = Repository.Search(newSearch).OrderBy(x => x.DateStatusChanged).Select(x => x.ToExcelSummaryRecord()).ToList();
            return weekly;
        }

        private IEnumerable<MeterChangeOutExcelSummaryRecord> YearToDateExport(SearchMeterChangeOutCompletions search)
        {
            var ytdSearch = new SearchMeterChangeOut {
                CalledInByContractorMeterCrew = search.CalledInByContractorMeterCrew,
                DateStatusChanged = search.DateStatusChanged,
                MeterChangeOutStatus = search.MeterChangeOutStatus,
                OperatingCenter = search.OperatingCenter,
                EnablePaging = false
            };
            if (ytdSearch.DateStatusChanged.Operator != RangeOperator.Between)
            {
                var year = ytdSearch.DateStatusChanged.End.Value.Year;
                ytdSearch.DateStatusChanged.Start = new DateTime(year, 1, 1);
                ytdSearch.DateStatusChanged.End = new DateTime(year, 12, 31);
            }
            else
            {
                ytdSearch.DateStatusChanged.Start = new DateTime(ytdSearch.DateStatusChanged.Start.Value.Year, 1, 1);
                ytdSearch.DateStatusChanged.End = new DateTime(ytdSearch.DateStatusChanged.End.Value.Year, 12, 31);
            }
            return Repository.Search(ytdSearch).OrderBy(x => x.DateStatusChanged).Select(x => x.ToExcelSummaryRecord()).ToList();
        }



        #endregion

        #region Actions

        private string GetExportFilename(SearchMeterChangeOutCompletions search)
        {
            Func<DateTime?, string> doWeirdDateFormat =
                   (d) => {
                       // If this code is still running after the year 2099, I'm sorry. -Ross 12/5/2016
                       // They requested the date only have the year formatted with the last two digits bug 3405. 

                       if (!d.HasValue) { return string.Empty; }

                       var val = d.Value;
                       return $"{val.Month:D2}{val.Day:D2}{val:yy}";
                   };

            var opc = "";

            if (search.OperatingCenter != null)
            {
                var opcRepo =
                    _container.GetInstance<IOperatingCenterRepository>();

                var opCenters = search.OperatingCenter.Select(x => opcRepo.Find(x).OperatingCenterCode);
                opc = string.Join("_", opCenters);
            }

            var start = doWeirdDateFormat(search.DateStatusChanged?.Start);
            var end = doWeirdDateFormat(search.DateStatusChanged?.End);
            return $"MeterChangeOuts_{start}_{end}_{opc}";
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchMeterChangeOutCompletions search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => {
                        search.EnablePaging = false;
                        var results = Repository.GetCompletionsReport(search).ToList();
                        search.Count = results.Count;
                        search.Results = results;
                    }
                }));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var weekly = WeeklyExport(search);
                    var weeklySummary = weekly.GroupBy(x => new { x.Date, x.MeterSize, x.OperatingCenter}).Select(z => new {
                        z.Key.Date,
                        Changed = z.Count(),
                        z.Key.MeterSize,
                        z.Key.OperatingCenter
                    });
                    return new ExcelResult(GetExportFilename(search))
                        .AddSheet(weekly, new MMSINC.Utilities.Excel.ExcelExportSheetArgs { SheetName = "Weekly" })
                        .AddSheet(YearToDateExport(search), new MMSINC.Utilities.Excel.ExcelExportSheetArgs { SheetName = "YTD" })
                        .AddSheet(weeklySummary, new MMSINC.Utilities.Excel.ExcelExportSheetArgs { SheetName = "DailyTallyBySize" });
                });
            });
        }
        
        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            var now = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            return ActionHelper.DoSearch(new SearchMeterChangeOutCompletions
            {
                DateStatusChanged = new DateRange() {
                    Start = now.AddDays(-(int)now.DayOfWeek).BeginningOfDay(),
                    End = now.AddDays(7-(int)now.DayOfWeek).EndOfDay(),
                    Operator = RangeOperator.Between
                },
                MeterChangeOutStatus = new[] {MeterChangeOutStatus.Indices.CHANGED}
            });
        }

        #endregion
    }
}