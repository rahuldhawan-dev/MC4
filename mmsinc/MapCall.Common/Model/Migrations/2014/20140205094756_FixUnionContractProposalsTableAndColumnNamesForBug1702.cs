using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140205094756), Tags("Production")]
    public class FixUnionContractProposalsTableAndColumnNamesForBug1702 : Migration
    {
        public struct OldColumnNames
        {
            public const string ID = "Proposal_ID",
                                NEGOTIATION_TIMING = "Negotiation_Timing",
                                NEGOTIATION_STRATEGY = "Negotiation_Strategy",
                                PROPOSAL_GROUPING = "Proposal_Grouping",
                                STRIKE_PROPOSAL = "Strike_Proposal",
                                CROSS_REFERENCE_NUMBER = "Cross_Reference_Number",
                                PROPOSAL_CLOSED_DATE = "Proposal_Closed_Date",
                                CONTRACT_ID = "Contract_ID",
                                PROPOSAL_DESCRIPTION = "Proposal_Description",
                                PROPOSAL_STATUS = "Proposal_Status",
                                MANAGEMENT_OR_UNION = "Management_or_Union",
                                TARGET_VALUE_OF_CHANGE = "Target_Value_of_Change",
                                VALUATION_ASSUMPTIONS = "Valuation_Assumptions",
                                IMPACT_OF_CHANGE = "Impact_of_Change",
                                TO_ACHIEVE_BENEFIT_OF_CHANGE = "To_Achieve_Benefit_of_Change",
                                IMPACT_ON_HEALTH_SAFETY = "Impact_on_Health_Safety",
                                IMPACT_ON_MANAGEMENTS_RIGHTS = "Impact_on_Managements_Rights",
                                IMPACT_ON_OPERATIONAL_EFFICIENCY = "Impact_on_Operational_Efficiency",
                                IMPACT_ON_OVERTIME = "Impact_on_Overtime",
                                ANTICIPATED_RESPONSE_FROM_OPPOSITE_SIDE = "Anticipated_Response_From_Opposite_Side",
                                PRINTING_SEQUENCE = "Printing_Sequence",
                                PRIMARY_DRIVER_FOR_PROPOSAL = "Primary_Driver_For_Proposal",
                                AFFECTED_DEPARTMENT = "Affected_Department",
                                IMPACT_ON_ATTENDANCE = "Impact_on_Attendance",
                                IMPACT_ON_CUSTOMER_SERVICE = "Impact_on_Customer_Service",
                                IMPACT_ON_ECONOMICS = "Impact_on_Economics",
                                IMPACT_ON_BENEFITS = "Impact_on_Benefits",
                                IMPACT_ON_STAFFING_LEVELS = "Impact_on_Staffing_Levels",
                                IMPACT_ON_REGULATORY_COMPLIANCE = "Impact_on_Regulatory_Compliance",
                                TALKING_POINTS = "Talking_Points",
                                IMPLEMENTATION_ITEMS = "Implementation_Items";
        }

        public struct NewColumnNames
        {
            public const string ID = "Id",
                                NEGOTIATION_TIMING = "NegotiationTiming",
                                NEGOTIATION_STRATEGY = "NegotiationStrategy",
                                PROPOSAL_GROUPING = "ProposalGrouping",
                                STRIKE_PROPOSAL = "StrikeProposal",
                                CROSS_REFERENCE_NUMBER = "CrossReferenceNumber",
                                PROPOSAL_CLOSED_DATE = "ProposalClosedDate",
                                CONTRACT_ID = "ContractID",
                                PROPOSAL_DESCRIPTION = "ProposalDescription",
                                PROPOSAL_STATUS = "ProposalStatus",
                                MANAGEMENT_OR_UNION_ID = "ManagementorUnionId",
                                TARGET_VALUE_OF_CHANGE = "TargetValueOfChange",
                                VALUATION_ASSUMPTIONS = "ValuationAssumptions",
                                IMPACT_OF_CHANGE = "ImpactOfChange",
                                TO_ACHIEVE_BENEFIT_OF_CHANGE = "ToAchieveBenefitOfChange",
                                IMPACT_ON_HEALTH_SAFETY = "ImpactonHealthSafety",
                                IMPACT_ON_MANAGEMENTS_RIGHTS = "ImpactonManagementsRights",
                                IMPACT_ON_OPERATIONAL_EFFICIENCY = "ImpactonOperationalEfficiency",
                                IMPACT_ON_OVERTIME = "ImpactonOvertime",
                                ANTICIPATED_RESPONSE_FROM_OPPOSITE_SIDE = "AnticipatedResponseFromOppositeSide",
                                PRINTING_SEQUENCE = "PrintingSequence",
                                PRIMARY_DRIVER_FOR_PROPOSAL = "PrimaryDriverForProposal",
                                AFFECTED_DEPARTMENT = "AffectedDepartment",
                                IMPACT_ON_ATTENDANCE = "ImpactOnAttendance",
                                IMPACT_ON_CUSTOMER_SERVICE = "ImpactOnCustomerService",
                                IMPACT_ON_ECONOMICS = "ImpactOnEconomics",
                                IMPACT_ON_BENEFITS = "ImpactOnBenefits",
                                IMPACT_ON_STAFFING_LEVELS = "ImpactOnStaffingLevels",
                                IMPACT_ON_REGULATORY_COMPLIANCE = "ImpactOnRegulatoryCompliance",
                                TALKING_POINTS = "TalkingPoints",
                                IMPLEMENTATION_ITEMS = "ImplementationItems";
        }

        public struct OldTableNames
        {
            public const string UNION_CONTRACT_PROPOSALS = "tblContract_PROPOSALS";
        }

        public struct NewTableNames
        {
            public const string UNION_CONTRACT_PROPOSALS = "UnionContractProposals",
                                MANAGEMENT_OR_UNION = "ManagementOrUnion";
        }

        public override void Up()
        {
            Create.Table(NewTableNames.MANAGEMENT_OR_UNION)
                  .WithColumn("Id").AsInt32().Identity().NotNullable().PrimaryKey()
                  .WithColumn("Description").AsAnsiString(10).NotNullable().Unique();

            Execute.Sql("INSERT INTO {0} (Description) SELECT 'Union';", NewTableNames.MANAGEMENT_OR_UNION);
            Execute.Sql("INSERT INTO {0} (Description) SELECT 'Management';", NewTableNames.MANAGEMENT_OR_UNION);

            Execute.Sql("UPDATE {0} SET {1} = {2}.Id FROM {2} WHERE {1} = {2}.Description;",
                OldTableNames.UNION_CONTRACT_PROPOSALS, OldColumnNames.MANAGEMENT_OR_UNION,
                NewTableNames.MANAGEMENT_OR_UNION);

            Alter.Column(OldColumnNames.MANAGEMENT_OR_UNION)
                 .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS).AsInt32().Nullable()
                 .ForeignKey("FK_UnionContractProposals_ManagementOrUnion_ManagementOrUnionId",
                      NewTableNames.MANAGEMENT_OR_UNION, "Id");

            Alter.Column(OldColumnNames.CONTRACT_ID)
                 .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS).AsInt32().NotNullable()
                 .ForeignKey("FK_UnionContractProposals_UnionContracts_ContractId",
                      FixUnionContractsTableAndColumnNamesForBug1702.NEW_TABLE_NAME,
                      FixUnionContractsTableAndColumnNamesForBug1702.NewColumnNames.ID);

            Rename.Table(OldTableNames.UNION_CONTRACT_PROPOSALS).To(NewTableNames.UNION_CONTRACT_PROPOSALS);

            Rename.Column(OldColumnNames.ID).OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS).To(NewColumnNames.ID);
            Rename.Column(OldColumnNames.NEGOTIATION_TIMING)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.NEGOTIATION_TIMING);
            Rename.Column(OldColumnNames.NEGOTIATION_STRATEGY)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.NEGOTIATION_STRATEGY);
            Rename.Column(OldColumnNames.PROPOSAL_GROUPING)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.PROPOSAL_GROUPING);
            Rename.Column(OldColumnNames.STRIKE_PROPOSAL)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.STRIKE_PROPOSAL);
            Rename.Column(OldColumnNames.CROSS_REFERENCE_NUMBER)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.CROSS_REFERENCE_NUMBER);
            Rename.Column(OldColumnNames.PROPOSAL_CLOSED_DATE)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.PROPOSAL_CLOSED_DATE);
            Rename.Column(OldColumnNames.CONTRACT_ID)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.CONTRACT_ID);
            Rename.Column(OldColumnNames.PROPOSAL_DESCRIPTION)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.PROPOSAL_DESCRIPTION);
            Rename.Column(OldColumnNames.PROPOSAL_STATUS)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.PROPOSAL_STATUS);
            Rename.Column(OldColumnNames.MANAGEMENT_OR_UNION)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.MANAGEMENT_OR_UNION_ID);
            Rename.Column(OldColumnNames.TARGET_VALUE_OF_CHANGE)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.TARGET_VALUE_OF_CHANGE);
            Rename.Column(OldColumnNames.VALUATION_ASSUMPTIONS)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.VALUATION_ASSUMPTIONS);
            Rename.Column(OldColumnNames.IMPACT_OF_CHANGE)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.IMPACT_OF_CHANGE);
            Rename.Column(OldColumnNames.TO_ACHIEVE_BENEFIT_OF_CHANGE)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.TO_ACHIEVE_BENEFIT_OF_CHANGE);
            Rename.Column(OldColumnNames.IMPACT_ON_HEALTH_SAFETY)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.IMPACT_ON_HEALTH_SAFETY);
            Rename.Column(OldColumnNames.IMPACT_ON_MANAGEMENTS_RIGHTS)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.IMPACT_ON_MANAGEMENTS_RIGHTS);
            Rename.Column(OldColumnNames.IMPACT_ON_OPERATIONAL_EFFICIENCY)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.IMPACT_ON_OPERATIONAL_EFFICIENCY);
            Rename.Column(OldColumnNames.IMPACT_ON_OVERTIME)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.IMPACT_ON_OVERTIME);
            Rename.Column(OldColumnNames.ANTICIPATED_RESPONSE_FROM_OPPOSITE_SIDE)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.ANTICIPATED_RESPONSE_FROM_OPPOSITE_SIDE);
            Rename.Column(OldColumnNames.PRINTING_SEQUENCE)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.PRINTING_SEQUENCE);
            Rename.Column(OldColumnNames.PRIMARY_DRIVER_FOR_PROPOSAL)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.PRIMARY_DRIVER_FOR_PROPOSAL);
            Rename.Column(OldColumnNames.AFFECTED_DEPARTMENT)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.AFFECTED_DEPARTMENT);
            Rename.Column(OldColumnNames.IMPACT_ON_ATTENDANCE)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.IMPACT_ON_ATTENDANCE);
            Rename.Column(OldColumnNames.IMPACT_ON_CUSTOMER_SERVICE)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.IMPACT_ON_CUSTOMER_SERVICE);
            Rename.Column(OldColumnNames.IMPACT_ON_ECONOMICS)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.IMPACT_ON_ECONOMICS);
            Rename.Column(OldColumnNames.IMPACT_ON_BENEFITS)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.IMPACT_ON_BENEFITS);
            Rename.Column(OldColumnNames.IMPACT_ON_STAFFING_LEVELS)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.IMPACT_ON_STAFFING_LEVELS);
            Rename.Column(OldColumnNames.IMPACT_ON_REGULATORY_COMPLIANCE)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.IMPACT_ON_REGULATORY_COMPLIANCE);
            Rename.Column(OldColumnNames.TALKING_POINTS)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.TALKING_POINTS);
            Rename.Column(OldColumnNames.IMPLEMENTATION_ITEMS)
                  .OnTable(NewTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(NewColumnNames.IMPLEMENTATION_ITEMS);

            Execute.Sql("UPDATE DataType SET Table_Name = '{0}' WHERE Table_Name = '{1}';",
                NewTableNames.UNION_CONTRACT_PROPOSALS, OldTableNames.UNION_CONTRACT_PROPOSALS);
        }

        public override void Down()
        {
            Rename.Table(NewTableNames.UNION_CONTRACT_PROPOSALS).To(OldTableNames.UNION_CONTRACT_PROPOSALS);

            Rename.Column(NewColumnNames.ID).OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS).To(OldColumnNames.ID);
            Rename.Column(NewColumnNames.NEGOTIATION_TIMING)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.NEGOTIATION_TIMING);
            Rename.Column(NewColumnNames.NEGOTIATION_STRATEGY)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.NEGOTIATION_STRATEGY);
            Rename.Column(NewColumnNames.PROPOSAL_GROUPING)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.PROPOSAL_GROUPING);
            Rename.Column(NewColumnNames.STRIKE_PROPOSAL)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.STRIKE_PROPOSAL);
            Rename.Column(NewColumnNames.CROSS_REFERENCE_NUMBER)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.CROSS_REFERENCE_NUMBER);
            Rename.Column(NewColumnNames.PROPOSAL_CLOSED_DATE)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.PROPOSAL_CLOSED_DATE);
            Rename.Column(NewColumnNames.CONTRACT_ID)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.CONTRACT_ID);
            Rename.Column(NewColumnNames.PROPOSAL_DESCRIPTION)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.PROPOSAL_DESCRIPTION);
            Rename.Column(NewColumnNames.PROPOSAL_STATUS)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.PROPOSAL_STATUS);
            Rename.Column(NewColumnNames.MANAGEMENT_OR_UNION_ID)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.MANAGEMENT_OR_UNION);
            Rename.Column(NewColumnNames.TARGET_VALUE_OF_CHANGE)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.TARGET_VALUE_OF_CHANGE);
            Rename.Column(NewColumnNames.VALUATION_ASSUMPTIONS)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.VALUATION_ASSUMPTIONS);
            Rename.Column(NewColumnNames.IMPACT_OF_CHANGE)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.IMPACT_OF_CHANGE);
            Rename.Column(NewColumnNames.TO_ACHIEVE_BENEFIT_OF_CHANGE)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.TO_ACHIEVE_BENEFIT_OF_CHANGE);
            Rename.Column(NewColumnNames.IMPACT_ON_HEALTH_SAFETY)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.IMPACT_ON_HEALTH_SAFETY);
            Rename.Column(NewColumnNames.IMPACT_ON_MANAGEMENTS_RIGHTS)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.IMPACT_ON_MANAGEMENTS_RIGHTS);
            Rename.Column(NewColumnNames.IMPACT_ON_OPERATIONAL_EFFICIENCY)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.IMPACT_ON_OPERATIONAL_EFFICIENCY);
            Rename.Column(NewColumnNames.IMPACT_ON_OVERTIME)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.IMPACT_ON_OVERTIME);
            Rename.Column(NewColumnNames.ANTICIPATED_RESPONSE_FROM_OPPOSITE_SIDE)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.ANTICIPATED_RESPONSE_FROM_OPPOSITE_SIDE);
            Rename.Column(NewColumnNames.PRINTING_SEQUENCE)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.PRINTING_SEQUENCE);
            Rename.Column(NewColumnNames.PRIMARY_DRIVER_FOR_PROPOSAL)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.PRIMARY_DRIVER_FOR_PROPOSAL);
            Rename.Column(NewColumnNames.AFFECTED_DEPARTMENT)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.AFFECTED_DEPARTMENT);
            Rename.Column(NewColumnNames.IMPACT_ON_ATTENDANCE)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.IMPACT_ON_ATTENDANCE);
            Rename.Column(NewColumnNames.IMPACT_ON_CUSTOMER_SERVICE)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.IMPACT_ON_CUSTOMER_SERVICE);
            Rename.Column(NewColumnNames.IMPACT_ON_ECONOMICS)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.IMPACT_ON_ECONOMICS);
            Rename.Column(NewColumnNames.IMPACT_ON_BENEFITS)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.IMPACT_ON_BENEFITS);
            Rename.Column(NewColumnNames.IMPACT_ON_STAFFING_LEVELS)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.IMPACT_ON_STAFFING_LEVELS);
            Rename.Column(NewColumnNames.IMPACT_ON_REGULATORY_COMPLIANCE)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.IMPACT_ON_REGULATORY_COMPLIANCE);
            Rename.Column(NewColumnNames.TALKING_POINTS)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.TALKING_POINTS);
            Rename.Column(NewColumnNames.IMPLEMENTATION_ITEMS)
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                  .To(OldColumnNames.IMPLEMENTATION_ITEMS);

            Delete.ForeignKey("FK_UnionContractProposals_ManagementOrUnion_ManagementOrUnionId")
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS);

            Alter.Column(OldColumnNames.MANAGEMENT_OR_UNION)
                 .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                 .AsAnsiString(10)
                 .Nullable();

            Execute.Sql("UPDATE {0} SET {1} = {2}.Description FROM {2} WHERE {1} = {2}.Id;",
                OldTableNames.UNION_CONTRACT_PROPOSALS, OldColumnNames.MANAGEMENT_OR_UNION,
                NewTableNames.MANAGEMENT_OR_UNION);

            Delete.Table(NewTableNames.MANAGEMENT_OR_UNION);

            Delete.ForeignKey("FK_UnionContractProposals_UnionContracts_ContractId")
                  .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS);

            Alter.Column(OldColumnNames.CONTRACT_ID)
                 .OnTable(OldTableNames.UNION_CONTRACT_PROPOSALS)
                 .AsFloat().NotNullable();

            Execute.Sql("UPDATE DataType SET Table_Name = '{0}' WHERE Table_Name = '{1}';",
                OldTableNames.UNION_CONTRACT_PROPOSALS, NewTableNames.UNION_CONTRACT_PROPOSALS);
        }
    }
}
