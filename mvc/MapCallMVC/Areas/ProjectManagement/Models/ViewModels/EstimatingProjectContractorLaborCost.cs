using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.ProjectManagement.Models.ViewModels
{
    public class EditEstimatingProjectContractorLaborCost : ViewModel<EstimatingProjectContractorLaborCost>
    {
        #region Properties

        [Required, EntityMustExist(typeof(EstimatingProject)), EntityMap]
        public virtual int EstimatingProject { get; set; }
        [Required, ComboBox, EntityMap, EntityMustExist(typeof(ContractorLaborCost))]
        public virtual int? ContractorLaborCost { get; set; }
        [Required]
        public virtual int? Quantity { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(AssetType))]
        public virtual int? AssetType { get; set; }
        
        #endregion
        
        #region Constructors

        public EditEstimatingProjectContractorLaborCost(IContainer container) : base(container) { }

        #endregion
    }
}