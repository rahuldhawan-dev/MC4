using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class UnionContractProposal : IEntity, IValidatableObject, IThingWithNotes, IThingWithDocuments,
        IThingWithOperatingCenter
    {
        #region Properties

        #region Table Columns

        public virtual int Id { get; set; }
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

        #endregion

        #region References

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual UnionContractProposalPrioritization Prioritization { get; set; }
        public virtual UnionContractProposalNegotiationTiming NegotiationTiming { get; set; }
        public virtual UnionContractProposalGrouping Grouping { get; set; }
        public virtual UnionContractProposalPrintingSequence PrintingSequence { get; set; }
        public virtual UnionContractProposalStatus Status { get; set; }
        public virtual UnionContractProposalAffectedDepartment AffectedDepartment { get; set; }
        public virtual ManagementOrUnion ManagementOrUnion { get; set; }

        [Required]
        public virtual UnionContract Contract { get; set; }

        // NOTE: Not called Notes because of the other Notes property.
        public virtual IList<Note<UnionContractProposal>> UnionContractProposalNotes { get; set; }
        public virtual IList<Document<UnionContractProposal>> Documents { get; set; }

        public virtual PrimaryDriverForProposal PrimaryDriverForProposal { get; set; }

        #endregion

        #region Logical Properties

        public virtual string TableName => FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames
           .UNION_CONTRACT_PROPOSALS;

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        public virtual IList<INoteLink> LinkedNotes => UnionContractProposalNotes.Cast<INoteLink>().ToList();

        [Display(Name = "Documents")]
        public virtual int DocumentCount => Documents.Count;

        [Display(Name = "Notes")]
        public virtual int NoteCount => UnionContractProposalNotes.Count;

        #endregion

        #endregion

        #region Constructors

        public UnionContractProposal()
        {
            UnionContractProposalNotes = new List<Note<UnionContractProposal>>();
            Documents = new List<Document<UnionContractProposal>>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
