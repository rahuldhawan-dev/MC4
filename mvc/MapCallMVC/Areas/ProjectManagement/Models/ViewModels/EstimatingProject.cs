using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.ProjectManagement.Models.ViewModels
{
    public class EstimatingProjectViewModel : ViewModel<EstimatingProject>
    {
        #region Properties

        [Required, DropDown, EntityMap("ProjectType"),
         EntityMustExist(typeof(EstimatingProjectType))]
        public virtual int ProjectType { get; set; }

        [Required, EntityMap("Town"), EntityMustExist(typeof(Town)),
         DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter",
             PromptText = "Please select an Operating Center above")]
        public virtual int Town { get; set; }

        [Required, DropDown, EntityMap("OperatingCenter"), EntityMustExist(typeof(OperatingCenter))]
        public virtual int OperatingCenter { get; set; }

        [DropDown("Contractors", "Contractor", "GetFrameworkContractorsByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center above")]
        [EntityMap("Contractor"), EntityMustExist(typeof(Contractor))]
        [RequiredWhen("ProjectType", ComparisonType.EqualTo,EstimatingProjectType.Indices.FRAMEWORK)]
        public virtual int? Contractor { get; set; }

        [Required, DropDown, EntityMap("Estimator"), EntityMustExist(typeof(Employee))]
        public virtual int Estimator { get; set; }

        [Required, StringLength(CreateTablesForBug1774.StringLengths.EstimatingProjects.PROJECT_NUMBER),
         Display(Name = "Preliminary File Number")]
        public virtual string ProjectNumber { get; set; }

        [Required, StringLength(CreateTablesForBug1774.StringLengths.EstimatingProjects.PROJECT_NAME)]
        public virtual string ProjectName { get; set; }

        [Required, StringLength(CreateTablesForBug1774.StringLengths.EstimatingProjects.STREET)]
        public virtual string Street { get; set; }

        [StringLength(MakeDescriptionFieldLongerForBug1968.STRING_LENGTH)]
        public virtual string Description { get; set; }

        [Required, Range(0, 100)]
        public virtual int? OverheadPercentage { get; set; }

        [Required, Range(0, 100)]
        public virtual int? ContingencyPercentage { get; set; }

        public virtual decimal? LumpSum { get; set; }
     
        [Required]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public virtual DateTime? EstimateDate { get; set; }
       
        [Multiline]
        public virtual string Remarks { get; set; }

        [StringLength(18)]
        public virtual string WBSNumber { get; set; }

        [StringLength(AddJDEPayrollNumberToEstimatingProjectsForBug2340.LENGTH, MinimumLength = AddJDEPayrollNumberToEstimatingProjectsForBug2340.LENGTH)]
        [DisplayName("JDE Payroll Number")]
        public virtual string JDEPayrollNumber { get; set; }

        #endregion

        #region Constructors

        public EstimatingProjectViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateEstimatingProject : EstimatingProjectViewModel
    {
        #region Constructors

        public CreateEstimatingProject(IContainer container) : base(container) { }

        #endregion
    }

    public class EditEstimatingProject : EstimatingProjectViewModel
    {
        #region Constructors

        public EditEstimatingProject(IContainer container) : base(container) { }

        #endregion
    }

    public class AddEstimatingProjectMaterial : ViewModel<EstimatingProject>
    {
        #region Properties

        [Required, AutoMap(MapDirections.None)]
        public virtual int? Quantity { get; set; }

        [Required, ComboBox, EntityMap(MapDirections.None /* Manually mapped */), EntityMustExist(typeof(Material))]
        public virtual int? Material { get; set; }

        [Required, DropDown, EntityMap(MapDirections.None /* Manually mapped */),
         EntityMustExist(typeof(AssetType))]
        public virtual int? AssetType { get; set; }

        #endregion

        #region Constructors

        public AddEstimatingProjectMaterial(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override EstimatingProject MapToEntity(EstimatingProject entity)
        {
            entity = base.MapToEntity(entity);
            var material = _container.GetInstance<IMaterialRepository>().Find(Material.Value);
            var assetType =
                _container.GetInstance<MMSINC.Data.NHibernate.IRepository<AssetType>>().Find(AssetType.Value);
            entity.Materials.Add(new EstimatingProjectMaterial {
                EstimatingProject = entity,
                Quantity = Quantity.Value,
                Material = material,
                AssetType = assetType
            });
            return entity;
        }

        #endregion
    }

    public class RemoveEstimatingProjectMaterial : ViewModel<EstimatingProject>
    {
        #region Properties

        [Required, DoesNotAutoMap("Mapped manually.")]
        public virtual int EstimatingProjectMaterialId { get; set; }

        #endregion

        #region Constructors

        public RemoveEstimatingProjectMaterial(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override EstimatingProject MapToEntity(EstimatingProject entity)
        {
            // Do not call base.MapToEntity, this is entirely manually mapped.
            entity.Materials.Remove(
                _container.GetInstance<MMSINC.Data.NHibernate.IRepository<EstimatingProjectMaterial>>()
                          .Find(EstimatingProjectMaterialId));
            return entity;
        }

        #endregion
    }

    public class AddEstimatingProjectContractorLaborCost : ViewModel<EstimatingProject>
    {
        #region Properties

        [DoesNotAutoMap("Manually mapped")]
        [Required, ComboBox, EntityMustExist(typeof(ContractorLaborCost))]
        public virtual int? ContractorLaborCost { get; set; }
        
        [Required, DoesNotAutoMap("Manually mapped")]
        public virtual int? Quantity { get; set; }
        
        [Required, DropDown, EntityMustExist(typeof(AssetType)), DoesNotAutoMap("Manually mapped")]
        public virtual int? AssetType { get; set; }

        #endregion

        #region Constructors

        public AddEstimatingProjectContractorLaborCost(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override EstimatingProject MapToEntity(EstimatingProject entity)
        {
            entity = base.MapToEntity(entity);
            var cost =
                _container.GetInstance<MMSINC.Data.NHibernate.IRepository<ContractorLaborCost>>()
                          .Find(ContractorLaborCost.Value);
            var assetType = _container.GetInstance<MMSINC.Data.NHibernate.IRepository<AssetType>>().Find(AssetType.Value);

            entity.ContractorLaborCosts.Add(new EstimatingProjectContractorLaborCost {
                EstimatingProject = entity,
                ContractorLaborCost = cost,
                Quantity = Quantity.Value,
                AssetType = assetType
            });

            return entity;
        }

        #endregion
    }

    public class RemoveEstimatingProjectContractorLaborCost : ViewModel<EstimatingProject>
    {
        #region Properties

        [Required, DoesNotAutoMap("Manually mapped")]
        public virtual int EstimatingProjectContractorLaborCostId { get; set; }

        #endregion

        #region Constructors

        public RemoveEstimatingProjectContractorLaborCost(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override EstimatingProject MapToEntity(EstimatingProject entity)
        {
            entity.ContractorLaborCosts.Remove(
                _container.GetInstance<MMSINC.Data.NHibernate.IRepository<EstimatingProjectContractorLaborCost>>()
                          .Find(EstimatingProjectContractorLaborCostId));
            return base.MapToEntity(entity);
        }

        #endregion
    }

    public class AddEstimatingProjectOtherCost : ViewModel<EstimatingProject>
    {
        #region Properties

        [Required, AutoMap(MapDirections.None)]
        public virtual int? Quantity { get; set; }

        [Required, StringLength(EstimatingProjectOtherCost.StringLengths.DESCRIPTION), AutoMap(MapDirections.None)]
        public virtual string Description { get; set; }

        [Required, AutoMap(MapDirections.None)]
        public virtual decimal? Cost { get; set; }

        [Required, AutoMap(MapDirections.None), DropDown, EntityMustExist(typeof(AssetType))]
        public virtual int? AssetType { get; set; }

        #endregion

        #region Constructors

        public AddEstimatingProjectOtherCost(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override EstimatingProject MapToEntity(EstimatingProject entity)
        {
            entity.OtherCosts.Add(new EstimatingProjectOtherCost {
                EstimatingProject = entity,
                Quantity = Quantity.Value,
                Description = Description,
                Cost = Cost.Value,
                AssetType = _container.GetInstance<MMSINC.Data.NHibernate.IRepository<AssetType>>().Find(AssetType.Value)
            });
            return base.MapToEntity(entity);
        }

        #endregion
    }

    public class RemoveEstimatingProjectOtherCost : ViewModel<EstimatingProject>
    {
        #region Properties

        [Required, DoesNotAutoMap("Manually mapped")]
        public virtual int OtherCostId { get; set; }

        #endregion

        #region Constructors

        public RemoveEstimatingProjectOtherCost(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override EstimatingProject MapToEntity(EstimatingProject entity)
        {
            entity.OtherCosts.Remove(
                _container.GetInstance<MMSINC.Data.NHibernate.IRepository<EstimatingProjectOtherCost>>()
                          .Find(OtherCostId));
            return base.MapToEntity(entity);
        }

        #endregion
    }

    public class AddEstimatingProjectCompanyLaborCost : ViewModel<EstimatingProject>
    {
        #region Properties

        [Required, DoesNotAutoMap]
        public virtual int? Quantity { get; set; }

        [Required, DropDown, EntityMap(MapDirections.None /* Manually mapped */),
         EntityMustExist(typeof(CompanyLaborCost))]
        public virtual int? CompanyLaborCost { get; set; }

        [Required, DropDown, EntityMap(MapDirections.None /* Manually mapped */), EntityMustExist(typeof(AssetType))]
        public virtual int? AssetType { get; set; }

        #endregion

        #region Constructors

        public AddEstimatingProjectCompanyLaborCost(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override EstimatingProject MapToEntity(EstimatingProject entity)
        {
            entity = base.MapToEntity(entity);
            var companyLaborCost =
                _container.GetInstance<MMSINC.Data.NHibernate.IRepository<CompanyLaborCost>>()
                          .Find(CompanyLaborCost.Value);
            var assetType = _container.GetInstance<MMSINC.Data.NHibernate.IRepository<AssetType>>().Find(AssetType.Value);

            entity.CompanyLaborCosts.Add(new EstimatingProjectCompanyLaborCost {
                EstimatingProject = entity,
                Quantity = Quantity.Value,
                CompanyLaborCost = companyLaborCost,
                AssetType = assetType
            });
            return entity;
        }

        #endregion
    }

    public class RemoveEstimatingProjectCompanyLaborCost : ViewModel<EstimatingProject>
    {
        #region Properties

        [Required, DoesNotAutoMap("Manually mapped")]
        public virtual int EstimatingProjectCompanyLaborCostId { get; set; }

        #endregion

        #region Constructors

        public RemoveEstimatingProjectCompanyLaborCost(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override EstimatingProject MapToEntity(EstimatingProject entity)
        {
            entity.CompanyLaborCosts.Remove(
                _container.GetInstance<MMSINC.Data.NHibernate.IRepository<EstimatingProjectCompanyLaborCost>>()
                          .Find(EstimatingProjectCompanyLaborCostId));
            return base.MapToEntity(entity);
        }

        #endregion
    }

    public class AddEstimatingProjectPermit : ViewModel<EstimatingProject>
    {
        #region Properties

        [Required, DropDown, EntityMap(MapDirections.None /* Mapped manually */),
         EntityMustExist(typeof(PermitType))]
        public virtual int? PermitType { get; set; }

        [Required, DoesNotAutoMap]
        public virtual int? Quantity { get; set; }

        [DoesNotAutoMap("Mapped manually")]
        [Required, DisplayFormat(DataFormatString = "{0:c}")]
        public virtual decimal? Cost { get; set; }

        [Required, DoesNotAutoMap, DropDown, EntityMustExist(typeof(AssetType))]
        public virtual int? AssetType { get; set; }

        #endregion

        #region Constructors

        public AddEstimatingProjectPermit(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override EstimatingProject MapToEntity(EstimatingProject entity)
        {
            entity = base.MapToEntity(entity);
            entity.Permits.Add(new EstimatingProjectPermit {
                EstimatingProject = entity,
                PermitType = _container.GetInstance<MMSINC.Data.NHibernate.IRepository<PermitType>>().Find(PermitType.Value),
                Quantity = Quantity.Value,
                Cost = Cost.Value,
                AssetType = _container.GetInstance<MMSINC.Data.NHibernate.IRepository<AssetType>>().Find(AssetType.Value)
            });
            return entity;
        }

        #endregion
    }

    public class RemoveEstimatingProjectPermit : ViewModel<EstimatingProject>
    {
        #region Properties

        [Required, DoesNotAutoMap("mapped manually")]
        public virtual int EstimatingProjectPermitId { get; set; }

        #endregion

        #region Constructors

        public RemoveEstimatingProjectPermit(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override EstimatingProject MapToEntity(EstimatingProject entity)
        {
            entity.Permits.Remove(
                _container.GetInstance<MMSINC.Data.NHibernate.IRepository<EstimatingProjectPermit>>()
                          .Find(EstimatingProjectPermitId));
            return base.MapToEntity(entity);
        }

        #endregion
    }

    public class SearchEstimatingProject : SearchSet<EstimatingProject>
    {
        #region Properties

        public int? Id { get; set; }

        [DropDown]
        public int? ProjectType { get; set; }

        [Display(Name = "Preliminary File Number")]
        public string ProjectNumber { get; set; }

        public string ProjectName { get; set; }

        [DropDown]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter",
            PromptText = "Please select an Operating Center above")]
        public int? Town { get; set; }

        [DropDown]
        public int? Estimator { get; set; }

        public DateRange EstimateDate { get; set; }
        public string WBSNumber { get; set; }
        public string CreatedBy { get; set; }

        #endregion
    }

    public class MaterialRequisitionForm : ViewModel<EstimatingProject>
    {
        #region Properties

        public OperatingCenter OperatingCenter { get; set; }

        [Display(Name = "Preliminary File Number")]
        public string ProjectNumber { get; set; }

        public string ProjectName { get; set; }
        public virtual string WBSNumber { get; set; }
        public string Street { get; set; }
        public string JDEPayrollNumber { get; set; }
        public Town Town { get; set; }
        public Contractor Contractor { get; set; }
        public Employee Estimator { get; set; }
        public IList<EstimatingProjectMaterial> Materials { get; set; }
        public IEnumerable<EstimatingProjectMaterial> GroupedMaterials { get; set; }

        [DoesNotAutoMap]
        public bool DoNotOrder { get; set; }

        #endregion

        #region Constructors

        public MaterialRequisitionForm(IContainer container) : base(container) { }

        #endregion
    }

    public class AssetsRetiredSummary : ViewModel<EstimatingProject>
    {
        public OperatingCenter OperatingCenter { get; set; }
        public string ProjectName { get; set; }
        public string WBSNumber { get; set; }
        public virtual string JDEPayrollNumber { get; set; }

        public AssetsRetiredSummary(IContainer container) : base(container) { }
    }

    public class ScheduleOfValuesForm : ViewModel<EstimatingProject>
    {
        public ScheduleOfValuesForm(IContainer container) : base(container) {}

        public string ProjectName { get; set; }
        
        [Display(Name = "Project Description")]
        public string Description { get; set; }
        
        [Display(Name = "Preliminary File Number")]
        public string ProjectNumber { get; set; }
       
        [Display(Name = "Location")]
        public string Street { get; set; }
      
        public Town Town { get; set; }
        public OperatingCenter OperatingCenter { get; set; }
        public virtual string WBSNumber { get; set; }

        public IList<EstimatingProjectContractorLaborCost> ContractorLaborCosts { get; set; }
    }

    public class TaskOrderGeneratorForm : ViewModel<EstimatingProject>
    {
        #region Properties

        [DoesNotAutoMap]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE), Required]
        public DateTime? EffectiveAgreementDate { get; set; }

        [DoesNotAutoMap]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE), Required]
        public DateTime? BeginDate { get; set; }

        [DoesNotAutoMap]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE), Required]
        public DateTime? SubstantialCompletionDate { get; set; }

        [DoesNotAutoMap]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE), Required]
        public DateTime? EndDate { get; set; }

        [DoesNotAutoMap]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE_WITH_MONTH_TEXT_FORMAT), Required]
        public DateTime? ContractorAgreementDate { get; set; }

        [Required, DoesNotAutoMap]
        public string AsDetailedIn { get; set; }

        [DoesNotAutoMap]
        public string AdditionalInvoiceInstructions { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        public virtual decimal TotalContractorLaborCost { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        public virtual decimal TotalOtherCost { get; set; }

        public virtual Contractor Contractor { get; set; }
        public virtual string ProjectName { get; set; }
        public virtual string WBSNumber { get; set; }
        public virtual string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        public virtual decimal? LumpSum { get; set; }

        public virtual OperatingCenter OperatingCenter { get; set; }

        public virtual EstimatingProjectType ProjectType { get; set; }

        public virtual IList<EstimatingProjectContractorLaborCost> ContractorLaborCosts { get; set; }
        public virtual IList<EstimatingProjectContractorLaborCost> GroupedContractorLaborCosts { get; set; }
        public virtual IList<EstimatingProjectOtherCost> OtherCosts { get; set; }

        [DoesNotAutoMap, DisplayFormat(DataFormatString = "{0:c}")]
        public virtual decimal TotalOfAllEstimatedCosts
        {
            get
            {
                return TotalContractorLaborCost + TotalOtherCost;
            }
        }

        #endregion

        #region Constructors

        public TaskOrderGeneratorForm(IContainer container) : base(container) { }

        #endregion
    }

    // NOTE: This only needs to be a ViewModel<T> to work with ActionHelper.
    public class CreateEstimateForm : ViewModel<EstimatingProject>
    {
        #region Properties

        [DropDown, DoesNotAutoMap]
        public string AssignedTo { get; set; }

        [DoesNotAutoMap]
        public string OldJDETaskNumber { get; set; }
       
        [DoesNotAutoMap]
        public string JDEPayrollTask { get; set; }
        
        [DoesNotAutoMap]
        public string ProjectLocation { get; set; }
       
        [DoesNotAutoMap]
        public decimal FTCApplied { get; set; }
       
        [DoesNotAutoMap]
        public decimal FTCTotal { get; set; }

        #endregion

        #region Constructors

        public CreateEstimateForm(IContainer container) : base(container) { }

        #endregion
    }

    public class EstimateForm : ViewModel<EstimatingProject>
    {
        #region Properties

        [DoesNotAutoMap("Display only")]
        public EstimatingProject Project { get; protected set; }

        [DoesNotAutoMap]
        public CreateEstimateForm Throwaways { get; set; }

        // This is used in the EstimateController pdf
        [DoesNotAutoMap, DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public decimal AdminInstallCost
        {
            get
            {
                return Project.TotalContractorLaborCost +
                       Project.TotalPermitCost + 
                       Project.TotalOtherCost + 
                       Project.LumpSum.GetValueOrDefault() +
                       Project.TotalCompanyLaborCost;
            }
        }

        // This is used in the EstimateController pdf
        [DoesNotAutoMap, DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public decimal AdminInstallAndMaterialCost
        {
            get { return AdminInstallCost + Project.TotalMaterialCost; }
        }

        [DoesNotAutoMap, DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public decimal OmmissionsAndContingencies
        {
            get { return AdminInstallAndMaterialCost * Project.ContingencyPercentageAsDecimal; }
        }

        [DoesNotAutoMap, DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public decimal TotalDirectCost
        {
            get { return AdminInstallAndMaterialCost + OmmissionsAndContingencies; }
        }

        [DoesNotAutoMap, DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public decimal ConstructionOverheadCost
        {
            get { return TotalDirectCost * Project.OverheadPercentageAsDecimal; }
        }

        [DoesNotAutoMap, DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public decimal SubTotal
        {
            get { return TotalDirectCost + ConstructionOverheadCost; }
        }

        // This is a placeholder for when Keane or someone decides that the FTC thing
        // needs to be added to this.
        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public decimal TotalEstimatedCost
        {
            get { return SubTotal; }
        }

        public bool IsNonFramework
        {
            get { return Project.IsNonFramework; }
        }

        #endregion

        #region Constructors

        public EstimateForm(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void Map(EstimatingProject entity)
        {
            base.Map(entity);
            Project = entity;
        }

        #endregion
    }

    public class CreateBidForm : ViewModel<EstimatingProject>
    {
        #region Properties

        [Required, DoesNotAutoMap]
        public string ProjectBidTitle { get; set; }

        [DoesNotAutoMap]
        [Required, DropDown, EntityMustExist(typeof(Employee)), Display(Name = "Project Manager")]
        public int? Employee { get; set; }
       
        [Required, DateTimePicker, DoesNotAutoMap]
        public DateTime? DueDateTime { get; set; }
        
        [DoesNotAutoMap]
        public DateTime? SubstantiallyCompleteDate { get; set; }
       
        [DoesNotAutoMap]
        public DateTime? CompleteAndReadyForBillingDate { get; set; }
       
        [DoesNotAutoMap]
        public bool NoPdf { get; set; }

        #endregion

        #region Constructors

        public CreateBidForm(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = base.Validate(validationContext).ToList();

            if (!Employee.HasValue)
            {
                return results;
            }

            var employee = _container.GetInstance<IEmployeeRepository>().Find(Employee.Value);
            var format =
                "Chosen employee has no value for '{0}'.  Please update their employee record, or choose another employee.";

            if (employee.OperatingCenter == null)
            {
                results.Add(new ValidationResult(String.Format(format, "OperatingCenter")));
            }

            if (String.IsNullOrWhiteSpace(employee.PhoneWork))
            {
                results.Add(new ValidationResult(String.Format(format, "PhoneWork")));
            }

            if (String.IsNullOrWhiteSpace(employee.EmailAddress))
            {
                results.Add(new ValidationResult(String.Format(format, "EmailAddress")));
            }

            return results;
        }

        #endregion
    }

    public class BidForm : ViewModel<EstimatingProject>
    {
        #region Properties

        [DoesNotAutoMap]
        public EstimatingProject Project { get; protected set; }

        [DoesNotAutoMap]
        public CreateBidForm Throwaways { get; protected set; }

        [DoesNotAutoMap]
        public Employee Employee { get; protected set; }

        #endregion

        #region Constructors

        [DefaultConstructor]
        public BidForm(IContainer container) : base(container) {}

        public BidForm(IContainer container, CreateBidForm throwaways) : this(container)
        {
            Throwaways = throwaways;
            Employee = _container.GetInstance<IEmployeeRepository>().Find(throwaways.Employee.Value);
        }

        #endregion

        #region Exposed Methods

        public override void Map(EstimatingProject entity)
        {
            base.Map(entity);
            Project = entity;
        }

        #endregion
    }

    public class FieldReviewUnitSummary : ViewModel<EstimatingProject>
    {
        #region Properties

        public OperatingCenter OperatingCenter { get; set; }

        [Display(Name = "Preliminary File Number")]
        public string ProjectNumber { get; set; }

        public string ProjectName { get; set; }
        public virtual string WBSNumber { get; set; }
        public string Street { get; set; }
        public Town Town { get; set; }
        public Contractor Contractor { get; set; }
        public Employee Estimator { get; set; }

        public virtual IList<EstimatingProjectContractorLaborCost> GroupedContractorLaborCosts { get; set; }

        #endregion

        #region Constructors

        public FieldReviewUnitSummary(IContainer container) : base(container) {}

        #endregion
    }
}