using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.ProjectManagement.Models.ViewModels
{
    public class BaseContractorOverrideLaborCostViewModel : ViewModel<ContractorOverrideLaborCost>
    {
        #region Properties

        [DropDown, Required, EntityMap, EntityMustExist(typeof(Contractor))]
        public int? Contractor { get; set; }

        [DropDown, Required, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [DropDown("ProjectManagement", "ContractorLaborCost", "FindByOperatingCenterId", DependsOn = "OperatingCenter"), Required, EntityMap, EntityMustExist(typeof(ContractorLaborCost))]
        public int? ContractorLaborCost { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS, ApplyFormatInEditMode = true)]
        [Range(0, Double.MaxValue, ErrorMessage = "Cost must be greater than or equal to zero."), RequiredWhen("Percentage", ComparisonType.EqualTo, null)]
        public decimal? Cost { get; set; }
        [DisplayFormat(DataFormatString = "{0:p}"), RequiredWhen("Cost", ComparisonType.EqualTo, null)]
        [Range(1, 100)]
        public virtual int? Percentage { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public DateTime? EffectiveDate { get; set; }

        #endregion

        #region Constructors

        public BaseContractorOverrideLaborCostViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateContractorOverrideLaborCost : BaseContractorOverrideLaborCostViewModel
    {
        public CreateContractorOverrideLaborCost(IContainer container) : base(container) { }
    }

    public class EditContractorOverrideLaborCost : BaseContractorOverrideLaborCostViewModel
    {
        public EditContractorOverrideLaborCost(IContainer container) : base(container) { }
    }

    public class SearchContractorOverrideLaborCost : SearchSet<ContractorOverrideLaborCost>
    {
        #region Properties

        [DropDown]
        public int? Contractor { get; set; }

        [DropDown]
        public int? ContractorLaborCost { get; set; }

        [DropDown]
        public int? OperatingCenter { get; set; }

        #endregion
    }
}