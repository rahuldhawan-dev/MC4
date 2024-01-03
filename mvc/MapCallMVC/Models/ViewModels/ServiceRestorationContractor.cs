using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public abstract class ServiceRestorationContractorViewModel : ViewModel<ServiceRestorationContractor>
    {
        [Required, StringLength(ServiceRestorationContractorMap.StringLengths.CONTRACTOR)]
        public string Contractor { get; set; }
        [Required, EntityMustExist(typeof(OperatingCenter)), DropDown, EntityMap]
        public int? OperatingCenter { get; set; }
        [Required]
        public bool? FinalRestoration { get; set; }
        [Required]
        public bool? PartialRestoration { get; set; }

        #region Constructors

        public ServiceRestorationContractorViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class CreateServiceRestorationContractor : ServiceRestorationContractorViewModel
    {
        public CreateServiceRestorationContractor(IContainer container) : base(container) { }
    }

    public class EditServiceRestorationContractor : ServiceRestorationContractorViewModel
    {
        public EditServiceRestorationContractor(IContainer container) : base(container) { }
    }

    public class SearchServiceRestorationContractor : SearchSet<ServiceRestorationContractor>
    {
        #region Properties

        public SearchString Contractor { get; set; }
        [DropDown, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }
        public bool? FinalRestoration { get; set; }
        public bool? PartialRestoration { get; set; }

        #endregion
    }
}