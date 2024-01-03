using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.WaterQuality.Models.ViewModels
{
    public abstract class WaterQualityComplaintViewModel : ViewModel<WaterQualityComplaint>
    {
        #region Fields

        private WaterQualityComplaint _display;

        #endregion

        #region Properties

        [DoesNotAutoMap("Display only")]
        public WaterQualityComplaint Display => _display ?? (_display = _container.GetInstance<IRepository<WaterQualityComplaint>>().Find(Id));

        [EntityMustExist(typeof(PublicWaterSupply)), EntityMap]
        public virtual int? PublicWaterSupply { get; set; }
        
        [EntityMustExist(typeof(WaterQualityComplaintType)), DropDown, EntityMap]
        public int? Type { get; set; }

        [Required]
        [Coordinate, DisplayName("Coordinates")]
        [EntityMustExist(typeof(Coordinate))]
        [EntityMap("Coordinate")]
        public virtual int? CoordinateId { get; set; }

        [DateOnly]
        public DateTime? DateComplaintReceived { get; set; }

        [DateTimePicker, View(FormatStyle.DateTimeWithoutSeconds)]
        public DateTime? InitialLocalResponseDate { get; set; }

        [EntityMustExist(typeof(WaterQualityComplaintLocalResponseType)), DropDown, EntityMap]
        public int? InitialLocalResponseType { get; set; }

        [View(DisplayName = "Initial Local Contact By")]
        [Required, EntityMustExist(typeof(Employee)), EntityMap]
        [AutoComplete("Employee", "ActiveEmployeesDescriptionByEmployeePartialFirstOrLastName", DisplayProperty = nameof(EmployeeDisplayItem.Display)), Description("Enter the employee's first or last name to populate autocomplete list")]
        public virtual int? InitialLocalContact { get; set; }
        
        [EntityMustExist(typeof(Employee)), EntityMap]
        [AutoComplete("Employee", "ActiveEmployeesDescriptionByEmployeePartialFirstOrLastName", DisplayProperty = nameof(EmployeeDisplayItem.Display)), Description("Enter the employee's first or last name to populate autocomplete list")]
        public virtual int? NotificationCompletedBy { get; set; }

        [StringLength(WaterQualityComplaint.StringLengths.NOTIFICATION_CREATED_BY)]
        public string NotificationCreatedBy { get; set; }

        [DateTimePicker]
        public DateTime? NotificationCompletionDate { get; set; }

        [StringLength(WaterQualityComplaint.StringLengths.CUSTOMER_NAME)]
        public string CustomerName { get; set; }

        [StringLength(WaterQualityComplaint.StringLengths.HOME_PHONE_NUMBER)]
        public string HomePhoneNumber { get; set; }

        [StringLength(WaterQualityComplaint.StringLengths.EXTENSION)]
        public string Ext { get; set; }

        [StringLength(WaterQualityComplaint.StringLengths.STREET_NUMBER)]
        public string StreetNumber { get; set; }

        [StringLength(WaterQualityComplaint.StringLengths.STREET_NAME)]
        public string StreetName { get; set; }

        [StringLength(WaterQualityComplaint.StringLengths.APARTMENT_NUMBER)]
        public string ApartmentNumber { get; set; }

        [Required, EntityMustExist(typeof(State)), DropDown, EntityMap]
        public int? State { get; set; }

        [Required, EntityMustExist(typeof(Town)), DropDown("", "Town", "ByStateId", DependsOn = "State"), EntityMap]
        public int? Town { get; set; }

        [Required, EntityMustExist(typeof(OperatingCenter)), EntityMap]
        public virtual int? OperatingCenter { get; set; }

        [EntityMustExist(typeof(TownSection)), DropDown("", "TownSection", "ByTownId", DependsOn = "Town"), EntityMap]
        public virtual int? TownSection { get; set; }

        [StringLength(WaterQualityComplaint.StringLengths.ZIP_CODE)]
        public string ZipCode { get; set; }

        [Required, StringLength(WaterQualityComplaint.StringLengths.PREMISE_NUMBER)]
        public string PremiseNumber { get; set; }

        [StringLength(WaterQualityComplaint.StringLengths.ACCOUNT_NUMBER)]
        public string AccountNumber { get; set; }

        [DateOnly]
        public DateTime? ComplaintStartDate { get; set; }

        [EntityMustExist(typeof(WaterQualityComplaintProblemArea)), DropDown, EntityMap]
        public int? ProblemArea { get; set; }

        [EntityMustExist(typeof(WaterQualityComplaintSource)), DropDown, EntityMap]
        public int? Source { get; set; }

        public bool? SiteVisitRequired { get; set; }

        [StringLength(WaterQualityComplaint.StringLengths.SITE_VISIT_BY)]
        public string SiteVisitBy { get; set; }

        public bool? WaterFilterOnComplaintSource { get; set; }
        public bool? CrossConnectionDetected { get; set; }

        [EntityMustExist(typeof(WaterQualityComplaintProbableCause)), DropDown, EntityMap]
        public int? ProbableCause { get; set; }

        [EntityMustExist(typeof(WaterQualityComplaintActionsWhichCanBeTaken)), DropDown, EntityMap]
        public int? ActionTaken { get; set; }

        [EntityMustExist(typeof(WaterQualityComplaintCustomerExpectation)), DropDown, EntityMap]
        public int? CustomerExpectation { get; set; }

        [RequiredWhen("NotificationCompletionDate", ComparisonType.NotEqualTo, null)]
        [EntityMustExist(typeof(WaterQualityComplaintCustomerSatisfaction)), DropDown, EntityMap]
        public int? CustomerSatisfaction { get; set; }

        public bool? RootCauseIdentified { get; set; }

        [EntityMustExist(typeof(WaterQualityComplaintRootCause)), DropDown, EntityMap]
        public int? RootCause { get; set; }

        #endregion

        #region Constructors

        public WaterQualityComplaintViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateWaterQualityComplaint : WaterQualityComplaintViewModel
    {
        #region Properties

        [DropDown("", "OperatingCenter", "ActiveByTownId", DependsOn = nameof(Town))]
        public override int? OperatingCenter
        {
            get => base.OperatingCenter; set => base.OperatingCenter = value;
        }

        [DropDown("", "PublicWaterSupply", "ActiveByOperatingCenterId", DependsOn = "OperatingCenter")]
        public override int? PublicWaterSupply
        {
            get => base.PublicWaterSupply; 
            set => base.PublicWaterSupply = value;
        }

        [EntityMustExist(typeof(TownSection)), DropDown("", "TownSection", "ActiveByTownId", DependsOn = "Town"), EntityMap]
        public override int? TownSection { get; set; }

        #endregion

        #region Constructors

        public CreateWaterQualityComplaint(IContainer container) : base(container) { }

        #endregion
    }

    public class EditWaterQualityComplaint : WaterQualityComplaintViewModel
    {
        #region Properties

        [DropDown("", "OperatingCenter", "ByTownId", DependsOn = nameof(Town))]
        public override int? OperatingCenter
        {
            get => base.OperatingCenter; set => base.OperatingCenter = value;
        }

        [DropDown("", "PublicWaterSupply", "ByOperatingCenterId", DependsOn = "OperatingCenter")]
        public override int? PublicWaterSupply
        {
            get => base.PublicWaterSupply; 
            set => base.PublicWaterSupply = value;
        }

        #endregion

        #region Constructors

        public EditWaterQualityComplaint(IContainer container) : base(container) { }

        #endregion
    }

    public class SearchWaterQualityComplaint : SearchSet<WaterQualityComplaint>
    {
        #region Properties

        [DropDown]
        public int? State { get; set; }

        [DropDown]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByStateId", DependsOn = "State")]
        public int? Town { get; set; }

        public string ZipCode { get; set; }
        public SearchString StreetName { get; set; }
        public DateRange DateComplaintReceived { get; set; }

        [View(DisplayName = "Initial Local Contact By")]
        [EntityMustExist(typeof(Employee))]
        [AutoComplete("Employee", "ActiveEmployeesDescriptionByEmployeePartialFirstOrLastName", DisplayProperty = nameof(EmployeeDisplayItem.Display)), Description("Enter the employee's first or last name to populate autocomplete list")]
        public int? InitialLocalContact { get; set; }
        public DateRange NotificationCompletionDate { get; set; }

        [DropDown]
        public int? CustomerSatisfaction { get; set; }

        [MultiSelect]
        public int[] Type { get; set; }

        public bool? SiteVisitRequired { get; set; }
        public bool? RootCauseIdentified { get; set; }
        public SearchString NotificationCreatedBy { get; set; }
        public bool? FollowUpPostCardSent { get; set; }
        public bool? IsClosed { get; set; }
        public bool? Imported { get; set; }
        public bool? HasCoordinate { get; set; }

        [DisplayName("SAP Notification #")]
        public SearchString OrcomOrderNumber { get; set; }

        #endregion
    }

    public class SearchWaterQualityComplaintByStateForYear : SearchSet<WaterQualityComplaintByStateForYearReportItem>,
        ISearchWaterQualityComplaintByStateForYear
    {
        [DropDown, Required]
        public int Year { get; set; }

        [DropDown, Required]
        public string State { get; set; }
    }
    
    public class AddWaterQualityComplaintSampleResult : ViewModel<WaterQualityComplaint>
    {
        #region Properties

        [Required, DropDown, EntityMustExist(typeof(WaterConstituent)), AutoMap(MapDirections.None)]
        public int? WaterConstituent { get; set; }

        [Required, AutoMap(MapDirections.None)]
        public DateTime? SampleDate { get; set; }

        [StringLength(50)]
        [Required, AutoMap(MapDirections.None)]
        public string SampleValue { get; set; }

        [StringLength(25)]
        [AutoMap(MapDirections.None)]
        public string UnitOfMeasure { get; set; }

        [StringLength(50)]
        [Required, AutoMap(MapDirections.None)]
        public string AnalysisPerformedBy { get; set; }

        #endregion

        #region Constructors

        public AddWaterQualityComplaintSampleResult(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override WaterQualityComplaint MapToEntity(WaterQualityComplaint entity)
        {
            entity.SampleResults.Add(new WaterQualityComplaintSampleResult {
                Complaint = entity,
                WaterConstituent = WaterConstituent.HasValue ? _container.GetInstance<IRepository<WaterConstituent>>().Find(WaterConstituent.Value) : null,
                SampleDate = SampleDate.Value,
                SampleValue = SampleValue,
                UnitOfMeasure = UnitOfMeasure,
                AnalysisPerformedBy = AnalysisPerformedBy
            });
            return base.MapToEntity(entity);
        }

        #endregion
    }

    public class RemoveWaterQualityComplaintSampleResult : ViewModel<WaterQualityComplaint>
    {
        #region Properties

        [Required, DoesNotAutoMap("Manually mapped")]
        public virtual int SampleResultId { get; set; }

        #endregion

        #region Constructors

        public RemoveWaterQualityComplaintSampleResult(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override WaterQualityComplaint MapToEntity(WaterQualityComplaint entity)
        {
            // Don't call base.MapToEntity as there's nothing for it to do.

            entity.SampleResults.Remove(
                _container.GetInstance<IRepository<WaterQualityComplaintSampleResult>>().Find(SampleResultId));

            return entity;
        }

        #endregion
    }
}
