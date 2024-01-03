using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class UnionContractProposalViewModel : ViewModel<UnionContractProposal>
    {
        #region Properties

        public virtual string NegotiationStrategy { get; set; }

        [Required]
        public virtual bool Flag { get; set; }

        [Required]
        public virtual bool StrikeProposal { get; set; }

        [Required]
        public virtual bool Reviewed { get; set; }

        [StringLength(25)]
        public virtual string CrossReferenceNumber { get; set; }

        public virtual DateTime? ProposalClosedDate { get; set; }
        public virtual string ProposalDescription { get; set; }
        public virtual decimal? TargetValueOfChange { get; set; }
        public virtual string ValuationAssumptions { get; set; }
        public virtual string ImpactOfChange { get; set; }

        [StringLength(255)]
        public virtual string ToAchieveBenefitOfChange { get; set; }

        public virtual bool? ImpactOnHealthSafety { get; set; }
        public virtual bool? ImpactOnManagementsRights { get; set; }
        public virtual bool? ImpactOnOperationalEfficiency { get; set; }
        public virtual bool? ImpactOnOvertime { get; set; }
        public virtual int? CurrentPageNumber { get; set; }
        public virtual string AnticipatedResponseFromOppositeSide { get; set; }
        public virtual string Notes { get; set; }

        [StringLength(50)]
        public virtual string Sme { get; set; }

        public virtual bool? ImpactOnAttendance { get; set; }
        public virtual bool? ImpactOnCustomerService { get; set; }
        public virtual bool? ImpactOnEconomics { get; set; }
        public virtual bool? ImpactOnBenefits { get; set; }
        public virtual bool? ImpactOnStaffingLevels { get; set; }
        public virtual bool? ImpactOnRegulatoryCompliance { get; set; }
        public virtual bool? CostModelNeeded { get; set; }
        public virtual string TalkingPoints { get; set; }
        public virtual string ImplementationItems { get; set; }

        [EntityMustExist(typeof(UnionContractProposalPrioritization)),
         EntityMap, DropDown]
        public virtual int? Prioritization { get; set; }

        [EntityMustExist(typeof(UnionContractProposalNegotiationTiming)),
         EntityMap, DropDown]
        public virtual int? NegotiationTiming { get; set; }

        [EntityMustExist(typeof(UnionContractProposalGrouping)), EntityMap,
         DropDown]
        public virtual int? Grouping { get; set; }

        [EntityMustExist(typeof(UnionContractProposalPrintingSequence)),
         EntityMap, DropDown]
        public virtual int? PrintingSequence { get; set; }

        [EntityMustExist(typeof(UnionContractProposalStatus)), EntityMap,
         DropDown]
        public virtual int? Status { get; set; }

        [EntityMustExist(typeof(UnionContractProposalAffectedDepartment)),
         EntityMap, DropDown]
        public virtual int? AffectedDepartment { get; set; }

        [EntityMustExist(typeof(ManagementOrUnion)), EntityMap,
         DropDown]
        public virtual int? ManagementOrUnion { get; set; }

        [Required, EntityMustExist(typeof(UnionContract)), EntityMap, DropDown]
        public virtual int? Contract { get; set; }

        [EntityMustExist(typeof(PrimaryDriverForProposal)), EntityMap, DropDown]
        public virtual int? PrimaryDriverForProposal { get; set; }

        #endregion

        #region Constructors

        public UnionContractProposalViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class EditUnionContractProposal : UnionContractProposalViewModel
    {
        public EditUnionContractProposal(IContainer container) : base(container) {}
    }

    public class CreateUnionContractProposal : UnionContractProposalViewModel
    {
        public CreateUnionContractProposal(IContainer container) : base(container) {}
    }

    public class SearchUnionContractProposal : SearchSet<UnionContractProposal>
    {
        #region Properties

        [DropDown, Display(Name = "Contract ID")]
        public int? Contract { get; set; }

        // There's a logical property for OperatingCenter that gets this from the contract.
        [DropDown, DisplayName("Operating Center")] 
        public int? OperatingCenter { get; set; }
        [DropDown, SearchAlias("c.Local", "l", "Id", Required = true)]
        public int? Local { get; set; }
        [DropDown, Display(Name = "Bargaining Unit"), SearchAlias("l.Union", "u", "Id", Required = true)]
        public int? Union { get; set; }
        [DropDown, Display(Name = "Management or Union")]
        public int? ManagementOrUnion { get; set; }
        [DropDown, Display(Name = "Proposal Status")]
        public int? Status { get; set; }
        [Display(Name = "Proposal ID")]
        public int? EntityId { get; set; }
        [DropDown]
        public int? NegotiationTiming { get; set; }
        public string CrossReferenceNumber { get; set; }
        public NumericRange TargetValueOfChange { get; set; }
        [DropDown, Display(Name = "Proposal Grouping")]
        public int? Grouping { get; set; }

        [DropDown]
        public int? PrimaryDriverForProposal { get; set; }

        public bool? CostModelNeeded { get; set; }

        #endregion

        #region Exposed Methods

        //public void EnsureSearchValues(SearchMappableArgs args)
        //{
        //    if (args.Properties.ContainsKey("EntityId"))
        //    {
        //        args.Properties.Add("Id", args.Properties["EntityId"]);
        //        args.Properties.Remove("EntityId");
        //    }
        //}

        #endregion
    }
}