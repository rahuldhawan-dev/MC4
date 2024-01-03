using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class TankInspectionQuestionViewModel : ViewModel<TankInspectionQuestion>
    {
        #region Properties

        [Required]
        [EntityMap, EntityMustExist(typeof(TankInspectionQuestionType))]
        public int TankInspectionQuestionType { get; set; }
        [RequiredWhen(nameof(RepairsNeeded), true)]
        [Multiline, StringLength(MapCall.Common.Model.Entities.TankInspection.StringLengths.COMMENT_STRING_LENGTH)]
        public string ObservationAndComments { get; set; }
        [Required, BoolFormat("Yes", "No")]
        public bool? RepairsNeeded { get; set; }
        [Required, BoolFormat("Yes", "No")]
        public bool? TankInspectionAnswer { get; set; }
        public DateTime? CorrectiveWoDateCreated { get; set; }
        public DateTime? CorrectiveWoDateCompleted { get; set; }
        [DoesNotAutoMap("Display only. Set from the parent TankInspectionQuestionViewModel")]
        public string QuestionTypeDescription { get; set; }
        [DoesNotAutoMap("Display only. Set from the parent TankInspectionQuestionViewModel")]
        public int QuestionGroupId { get; set; }

        #endregion

        #region Constructors

        public TankInspectionQuestionViewModel(IContainer container) : base(container) { }

        #endregion

    }
}
