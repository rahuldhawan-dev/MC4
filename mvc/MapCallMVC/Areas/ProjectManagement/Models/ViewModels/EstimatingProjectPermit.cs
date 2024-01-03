using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.ProjectManagement.Models.ViewModels
{
    public class EstimatingProjectPermitViewModel : ViewModel<EstimatingProjectPermit>
    {
        public EstimatingProjectPermitViewModel(IContainer container) : base(container) { }
    }

    public class EditEstimatingProjectPermit : ViewModel<EstimatingProjectPermit>
    {
        #region Properties

        [Required, EntityMustExist(typeof(EstimatingProject)), EntityMap]
        public virtual int? EstimatingProject { get; set; }
        [Required]
        public virtual int Quantity { get; set; }
        [DropDown, EntityMap, Required]
        public virtual int PermitType { get; set; }
        [DropDown, EntityMap, Required]
        public virtual int AssetType { get; set; }
        [Required, DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS, ApplyFormatInEditMode = true)]
        public virtual decimal Cost { get; set; }

        #endregion
        
        #region Constructors

        public EditEstimatingProjectPermit(IContainer container) : base(container) {}

        #endregion
    }
}