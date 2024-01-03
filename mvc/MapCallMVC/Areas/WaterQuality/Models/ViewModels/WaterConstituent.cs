using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.WaterQuality.Models.ViewModels
{
    public class WaterConstituentViewModel : ViewModel<WaterConstituent>
    {
        #region Properties

        [Required]
        public string Description { get; set; }

        public float? Min { get; set; }
        public float? Max { get; set; }
        public float? Mcl { get; set; }
        public float? Mclg { get; set; }
        public float? Smcl { get; set; }

        [StringLength(255)]
        public string ActionLimit { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(UnitOfWaterSampleMeasure))]
        public int? UnitOfMeasure { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(DrinkingWaterContaminantCategory))]
        public int? DrinkingWaterContaminantCategory { get; set; }
        
        [DropDown, EntityMap, EntityMustExist(typeof(WasteWaterContaminantCategory))]
        public int? WasteWaterContaminantCategory { get; set; }

        [StringLength(255)]
        public string Regulation { get; set; }

        [StringLength(255)]
        public string SamplingFrequency { get; set; }

        [StringLength(255)]
        public string SamplingMethod { get; set; }

        [StringLength(255)]
        public string SampleContainerSizeMl { get; set; }

        [StringLength(255)]
        public string HoldingTimeHrs { get; set; }

        [StringLength(255)]
        public string PreservativeQuenchingAgent { get; set; }

        [StringLength(255)]
        public string AnalyticalMethod { get; set; }

        [StringLength(255)]
        public string TatBellvileDays { get; set; }

        #endregion

        #region Constructors

        public WaterConstituentViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class CreateWaterConstituent : WaterConstituentViewModel
    {
        #region Constructors

        public CreateWaterConstituent(IContainer container) : base(container) {}

        #endregion
    }

    public class EditWaterConstituent : WaterConstituentViewModel
    {
        #region Constructors

        public EditWaterConstituent(IContainer container) : base(container) {}

        #endregion
    }

    public class SearchWaterConstituent : SearchSet<WaterConstituent>
    {
        #region Properties

        [DisplayName("Id")]
        public int? EntityId { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(DrinkingWaterContaminantCategory))]
        public int? DrinkingWaterContaminantCategory { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(WasteWaterContaminantCategory))]
        public int? WasteWaterContaminantCategory { get; set; }
        public string Description { get; set; }
        public string SamplingFrequency { get; set; }

        [DisplayName("No EPA Limits")]
        public bool? NoEPALimits { get; set; }

        #endregion
    }
}