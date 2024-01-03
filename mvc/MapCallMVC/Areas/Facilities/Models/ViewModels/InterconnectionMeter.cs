using System.ComponentModel.DataAnnotations;
using MMSINC.Metadata;
using MMSINC.Validation;
using MapCall.Common.Model.Entities;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class CreateInterconnectionMeter
    {
        [Required, DropDown, EntityMustExist(typeof(Interconnection))]
        public virtual int? Interconnection { get; set; }
        [Required, DropDown, EntityMustExist(typeof(MeterProfile))]
        public virtual int? MeterProfile { get; set; }
        [Required, DropDown("", "Meter", "ByProfileId", DependsOn = "MeterProfile"), EntityMustExist(typeof(Meter))]
        public virtual int? Meter { get; set; }
    }

    public class DestroyInterconnectionMeter
    {
        [Required]
        public virtual int InterconnectionId { get; set; }
        [Required]
        public virtual int MeterId { get; set; }
    }
}