using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class EquipmentModelViewModel : ViewModel<EquipmentModel>
    {
        #region Properties

        [Required, StringLength(EquipmentModel.StringLengths.DESCRIPTION)]
        public virtual string Description { get; set; }

        [Required, DropDown]
        [EntityMap]
        [EntityMustExist(typeof(EquipmentManufacturer))]
        public virtual int? EquipmentManufacturer { get; set; }

        #endregion

        #region Constructors

        public EquipmentModelViewModel(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }

    public class CreateEquipmentModel : EquipmentModelViewModel
    {
        #region Constructors

        public CreateEquipmentModel(IContainer container) : base(container) {}

        #endregion
    }

    public class EditEquipmentModel : EquipmentModelViewModel
    {
        #region Constructors

        public EditEquipmentModel(IContainer container) : base(container) {}

        #endregion
    }

    public class SearchEquipmentModel : SearchSet<EquipmentModel>
    {
        #region Properties

        public virtual string Description { get; set; }
        
        [DropDown]
        [DisplayName("Equipment Manufacturer")]
        [EntityMap("EquipmentManufacturer")]
        [EntityMustExist(typeof(EquipmentManufacturer))]
        public virtual int? EquipmentManufacturer { get; set; }

        #endregion
    }

}