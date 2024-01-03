using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class FacilityProcessStepTriggerActionViewModel : ViewModel<FacilityProcessStepTriggerAction>
    {
        #region Properties

        [Secured, Required, EntityMap, EntityMustExist(typeof(FacilityProcessStepTrigger))]
        public int? Trigger { get; set; }

        [Required]
        public int? Sequence { get; set; }

        [Required, StringLength(FacilityProcessStepTriggerAction.MAX_ACTION_LENGTH)]
        public string Action { get; set; }

        [Required, StringLength(FacilityProcessStepTriggerAction.MAX_ACTION_RESPONSE_LENGTH)]
        public string ActionResponse { get; set; }

        #endregion

        #region Constructor

        public FacilityProcessStepTriggerActionViewModel(IContainer container) : base(container) { }
        
        #endregion
    }
}