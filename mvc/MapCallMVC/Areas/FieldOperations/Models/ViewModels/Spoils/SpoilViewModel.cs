using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.Spoils
{
    public class SpoilViewModel : ViewModel<Spoil>
    {
        #region Constructor

        public SpoilViewModel(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [Required]
        public decimal? Quantity { get; set; }

        [Required, DropDown("FieldOperations", "SpoilStorageLocation", "ByOperatingCenterId", DependsOn = nameof(OperatingCenter)),
         EntityMap, EntityMustExist(typeof(SpoilStorageLocation))]
        public int? SpoilStorageLocation { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(WorkOrder))]
        public int? WorkOrder { get; set; }

        [DoesNotAutoMap]
        public int? OperatingCenter { get; set; }

        #endregion
    }
}
