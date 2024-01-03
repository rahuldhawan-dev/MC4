using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels
{
    public class ChemicalViewModel : ViewModel<Chemical>
    {
        #region Properties

        [Required]
        public virtual string Name { get; set; }
        
        [DropDown, EntityMap, EntityMustExist(typeof(ChemicalType))]
        public virtual int? ChemicalType { get; set; }
        [Required]
        public virtual string PartNumber { get; set; }
        public virtual decimal? PricePerPoundWet { get; set; }
        public virtual float? WetPoundsPerGal { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(PackagingType))]
        public virtual int? PackagingType { get; set; }
        public virtual float? PackagingQuantities { get; set; }
        public virtual string PackagingUnits { get; set; }
        public virtual string ChemicalSymbol { get; set; }
        [MultiSelect, EntityMap, EntityMustExist(typeof(StateOfMatter)), View("Chemical Forms")]
        public virtual int[] ChemicalStates { get; set; }
        public virtual string Appearance { get; set; }
        public virtual float? ChemicalConcentrationLiquid { get; set; }
        public virtual float? ConcentrationLbsPerGal { get; set; }
        public virtual float? SpecificGravityMin { get; set; }
        public virtual float? SpecificGravityMax { get; set; }
        public virtual float? RatioResidualProduction { get; set; }
        public virtual string CasNumber { get; set; }
        [StringLength(Chemical.StringLengths.SDS_HYPERLINK)]
        public virtual string SdsHyperlink { get; set; }
        public virtual int? SubNumber { get; set; }
        public virtual int? DepartmentOfTransportationNumber { get; set; }
        [BoolFormat("Pure", "Mixture")]
        public virtual bool? IsPure { get; set; }
        public virtual bool? TradeSecret { get; set; }
        public virtual bool? EmergencyPlanningCommunityRightToKnowActOnly { get; set; }
        [CheckBoxList, EntityMap, EntityMustExist(typeof(HealthHazard))]
        public virtual int[] HealthHazards { get; set; }
        [CheckBoxList, EntityMap, EntityMustExist(typeof(PhysicalHazard))]
        public virtual int[] PhysicalHazards { get; set; }
        
        [CheckBox]
        public virtual bool? ExtremelyHazardousChemical { get; set; }

        #endregion

        #region Constructors

        public ChemicalViewModel(IContainer container) : base(container) {}

        #endregion
	}

    public class CreateChemical : ChemicalViewModel
    {
        #region Constructors

        public CreateChemical(IContainer container) : base(container) {}

        #endregion
	}

    public class EditChemical : ChemicalViewModel
    {
        #region Constructors

        public EditChemical(IContainer container) : base(container) {}

        #endregion
	}

    public class SearchChemical : SearchSet<Chemical>
    {
        #region Properties

        public SearchString Name { get; set; }
        public SearchString PartNumber { get; set; }
        [MultiSelect, EntityMap, EntityMustExist(typeof(ChemicalType))]
        public int[] ChemicalType { get; set; }
        
        public bool? ExtremelyHazardousChemical { get; set; }

        #endregion
    }
}