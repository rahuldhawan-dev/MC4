using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class UnionContractViewModel : ViewModel<UnionContract>
    {
        #region Properties

        [DropDown]
        [EntityMustExist(typeof(OperatingCenter))]
        [EntityMap]
        [Display(Name = "OperatingCenter")]
        public virtual int? OperatingCenter { get; set; }

        [DropDown("Local", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center", Area = "")]
        [EntityMustExist(typeof(Local))]
        [EntityMap]
        public virtual int? Local { get; set; }
        [Required]
        public virtual DateTime? StartDate { get; set; }
        [Required]
        public virtual DateTime? EndDate { get; set; }
        public virtual float? PercentIncreaseYr1 { get; set; }
        public virtual float? PercentIncreaseYr2 { get; set; }
        public virtual float? PercentIncreaseYr3 { get; set; }
        public virtual float? PercentIncreaseYr4 { get; set; }
        public virtual float? PercentIncreaseYr5 { get; set; }
        public virtual float? PercentIncreaseYr6 { get; set; }
        public virtual DateTime? NewContractExpirationDate { get; set; }
        public virtual DateTime? NewContractEffectiveDate { get; set; }
        [StringLength(50)]
        public virtual string TermOfContract { get; set; }
        public virtual DateTime? DateOfMoa { get; set; }
        public virtual string CompanyNegotiatingCommittee { get; set; }
        public virtual string UnionNegotiatingCommittee { get; set; }
        public virtual bool? ContractExtended { get; set; }
        public virtual DateTime? ContractExtensionDate { get; set; }
        public virtual string CompanyKeyObjectivesSummary { get; set; }
        public virtual float? RatificationVoteFor { get; set; }
        public virtual float? RatificationVoteAgainst { get; set; }
        public virtual float? TotalBargainingUnitMembers { get; set; }
        public virtual string Notes { get; set; }
        public virtual bool? Retroactivity { get; set; }

        #endregion

        #region Constructors

        public UnionContractViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class CreateUnionContract : UnionContractViewModel
    {
        #region Constructors

        public CreateUnionContract(IContainer container) : base(container) {}

        #endregion
    }

    public class EditUnionContract : UnionContractViewModel
    {
        #region Constructors

        public EditUnionContract(IContainer container) : base(container) {}

        #endregion
    }

    public class SearchUnionContract : SearchSet<UnionContract>
    {
        #region Properties

        [DropDown]
        public int? OperatingCenter { get; set; }
        [DropDown]
        public int? Local { get; set; }
        [DropDown, View("Bargaining Unit"), SearchAlias("Local", "Union.Id")]
        public int? Union { get; set; }
        public DateRange StartDate { get; set; }
        public DateRange EndDate { get; set; }

        #endregion
    }
}