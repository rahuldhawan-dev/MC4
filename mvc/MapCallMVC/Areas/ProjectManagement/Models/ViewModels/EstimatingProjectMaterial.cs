using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.ProjectManagement.Models.ViewModels
{
    public class EstimatingProjectMaterialViewModel : ViewModel<EstimatingProjectMaterial>
    {
        #region Properties

        [Required, EntityMustExist(typeof(EstimatingProject)),EntityMap]
        public virtual int EstimatingProject { get; set; }
        [Required, ComboBox, EntityMustExist(typeof(Material)), EntityMap]
        public virtual int Material { get; set; }
        [Required]
        public virtual int Quantity { get; set; }
        
        #endregion

        #region Constructors

        public EstimatingProjectMaterialViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateEstimatingProjectMaterial : EstimatingProjectMaterialViewModel
    {
        public CreateEstimatingProjectMaterial(IContainer container) : base(container) { }
    }

    public class EditEstimatingProjectMaterial : EstimatingProjectMaterialViewModel
    {
        #region Properties

        [DropDown, Required, EntityMap, EntityMustExist(typeof(AssetType))]
        public virtual int AssetType { get; set; }

        #endregion
        
        public EditEstimatingProjectMaterial(IContainer container) : base(container) { }
    }
}