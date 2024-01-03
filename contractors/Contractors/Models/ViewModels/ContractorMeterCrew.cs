using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DataAnnotationsExtensions;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Validation;
using StructureMap;

namespace Contractors.Models.ViewModels
{
    public abstract class ContractorMeterCrewViewModel : ViewModel<ContractorMeterCrew>
    {
        #region Constructors

        public ContractorMeterCrewViewModel(IContainer container) : base(container) { }

        #endregion

        #region Properties

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

        #region Public Methods

        public override ContractorMeterCrew MapToEntity(ContractorMeterCrew entity)
        {
            base.MapToEntity(entity);
            entity.Contractor = _container.GetInstance<IAuthenticationService<ContractorUser>>().CurrentUser.Contractor;
            return entity;
        }

        #endregion
    }

    public class EditContractorMeterCrew : ContractorMeterCrewViewModel
    {
        #region Constructors

        public EditContractorMeterCrew(IContainer container) : base(container) { }

        #endregion
    }

    [StringLengthNotRequired]
    public class SearchContractorMeterCrew : SearchSet<ContractorMeterCrew>
    {

    }
}