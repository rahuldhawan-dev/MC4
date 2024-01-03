using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class LockoutFormAnswerViewModel : ViewModel<LockoutFormAnswer>
    {
        public LockoutFormAnswerViewModel(IContainer container) : base(container) { }

        [Secured, Required, EntityMap, EntityMustExist(typeof(LockoutFormQuestion))]
        public int LockoutFormQuestion { get; set; }

        [RequiredWhen("Category", ComparisonType.NotEqualToAny, new[]{ MapCall.Common.Model.Entities.LockoutFormQuestionCategory.Indices.MANAGEMENT, MapCall.Common.Model.Entities.LockoutFormQuestionCategory.Indices.RETURN_TO_SERVICE })]
        [ClientCallback("LockoutForm.validateAnswer", ErrorMessage = "Required")]
        public bool? Answer { get; set; }

        [RequiredWhen("Answer", ComparisonType.EqualTo, false)]
        public string Comments { get; set; }

        [AutoMap(MapDirections.None)]
        public LockoutFormQuestion LockoutFormQuestionDisplay { get; set; }

        [DoesNotAutoMap]
        public int Category { get; set; }
    }
}