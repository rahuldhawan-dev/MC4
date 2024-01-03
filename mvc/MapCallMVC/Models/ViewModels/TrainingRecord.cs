using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class TrainingRecordViewModel : ViewModel<TrainingRecord>
    {
        #region Constants

        public const int DEFAULT_MAXIMUM_CLASS_SIZE = 10;

        #endregion

        #region Properties

        [EntityMustExist(typeof(TrainingModule))]
        [EntityMap]
        [ComboBox]
        [Required]
        public virtual int? TrainingModule { get; set; }
        [EntityMustExist(typeof(Employee))]
        [EntityMap]
        [DropDown]
        public virtual int? Instructor { get; set; }
        [EntityMustExist(typeof(Employee))]
        [EntityMap]
        [DropDown]
        public virtual int? SecondInstructor { get; set; }
        [EntityMustExist(typeof(ClassLocation))]
        [EntityMap]
        [DropDown]
        [Required]
        public virtual int? ClassLocation { get; set; }
        public virtual DateTime? HeldOn { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode=true)]
        public virtual DateTime? ScheduledDate { get; set; }
        [Required, DefaultValue(10), Range(1, int.MaxValue)]
        public virtual int? MaximumClassSize { get; set; }

        // display Class Location Details
        [DisplayName("Class Location Details")]
        public virtual string CourseLocation { get; set; }
        [StringLength(TrainingRecord.StringLengths.OUTSIDE_INSTRUCTOR)]
        public virtual string OutsideInstructor { get; set; }
        [RequiredWhen("OutsideInstructor", ComparisonType.NotEqualTo, null)]
        public virtual string OutsideInstructorTitle { get; set; }

        [DropDown, EntityMustExist(typeof(TrainingContactHoursProgramCoordinator))]
        [EntityMap]
        public virtual int? ProgramCoordinator { get; set; }

        #endregion

        #region Constructors

        public TrainingRecordViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class CreateTrainingRecord : TrainingRecordViewModel
    {
        #region Constructors

        public CreateTrainingRecord(IContainer container) : base(container)
        {
            if (!MaximumClassSize.HasValue)
            {
                MaximumClassSize = DEFAULT_MAXIMUM_CLASS_SIZE;
            }
        }

        #endregion
    }

    public class EditTrainingRecord : TrainingRecordViewModel
    {
        #region Properties

        public virtual bool Canceled { get; set; }

        #endregion
        
        #region Constructors

        public EditTrainingRecord(IContainer container) : base(container) { }

        #endregion
    }

    public class FinalizeTrainingRecord : ViewModel<TrainingRecord>
    {
        #region Constructors

        public FinalizeTrainingRecord(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override TrainingRecord MapToEntity(TrainingRecord entity)
        {
            var now = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            var whom = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.UserName;
            var repo = _container.GetInstance<MMSINC.Data.NHibernate.IRepository<EmployeeLink>>();
            var dataType =
                _container.GetInstance<IDataTypeRepository>()
                    .GetByTableNameAndDataTypeName(TrainingRecordMap.TABLE_NAME,
                        TrainingRecord.DataTypeNames.EMPLOYEES_ATTENDED).Single();

            foreach (var employee in entity.EmployeesScheduled.Where(employee => entity.EmployeesAttended.All(e => e.Employee.Id != employee.Employee.Id)))
            {
                repo.Save(new EmployeeLink {
                    DataType = dataType,
                    Employee = employee.Employee,
                    LinkedId = entity.Id,
                    LinkedOn = now,
                    LinkedBy = whom
                });
            }

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var ret = base.Validate(validationContext).ToList();

            if (Original != null && Original.EmployeesAttended.Any())
            {
                ret.Add(new ValidationResult("Cannot 'finalize' a training record that already has Attended Employees listed."));
            }

            return ret;
        }

        #endregion
    }

    public class SearchTrainingRecord : SearchSet<TrainingRecord>
    {
        #region Properties

        public DateRange HeldOn { get; set; }
        public DateRange ScheduledDate { get; set; }
        [MultiSelect, SearchAlias("Instructor", "i", "OperatingCenter.Id")]
        public int[] InstructorOperatingCenter { get; set; }
        [DropDown("", "Employee", "GetByOperatingCenterId", DependsOn = "InstructorOperatingCenter", PromptText = "Select an Operating Center above")]
        public int? Instructor { get; set; }
        [DropDown("", "Employee", "GetByOperatingCenterId", DependsOn = "InstructorOperatingCenter", PromptText = "Select an Operating Center above")]
        public int? SecondInstructor { get; set; }
        [MultiSelect, SearchAlias("ClassLocation", "cl", "OperatingCenter.Id")]
        public int[] ClassLocationOperatingCenter { get; set; }
        [DropDown]
        public int? ClassLocation { get; set; }
        [DropDown, SearchAlias("TrainingModule", "TrainingRequirement.Id")]
        public int? TrainingRequirement { get; set; }
        [ComboBox, SearchAlias("TrainingModule", "Id")]
        public int? TrainingModule { get; set; }
        public string OutsideInstructor { get; set; }
        [DisplayName("TrainingRecordId")]
        public int? EntityId { get; set; }
        // TODO: alias from TrainingModule
        //public Range<int> Hours { get; set; } 
        [Search(CanMap = false)]
        public bool? OnlyOpen { get; set; }

        // IsOpen is not used on the page, it's only used in conjunction with OnlyOpen.
        public bool? IsOpen { get; set; }
        
        public bool? HasEnoughTrainingSessionsHoursForTrainingModule { get; set; }
        [DisplayName("LEARN ID")]
        [SearchAlias("TrainingModule", "AmericanWaterCourseNumber")]
        public string AmericanWaterCourseNumber { get; set; }

        [Search(CanMap = false)]
        public int[] Ids { get; set; }
        public virtual bool? HasAttachedDocuments { get; set; }

        #region Calendar-only properties

        /// <summary>
        /// Indicates whether the Ids search should be forced to search an empty array.
        /// </summary>
        [Search(CanMap = false)]
        public bool? IdsRequired { get; set; }

        // Calendar script passes these in? Not sure where the values are coming from, but they are
        // sent from the client-side when opening the calendar tab.
        [Search(CanMap = false)]
        public DateTime? Start { get; set; }
        [Search(CanMap = false)]
        public DateTime? End { get; set; }

        #endregion

        #endregion

        #region Exposed Methods

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);

            var searchableIds = Ids;
            if (searchableIds == null && IdsRequired.GetValueOrDefault())
            {
                // Need an empty array here if IdsRequired is true.
                searchableIds = new int[0];
            }

            if (searchableIds != null)
            {
                mapper.MappedProperties[nameof(EntityId)].Value = searchableIds;
            }

            if (OnlyOpen.GetValueOrDefault())
            {
                mapper.MappedProperties[nameof(IsOpen)].Value = true;
            }

            if (Start.HasValue && End.HasValue)
            {
                var startMap = mapper.MappedProperties[nameof(Start)];
                var endMap = mapper.MappedProperties[nameof(End)];
                var range = new DateRange
                {
                    Operator = RangeOperator.Between,
                    Start = Start,
                    End = End
                };
                startMap.ActualName = nameof(TrainingRecord.MinSessionDate);
                startMap.Value = range;
                endMap.ActualName = nameof(TrainingRecord.MaxSessionDate);
                endMap.Value = range;
                // NOTE: By adding OrConstraintSearchModifier, the default equality checks for Start/End
                // will be removed.
                mapper.SearchModifiers.Add(new OrConstraintSearchModifier(nameof(Start), nameof(End)));
            }
        }

        #endregion
    }

    public class AddTrainingSession : ViewModel<TrainingRecord>
    {
        #region Constants

        public const string INSTRUCTOR_BOOKED = "Instructor is already booked during this time period.";
        public const string SECOND_INSTRUCTOR_BOOKED = "Second Instructor is already booked during this time period.";

        #endregion

        #region Properties

        [Required, AutoMap(MapDirections.None), DateTimePicker]
        public virtual DateTime? StartDateTime { get; set; }
        [Required, AutoMap(MapDirections.None), DateTimePicker]
        public virtual DateTime? EndDateTime { get; set; }
        [EntityMap]
        public virtual int? Instructor { get; set; }
        [EntityMap]
        public virtual int? SecondInstructor { get; set; }

        #endregion

        #region Constructors

        public AddTrainingSession(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override TrainingRecord MapToEntity(TrainingRecord entity)
        {
            entity.TrainingSessions.Add(new TrainingSession
            {
                TrainingRecord = entity,
                StartDateTime = StartDateTime.Value,
                EndDateTime = EndDateTime.Value
            });
            return base.MapToEntity(entity);
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var repo = _container.GetInstance<ITrainingSessionRepository>();
            if (Instructor.HasValue)
            {
                var instructorBooked = repo.InstructorBooked(Instructor.Value, StartDateTime.Value, EndDateTime.Value);
                if (instructorBooked)
                    yield return new ValidationResult(INSTRUCTOR_BOOKED);
            }
            if (SecondInstructor.HasValue)
            {
                var secondInstructorBooked = repo.InstructorBooked(SecondInstructor.Value, StartDateTime.Value, EndDateTime.Value);
                if (secondInstructorBooked)
                    yield return new ValidationResult(SECOND_INSTRUCTOR_BOOKED);
            }
        }

        #endregion
    }

    public class RemoveTrainingSession : ViewModel<TrainingRecord>
    {
        #region Properties

        [Required, DoesNotAutoMap("Mapped manually.")]
        public virtual int TrainingSessionId { get; set; }

        #endregion

        #region Constructors

        public RemoveTrainingSession(IContainer container) : base(container) { }

        #endregion

        public override TrainingRecord MapToEntity(TrainingRecord entity)
        {
            entity.TrainingSessions.Remove(
                _container.GetInstance<MMSINC.Data.NHibernate.IRepository<TrainingSession>>()
                    .Find(TrainingSessionId));
            return base.MapToEntity(entity);
        }
    }

    public class SearchTrainingTotalHours : SearchSet<TrainingTotalHoursReportItem>, ISearchTrainingTotalHours
    {
        [SearchAlias("allEmp.OperatingCenter", "opCntr", "Id"), DropDown]
        public int? OperatingCenter { get; set; }
        [ComboBox]
        [SearchAlias("TrainingModule", "module", "Id")]
        public int? TrainingModule { get; set; }

        [DropDown, DisplayName("Common Name")]
        [SearchAlias("requirement.PositionGroupCommonNames", "commonName", "Id")]
        public int? PositionGroupCommonName { get; set; }

        [Search(CanMap = false), MaxCurrentYear, Required]
        public int? Year { get; set; }

        public SearchTrainingTotalHours()
        {
            EnablePaging = false;
        }
    }
}

