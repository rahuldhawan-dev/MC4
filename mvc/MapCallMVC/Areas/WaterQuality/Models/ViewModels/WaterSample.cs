using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.WaterQuality.Models.ViewModels
{
    public class WaterSampleViewModel : ViewModel<WaterSample>
    {
        #region Properties

        [Required, DateTimePicker]
        public DateTime? SampleDate { get; set; }
        [StringLength(WaterSample.StringLengths.COLLECTED_BY)]
        public string CollectedBy { get; set; }
        [StringLength(WaterSample.StringLengths.ANALYSIS_PERFORMED_BY)]
        public string AnalysisPerformedBy { get; set; }
        public float? SampleValue { get; set; }
        public string Notes { get; set; }
        public bool NonDetect { get; set; }
        public bool IsInvalid { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(ReasonForSampleInvalidation))]
        [RequiredWhen("IsInvalid", true)]
        public int? ReasonForInvalidation { get; set; }

        #endregion

        #region Constructors

        public WaterSampleViewModel(IContainer container) : base(container) {}

        #endregion
	}

    public class CreateWaterSample : WaterSampleViewModel
    {
        #region Properties

        [AutoMap(MapDirections.None)]
        [DropDown]
        public int? OperatingCenter { get; set; }

        [AutoMap(MapDirections.None), DropDown("WaterQuality", "SampleSite", "ByOperatingCenterIdWithMatrices", DependsOn = "OperatingCenter", PromptText = "Please select Operating Center above.")]
        public int? SampleSite { get; set; }

        [Required, DropDown("WaterQuality", "SampleIdMatrix", "BySampleSiteId", DependsOn = "SampleSite", PromptText = "Please select Sample Site above."), EntityMap, EntityMustExist(typeof(SampleIdMatrix))]
        public int? SampleIdMatrix { get; set; }

        [Required, DropDown("WaterQuality", "UnitOfWaterSampleMeasure", "ForSampleIdMatrix", DependsOn = "SampleIdMatrix", PromptText = "Please select Sample Id Matrix above."), EntityMap, EntityMustExist(typeof(UnitOfWaterSampleMeasure))]
        public int UnitOfMeasure { get; set; }

        #endregion

        #region Exposed Methods

        public override void SetDefaults()
        {
            base.SetDefaults();
            CollectedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.UserName;
            if (SampleSite.HasValue)
            {
                var sampleSite = _container.GetInstance<IRepository<SampleSite>>().Find(SampleSite.Value);
                OperatingCenter = sampleSite.OperatingCenter.Id;
                if (sampleSite.SampleIdMatrices.Count == 1)
                {
                    SampleIdMatrix = sampleSite.SampleIdMatrices.FirstOrDefault().Id;
                }
            }
        }

        #endregion
        
        #region Constructors

		public CreateWaterSample(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override WaterSample MapToEntity(WaterSample entity)
        {
            entity = base.MapToEntity(entity);
            entity.AnalysisPerformedBy = string.IsNullOrWhiteSpace(entity.AnalysisPerformedBy)
                ? entity.SampleIdMatrix.PerformedBy
                : entity.AnalysisPerformedBy;
            return entity;
        }

        #endregion
    }

    public class EditWaterSample : WaterSampleViewModel
    {
        #region Properties
        
        [Required, DropDown, EntityMap, EntityMustExist(typeof(SampleIdMatrix))]
        public int? SampleIdMatrix { get; set; }

        #endregion

        #region Constructors

		public EditWaterSample(IContainer container) : base(container) {}

        #endregion
	}

    public class SearchWaterSample : SearchSet<WaterSample>
    {
        #region Properties

        [SearchAlias("sim.SampleSite", "ss", "State.Id")]
        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [DropDown]
        [SearchAlias("sim.SampleSite", "ss", "PublicWaterSupply.Id")]
        public virtual int? PublicWaterSupply { get; set; }

        [SearchAlias("sim.SampleSite", "ss", "OperatingCenter.Id")]
        [MultiSelect("", "OperatingCenter", "GetByPublicWaterSupplyForWaterQuality", DependsOn = nameof(PublicWaterSupply), PromptText = "Please select a PWSID above.")]
        public int[] OperatingCenter { get; set; }

        [DropDown("WaterQuality", "SampleSite", "ByOperatingCenterIds", DependsOn = "OperatingCenter")]
        [SearchAlias("SampleIdMatrix", "sim", "SampleSite.Id", Required = true)]
        public int? SampleSite { get; set; }

        [DropDown("WaterQuality", "SampleIDMatrix", "BySampleSiteId", DependsOn = "SampleSite"), EntityMap, EntityMustExist(typeof(SampleIdMatrix))]
        public int? SampleIdMatrix { get; set; }

        [DisplayName("Id")]
        public int? EntityId { get; set; }

        public DateRange SampleDate { get; set; }

        [MultiSelect]
        [SearchAlias("sim.WaterConstituent", "wc", "Id")]
        public int[] WaterConstituent { get; set; }

        [SearchAlias("SampleIdMatrix", "sim", "Parameter")]
        public SearchString Parameter { get; set; }

        public NumericRange SampleValue { get; set; }

        [SearchAlias("sim.SampleSite", "ss", "BactiSite")]
        public bool? BactiSite { get; set; }

        [SearchAlias("sim.SampleSite", "ss", "LeadCopperSite")]
        public bool? LeadCopperSite { get; set; }

        public bool? NonDetect { get; set; }
        public bool? IsInvalid { get; set; }
        public string CollectedBy { get; set; }

        #endregion
    }
}