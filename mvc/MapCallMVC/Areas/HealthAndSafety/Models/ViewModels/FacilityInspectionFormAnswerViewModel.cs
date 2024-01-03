using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class FacilityInspectionFormAnswerViewModel : ViewModel<FacilityInspectionFormAnswer>
    {
        public FacilityInspectionFormAnswerViewModel(IContainer container) : base(container) { }

        [Secured, Required, EntityMap, EntityMustExist(typeof(FacilityInspectionFormQuestion))]
        public int FacilityInspectionFormQuestion { get; set; }

        public int? ApcInspectionItem { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public bool? IsSafe { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public bool? IsPictureTaken { get; set; }

        public string Comments { get; set; }

        [AutoMap(MapDirections.None)]
        public FacilityInspectionFormQuestion FacilityInspectionFormQuestionDisplay { get; set; }

        [DoesNotAutoMap]
        public int Category { get; set; }
    }
}