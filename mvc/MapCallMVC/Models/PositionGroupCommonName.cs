using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using StructureMap;

namespace MapCallMVC.Models
{
    public class PositionGroupCommonNameViewModel : ViewModel<PositionGroupCommonName>
    {
        [Required]
        public string Description { get; set; }

        public PositionGroupCommonNameViewModel(IContainer container) : base(container) { }
    }

    public class CreatePositionGroupCommonName : PositionGroupCommonNameViewModel
    {
        public CreatePositionGroupCommonName(IContainer container) : base(container) { }
    }

    public class EditPositionGroupCommonName : PositionGroupCommonNameViewModel
    {
        public EditPositionGroupCommonName(IContainer container) : base(container) { }
    }
}