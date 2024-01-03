using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallScheduler.JobHelpers.NonRevenueWaterEntryCreator
{
    /// <summary>
    ///     A service which creates Non-Revenue Water entries for each Operating Center at the beginning of every month.
    /// </summary>
    public class NonRevenueWaterEntryCreatorService : INonRevenueWaterEntryCreatorService
    {
        #region Constants

        private const string BUSINESS_UNIT_PLACEHOLDER = "999999";

        #endregion

        #region Private Members

        private readonly ILog _log;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IRepository<OperatingCenter> _operatingCenterRepository;
        private readonly IRepository<WorkOrder> _workOrderRepository;
        private readonly IHydrantInspectionRepository _hydrantInspectionRepository;
        private readonly IRepository<NonRevenueWaterEntry> _nonRevenueWaterRepository;
        private DateTime _currentDate;
        private DateTime _lastMonth;

        #endregion

        #region Constructors

        public NonRevenueWaterEntryCreatorService(
            ILog log,
            IDateTimeProvider dateTimeProvider,
            IRepository<OperatingCenter> operatingCenterRepository,
            IRepository<WorkOrder> workOrderRepository,
            IHydrantInspectionRepository hydrantInspectionRepository,
            IRepository<NonRevenueWaterEntry> nonRevenueWaterRepository)
        {
            _log = log;
            _dateTimeProvider = dateTimeProvider;
            _operatingCenterRepository = operatingCenterRepository;
            _workOrderRepository = workOrderRepository;
            _hydrantInspectionRepository = hydrantInspectionRepository;
            _nonRevenueWaterRepository = nonRevenueWaterRepository;
        }

        #endregion

        #region Private Methods

        private NonRevenueWaterEntry BuildNonRevenueWaterEntry(OperatingCenter operatingCenter,
            IEnumerable<HydrantFlushingReportItem> hydrantFlushings,
            SearchWaterLoss waterLoss)
        {
            var nonRevenueWaterEntry = new NonRevenueWaterEntry {
                OperatingCenter = operatingCenter,
                Month = _lastMonth.Month,
                Year = _lastMonth.Year
            };

            foreach (var flush in hydrantFlushings.Where(flush => flush.TotalGallons > 0))
            {
                nonRevenueWaterEntry.NonRevenueWaterDetails.Add(new NonRevenueWaterDetail {
                    NonRevenueWaterEntry = nonRevenueWaterEntry,
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_lastMonth.Month),
                    Year = _lastMonth.Year.ToString(),
                    BusinessUnit = string.IsNullOrWhiteSpace(flush.BusinessUnit)
                        ? BUSINESS_UNIT_PLACEHOLDER
                        : flush.BusinessUnit,
                    WorkDescription = "HYDRANT FLUSHING",
                    TotalGallons = decimal.ToInt64(flush.TotalGallons)
                });
            }

            foreach (var lostWater in waterLoss.Results)
            {
                nonRevenueWaterEntry.NonRevenueWaterDetails.Add(new NonRevenueWaterDetail {
                    NonRevenueWaterEntry = nonRevenueWaterEntry,
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(lostWater.Month),
                    Year = lostWater.Year.ToString(),

                    // Business Unit is required; however, the Water Loss report returns records with a null BU.
                    // Replacing nulls with 999999 for now
                    BusinessUnit = string.IsNullOrWhiteSpace(lostWater.BusinessUnit)
                        ? BUSINESS_UNIT_PLACEHOLDER
                        : lostWater.BusinessUnit,
                    WorkDescription = lostWater.WorkDescriptionDescription,
                    TotalGallons = (int)lostWater.TotalGallons
                });
            }

            return nonRevenueWaterEntry;
        }

        private SearchWaterLoss GetWaterLoss(IEntity operatingCenter)
        {
            var searchWaterLoss = new SearchWaterLoss {
                Date = new RequiredDateRange {
                    Operator = RangeOperator.Between,
                    Start = _lastMonth.GetBeginningOfMonth(),
                    End = _lastMonth.GetEndOfMonth()
                },
                OperatingCenter = new[] { operatingCenter.Id }
            };

            _workOrderRepository.GetWaterLossReport(searchWaterLoss);

            return searchWaterLoss;
        }

        private List<HydrantFlushingReportItem> GetHydrantFlushings(IEntity operatingCenter) =>
            _hydrantInspectionRepository.GetFlushingReport(new SearchHydrantFlushing {
                                             OperatingCenter = operatingCenter.Id,
                                             Year = _lastMonth.Year
                                         })
                                        .Where(x => x.Month == _lastMonth.Month)
                                        .ToList();

        #endregion

        #region Exposed Methods

        public void Process()
        {
            // To run locally any day of the month:
            // _currentDate = _dateTimeProvider.GetCurrentDate().GetBeginningOfMonth(); 
            _currentDate = _dateTimeProvider.GetCurrentDate();
            _lastMonth = _currentDate.AddMonths(-1);

            var nonRevenueWaterEntries = new List<NonRevenueWaterEntry>();

            // Is today the first of the month?
            // If it isn't, just move along, nothing to see here.
            // Otherwise, carry on my wayward son
            if (_currentDate.Date != _currentDate.GetBeginningOfMonth().Date)
            {
                return;
            }

            _log.Info("Begin Non Revenue Water Entry creation process.");

            // Grab all the active Operating Centers that can have Work Orders.
            // Work Orders are how Water Loss (aka Non-Revenue Water) is reported in MapCall
            var operatingCenters =
                _operatingCenterRepository.Linq.Where(x => x.IsActive && x.WorkOrdersEnabled);

            // Retrieve Water Loss and Hydrant Flushing info from last month for each Operating Center
            // and create a Non-Revenue Water entry, regardless of whether the OC has water loss or not.
            foreach (var operatingCenter in operatingCenters)
            {
                var waterLoss = GetWaterLoss(operatingCenter);
                var hydrantFlushings = GetHydrantFlushings(operatingCenter);
                nonRevenueWaterEntries.Add(BuildNonRevenueWaterEntry(operatingCenter, hydrantFlushings, waterLoss));

                _log.Info($"NonRevenueWater entry created for {operatingCenter.Description} with" +
                          $" {waterLoss.Results.Count()} water loss records and {hydrantFlushings.Count} hydrant flushing records.");
            }

            _log.Info($"saving {nonRevenueWaterEntries.Count} non revenue water entries.");
            _nonRevenueWaterRepository.Save(nonRevenueWaterEntries);

            _log.Info("End Non Revenue Water Entry creation process.");
        }

        #endregion
    }
}
