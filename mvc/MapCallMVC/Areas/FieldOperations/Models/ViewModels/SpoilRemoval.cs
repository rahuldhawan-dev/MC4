using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;
using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class SpoilRemovalViewModel : ViewModel<SpoilRemoval>
    {
        #region Properties

        [DoesNotAutoMap]
        public virtual int? State { get; set; }

        [DoesNotAutoMap]
        public virtual int? OperatingCenter { get; set; }

        [DropDown("FieldOperations", "SpoilStorageLocation", "ByOperatingCenterId", DependsOn = "OperatingCenter",
             PromptText = "Please select a OperatingCenter above."), Required, EntityMap,
         EntityMustExist(typeof(SpoilStorageLocation))]
        public int? RemovedFrom { get; set; }

        [DropDown("FieldOperations", "SpoilFinalProcessingLocation", "GetByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select a OperatingCenter above."), Required, EntityMap, EntityMustExist(typeof(SpoilFinalProcessingLocation))]
        public int? FinalDestination { get; set; }
        [Required, View(FormatStyle.Date)]
        public virtual DateTime? DateRemoved { get; set; }
        public virtual decimal Quantity { get; set; }

        #endregion

        #region Constructors

        public SpoilRemovalViewModel(IContainer container) : base(container) { }

        #endregion
    }
}