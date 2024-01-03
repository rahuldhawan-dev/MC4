using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140207144000), Tags("Production")]
    public class NormalizeOutUnionContractProposalLookupsForBug1702 : Migration
    {
        public struct NewTableNames
        {
            public const string PRIORITIZATIONS = "UnionContractProposalPrioritizations",
                                NEGOTIATION_TIMINGS = "UnionContractProposalNegotiationTimings",
                                PRINTING_SEQUENCES = "UnionContractProposalPrintingSequences",
                                STATUSES = "UnionContractProposalStatuses",
                                GROUPINGS = "UnionContractProposalGroupings",
                                AFFECTED_DEPARTMENTS = "UnionContractProposalAffectedDepartments";
        }

        public struct NewColumnNames
        {
            public struct Common
            {
                public const string ID = "Id", DESCRIPTION = "Description";
            }

            public struct UnionContractProposals
            {
                public const string PRIORITIZATION_ID = "PrioritizationId",
                                    NEGOTIATION_TIMING_ID = "NegotiationTimingId",
                                    PRINTING_SEQUENCE_ID = "PrintingSequenceId",
                                    STATUS_ID = "StatusId",
                                    GROUPING_ID = "GroupingId",
                                    AFFECTED_DEPARTMENT_ID = "AffectedDepartmentId";
            }
        }

        public struct NewDescriptionColumnLengths
        {
            // proposal_prioritization
            public const int PRIORITIZATION = 20,
                             // proposal_negotiation_timing
                             NEGOTIATION_TIMING = 3,
                             // proposal_printing_sequence
                             PRINTING_SEQUENCE = 2,
                             // proposal_status
                             STATUS = 21,
                             // proposal_grouping
                             GROUPING = 1,
                             // proposal_affected_department
                             AFFECTED_DEPARTMENT = 14;
        }

        public struct OldColumnNames
        {
            public struct UnionContractProposals
            {
                public const string PRIORITIZATION = "Prioritization",
                                    NEGOTIATION_TIMING = "NegotiationTiming",
                                    PRINTING_SEQUENCE = "PrintingSequence",
                                    STATUS = "ProposalStatus",
                                    GROUPING = "ProposalGrouping",
                                    AFFECTED_DEPARTMENT = "AffectedDepartment";
            }
        }

        public struct OldLookupTypes
        {
            public const string PRIORITIZATIONS = "proposal_prioritization",
                                NEGOTIATION_TIMINGS = "proposal_negotiation_timing",
                                PRINTING_SEQUENCES = "proposal_printing_sequence",
                                STATUSES = "proposal_status",
                                GROUPINGS = "proposal_grouping",
                                AFFECTED_DEPARTMENTS = "proposal_affected_department";
        }

        public override void Up()
        {
            Rename.Column(OldColumnNames.UnionContractProposals.PRIORITIZATION)
                  .OnTable(
                       FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.UnionContractProposals.PRIORITIZATION_ID);
            Rename.Column(OldColumnNames.UnionContractProposals.NEGOTIATION_TIMING)
                  .OnTable(
                       FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.UnionContractProposals.NEGOTIATION_TIMING_ID);
            Rename.Column(OldColumnNames.UnionContractProposals.PRINTING_SEQUENCE)
                  .OnTable(
                       FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.UnionContractProposals.PRINTING_SEQUENCE_ID);
            Rename.Column(OldColumnNames.UnionContractProposals.STATUS)
                  .OnTable(
                       FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.UnionContractProposals.STATUS_ID);
            Rename.Column(OldColumnNames.UnionContractProposals.GROUPING)
                  .OnTable(
                       FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.UnionContractProposals.GROUPING_ID);
            Rename.Column(OldColumnNames.UnionContractProposals.AFFECTED_DEPARTMENT)
                  .OnTable(
                       FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.UnionContractProposals.AFFECTED_DEPARTMENT_ID);

            this.ExtractLookupTableLookup(
                FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS,
                NewColumnNames.UnionContractProposals.PRIORITIZATION_ID, NewTableNames.PRIORITIZATIONS,
                NewDescriptionColumnLengths.PRIORITIZATION, OldLookupTypes.PRIORITIZATIONS);
            this.ExtractLookupTableLookup(
                FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS,
                NewColumnNames.UnionContractProposals.NEGOTIATION_TIMING_ID, NewTableNames.NEGOTIATION_TIMINGS,
                NewDescriptionColumnLengths.NEGOTIATION_TIMING, OldLookupTypes.NEGOTIATION_TIMINGS);
            this.ExtractLookupTableLookup(
                FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS,
                NewColumnNames.UnionContractProposals.PRINTING_SEQUENCE_ID, NewTableNames.PRINTING_SEQUENCES,
                NewDescriptionColumnLengths.PRINTING_SEQUENCE, OldLookupTypes.PRINTING_SEQUENCES);
            this.ExtractLookupTableLookup(
                FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS,
                NewColumnNames.UnionContractProposals.STATUS_ID, NewTableNames.STATUSES,
                NewDescriptionColumnLengths.STATUS, OldLookupTypes.STATUSES);
            this.ExtractLookupTableLookup(
                FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS,
                NewColumnNames.UnionContractProposals.GROUPING_ID, NewTableNames.GROUPINGS,
                NewDescriptionColumnLengths.GROUPING, OldLookupTypes.GROUPINGS);
            this.ExtractLookupTableLookup(
                FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS,
                NewColumnNames.UnionContractProposals.AFFECTED_DEPARTMENT_ID, NewTableNames.AFFECTED_DEPARTMENTS,
                NewDescriptionColumnLengths.AFFECTED_DEPARTMENT, OldLookupTypes.AFFECTED_DEPARTMENTS);
        }

        public override void Down()
        {
            this.ReplaceLookupTableLookup(
                FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS,
                NewColumnNames.UnionContractProposals.PRIORITIZATION_ID, NewTableNames.PRIORITIZATIONS,
                NewDescriptionColumnLengths.PRIORITIZATION, OldLookupTypes.PRIORITIZATIONS);
            this.ReplaceLookupTableLookup(
                FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS,
                NewColumnNames.UnionContractProposals.NEGOTIATION_TIMING_ID, NewTableNames.NEGOTIATION_TIMINGS,
                NewDescriptionColumnLengths.NEGOTIATION_TIMING, OldLookupTypes.NEGOTIATION_TIMINGS);
            this.ReplaceLookupTableLookup(
                FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS,
                NewColumnNames.UnionContractProposals.PRINTING_SEQUENCE_ID, NewTableNames.PRINTING_SEQUENCES,
                NewDescriptionColumnLengths.PRINTING_SEQUENCE, OldLookupTypes.PRINTING_SEQUENCES);
            this.ReplaceLookupTableLookup(
                FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS,
                NewColumnNames.UnionContractProposals.STATUS_ID, NewTableNames.STATUSES,
                NewDescriptionColumnLengths.STATUS, OldLookupTypes.STATUSES);
            this.ReplaceLookupTableLookup(
                FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS,
                NewColumnNames.UnionContractProposals.GROUPING_ID, NewTableNames.GROUPINGS,
                NewDescriptionColumnLengths.GROUPING, OldLookupTypes.GROUPINGS);
            this.ReplaceLookupTableLookup(
                FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS,
                NewColumnNames.UnionContractProposals.AFFECTED_DEPARTMENT_ID, NewTableNames.AFFECTED_DEPARTMENTS,
                NewDescriptionColumnLengths.AFFECTED_DEPARTMENT, OldLookupTypes.AFFECTED_DEPARTMENTS);

            Rename.Column(NewColumnNames.UnionContractProposals.PRIORITIZATION_ID)
                  .OnTable(
                       FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.UnionContractProposals.PRIORITIZATION);
            Rename.Column(NewColumnNames.UnionContractProposals.NEGOTIATION_TIMING_ID)
                  .OnTable(
                       FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.UnionContractProposals.NEGOTIATION_TIMING);
            Rename.Column(NewColumnNames.UnionContractProposals.PRINTING_SEQUENCE_ID)
                  .OnTable(
                       FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.UnionContractProposals.PRINTING_SEQUENCE);
            Rename.Column(NewColumnNames.UnionContractProposals.STATUS_ID)
                  .OnTable(
                       FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.UnionContractProposals.STATUS);
            Rename.Column(NewColumnNames.UnionContractProposals.GROUPING_ID)
                  .OnTable(
                       FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.UnionContractProposals.GROUPING);
            Rename.Column(NewColumnNames.UnionContractProposals.AFFECTED_DEPARTMENT_ID)
                  .OnTable(
                       FixUnionContractProposalsTableAndColumnNamesForBug1702.NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.UnionContractProposals.AFFECTED_DEPARTMENT);
        }
    }
}
