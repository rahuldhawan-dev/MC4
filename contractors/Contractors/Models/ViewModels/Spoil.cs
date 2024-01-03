using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace Contractors.Models.ViewModels
{
    public abstract class SpoilViewModel : ViewModel<Spoil>
    {
        #region Properties

        [Required]
        [Range(0, 9999.99)]  // Min = 0 , Max = 9999.99? That's what's on 271
        public decimal? Quantity { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(SpoilStorageLocation))]
        public int? SpoilStorageLocation { get; set; }

        #endregion

        #region Constructors

        public SpoilViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class SpoilNew : SpoilViewModel
    {
        #region Properties

        [Required, EntityMap, EntityMustExist(typeof(WorkOrder))]
        public int WorkOrder { get; set; }

        #endregion

        #region Constructors

        public SpoilNew(IContainer container) : base(container) { }

        #endregion
    }

    public class EditSpoil : SpoilViewModel
    {
        #region Constructors

        public EditSpoil(IContainer container) : base(container) { }

        #endregion
    }
}