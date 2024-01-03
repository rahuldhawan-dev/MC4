using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using StructureMap;

namespace MapCallMVC.Areas.Admin.Models
{
    public class CreateWaterSystem : ViewModel<WaterSystem>
    {
        [StringLength(50)]
        public virtual string Description { get; set; }
        [StringLength(255)]
        public virtual string LongDescription { get; set; }
        
        public CreateWaterSystem(IContainer container) : base(container) { }
    }
}
