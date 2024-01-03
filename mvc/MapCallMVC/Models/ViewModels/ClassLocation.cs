using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class ClassLocationViewModel : EntityLookupViewModel<ClassLocation>
    {
        public ClassLocationViewModel(IContainer container) : base(container) {}

        // only here because 'Description' in ClassLocation is a logical property
        public virtual string Name { get; set; }

        [Required, DropDown, EntityMap]
        public virtual int OperatingCenter { get; set; }

        [Required, StringLength(35), EntityMap(MapDirections.None)]
        public override string Description
        {
            get { return Name; }
            set { Name = value; }
        }
    }
}