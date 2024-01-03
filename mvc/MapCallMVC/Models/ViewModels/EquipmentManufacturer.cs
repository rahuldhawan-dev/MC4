using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class SearchEquipmentManufacturer : SearchSet<EquipmentManufacturer>
    {
        [DropDown, EntityMap, EntityMustExist(typeof(EquipmentType))]
        public virtual int? EquipmentType { get; set; }

        public SearchString MapCallDescription { get; set; }

        public SearchString Description { get; set; }
    }

    public abstract class EquipmentManufacturerViewModel : ViewModel<EquipmentManufacturer>
    {
        [DropDown, EntityMap, EntityMustExist(typeof(EquipmentType)), Required]
        public virtual int? EquipmentType { get; set; }

        public string MapCallDescription { get; set; }

        [Required]
        public string Description { get; set; }

        public EquipmentManufacturerViewModel(IContainer container) : base(container) { }
    }

    public class CreateEquipmentManufacturer : EquipmentManufacturerViewModel
    {
        public CreateEquipmentManufacturer(IContainer container) : base(container) { }
    }

    public class EditEquipmentManufacturer : EquipmentManufacturerViewModel
    {
        public EditEquipmentManufacturer(IContainer container) : base(container) { }
    }
}