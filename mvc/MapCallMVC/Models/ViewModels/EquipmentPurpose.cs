using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class EquipmentPurposeViewModel : ViewModel<EquipmentPurpose>
    {
        #region Properties

        [DropDown]
        [DisplayName("Category")]
        [EntityMap]
        [EntityMustExist(typeof(EquipmentCategory))]
        public virtual int? EquipmentCategory { get; set; }
        [DropDown]
        [DisplayName("Detail Type")]
        [EntityMustExist(typeof(EquipmentLifespan))]
        [EntityMap]
        public virtual int? EquipmentLifespan { get; set; }
        [DropDown]
        [DisplayName("Subcategory")]
        [EntityMustExist(typeof(EquipmentSubCategory))]
        [EntityMap]
        public virtual int? EquipmentSubCategory { get; set; }

        [DropDown, EntityMustExist(typeof(EquipmentType)), EntityMap]
        public int? EquipmentType { get; set; }

        [Required, StringLength(50)]
        public virtual string Description { get; set; }
        [Required, StringLength(4)]
        public virtual string Abbreviation { get; set; }

        #endregion

        #region Constructors

        public EquipmentPurposeViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class CreateEquipmentPurpose : EquipmentPurposeViewModel
    {
        #region Constructors

        public CreateEquipmentPurpose(IContainer container) : base(container) {}

        #endregion
    }

    public class EditEquipmentPurpose : EquipmentPurposeViewModel
    {
        #region Constructors

        public EditEquipmentPurpose(IContainer container) : base(container) {}

        #endregion
    }

    public class SearchEquipmentPurpose : SearchSet<EquipmentPurpose>
    {
        #region Properties

        [StringLength(EquipmentPurpose.StringLengths.DESCRIPTION)]
        public string Description { get; set; }
        [StringLength(EquipmentPurpose.StringLengths.ABBREVIATION)]
        public string Abbreviation { get; set; }
        public bool HasNoEquipmentType { get; set; }

        #endregion

        #region Exposed Methods

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);

            // we only want to search in the affirmative, we don't care unless the user
            // checked the box
            if (!HasNoEquipmentType)
            {
                mapper.MappedProperties[nameof(HasNoEquipmentType)].Value = null;
            }
        }

        #endregion
    }
}
