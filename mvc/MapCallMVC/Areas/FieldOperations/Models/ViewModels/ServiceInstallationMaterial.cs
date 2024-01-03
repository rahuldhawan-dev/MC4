using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class ServiceInstallationMaterialViewModel : ViewModel<ServiceInstallationMaterial>
    {
        #region Properties

        [DropDown, Required, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }
        [DropDown, Required, EntityMap, EntityMustExist(typeof(ServiceCategory))]
        public int? ServiceCategory { get; set; }
        [DropDown,Required, EntityMap, EntityMustExist(typeof(ServiceSize))]
        public int? ServiceSize { get; set; }
        public int? SortOrder { get; set; }
        public string Description { get; set; }
        public string PartQuantity { get; set; }
        public string PartSize { get; set; }

        #endregion

        #region Constructors

        public ServiceInstallationMaterialViewModel(IContainer container) : base(container) { }

        #endregion
    }
}