using System;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public abstract class ContractorMeterCrewViewModel : ViewModel<ContractorMeterCrew>
    {
        #region Constructors

        public ContractorMeterCrewViewModel(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [DropDown, Required, EntityMap, EntityMustExist(typeof(Contractor))]
        public int? Contractor { get; set; }

        [Required, StringLength(ContractorMeterCrew.StringLengths.DESCRIPTION)]
        public string Description { get; set; }

        [Required, Min(0)]
        public int? AMMeters { get; set; }

        [Required, Min(0)]
        public int? PMMeters { get; set; }

        [Required, Min(0)]
        public int? AMLargeMeters { get; set; }

        [Required, Min(0)]
        public int? PMLargeMeters { get; set; }

        public bool IsActive { get; set; }

        #endregion
    }

    public class CreateContractorMeterCrew : ContractorMeterCrewViewModel
    {
        #region Constructors

        public CreateContractorMeterCrew(IContainer container) : base(container) { }

        #endregion
    }

    public class EditContractorMeterCrew : ContractorMeterCrewViewModel
    {
        #region Constructors

        public EditContractorMeterCrew(IContainer container) : base(container) { }

        #endregion
    }

    public class SearchContractorMeterCrew : SearchSet<ContractorMeterCrew>
    {

    }

}