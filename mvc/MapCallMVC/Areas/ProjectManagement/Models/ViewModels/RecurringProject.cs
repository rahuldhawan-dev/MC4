using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using Microsoft.Ajax.Utilities;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.ProjectManagement.Models.ViewModels
{
    public class RecurringProjectViewModel : ViewModel<RecurringProject>
    { 

        #region Properties

        //[AutoMap(MapDirections.None)]
        //public OperatingCenter DisplayOperatingCenter { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(OperatingCenter)), DropDown]
        public int? OperatingCenter { get; set; }
        [Required, EntityMap, EntityMustExist(typeof(Town))]
        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? Town { get; set; }
        [Required, EntityMap, EntityMustExist(typeof(RecurringProjectType)), DropDown]
        public int? RecurringProjectType { get; set; }
        [Required, EntityMap, EntityMustExist(typeof(PipeDiameter)), DropDown]
        public int? ProposedDiameter { get; set; }
        [Required, EntityMap, EntityMustExist(typeof(PipeMaterial)), DropDown]
        public int? ProposedPipeMaterial { get; set; }
        [Required, EntityMap, EntityMustExist(typeof(AssetInvestmentCategory)), DropDown]
        public int? AcceleratedAssetInvestmentCategory { get; set; }
        [EntityMap, EntityMustExist(typeof(AssetInvestmentCategory)), DropDown]
        public int? SecondaryAssetInvestmentCategory { get; set; }
        [EntityMap, EntityMustExist(typeof(RecurringProjectStatus)), DropDown]
        public int? Status { get; set; }
        [Coordinate(IconSet = IconSets.Shovels)]
        [Required(ErrorMessage = "A coordinate is required when infomaster mains are not selected."), EntityMap, EntityMustExist(typeof(Coordinate))]
        public int? Coordinate { get; set; }
        [EntityMap, EntityMustExist(typeof(FoundationalFilingPeriod)), DropDown]
        public int? FoundationalFilingPeriod { get; set; }
        [Required, EntityMap, EntityMustExist(typeof(AssetCategory)), DropDown]
        public int? AssetCategory { get; set; }
        [Required, EntityMap, EntityMustExist(typeof(AssetType)), DropDown]
        public int? AssetType { get; set; }
        [EntityMap, EntityMustExist(typeof(RecurringProjectRegulatoryStatus)), DropDown]
        public int? RegulatoryStatus { get; set; }
        [EntityMap, EntityMustExist(typeof(OverrideInfoMasterReason)), DropDown]
        [RequiredWhen("TotalInfoMasterScore", ComparisonType.EqualToAny, new[] { "1", "1.0", "1.1", "1.2", "1.3", "1.4", "1.5", "1.6", "1.7", "1.8", "1.9", "2", "2.0", "2.1", "2.2", "2.3", "2.4", "2.5" })]
        public int? OverrideInfoMasterReason { get; set; }
        [Required, StringLength(255)]
        public string ProjectTitle { get; set; }
        [Required, Multiline]
        public string ProjectDescription { get; set; }
        public int? District { get; set; }
        public int? OriginationYear { get; set; }
        [StringLength(14)]
        public string HistoricProjectID { get; set; }
        [Required]
        public int? NJAWEstimate { get; set; }
        [Required]
        public int? ProposedLength { get; set; }
        [Multiline]
        public string Justification { get; set; }
        public int? EstimatedProjectDuration { get; set; }
        public DateTime? EstimatedInServiceDate { get; set; }
        public DateTime? ActualInServiceDate { get; set; }
        [StringLength(10)]
        public string TotalInfoMasterScore { get; set; }
        public decimal? FinalCriteriaScore { get; set; }
        public decimal? FinalRawScore { get; set; }
        [StringLength(18), RequiredWhen("ActualInServiceDate", ComparisonType.NotEqualTo, null)]
        public virtual string WBSNumber { get; set; }
        [RequiredWhen("TotalInfoMasterScore", ComparisonType.EqualToAny, new[] { "1", "1.0", "1.1", "1.2", "1.3", "1.4", "1.5", "1.6", "1.7", "1.8", "1.9", "2", "2.0", "2.1", "2.2", "2.3", "2.4", "2.5"})]
        public bool? OverrideInfoMasterDecision { get; set; }
        [RequiredWhen("TotalInfoMasterScore", ComparisonType.EqualToAny, new[] { "1", "1.0", "1.1", "1.2", "1.3", "1.4", "1.5", "1.6", "1.7", "1.8", "1.9", "2", "2.0", "2.1", "2.2", "2.3", "2.4", "2.5" })]
        [Multiline]
        public string OverrideInfoMasterJustification { get; set; }

        [StringLength(6)]
        public string CPSReferenceId { get; set; }

        //public virtual IList<RecurringProjectEndorsement> ProjectEndorsements { get; set; }
        //public virtual IList<HighCostFactor> HighCostFactors { get; set; }
        [CheckBoxList]
        public virtual List<int> HighCostFactors { get; set; }


        [MultiSelect, EntityMap, View(Description="Please select one or more inaccuracy reason using ctlr+click.")]
        [RequiredWhen("OverrideInfoMasterReason", ComparisonType.EqualTo, MapCall.Common.Model.Entities.OverrideInfoMasterReason.Indices.GIS_DATA_INCORRECT)]
        public virtual List<int> GISDataInaccuracies { get; set; }
        [MultiSelect("FieldOperations", "WorkOrder", "ByTownIdForMainBreaks", DependsOn="Town", PromptText = "Please select a town.")]
        [RequiredWhen("GISDataInaccuracies", ComparisonType.Contains, GISDataInaccuracyType.Indices.MAIN_BREAK)]
        public virtual List<int> MainBreakOrders { get; set; }
        [EntityMap, EntityMustExist(typeof(PipeDiameter)), DropDown]
        [RequiredWhen("GISDataInaccuracies", ComparisonType.Contains,GISDataInaccuracyType.Indices.DIAMETER)]
        public virtual int? CorrectDiameter { get; set; }
        [EntityMap, EntityMustExist(typeof(PipeMaterial)), DropDown]
        [RequiredWhen("GISDataInaccuracies", ComparisonType.Contains, GISDataInaccuracyType.Indices.MATERIAL)]
        public virtual int? CorrectMaterial { get; set; }
        [RequiredWhen("GISDataInaccuracies", ComparisonType.Contains, GISDataInaccuracyType.Indices.DATE)]
        public virtual DateTime? CorrectInstallationDate { get; set; }


        public decimal? ExistingDiameterOverride { get; set; }
        public string ExistingPipeMaterialOverride { get; set; }
        [ClientCallback("RecurringProjectEdit.validateDecadeInstalledOverride", ErrorMessage = "Please enter a valid decade. Must end in 0.")]
        public int? DecadeInstalledOverride { get; set; }

        [DoesNotAutoMap("Not an actual view property, set by MapToEntity and used by controller.")]
        public bool SendNotificationOnSave { get; set; }
        [DoesNotAutoMap("Not an actual view property, set by MapToEntity and used by controller.")]
        public bool SendGISDataIncorrectOnSave { get; set; }

        [DisplayName("Upload File"), DoesNotAutoMap]
        [FileUpload]
        public AjaxFileUpload FileUpload { get; set; }

        #endregion

        #region Constructors

        public RecurringProjectViewModel(IContainer container) : base(container)
        {
            HighCostFactors = HighCostFactors ?? new List<int>();
            GISDataInaccuracies = GISDataInaccuracies ?? new List<int>();
            MainBreakOrders = MainBreakOrders ?? new List<int>();

        }

        #endregion

        #region Private Methods

        protected void MapHighCostFactors(RecurringProject entity)
        {
            var highCostFactorRepository = _container.GetInstance<IRepository<HighCostFactor>>();
            entity.HighCostFactors.Clear();
            foreach (var hcf in HighCostFactors)
            {
                entity.HighCostFactors.Add(highCostFactorRepository.Find(hcf));
            }
        }
        protected void MapGISDataInaccuracies(RecurringProject entity)
        {
            var repo = _container.GetInstance<IRepository<GISDataInaccuracyType>>();
            entity.GISDataInaccuracies.Clear();
            foreach (var gdi in GISDataInaccuracies)
            {
                entity.GISDataInaccuracies.Add(repo.Find(gdi));
            }
        }
        protected void MapMainBreakOrders(RecurringProject entity)
        {
            var repo = _container.GetInstance<IRepository<WorkOrder>>();
            entity.MainBreakOrders.Clear();
            foreach (var wo in MainBreakOrders)
            {
                entity.MainBreakOrders.Add(repo.Find(wo));
            }
        }

        #endregion

        #region Exposed Methods

        public override RecurringProject MapToEntity(RecurringProject entity)
        {
            if (ActualInServiceDate.HasValue)
            {
                Status = RecurringProjectStatus.Indices.COMPLETE;
            }
            base.MapToEntity(entity);
            MapHighCostFactors(entity);
            MapGISDataInaccuracies(entity);
            MapMainBreakOrders(entity);
            return entity;
        }


        #endregion
    }

    public class CreateRecurringProject : RecurringProjectViewModel
    {
        #region Constants

        public const string NO_MAINS = "Mains must be linked to new Recurring Projects";

        #endregion

        #region Properties

        public virtual IList<PipeDataLookupValue> PipeDataLookupValues { get; set; }
        public virtual IList<CreateRecurringProjectMain> RecurringProjectMains { get; set; }

        #endregion

        #region Constructors

        public CreateRecurringProject(IContainer container) : base(container)
        {
            PipeDataLookupValues = new List<PipeDataLookupValue>();
            RecurringProjectMains = new List<CreateRecurringProjectMain>();
        }

        #endregion

        #region Private Methods

        private void SetSendGISNotificationsOnSave()
        {
            SendGISDataIncorrectOnSave = false;

            if (OverrideInfoMasterReason.HasValue && OverrideInfoMasterReason == MapCall.Common.Model.Entities.OverrideInfoMasterReason.Indices.GIS_DATA_INCORRECT)
            {
                SendGISDataIncorrectOnSave = true;
            }
        }

        private void MapMainsToEntity(RecurringProject entity)
        {
            foreach (var main in RecurringProjectMains)
            {
                var rpm = new RecurringProjectMain();
                main.MapToEntity(rpm);
                rpm.RecurringProject = entity;
                entity.RecurringProjectMains.Add(rpm);
            }
        }

        private IEnumerable<ValidationResult> ValidateMains()
        {
            // Bug 3351:
            // If RecurringProjectType == "New" we can ignore this
            // If AssetCategory == "Wastewater" we can ignore this.

            // NOTE: RecurringProjectType and AssetCategory are required fields so it's assumed these values 
            // are set and validated before this validation method is called.

            if (_container.GetInstance<IRepository<RecurringProjectType>>()
                    .Find(RecurringProjectType.GetValueOrDefault())?.Description == "New")
            {
                yield break;
            }

            if (
                _container.GetInstance<IRepository<AssetCategory>>()
                    .Find(AssetCategory.GetValueOrDefault())?.Description == "Wastewater")
            {
                yield break;
            }

            if (RecurringProjectMains.Count == 0)
                yield return new ValidationResult(NO_MAINS, new[] { "RecurringProjectMains" });
        }

        #endregion

        #region Exposed Methods

        public override RecurringProject MapToEntity(RecurringProject entity)
        {
            // project status -- not a default because we want this to error if someone decided to remove it from the db
            Status = RecurringProjectStatus.Indices.PROPOSED;
            SetSendGISNotificationsOnSave();

            if (Convert.ToDouble(entity.TotalInfoMasterScore) <= 2.5 && !entity.TotalInfoMasterScore.IsNullOrWhiteSpace() && OverrideInfoMasterDecision != true)
            {
                OverrideInfoMasterDecision = true;
                entity.OverrideInfoMasterDecision = true;
            }

            base.MapToEntity(entity);

            var currentActivePipeDataValues = _container.GetInstance<IRepository<PipeDataLookupValue>>().GetAll().Where(x => x.IsDefault && x.IsEnabled);
            foreach (var pdv in currentActivePipeDataValues)
            {
                entity.PipeDataLookupValues.Add(pdv);
            }

            MapMainsToEntity(entity);

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateMains());
        }

        #endregion
    }

    public class EditRecurringProject : RecurringProjectViewModel
    {
        #region Constants

        public const short MAIN_CUTOFF_ID = 7500; // I"M ALSO IN edit.js, if you update me, please update me there too.
        public const string NO_MAINS = "Mains must be linked to Recurring Projects";

        #endregion

        #region Fields

        protected readonly IViewModelFactory _viewModelFactory;

        #endregion

        #region Display Only

        private RecurringProject _displayRecurringProject;

        [AutoMap(MapDirections.None)]
        public RecurringProject DisplayRecurringProject
        {
            get
            {
                if (_displayRecurringProject == null)
                    _displayRecurringProject = _container.GetInstance<IRecurringProjectRepository>().Find(Id);
                return _displayRecurringProject;
            }
        }
        
        #endregion

        #region Properties

        public virtual IList<EditRecurringProjectMain> RecurringProjectMains { get; set; }

        [ClientCallback("RecurringProjectEdit.validateWBSNumber")]
        public override string WBSNumber
        {
            get { return base.WBSNumber; }
            set { base.WBSNumber = value; }
        }

        #endregion

        #region Private Methods

        private void SetSendNotificationsOnSave(RecurringProject entity)
        {
            SendNotificationOnSave = false;

            if ((entity.Status == null || entity.Status.Id != RecurringProjectStatus.Indices.COMPLETE) && Status == RecurringProjectStatus.Indices.COMPLETE)
            {
                SendNotificationOnSave = true;
            }
        }

        private void SetSendGISNotificationsOnSave(RecurringProject entity)
        {
            SendGISDataIncorrectOnSave = false;

            if ((entity.OverrideInfoMasterReason == null && OverrideInfoMasterReason.HasValue && OverrideInfoMasterReason == MapCall.Common.Model.Entities.OverrideInfoMasterReason.Indices.GIS_DATA_INCORRECT) || 
                (entity.OverrideInfoMasterReason != null && entity.OverrideInfoMasterReason.Id != MapCall.Common.Model.Entities.OverrideInfoMasterReason.Indices.GIS_DATA_INCORRECT && OverrideInfoMasterReason == MapCall.Common.Model.Entities.OverrideInfoMasterReason.Indices.GIS_DATA_INCORRECT))
            {
                SendGISDataIncorrectOnSave = true;
            }
        }

        private void CalculateScores()
        {
            if (Status.HasValue 
                && Status == RecurringProjectStatus.Indices.AP_APPROVED
                && DisplayRecurringProject.Status.Id != Status
                && DisplayRecurringProject.PipeDataLookupValues.Count > 0)
            {
                FinalCriteriaScore = DisplayRecurringProject.PipeDataLookupValues.Sum(x => x.VariableScore)/
                                     DisplayRecurringProject.PipeDataLookupValues.Count(x => x.VariableScore > 0);
                FinalRawScore = DisplayRecurringProject.PipeDataLookupValues.Sum(x => x.PriorityWeightedScore)/
                                DisplayRecurringProject.PipeDataLookupValues.Count(x => x.VariableScore > 0);
            }
        }

        private IEnumerable<ValidationResult> ValidateMains()
        {
            // Bug 3351:
            // If RecurringProjectType == "New" we can ignore this
            // If AssetCategory == "Wastewater" we can ignore this.

            // NOTE: RecurringProjectType and AssetCategory are required fields so it's assumed these values 
            // are set and validated before this validation method is called.

            if (_container.GetInstance<IRepository<RecurringProjectType>>()
                    .Find(RecurringProjectType.GetValueOrDefault())?.Description == "New")
            {
                yield break;
            }

            if (
                _container.GetInstance<IRepository<AssetCategory>>()
                    .Find(AssetCategory.GetValueOrDefault())?.Description == "Wastewater")
            {
                yield break;
            }

            if (Id > MAIN_CUTOFF_ID && RecurringProjectMains.Count == 0)
                yield return new ValidationResult(NO_MAINS, new[] { "RecurringProjectMains" });
        }

        private IEnumerable<ValidationResult> ValidateWBSNumber()
        {
            // Requirements for this validation come from bug 3990
            if (!OperatingCenter.HasValue || !Status.HasValue)
            {
                yield break;
            }

            var opc = _container.GetInstance<IOperatingCenterRepository>().Find(OperatingCenter.Value);
            if (opc.IsContractedOperations)
            {
                yield break;
            }

            var status = _container.GetInstance<IRepository<RecurringProjectStatus>>().Find(Status.Value);
            if (status.Id == RecurringProjectStatus.Indices.COMPLETE && string.IsNullOrWhiteSpace(WBSNumber))
            {
                yield return new ValidationResult("The WBS Charged field is required.", new[] { nameof(WBSNumber) });
            }
        }

        private void MapToViewModelHighCostFactors(RecurringProject entity)
        {
            HighCostFactors = new List<int>();
            var highCostFactors = _container.GetInstance<IRepository<HighCostFactor>>().GetAllSorted();
            foreach (var hcf in highCostFactors)
            {
                if (entity.HighCostFactors.Contains(hcf))
                {
                    HighCostFactors.Add(hcf.Id);
                }
            }
        }

        private void MapToViewModelGISDataInaccuracies(RecurringProject entity)
        {
            GISDataInaccuracies = entity.GISDataInaccuracies.Select(x => x.Id).ToList();
        }

        private void MapToViewModelMainBreakOrders(RecurringProject entity)
        {
            MainBreakOrders = entity.MainBreakOrders.Select(x => x.Id).ToList();
        }

        #endregion

        #region Exposed Methods

        public override RecurringProject MapToEntity(RecurringProject entity)
        {
            SetSendNotificationsOnSave(entity);
            SetSendGISNotificationsOnSave(entity);
            CalculateScores();
            entity.RecurringProjectMains.Clear();
            // these come from inputs in the form like this:
            // id: RecurringProjectMains_0__Layer
            // name: RecurringProjectMains[0].Layer
            foreach (var recurringProjectMain in RecurringProjectMains)
            {
                entity.RecurringProjectMains.Add(new RecurringProjectMain {
                    Guid = recurringProjectMain.Guid,
                    RecurringProject = entity,
                    Length = recurringProjectMain.Length.Value,
                    Layer = recurringProjectMain.Layer,
                    DateInstalled = recurringProjectMain.DateInstalled,
                    Diameter = recurringProjectMain.Diameter,
                    Material = recurringProjectMain.Material

                    //, TotalInfoMasterScore = recurringProjectMain.TotalInfoMasterScore.Value
                });
            }
            return base.MapToEntity(entity);
        }

        public override void Map(RecurringProject entity)
        {
            if (Convert.ToDouble(entity.TotalInfoMasterScore) <= 2.5 && !entity.TotalInfoMasterScore.IsNullOrWhiteSpace() && OverrideInfoMasterDecision != true)
            {
                OverrideInfoMasterDecision = true;
                entity.OverrideInfoMasterDecision = true;
            }

            base.Map(entity);
            MapToViewModelHighCostFactors(entity);
            MapToViewModelGISDataInaccuracies(entity);
            MapToViewModelMainBreakOrders(entity);
            foreach (var main in entity.RecurringProjectMains)
            {
                RecurringProjectMains.Add(_viewModelFactory.Build<EditRecurringProjectMain, RecurringProjectMain>(main));
            }
        }
        
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateMains()).Concat(ValidateWBSNumber());
        }
        
        #endregion

        #region Constructors

        public EditRecurringProject(IContainer container) : base(container) { }

        public EditRecurringProject(IContainer container, IViewModelFactory viewModelFactory) : base(container)
        {
            HighCostFactors = new List<int>();
            RecurringProjectMains = new List<EditRecurringProjectMain>();
            _viewModelFactory = viewModelFactory;
        }

        #endregion
	}

    public class RecurringProjectForGISNotification : ViewModel<RecurringProject>
    {
        public OperatingCenter OperatingCenter { get; set; }
        public virtual Town Town { get; set; }
        public virtual RecurringProjectType RecurringProjectType { get; set; }
        public virtual PipeDiameter ProposedDiameter { get; set; }
        public virtual PipeMaterial ProposedPipeMaterial { get; set; }
        public virtual AssetInvestmentCategory AcceleratedAssetInvestmentCategory { get; set; }
        public virtual AssetInvestmentCategory SecondaryAssetInvestmentCategory { get; set; }
        public virtual RecurringProjectRegulatoryStatus RegulatoryStatus { get; set; }
        [Description("Will always be set to COMPLETE if an Actual In Service Date is entered")]
        public virtual RecurringProjectStatus Status { get; set; }
        public virtual Coordinate Coordinate { get; set; }
        public virtual FoundationalFilingPeriod FoundationalFilingPeriod { get; set; }
        public virtual AssetCategory AssetCategory { get; set; }
        public virtual AssetType AssetType { get; set; }
        public virtual PipeDiameter CorrectDiameter { get; set; }
        public virtual PipeMaterial CorrectMaterial { get; set; }
        public virtual string ProjectTitle { get; set; }
        public virtual string ProjectDescription { get; set; }
        public virtual int? District { get; set; }
        public virtual int? OriginationYear { get; set; }
        public virtual string HistoricProjectID { get; set; }
        [DisplayName("NJAW Estimate (Dollars)"), DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY_NO_DECIMAL)]
        public virtual int NJAWEstimate { get; set; }
        [DisplayName("Proposed Length (ft)")]
        public virtual int? ProposedLength { get; set; }
        [DisplayName("CPS Ref #")]
        public virtual string CPSReferenceId { get; set; }
        public virtual string Justification { get; set; }
        [DisplayName("Estimated Project Duration (days)")]
        public virtual int? EstimatedProjectDuration { get; set; }
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? EstimatedInServiceDate { get; set; }
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? ActualInServiceDate { get; set; }
        [View(DisplayName = "Secondary Criteria Score", Description = "Once approved the final criteria score will be calculated and locked.")]
        public virtual decimal? FinalCriteriaScore { get; set; }
        [View(DisplayName = "Secondary Raw Score", Description = "Once approved the final raw score will be calculated and locked.")]
        public virtual decimal? FinalRawScore { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual string WBSNumber { get; set; }
        public virtual string ExistingPipeMaterialOverride { get; set; }
        public virtual int? DecadeInstalledOverride { get; set; }
        public virtual decimal? ExistingDiameterOverride { get; set; }
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? CorrectInstallationDate { get; set; }
        public virtual bool? OverrideInfoMasterDecision { get; set; }
        public virtual OverrideInfoMasterReason OverrideInfoMasterReason { get; set; }
        public virtual string OverrideInfoMasterJustification { get; set; }
        public virtual string TotalInfoMasterScore { get; set; }


        [DoesNotAutoMap("Controller sets this property then passes the model to a notification template.")]
        public string ModifiedByEmail { get; set; }
        [DoesNotAutoMap("Controller sets this property then passes the model to a notification template.")]
        public string ModifiedBy { get; set; }
        [DoesNotAutoMap("Controller sets this property then passes the model to a notification template.")]
        public string RecordUrl { get; set; }
        public IList<GISDataInaccuracyType> GISDataInaccuracies { get; set; }
        public IList<WorkOrder> MainBreakOrders { get; set; }

        #region Constructors

        public RecurringProjectForGISNotification(IContainer container) : base(container){ }

        #endregion
    }

    public class AddRecurringProjectEndorsement : ViewModel<RecurringProject>
    {
        [DropDown, AutoMap(MapDirections.None), Required]
        public int? EndorsementStatus { get; set; }

        [Required, AutoMap(MapDirections.None), Multiline]
        public string Comment { get; set; }

        #region Constructors

        public AddRecurringProjectEndorsement(IContainer container) : base(container) { }

        #endregion

        public override RecurringProject MapToEntity(RecurringProject entity)
        {
            entity = base.MapToEntity(entity);
            var endorsementStatus = _container.GetInstance<IRepository<EndorsementStatus>>().Find(EndorsementStatus.Value);
            entity.ProjectEndorsements.Add(new RecurringProjectEndorsement {
                RecurringProject = entity,
                EndorsementStatus = endorsementStatus,
                Comment = Comment,
                EndorsementDate = _container.GetInstance<IDateTimeProvider>().GetCurrentDate(),
                User = _container.GetInstance<IAuthenticationService<User>>().CurrentUser
            });
            return entity;
        }
    }

    public class RemoveRecurringProjectEndorsement : ViewModel<RecurringProject>
    {
        #region Properties

        [Required, DoesNotAutoMap("Mapped manually.")]
        public virtual int RecurringProjectEndorsementId { get; set; }

        #endregion

        #region Constructors

        public RemoveRecurringProjectEndorsement(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override RecurringProject MapToEntity(RecurringProject entity)
        {
            // Do not call base.MapToEntity as there's nothing for it to do.
            entity.ProjectEndorsements.Remove(
                _container.GetInstance<IRepository<RecurringProjectEndorsement>>()
                    .Find(RecurringProjectEndorsementId));
            return entity;
        }

        #endregion
    }

    public class SearchRecurringProject : SearchSet<RecurringProject>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center above")]
        public int? Town { get; set; }

        public string WBSNumber { get; set; }
        public string ProjectTitle { get; set; }
        [DisplayName("Project ID")]
        public int? EntityId { get; set; }
        public string HistoricProjectID { get; set; }
        [MultiSelect]
        public int[] Status { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(RecurringProjectType))]
        public int? RecurringProjectType { get; set; }
        public DateRange EstimatedInServiceDate { get; set; }
        public DateRange ActualInServiceDate { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(FoundationalFilingPeriod))]
        public int? FoundationalFilingPeriod { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(AssetCategory))]
        public int? AssetCategory { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(AssetType))]
        public int? AssetType { get; set; }

        public bool? HasMainsSelected { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(RecurringProjectRegulatoryStatus))]
        public int? RegulatoryStatus { get; set; }

        #endregion
    }
}
 