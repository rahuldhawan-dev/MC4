using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Utilities;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.ProjectManagement.Models.ViewModels
{
    public class EstimatingProjectOtherCostViewModel : ViewModel<EstimatingProjectOtherCost>
    {
        #region Properties

        [Required, EntityMustExist(typeof(EstimatingProject)), EntityMap]
        public virtual int? EstimatingProject { get; set; }
        [Required]
        public virtual int? Quantity { get; set; }
        [Required, StringLength(EstimatingProjectOtherCost.StringLengths.DESCRIPTION)]
        public virtual string Description { get; set; }
        [Required, DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS, ApplyFormatInEditMode = true)]
        public virtual decimal? Cost { get; set; }

        #endregion

        #region Constructors

        public EstimatingProjectOtherCostViewModel(IContainer container) : base(container) { }

        #endregion
    }
}