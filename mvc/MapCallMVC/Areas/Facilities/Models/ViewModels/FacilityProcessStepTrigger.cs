using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class FacilityProcessStepTriggerViewModel : ViewModel<FacilityProcessStepTrigger>
    {
        #region Properties

        [Required, StringLength(FacilityProcessStepTrigger.MAX_DESCRIPTION_LENGTH)]
        public string Description { get; set; }

        [Secured, Required, EntityMap, EntityMustExist(typeof(FacilityProcessStep))]
        public int? FacilityProcessStep { get; set; }

        [Required]
        public int? Sequence { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(FacilityProcessStepTriggerType))]
        public int? TriggerType { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(FacilityProcessStepTriggerLevel))]
        public int? TriggerLevel { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(FacilityProcessStepAlarm))]
        public int? Alarm { get; set; }

        #endregion

        #region Constructors

        public FacilityProcessStepTriggerViewModel(IContainer container) : base(container) { }

        #endregion
    }
}