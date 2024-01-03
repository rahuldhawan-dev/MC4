using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class OperatingCenterRepository : RepositoryBase<OperatingCenter>, IOperatingCenterRepository
    {
        #region Fields

        private readonly IRepository<TrainingRequirement> _trainingRequirementRepository;
        private readonly IRepository<TrainingRecord> _trainingRecordRepository;

        #endregion

        #region Constructors

        public OperatingCenterRepository(ISession session, IContainer container,
            IRepository<TrainingRequirement> trainingRequirementRepository,
            IRepository<TrainingRecord> trainingRecordRepository) : base(session, container)
        {
            _trainingRequirementRepository = trainingRequirementRepository;
            _trainingRecordRepository = trainingRecordRepository;
        }

        #endregion

        #region Exposed Methods

        #region Lookups/Casades

        public override IQueryable<OperatingCenter> GetAllSorted()
        {
            return Linq.OrderBy(x => x.OperatingCenterCode).AsQueryable();
        }

        public IQueryable<OperatingCenter> GetByStateId(int stateId)
        {
            return (from oc in Linq where oc.State.Id == stateId select oc);
        }

        public IQueryable<OperatingCenter> GetByTownId(int townId)
        {
            return (from oc in Linq where oc.OperatingCenterTowns.Any(x => x.Town.Id == townId) select oc);
        }

        public IQueryable<OperatingCenter> GetAllWithBappTeams()
        {
            return (from oc in Linq where oc.BappTeams.Any() select oc).ToList().AsQueryable();
        }

        public OperatingCenter GetByOperatingCenterCode(string opCode)
        {
            return Linq.SingleOrDefault(x => x.OperatingCenterCode == opCode);
        }

        #endregion

        #region Reports

        // TODO: Bug #2750 What if they had initial training for a previously assigned initial/recurring training module?
        public IEnumerable<OperatingCenterTrainingSummaryReportItem> GetOperatingCenterTrainingSummaryReportItems(
            ISearchOperatingCenterTrainingSummary search)
        {
            // USE LINQ TO DO THE CROSS JOIN BETWEEN OPERATING CENTERS AND TRAINING REQUIREMENTS
            var trainingRequirements = _trainingRequirementRepository.GetAll();
            var operatingCenterTrainingRequirements =
                (from oc in base.Linq
                 from tr in trainingRequirements
                 where
                     tr.IsActive
                 select
                     new OperatingCenterTrainingSummaryReportItem {
                         OperatingCenter = oc,
                         TrainingRequirement = tr,
                     }).ToList();

            if (search.OperatingCenter != null)
            {
                operatingCenterTrainingRequirements =
                    operatingCenterTrainingRequirements
                       .Where(x => search.OperatingCenter.Contains(x.OperatingCenter.Id))
                       .ToList();
            }

            if (search.TrainingRequirement.HasValue)
            {
                operatingCenterTrainingRequirements =
                    operatingCenterTrainingRequirements.Where(
                        tr => tr.TrainingRequirement.Id == search.TrainingRequirement).ToList();
            }

            if (search.IsOSHARequirement.HasValue)
            {
                operatingCenterTrainingRequirements =
                    operatingCenterTrainingRequirements.Where(
                        tr => tr.TrainingRequirement.IsOSHARequirement == search.IsOSHARequirement.Value).ToList();
            }

            var finalResults = new List<OperatingCenterTrainingSummaryReportItem>();
            finalResults.AddRange(operatingCenterTrainingRequirements.Select(x =>
                new OperatingCenterTrainingSummaryReportItem {
                    OperatingCenter = x.OperatingCenter,
                    TrainingRequirement = x.TrainingRequirement,
                    EmployeesInPositionGroups = x.OperatingCenter.Employees.Count(e => e.IsActive
                        && x.TrainingRequirement != null
                        && x.TrainingRequirement.PositionGroupCommonNames != null
                        && x.TrainingRequirement.PositionGroupCommonNames.Any(a =>
                            e.PositionGroup != null && e.PositionGroup.CommonName != null &&
                            e.PositionGroup.CommonName.Id == a.Id)
                    )
                }
            ));

            // Classes Scheduled > today;
            var scheduledClasses = _trainingRecordRepository.Where(x => x.ScheduledDate > DateTime.Now).ToList();

            // go lookup the counts and populate them?
            foreach (var reportitem in finalResults)
            {
                reportitem.ClassesScheduled = scheduledClasses.Count(x => x.TrainingModule != null &&
                                                                          x.TrainingModule.TrainingRequirement ==
                                                                          reportitem.TrainingRequirement
                                                                          && (x.TrainingModule ==
                                                                              reportitem.TrainingRequirement
                                                                                 .ActiveInitialTrainingModule
                                                                              || x.TrainingModule ==
                                                                              reportitem.TrainingRequirement
                                                                                 .ActiveRecurringTrainingModule
                                                                              || x.TrainingModule ==
                                                                              reportitem.TrainingRequirement
                                                                                 .ActiveInitialAndRecurringTrainingModule
                                                                          ));
                reportitem.OperatingCenterClassesScheduled = scheduledClasses.Count(x => x.ClassLocation != null &&
                    x.ClassLocation.OperatingCenter == reportitem.OperatingCenter
                    && (x.TrainingModule == reportitem.TrainingRequirement.ActiveInitialTrainingModule
                        || x.TrainingModule == reportitem.TrainingRequirement.ActiveRecurringTrainingModule
                        || x.TrainingModule == reportitem.TrainingRequirement.ActiveInitialAndRecurringTrainingModule));
                reportitem.EmployeesScheduledForClasses = scheduledClasses.Where(x =>
                                                                               (x.TrainingModule ==
                                                                                   reportitem.TrainingRequirement
                                                                                      .ActiveInitialTrainingModule
                                                                                   || x.TrainingModule ==
                                                                                   reportitem.TrainingRequirement
                                                                                      .ActiveRecurringTrainingModule
                                                                                   || x.TrainingModule ==
                                                                                   reportitem.TrainingRequirement
                                                                                      .ActiveInitialAndRecurringTrainingModule
                                                                               ) &&
                                                                               x.ClassLocation != null &&
                                                                               x.ClassLocation.OperatingCenter.Id ==
                                                                               reportitem.OperatingCenter.Id)
                                                                          .Sum(x => x.EmployeesScheduled.Count(y =>
                                                                               y.Employee.OperatingCenter ==
                                                                               reportitem.OperatingCenter));
                if (search.DueBy.HasValue)
                {
                    reportitem.ExpirationDateTime = search.DueBy.Value;
                }

                reportitem.SetEmployeeValues();
            }

            var ret =
                finalResults.OrderBy(x => x.OperatingCenter.OperatingCenterCode)
                            .ThenBy(x => x.TrainingRequirement.Description)
                            .ToList();
            search.Count = ret.Count;
            return ret;
        }

        public IEnumerable<OperatingCenterTrainingOverviewReportItem> GetOperatingCenterTrainingOverviewReportItems(
            ISearchOperatingCenterTrainingOverview search)
        {
            var trainingRequirements = _trainingRequirementRepository.GetAll();
            var operatingCenterTrainingRequirements =
                (from oc in base.Linq
                 from tr in trainingRequirements
                 where
                     tr.IsActive
                 select
                     new OperatingCenterTrainingSummaryReportItem {
                         OperatingCenter = oc,
                         TrainingRequirement = tr
                     }).ToList();

            if (search.State.HasValue)
            {
                operatingCenterTrainingRequirements =
                    operatingCenterTrainingRequirements.Where(x => search.State == x.OperatingCenter.State.Id).ToList();
            }

            if (search.IsOSHARequirement.HasValue)
            {
                operatingCenterTrainingRequirements =
                    operatingCenterTrainingRequirements.Where(
                        tr => tr.TrainingRequirement.IsOSHARequirement == search.IsOSHARequirement.Value).ToList();
            }

            foreach (var reportitem in operatingCenterTrainingRequirements)
            {
                reportitem.SetEmployeeValues();
            }

            var finalResults = new List<OperatingCenterTrainingOverviewReportItem>();
            finalResults.AddRange(
                operatingCenterTrainingRequirements
                   .GroupBy(x => x.OperatingCenter)
                   .Select(y => new OperatingCenterTrainingOverviewReportItem
                        {OperatingCenter = y.Key, Employees = y.Key.Employees.Count(e => e.IsActive)})
            );

            foreach (var item in finalResults)
            {
                item.EmployeeTrainingRecordsDue = operatingCenterTrainingRequirements
                                                 .Where(x => x.OperatingCenter == item.OperatingCenter)
                                                 .Sum(x => x.EmployeesOverDueRecurringTraining +
                                                           x.EmployeesOverDueInitialTraining +
                                                           x.EmployeesOverDueInitialAndRecurringTraining);

                item.EmployeeTrainingRecordsRequired = operatingCenterTrainingRequirements
                                                      .Where(x => x.OperatingCenter == item.OperatingCenter)
                                                      .Sum(x => x.EmployeeTrainingRecordsRequired);

                item.EmployeeTrainingRecordsCompleted =
                    item.EmployeeTrainingRecordsRequired - item.EmployeeTrainingRecordsDue;
            }

            var ret = finalResults.ToList().OrderBy(x => x.OperatingCenter.OperatingCenterCode);
            search.Count = ret.Count();
            return ret;
        }

        public IEnumerable<ArcFlashCompletionReportItem> GetArcFlashCompletions(
            ISearchSet<ArcFlashCompletionReportItem> search)
        {
            var results = new List<ArcFlashCompletionReportItem>();

            foreach (var operatingCenter in GetAllSorted())
            {
                var completed = operatingCenter.Facilities.Select(f => 
                    f.ArcFlashStudies.Count(s => s.ArcFlashStatus != null && s.ArcFlashStatus.Id == ArcFlashStatus.Indices.COMPLETED)).Sum();
                var pending = operatingCenter.Facilities.Select(f => 
                    f.ArcFlashStudies.Count(s => s.ArcFlashStatus != null && s.ArcFlashStatus.Id == ArcFlashStatus.Indices.PENDING)).Sum();
                var deferred = operatingCenter.Facilities.Select(f => 
                    f.ArcFlashStudies.Count(s => s.ArcFlashStatus != null && s.ArcFlashStatus.Id == ArcFlashStatus.Indices.DEFFERED)).Sum();
                
                var total = completed + pending + deferred;

                results.Add(new ArcFlashCompletionReportItem {
                    OperatingCenter = operatingCenter.Description,
                    TotalArcFlashFacilities = total,
                    NumberCompleted = completed,
                    NumberPending = pending,
                    NumberDeferred = deferred,
                    TotalCompletedNotDeferred = total == 0 ? 0 : completed / total
                });
            }

            search.Results = results;
            search.Count = results.Count;
            return search.Results;
        }

        #endregion

        #endregion
    }

    public interface IOperatingCenterRepository : IRepository<OperatingCenter>
    {
        #region Lookups/Cascades

        IQueryable<OperatingCenter> GetByStateId(int stateId);
        IQueryable<OperatingCenter> GetByTownId(int townId);
        IQueryable<OperatingCenter> GetAllWithBappTeams();
        OperatingCenter GetByOperatingCenterCode(string opCode);

        #endregion

        #region Reports

        IEnumerable<OperatingCenterTrainingSummaryReportItem> GetOperatingCenterTrainingSummaryReportItems(
            ISearchOperatingCenterTrainingSummary search);

        IEnumerable<OperatingCenterTrainingOverviewReportItem> GetOperatingCenterTrainingOverviewReportItems(
            ISearchOperatingCenterTrainingOverview search);

        IEnumerable<ArcFlashCompletionReportItem> GetArcFlashCompletions(
            ISearchSet<ArcFlashCompletionReportItem> search);

        #endregion
    }
}
