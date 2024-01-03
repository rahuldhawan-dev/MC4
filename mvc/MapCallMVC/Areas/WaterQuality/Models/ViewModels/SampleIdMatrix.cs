using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.WaterQuality.Models.ViewModels
{
    public class SampleIdMatrixViewModel : ViewModel<SampleIdMatrix>
    {
        #region Properties

        [AutoMap(MapDirections.None)]
        [DropDown]
        public int? OperatingCenter { get; set; }
        [DropDown("WaterQuality", "SampleSite", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText="Please select an operating center above.")]
        [EntityMap, EntityMustExist(typeof(SampleSite))]
        [Required]
        public int? SampleSite { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(WaterConstituent))]
        public int? WaterConstituent { get; set; }
        public string Parameter { get; set; }
        public bool RoutineSample { get; set; }
        public string ProcessStage { get; set; }
        public float? ProcessStageSequence { get; set; }
        public float? ParameterSequence { get; set; }
        public string SamplePurpose { get; set; }
        public string ProcessReasonForSample { get; set; }
        public string PerformedBy { get; set; }
        public float? Frequency { get; set; }
        public string DataStorageLocation { get; set; }
        public string MethodInstrumentLaboratory { get; set; }
        public string TatBellvilleLabHrs { get; set; }
        public string BellevilleSampleId { get; set; }
        public string InterferenceBy { get; set; }
        public string DataStorageLocationOnLineInstrument { get; set; }
        public string IHistorianSignalIdOnLineInstrument { get; set; }
        public string ComplianceReq { get; set; }
        public string ProcessTarget { get; set; }
        public string TriggerPhase1 { get; set; }
        public string ActionPhase1 { get; set; }
        public string TriggerPhase2 { get; set; }
        public string ActionPhase2 { get; set; }
        public string Comment { get; set; }
        public string ScadaNotes { get; set; }

        #endregion

        #region Constructors

        public SampleIdMatrixViewModel(IContainer container) : base(container) {}

        #endregion
	}

    public class CreateSampleIdMatrix : SampleIdMatrixViewModel
    {
        #region Constructors

		public CreateSampleIdMatrix(IContainer container) : base(container) {}

        #endregion
	}

    public class EditSampleIdMatrix : SampleIdMatrixViewModel
    {
        #region Constructors

		public EditSampleIdMatrix(IContainer container) : base(container) {}

        #endregion

        public override void Map(SampleIdMatrix entity)
        {
            base.Map(entity);
            OperatingCenter = entity.SampleSite?.OperatingCenter?.Id;
        }
    }

    public class SearchSampleIdMatrix : SearchSet<SampleIdMatrix>
    {
        #region Properties

        [SearchAlias("SampleSite", "ss", "State.Id")]
        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [DropDown]
        [SearchAlias("SampleSite", "ss", "PublicWaterSupply.Id")]
        public virtual int? PublicWaterSupply { get; set; }

        [SearchAlias("SampleSite", "ss", "OperatingCenter.Id")]
        [MultiSelect("", "OperatingCenter", "GetByPublicWaterSupplyForWaterQuality", DependsOn = nameof(PublicWaterSupply), PromptText = "Please select a PWSID above.")]
        public int? OperatingCenter { get; set; }

        [DisplayName("Id")]
        public int? EntityId { get; set; }
        
        [DropDown("WaterQuality", "SampleSite", "ByOperatingCenterIds", DependsOn = "OperatingCenter")]
        [EntityMap, EntityMustExist(typeof(SampleSite))]
        public int? SampleSite { get; set; }

        [MultiSelect]
        public int[] WaterConstituent { get; set; }

        public string ProcessStage { get; set; }
        public float? ProcessStageSequence { get; set; }
        public float? ParameterSequence { get; set; }
        public SearchString Parameter { get; set; }
        public string SamplePurpose { get; set; }
        public float? Frequency { get; set; }
        public string PerformedBy { get; set; }
        public string MethodInstrumentLaboratory { get; set; }

        #endregion
    }
}