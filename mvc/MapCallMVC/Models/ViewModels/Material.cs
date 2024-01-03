using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class MaterialViewModel : ViewModel<Material>
    {
        #region Properties

        [Required]
        public virtual string Description { get; set; }
        [StringLength(Material.THE_STRING_LENGTH)]
        public virtual string Size { get; set; }
        [Required, StringLength(Material.THE_STRING_LENGTH)]
        public virtual string PartNumber { get; set; }
        [Required]
        public virtual bool? IsActive { get; set; }
        [Required]
        public virtual bool? DoNotOrder { get; set; }

        #endregion

        #region Constructors

        public MaterialViewModel(IContainer container) : base(container) {}

        #endregion
	}

    public class CreateMaterial : MaterialViewModel
    {
        #region Constructors

		public CreateMaterial(IContainer container) : base(container) {}

        #endregion
	}

    public class EditMaterial : MaterialViewModel
    {
        #region Constructors

		public EditMaterial(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override Material MapToEntity(Material entity)
        {
            if (!IsActive.Value)
            {
                foreach (var operatingCenterStockedMaterial in entity.OperatingCenterStockedMaterials.ToList())
                {
                    entity.OperatingCenterStockedMaterials.Remove(operatingCenterStockedMaterial);
                }
            }
            return base.MapToEntity(entity); ;
        }

        #endregion
    }

    public class SearchMaterial : SearchSet<Material>
    {
        #region Properties

        public bool? IsActive { get; set; }
        public bool? DoNotOrder { get; set; }
        public SearchString PartNumber { get; set; }
        public SearchString Description { get; set; }

        #endregion
	}
}