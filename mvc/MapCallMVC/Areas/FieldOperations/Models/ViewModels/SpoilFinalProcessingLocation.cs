using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class SpoilFinalProcessingLocationViewModel : ViewModel<SpoilFinalProcessingLocation>
    {
        #region Properties

        [DoesNotAutoMap]
        public virtual int? State { get; set; }

        public virtual int? OperatingCenter { get; set; }

        [Required, StringLength(30)]
        public string Name { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center above"), EntityMap, EntityMustExist(typeof(Town))]
        public int? Town { get; set; }

        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Please select a Town above"), EntityMap, EntityMustExist(typeof(Street))]
        public int? Street { get; set; }

        #endregion

        #region Constructors

        public SpoilFinalProcessingLocationViewModel(IContainer container) : base(container) { }

        #endregion
    }
}