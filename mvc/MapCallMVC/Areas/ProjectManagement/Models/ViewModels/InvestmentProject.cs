using System;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.ProjectManagement.Models.ViewModels
{
    public class InvestmentProjectViewModel : ViewModel<InvestmentProject>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }
        public string ProjectNumber { get; set; }
        public string PPWorkOrder { get; set; }

        [DropDown("", "BusinessUnit", "FindByOperatingCenterId", DependsOn = "OperatingCenter"), EntityMap, EntityMustExist(typeof(BusinessUnit))]
        public int? BusinessUnit { get; set; }

        [StringLengthNotRequired]
        public string ProjectDescription { get; set; }

        [StringLengthNotRequired]
        public string ProjectObstacles { get; set; }

        [StringLengthNotRequired]
        public string ProjectRisks { get; set; }

        [StringLengthNotRequired]
        public string ProjectApproach { get; set; }
        public int? ProjectDurationMonths { get; set; }
        public decimal? EstimatedProjectCost { get; set; }
        public decimal? FinalProjectCost { get; set; }

        #region Employee dropdowns

        // The original page did not have any cascades for employee. ALL employees listed!
        [DropDown, EntityMap, EntityMustExist(typeof(Employee))]
        public int? AssetOwner { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(Employee))]
        public int? ProjectManager { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(Employee))]
        public int? ConstructionManager { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(Employee))]
        public int? CompanyInspector { get; set; }

        #endregion

        [StringLength(InvestmentProject.StringLengths.CONTRACTED_INSPECTOR)]
        public string ContractedInspector { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(Contractor))] // Contractors
        public int? EngineeringContractor { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(Contractor))]
        public int? ConstructionContractor { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(PublicWaterSupply))]
        public int? PublicWaterSupply { get; set; }

        [EntityMap, EntityMustExist(typeof(Facility))]
        [DropDown("", "Facility", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        public int? Facility { get; set; }
        public string StreetName { get; set; }

        [EntityMap, EntityMustExist(typeof(Town))]
        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        public int? Town { get; set; }

        [Coordinate, EntityMap]
        public int? Coordinate { get; set; }

        public DateTime? CIMDate { get; set; }
        public bool? ProjectFlagged { get; set; }
        public bool? CurrentYearActive { get; set; }
        public bool? BulkSale { get; set; }
        public bool? RateCase { get; set; }
        public bool? MISDates { get; set; }
        public bool? COE { get; set; }
        public bool? Geography { get; set; }
        public DateTime? ForecastedInServiceDate { get; set; }
        public DateTime? ControlDate { get; set; }
        public DateTime? PPDate { get; set; }
        public int? PPScore { get; set; }
        public DateTime? InServiceDate { get; set; }

        [DropDown, Range(2015, 9999)]
        public int? CPSReferenceYear { get; set; }
        public string CPSPriorityNumber { get; set; }
        public int? DurationLandAcquisitionInMonths { get; set; }
        public int? DurationPermitDesignInMonths { get; set; }
        public int? DurationConstructionInMonths { get; set; }
        public DateTime? TargetStartDate { get; set; }
        public DateTime? TargetEndDate { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(InvestmentProjectPhase))]
        public int? Phase { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(InvestmentProjectCategory))]
        public int? ProjectCategory { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(InvestmentProjectAssetCategory))]
        public int? AssetCategory { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(InvestmentProjectApprovalStatus))]
        public int? ApprovalStatus { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(InvestmentProjectStatus))]
        public int? ProjectStatus { get; set; }

        #endregion

        #region Constructors

        public InvestmentProjectViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateInvestmentProject : InvestmentProjectViewModel
    {
        #region Constructors

        public CreateInvestmentProject(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override InvestmentProject MapToEntity(InvestmentProject entity)
        {
            base.MapToEntity(entity);
            entity.CreatedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.UserName;
            return entity;
        }

        #endregion
    }

    public class SearchInvestmentProject : SearchSet<InvestmentProject>
    {
        #region Properties

        [DropDown]
        public int? OperatingCenter { get; set; }

        [DropDown]
        public int? PublicWaterSupply { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        public int? Town { get; set; }

        public string ProjectNumber { get; set; }
        public string PPWorkOrder { get; set; }

        [DropDown]
        public int? Phase { get; set; }

        [DropDown]
        public int? ProjectCategory { get; set; }

        [DropDown]
        public int? AssetCategory { get; set; }

        public decimal? EstimatedProjectCost { get; set; }

        public bool? ProjectFlagged { get; set; }

        public DateRange ForecastedInServiceDate { get; set; }

        public DateRange InServiceDate { get; set; }

        public string CreatedBy { get; set; }

        #endregion

    }
}