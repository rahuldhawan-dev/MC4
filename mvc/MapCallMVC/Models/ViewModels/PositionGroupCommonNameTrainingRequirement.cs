using System.ComponentModel.DataAnnotations;
using MMSINC.Metadata;
using MMSINC.Validation;
using MapCall.Common.Model.Entities;

namespace MapCallMVC.Models.ViewModels
{
    public class CreatePositionGroupCommonNameTrainingRequirement
    {
        #region Properties

        [Required, DropDown, EntityMustExist(typeof(PositionGroupCommonName))]
        public virtual int? PositionGroupCommonName { get; set; }
        [Required, DropDown, EntityMustExist(typeof(TrainingRequirement))]
        public virtual int? TrainingRequirement { get; set; }

        #endregion
    }

    public class DestroyPositionGroupCommonNameTrainingRequirement
    {
        [Required]
        public virtual int PositionGroupCommonNameId { get; set; }
        [Required]
        public virtual int TrainingRequirementId { get; set; }
    }
}