using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class CreateOperatingCenterServiceMaterial : ViewModel<OperatingCenterServiceMaterial>
    {
        #region Properties

        [DropDown, Required, EntityMustExist(typeof(OperatingCenter)), EntityMap]
        public int OperatingCenter { get; set; }
        [DropDown, Required, EntityMustExist(typeof(ServiceMaterial)), EntityMap]
        public int ServiceMaterial { get; set; }
        public bool NewServiceRecord { get; set; }

        #endregion

        #region Constructors

        public CreateOperatingCenterServiceMaterial(IContainer container) : base(container) { }

        #endregion
    }
}