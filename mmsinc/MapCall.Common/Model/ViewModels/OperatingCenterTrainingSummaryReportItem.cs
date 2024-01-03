using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.ViewModels
{
    public class OperatingCenterTrainingOverviewReportItem
    {
        public OperatingCenter OperatingCenter { get; set; }

        public int Employees { get; set; }

        // how many trainging records should exist?
        public int EmployeeTrainingRecordsRequired { get; set; }
        public int EmployeeTrainingRecordsCompleted { get; set; }
        public int EmployeeTrainingRecordsDue { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.PERCENTAGE)]
        public decimal? Percentage
        {
            get
            {
                if (EmployeeTrainingRecordsRequired > 0)
                    return (decimal)EmployeeTrainingRecordsCompleted / EmployeeTrainingRecordsRequired;
                return null;
            }
        }
    }

    public class OperatingCenterTrainingSummaryReportItem
    {
        private DateTime? _expirationDateTime;

        public DateTime ExpirationDateTime
        {
            get { return _expirationDateTime.HasValue ? _expirationDateTime.Value : DateTime.Now; }
            set { _expirationDateTime = value; }
        }

        public OperatingCenter OperatingCenter { get; set; }
        public TrainingRequirement TrainingRequirement { get; set; }

        public string Recurring
        {
            get { return TrainingRequirement.HasRecurringTrainingRequirement ? "Yes" : "No"; }
        }

        public int EmployeesInPositionGroups { get; set; }
        public int EmployeesOverDueInitialTraining { get; set; }
        public int EmployeesOverDueRecurringTraining { get; set; }
        public int EmployeesOverDueInitialAndRecurringTraining { get; set; }
        public int EmployeeTrainingRecordsRequired { get; set; }
        public int EmployeesScheduledForClasses { get; set; }
        public int ClassesScheduled { get; set; }
        public int OperatingCenterClassesScheduled { get; set; }

        public string IsOSHARequirement
        {
            get { return TrainingRequirement.IsOSHARequirement ? "Yes" : "No"; }
        }

        protected void SetEmployeesOverDueInitialTraining()
        {
            if (TrainingRequirement.ActiveInitialAndRecurringTrainingModule != null)
            {
                EmployeesOverDueInitialTraining = 0;
            }
            else
            {
                EmployeesOverDueInitialTraining =
                    (TrainingRequirement.TrainingFrequency.HasValue && TrainingRequirement.TrainingFrequency > 0)
                        ? // has a recurring requirement
                        OperatingCenter.Employees.Count(e =>
                            e.IsActive && e.PositionGroup != null && e.PositionGroup.CommonName != null
                            && TrainingRequirement.PositionGroupCommonNames.Any(a =>
                                e.PositionGroup.CommonName.Id == a.Id)
                            && e.AttendedTrainingRecords.Count(atr =>
                                atr.TrainingRecord.TrainingModule != null
                                && atr.TrainingRecord.TrainingModule.TrainingRequirement != null
                                && atr.TrainingRecord.TrainingModule.TrainingRequirement == TrainingRequirement) == 0)
                        : // no recurring requirement, one and done
                        OperatingCenter.Employees.Count(e =>
                            e.IsActive && e.PositionGroup != null && e.PositionGroup.CommonName != null
                            && TrainingRequirement.PositionGroupCommonNames.Any(a =>
                                e.PositionGroup.CommonName.Id == a.Id)
                            && e.AttendedTrainingRecords.Count(atr =>
                                atr.TrainingRecord.TrainingModule == TrainingRequirement.ActiveInitialTrainingModule) ==
                            0);
            }
        }

        protected void SetEmployeesOverDueRecurringTraining()
        {
            if (TrainingRequirement.ActiveInitialAndRecurringTrainingModule != null)
            {
                EmployeesOverDueRecurringTraining = 0;
            }
            else
            {
                if (TrainingRequirement.TrainingFrequency.HasValue && TrainingRequirement.TrainingFrequency > 0)
                {
                    EmployeesOverDueRecurringTraining =
                        OperatingCenter.Employees.Count(e =>
                            e.IsActive && e.PositionGroup != null && e.PositionGroup.CommonName != null
                            && TrainingRequirement.PositionGroupCommonNames.Any(a =>
                                e.PositionGroup.CommonName.Id == a.Id)
                            &&
                            // they have taken training in the past
                            e.AttendedTrainingRecords.Count(atr =>
                                atr.TrainingRecord.TrainingModule != null
                                && atr.TrainingRecord.TrainingModule.TrainingRequirement != null
                                && atr.TrainingRecord.TrainingModule.TrainingRequirement == TrainingRequirement) > 0
                            &&
                            // they don't have a current record
                            e.AttendedTrainingRecords.Count(atr =>
                                atr.TrainingRecord.TrainingModule != null
                                && atr.TrainingRecord.TrainingModule.TrainingRequirement != null
                                && atr.TrainingRecord.TrainingModule.TrainingRequirement == TrainingRequirement
                                && atr.TrainingRecord.HeldOn >
                                TrainingRequirement.TrainingFrequencyObj.GetExpirationDateFrom(ExpirationDateTime)) == 0
                        );
                }
                else
                {
                    EmployeesOverDueRecurringTraining = 0;
                }
            }
        }

        protected void SetEmployeesOverDueInitialAndRecurringTraining() // combined training modules
        {
            if (TrainingRequirement.ActiveInitialAndRecurringTrainingModule != null)
            {
                EmployeesOverDueInitialAndRecurringTraining =
                    OperatingCenter.Employees.Count(e =>
                        e.IsActive && e.PositionGroup != null && e.PositionGroup.CommonName != null
                        && TrainingRequirement.PositionGroupCommonNames.Any(a => e.PositionGroup.CommonName.Id == a.Id)
                        && e.AttendedTrainingRecords.Count(atr =>
                            atr.TrainingRecord.TrainingModule != null
                            && atr.TrainingRecord.TrainingModule.TrainingRequirement != null
                            && atr.TrainingRecord.TrainingModule.TrainingRequirement == TrainingRequirement
                            && atr.TrainingRecord.HeldOn >
                            TrainingRequirement.TrainingFrequencyObj.GetExpirationDateFrom(ExpirationDateTime)) == 0);
            }
            else
                EmployeesOverDueInitialAndRecurringTraining = 0;
        }

        protected void SetEmployeeTrainingRecordsRequired()
        {
            EmployeeTrainingRecordsRequired = OperatingCenter.Employees.Count(e =>
                e.IsActive && e.PositionGroup != null && e.PositionGroup.CommonName != null
                && TrainingRequirement.PositionGroupCommonNames.Any(a => e.PositionGroup.CommonName.Id == a.Id));
        }

        public void SetEmployeeValues()
        {
            SetEmployeesOverDueInitialTraining();
            SetEmployeesOverDueRecurringTraining();
            SetEmployeesOverDueInitialAndRecurringTraining();
            SetEmployeeTrainingRecordsRequired();
        }
    }

    public interface ISearchOperatingCenterTrainingSummary : ISearchSet<OperatingCenterTrainingSummaryReportItem>
    {
        int[] OperatingCenter { get; set; }
        int? TrainingRequirement { get; set; }
        int? State { get; set; }
        bool? IsOSHARequirement { get; set; }
        DateTime? DueBy { get; set; }
    }

    public interface ISearchOperatingCenterTrainingOverview : ISearchSet<OperatingCenterTrainingOverviewReportItem>
    {
        int? State { get; set; }
        bool? IsOSHARequirement { get; set; }
    }
}
