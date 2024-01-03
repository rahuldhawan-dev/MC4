using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.ViewModels
{
    public class EmployeeTrainingReportItem
    {
        #region Properties

        public string OperatingCenter { get; set; }

        [DisplayName("OSHA Requirement")]
        public bool? OSHARequirement { get; set; }

        public string TrainingRequirement { get; set; }
        public int TrainingRequirementId { get; set; }
        public string ActiveInitialTrainingModule { get; set; }
        public string ActiveInitialAndRecurringTrainingModule { get; set; }
        public string ActiveRecurringTrainingModule { get; set; }
        public int ActiveInitialTrainingModuleId { get; set; }
        public int ActiveInitialAndRecurringTrainingModuleId { get; set; }
        public int ActiveRecurringTrainingModuleId { get; set; }
        public float? TotalHours { get; set; }
        public string CommonName { get; set; }
        public string Position { get; set; }
        public int PositionGroupCommonNameId { get; set; }
        public string PositionGroup { get; set; }
        public int PositionGroupId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int EmployeeId { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public DateTime? RecentTrainingDate { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public DateTime? NextScheduledDate { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public DateTime? NextDueByDate { get; set; }

        #endregion
    }

    public class EmployeeTrainingByDateReportItem
    {
        #region Properties

        public string OperatingCenter { get; set; }

        [DisplayName("OSHA Requirement")]
        public bool? OSHARequirement { get; set; }

        public string TrainingRequirement { get; set; }
        public int TrainingRequirementId { get; set; }
        public string ActiveInitialTrainingModule { get; set; }
        public string ActiveInitialAndRecurringTrainingModule { get; set; }
        public string ActiveRecurringTrainingModule { get; set; }
        public int ActiveInitialTrainingModuleId { get; set; }
        public int ActiveInitialAndRecurringTrainingModuleId { get; set; }
        public int ActiveRecurringTrainingModuleId { get; set; }
        public float? TotalHours { get; set; }
        public string CommonName { get; set; }
        public string Position { get; set; }
        public int PositionGroupCommonNameId { get; set; }
        public string PositionGroup { get; set; }
        public int PositionGroupId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int EmployeeId { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public DateTime? DateAttended { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public DateTime? RecentTrainingDate { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public DateTime? NextScheduledDate { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public DateTime? NextDueByDate { get; set; }

        #endregion
    }

    public class EmployeeTrainingRecordExportItem
    {
        #region Constants

        public const string
            ITEM_TYPE = "CIL",
            COMPLETION_STATUS = "CIL-COMPLETE";

        #endregion

        #region Properties

        [DoesNotExport]
        public DateTime? AttendeesExportedDate { get; set; }

        [DoesNotExport]
        public bool Exported { get; set; }

        [DoesNotExport]
        public TrainingRecord TrainingRecord { get; set; }

        [DoesNotExport, DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public DateTime? ScheduledDate { get; set; }

        [DoesNotExport, DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public DateTime? HeldOn { get; set; }

        [DisplayName("User ID")]
        public string EmployeeId { get; set; }

        [DisplayName("LEARN Item Type")]
        public string ItemType { get; set; }

        [DisplayName("Item ID")]
        public string ItemID { get; set; }

        [DisplayName("Item Revision Date")]
        public string ItemRevisionDate { get; set; }

        [DisplayName("Item Revision Time")]
        public string ItemRevisionTime { get; set; }

        [DisplayName("Grade")]
        public string Grade { get; set; }

        [DisplayName("Completion Status")]
        public string CompletionStatus { get; set; }

        #region Completion Date

        [DoesNotExport]
        public DateTime CompletionDateTime { get; set; }

        [DisplayName("Completion Date")]
        public string CompletionDate
        {
            get { return String.Format(CommonStringFormats.DATE, CompletionDateTime); }
        }

        [DisplayName("Completion Time"), DisplayFormat(DataFormatString = CommonStringFormats.TIME_12)]
        public string CompletionTime
        {
            get { return String.Format(CommonStringFormats.TIME_12, CompletionDateTime); }
        }

        #endregion

        [DisplayName("Credit Hours")]
        public string CreditHours { get; set; }

        [DisplayName("CPE Hours")]
        public string CPEHours { get; set; }

        [DisplayName("Contact Hours")]
        public float? ContactHours { get; set; }

        [DisplayName("Total Hours")]
        public float? TotalHours { get; set; }

        #region Instructor Name

        [DoesNotExport]
        public Employee Instructor { get; set; }

        [DoesNotExport]
        public string OutsideInstructor { get; set; }

        [DisplayName("Instructor Name")]
        public string InstructorName
        {
            get { return (Instructor != null) ? Instructor.FullName : OutsideInstructor; }
        }

        #endregion

        #region Comments

        [DisplayName("Comments")]
        public string Comments
        {
            get { return string.Join(",", TrainingRecord.TrainingRecordNotes.Select(x => x.Note.Text)); }
        }

        #endregion

        [DisplayName("Scheduled Offering ID")]
        public string ScheduledOfferingID { get; set; }

        [DisplayName("Include in government reporting")]
        public string IncludeInGovernmentReporting { get; set; }

        [DisplayName("2483 Legal Entity")]
        public string e2483LegalEntity { get; set; }

        [DisplayName("2483 Employee Class")]
        public string e2483EmployeeClass { get; set; }

        [DisplayName("Hourly Rate")]
        public string HourlyRate { get; set; }

        [DisplayName("Hourly Rate Currency")]
        public string HourlyRateCurrency { get; set; }

        [DisplayName("Adjusted Hourly Rate")]
        public string AdjustedHourlyRate { get; set; }

        [DisplayName("Adjusted Hourly Rate Currency")]
        public string AdjustedHourlyRateCurrency { get; set; }

        [DisplayName("Training Purpose ID")]
        public string TrainingPurposeID { get; set; }

        [DisplayName("Training Action Category ID")]
        public string TrainingActionCategoryID { get; set; }

        [DisplayName("2483 Program I")]
        public string e2483ProgramI { get; set; }

        [DisplayName("2483 Program Hours I")]
        public string e2483ProgramHoursI { get; set; }

        [DisplayName("Training Funding I")]
        public string TrainingFundingI { get; set; }

        [DisplayName("Hours Outside of Work I")]
        public string HoursOutsideOfWorkI { get; set; }

        [DisplayName("Hours During Work I")]
        public string HoursDuringWorkI { get; set; }

        [DisplayName("2483 Program III")]
        public string e2483ProgramIII { get; set; }

        [DisplayName("2483 Program Hours III")]
        public string e2483ProgramHoursIII { get; set; }

        [DisplayName("Training Funding III")]
        public string TrainingFundingIII { get; set; }

        [DisplayName("Hours Outside of Work III")]
        public string HoursOutsideOfWorkIII { get; set; }

        [DisplayName("Hours During Work III")]
        public string HoursDuringWorkIII { get; set; }

        #endregion
    }

    public class EmployeeUserReconciliationReportItem
    {
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public int EmployeeId { get; set; }
        public int UserId { get; set; }
        public string EmployeeNumber { get; set; }
        public string EmployeeOperatingCenter { get; set; }
        public string UserOperatingCenter { get; set; }
    }

    public interface ISearchEmployeeTraining : ISearchSet<EmployeeTrainingReportItem>
    {
        #region Properties

        //[SearchAlias("OperatingCenter", "opCntr", "Id")]
        int? OperatingCenter { get; set; }

        bool? OSHARequirement { get; set; }

        //[SearchAlias("record.TrainingModule", "module", "Id")]
        int? TrainingRequirement { get; set; }

        //[SearchAlias("PositionGroupCommonName", "commonName", "Id")]
        int? PositionGroupCommonName { get; set; }

        //[SearchAlias("PositionGroup", "position", "Id")]
        int? PositionGroup { get; set; }

        string LastName { get; set; }

        [Search(CanMap = false)]
        DateRange RecentTrainingDate { get; set; }

        [Search(CanMap = false)]
        DateRange NextScheduledDate { get; set; }

        [Search(CanMap = false)]
        DateRange NextDueByDate { get; set; }

        #endregion
    }

    public interface ISearchEmployeeTrainingByDate : ISearchSet<EmployeeTrainingByDateReportItem>
    {
        #region Properties

        //[SearchAlias("OperatingCenter", "opCntr", "Id")]
        int? OperatingCenter { get; set; }

        bool? OSHARequirement { get; set; }

        //[SearchAlias("record.TrainingModule", "module", "Id")]
        int? TrainingRequirement { get; set; }

        //[SearchAlias("PositionGroupCommonName", "commonName", "Id")]
        int? PositionGroupCommonName { get; set; }

        //[SearchAlias("PositionGroup", "position", "Id")]
        int? PositionGroup { get; set; }

        string LastName { get; set; }

        [Search(CanMap = false)]
        DateRange RecentTrainingDate { get; set; }

        [Search(CanMap = false)]
        DateRange NextScheduledDate { get; set; }

        [Search(CanMap = false)]
        DateRange NextDueByDate { get; set; }

        [Search(CanMap = false)]
        DateRange DateAttended { get; set; }

        #endregion
    }

    public interface ISearchEmployeeUserReconciliation : ISearchSet<EmployeeUserReconciliationReportItem>
    {
        [SearchAlias("OperatingCenter", "eOpCntr", "Id")]
        int? EmployeeOperatingCenter { get; set; }

        [SearchAlias("user.OperatingCenter", "uOpCntr", "Id")]
        int? UserOperatingCenter { get; set; }
    }
}
