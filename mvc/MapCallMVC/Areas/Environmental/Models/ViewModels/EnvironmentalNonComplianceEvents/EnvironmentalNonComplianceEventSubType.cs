using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCallMVC.Models.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalNonComplianceEvents
{
    public class EnvironmentalNonComplianceEventSubTypeViewModel : EntityLookupViewModel<EnvironmentalNonComplianceEventSubType>
    {
        [DropDown, EntityMap, EntityMustExist(typeof(EnvironmentalNonComplianceEventType)), Required]
        public virtual int? EnvironmentalNonComplianceEventType { get; set; }

        public EnvironmentalNonComplianceEventSubTypeViewModel(IContainer container) : base(container) { }
    }
}