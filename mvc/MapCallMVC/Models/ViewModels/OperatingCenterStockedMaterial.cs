using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class SearchOperatingCenterStockedMaterial : SearchSet<OperatingCenterStockedMaterial>
    {
        #region Properties

        [DropDown]
        public int? OperatingCenter { get; set; }
        [SearchAlias("Material", "PartNumber")]
        public string PartNumber { get; set; }

        #endregion
    }

    public class CreateOperatingCenterStockedMaterial : ViewModel<OperatingCenterStockedMaterial>
    {
        [DropDown, Required, EntityMustExist(typeof(OperatingCenter)), EntityMap]
        public int OperatingCenter { get; set; }
        [DropDown, Required, EntityMustExist(typeof(Material)), EntityMap]
        public int Material { get; set; }
        public decimal? Cost { get; set; }

        public CreateOperatingCenterStockedMaterial(IContainer container) : base(container) {}
    }
}