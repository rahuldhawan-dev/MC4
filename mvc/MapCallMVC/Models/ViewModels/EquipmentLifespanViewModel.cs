using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using MapCall.Common.Model.Entities;
using MMSINC.Metadata;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class EquipmentLifespanViewModel : EntityLookupViewModel<EquipmentLifespan>
    {
        public EquipmentLifespanViewModel(IContainer container) : base(container) { }

        [Max(100.0), RegularExpression("^\\d*(\\.[05])?$", ErrorMessage = "Must be a number; decimal value can only be .5 or .0")]
        public virtual decimal? ExtendedLifeMajor { get; set; }
        
        [Max(100.0), RegularExpression("^\\d*(\\.[05])?$", ErrorMessage = "Must be a number; decimal value can only be .5 or .0")]
        public virtual decimal? ExtendedLifeMinor { get; set; }
        
        public virtual decimal? EstimatedLifespan { get; set; }
        
        public virtual bool? IsActive { get; set; }
    }
}
