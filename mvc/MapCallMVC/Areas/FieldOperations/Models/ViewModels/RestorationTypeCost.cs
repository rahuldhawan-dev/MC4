using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class RestorationTypeCostViewModel : ViewModel<RestorationTypeCost>
    {
        #region Properties

        [Required]
        public virtual double? Cost { get; set; }

        [Required]
        public virtual int? FinalCost { get; set; }

        #endregion

        #region Constructors

        public RestorationTypeCostViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateRestorationTypeCost : RestorationTypeCostViewModel
    {
        #region Properties

        [Required, DropDown, EntityMap, EntityMustExist(typeof(RestorationType))]
        public virtual int? RestorationType { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }

        #endregion

        #region Constructors

        public CreateRestorationTypeCost(IContainer container) : base(container) { }

        #endregion
    }

    public class EditRestorationTypeCost : RestorationTypeCostViewModel
    {
        #region Properties

        [DoesNotAutoMap]
        public RestorationTypeCost DisplayRestorationTypeCost => Original;

        #endregion

        #region Constructors

        public EditRestorationTypeCost(IContainer container) : base(container) { }

        #endregion
    }

    public class SearchRestorationTypeCost : SearchSet<RestorationTypeCost>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }

        #endregion
    }
}