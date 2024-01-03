using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140204162502), Tags("Production")]
    public class FixUnionContractsTableAndColumnNamesForBug1702 : Migration
    {
        public const string OLD_TABLE_NAME = "tblBargaining_Unit_Contracts",
                            NEW_TABLE_NAME = "UnionContracts";

        public struct OldColumnNames
        {
            public const string CONTRACT_ID = "Contract_ID",
                                OPCODE = "OpCode",
                                START_DATE = "Start_Date",
                                END_DATE = "End_Date",
                                PERCENT_INCREASE_YR_1 = "Percent_Increase_Yr_1",
                                PERCENT_INCREASE_YR_2 = "Percent_Increase_Yr_2",
                                PERCENT_INCREASE_YR_3 = "Percent_Increase_Yr_3",
                                PERCENT_INCREASE_YR_4 = "Percent_Increase_Yr_4",
                                PERCENT_INCREASE_YR_5 = "Percent_Increase_Yr_5",
                                PERCENT_INCREASE_YR_6 = "Percent_Increase_Yr_6",
                                NEW_CONTRACT_EXPIRATION_DATE = "New_Contract_Expiration_Date",
                                NEW_CONTRACT_EFFECTIVE_DATE = "New_Contract_Effective_Date",
                                TERM_OF_CONTRACT = "Term_Of_Contract",
                                DATE_OF_MOA = "Date_Of_MOA",
                                COMPANY_NEGOTIATING_COMMITTEE = "Company_Negotiating_Committee",
                                UNION_NEGOTIATING_COMMITTEE = "Union_Negotiating_Committee",
                                CONTRACT_EXTENDED = "Contract_Extended",
                                CONTRACT_EXTENSION_DATE = "Contract_Extension_Date",
                                COMPANY_KEY_OBJECTIVES_SUMMARY = "Company_Key_Objectives_Summary",
                                RATIFICATION_VOTE_FOR = "Ratification_Vote_For",
                                RATIFICATION_VOTE_AGAINST = "Ratification_Vote_Against",
                                TOTAL_BARGAINING_UNIT_MEMBERS = "Total_Bargaining_Unit_ Members";
        }

        public struct NewColumnNames
        {
            public const string ID = "Id",
                                OPERATING_CENTER_ID = "OperatingCenterId",
                                START_DATE = "StartDate",
                                END_DATE = "EndDate",
                                PERCENT_INCREASE_YR_1 = "PercentIncreaseYr1",
                                PERCENT_INCREASE_YR_2 = "PercentIncreaseYr2",
                                PERCENT_INCREASE_YR_3 = "PercentIncreaseYr3",
                                PERCENT_INCREASE_YR_4 = "PercentIncreaseYr4",
                                PERCENT_INCREASE_YR_5 = "PercentIncreaseYr5",
                                PERCENT_INCREASE_YR_6 = "PercentIncreaseYr6",
                                NEW_CONTRACT_EXPIRATION_DATE = "NewContractExpirationDate",
                                NEW_CONTRACT_EFFECTIVE_DATE = "NewContractEffectiveDate",
                                TERM_OF_CONTRACT = "TermOfContract",
                                DATE_OF_MOA = "DateOfMOA",
                                COMPANY_NEGOTIATING_COMMITTEE = "CompanyNegotiatingCommittee",
                                UNION_NEGOTIATING_COMMITTEE = "UnionNegotiatingCommittee",
                                CONTRACT_EXTENDED = "ContractExtended",
                                CONTRACT_EXTENSION_DATE = "ContractExtensionDate",
                                COMPANY_KEY_OBJECTIVES_SUMMARY = "CompanyKeyObjectivesSummary",
                                RATIFICATION_VOTE_FOR = "RatificationVoteFor",
                                RATIFICATION_VOTE_AGAINST = "RatificationVoteAgainst",
                                TOTAL_BARGAINING_UNIT_MEMBERS = "TotalBargainingUnitMembers";
        }

        public override void Up()
        {
            Rename.Table(OLD_TABLE_NAME).To(NEW_TABLE_NAME);

            Rename.Column(OldColumnNames.CONTRACT_ID).OnTable(NEW_TABLE_NAME).To(NewColumnNames.ID);
            Rename.Column(OldColumnNames.OPCODE).OnTable(NEW_TABLE_NAME).To(NewColumnNames.OPERATING_CENTER_ID);
            Rename.Column(OldColumnNames.START_DATE).OnTable(NEW_TABLE_NAME).To(NewColumnNames.START_DATE);
            Rename.Column(OldColumnNames.END_DATE).OnTable(NEW_TABLE_NAME).To(NewColumnNames.END_DATE);
            Rename.Column(OldColumnNames.PERCENT_INCREASE_YR_1).OnTable(NEW_TABLE_NAME)
                  .To(NewColumnNames.PERCENT_INCREASE_YR_1);
            Rename.Column(OldColumnNames.PERCENT_INCREASE_YR_2).OnTable(NEW_TABLE_NAME)
                  .To(NewColumnNames.PERCENT_INCREASE_YR_2);
            Rename.Column(OldColumnNames.PERCENT_INCREASE_YR_3).OnTable(NEW_TABLE_NAME)
                  .To(NewColumnNames.PERCENT_INCREASE_YR_3);
            Rename.Column(OldColumnNames.PERCENT_INCREASE_YR_4).OnTable(NEW_TABLE_NAME)
                  .To(NewColumnNames.PERCENT_INCREASE_YR_4);
            Rename.Column(OldColumnNames.PERCENT_INCREASE_YR_5).OnTable(NEW_TABLE_NAME)
                  .To(NewColumnNames.PERCENT_INCREASE_YR_5);
            Rename.Column(OldColumnNames.PERCENT_INCREASE_YR_6).OnTable(NEW_TABLE_NAME)
                  .To(NewColumnNames.PERCENT_INCREASE_YR_6);
            Rename.Column(OldColumnNames.NEW_CONTRACT_EXPIRATION_DATE).OnTable(NEW_TABLE_NAME)
                  .To(NewColumnNames.NEW_CONTRACT_EXPIRATION_DATE);
            Rename.Column(OldColumnNames.NEW_CONTRACT_EFFECTIVE_DATE).OnTable(NEW_TABLE_NAME)
                  .To(NewColumnNames.NEW_CONTRACT_EFFECTIVE_DATE);
            Rename.Column(OldColumnNames.TERM_OF_CONTRACT).OnTable(NEW_TABLE_NAME).To(NewColumnNames.TERM_OF_CONTRACT);
            Rename.Column(OldColumnNames.DATE_OF_MOA).OnTable(NEW_TABLE_NAME).To(NewColumnNames.DATE_OF_MOA);
            Rename.Column(OldColumnNames.COMPANY_NEGOTIATING_COMMITTEE).OnTable(NEW_TABLE_NAME)
                  .To(NewColumnNames.COMPANY_NEGOTIATING_COMMITTEE);
            Rename.Column(OldColumnNames.UNION_NEGOTIATING_COMMITTEE).OnTable(NEW_TABLE_NAME)
                  .To(NewColumnNames.UNION_NEGOTIATING_COMMITTEE);
            Rename.Column(OldColumnNames.CONTRACT_EXTENDED).OnTable(NEW_TABLE_NAME)
                  .To(NewColumnNames.CONTRACT_EXTENDED);
            Rename.Column(OldColumnNames.CONTRACT_EXTENSION_DATE).OnTable(NEW_TABLE_NAME)
                  .To(NewColumnNames.CONTRACT_EXTENSION_DATE);
            Rename.Column(OldColumnNames.COMPANY_KEY_OBJECTIVES_SUMMARY).OnTable(NEW_TABLE_NAME)
                  .To(NewColumnNames.COMPANY_KEY_OBJECTIVES_SUMMARY);
            Rename.Column(OldColumnNames.RATIFICATION_VOTE_FOR).OnTable(NEW_TABLE_NAME)
                  .To(NewColumnNames.RATIFICATION_VOTE_FOR);
            Rename.Column(OldColumnNames.RATIFICATION_VOTE_AGAINST).OnTable(NEW_TABLE_NAME)
                  .To(NewColumnNames.RATIFICATION_VOTE_AGAINST);
            Rename.Column(OldColumnNames.TOTAL_BARGAINING_UNIT_MEMBERS).OnTable(NEW_TABLE_NAME)
                  .To(NewColumnNames.TOTAL_BARGAINING_UNIT_MEMBERS);

            Execute.Sql(
                "UPDATE {0} SET {1} = OperatingCenters.{1} FROM OperatingCenters WHERE {0}.{1} = OperatingCenters.OperatingCenterCode;",
                NEW_TABLE_NAME, NewColumnNames.OPERATING_CENTER_ID);

            Alter.Column(NewColumnNames.OPERATING_CENTER_ID)
                 .OnTable(NEW_TABLE_NAME)
                 .AsInt32()
                 .Nullable()
                 .ForeignKey("FK_UnionContracts_OperatingCenters_OperatingCenterId", "OperatingCenters",
                      NewColumnNames.OPERATING_CENTER_ID);

            Execute.Sql("UPDATE {0} SET {1} = CASE {1} WHEN 'Yes' THEN 1 WHEN 'No' THEN 0 ELSE NULL END;",
                NEW_TABLE_NAME,
                NewColumnNames.CONTRACT_EXTENDED);

            Alter.Column(NewColumnNames.CONTRACT_EXTENDED)
                 .OnTable(NEW_TABLE_NAME)
                 .AsBoolean()
                 .Nullable();

            Create.ForeignKey("FK_UnionContracts_LocalBargainingUnits_LocalId")
                  .FromTable(NEW_TABLE_NAME)
                  .ForeignColumn("LocalId")
                  .ToTable("LocalBargainingUnits")
                  .PrimaryColumn("Id");

            Execute.Sql("UPDATE DataType SET Table_Name = '{0}' WHERE Table_Name = '{1}';", NEW_TABLE_NAME,
                OLD_TABLE_NAME);
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_UnionContracts_LocalBargainingUnits_LocalId").OnTable(NEW_TABLE_NAME);
            Delete.ForeignKey("FK_UnionContracts_OperatingCenters_OperatingCenterId").OnTable(NEW_TABLE_NAME);

            Rename.Table(NEW_TABLE_NAME).To(OLD_TABLE_NAME);

            Rename.Column(NewColumnNames.ID).OnTable(OLD_TABLE_NAME).To(OldColumnNames.CONTRACT_ID);
            Rename.Column(NewColumnNames.OPERATING_CENTER_ID).OnTable(OLD_TABLE_NAME).To(OldColumnNames.OPCODE);
            Rename.Column(NewColumnNames.START_DATE).OnTable(OLD_TABLE_NAME).To(OldColumnNames.START_DATE);
            Rename.Column(NewColumnNames.END_DATE).OnTable(OLD_TABLE_NAME).To(OldColumnNames.END_DATE);
            Rename.Column(NewColumnNames.PERCENT_INCREASE_YR_1).OnTable(OLD_TABLE_NAME)
                  .To(OldColumnNames.PERCENT_INCREASE_YR_1);
            Rename.Column(NewColumnNames.PERCENT_INCREASE_YR_2).OnTable(OLD_TABLE_NAME)
                  .To(OldColumnNames.PERCENT_INCREASE_YR_2);
            Rename.Column(NewColumnNames.PERCENT_INCREASE_YR_3).OnTable(OLD_TABLE_NAME)
                  .To(OldColumnNames.PERCENT_INCREASE_YR_3);
            Rename.Column(NewColumnNames.PERCENT_INCREASE_YR_4).OnTable(OLD_TABLE_NAME)
                  .To(OldColumnNames.PERCENT_INCREASE_YR_4);
            Rename.Column(NewColumnNames.PERCENT_INCREASE_YR_5).OnTable(OLD_TABLE_NAME)
                  .To(OldColumnNames.PERCENT_INCREASE_YR_5);
            Rename.Column(NewColumnNames.PERCENT_INCREASE_YR_6).OnTable(OLD_TABLE_NAME)
                  .To(OldColumnNames.PERCENT_INCREASE_YR_6);
            Rename.Column(NewColumnNames.NEW_CONTRACT_EXPIRATION_DATE).OnTable(OLD_TABLE_NAME)
                  .To(OldColumnNames.NEW_CONTRACT_EXPIRATION_DATE);
            Rename.Column(NewColumnNames.NEW_CONTRACT_EFFECTIVE_DATE).OnTable(OLD_TABLE_NAME)
                  .To(OldColumnNames.NEW_CONTRACT_EFFECTIVE_DATE);
            Rename.Column(NewColumnNames.TERM_OF_CONTRACT).OnTable(OLD_TABLE_NAME).To(OldColumnNames.TERM_OF_CONTRACT);
            Rename.Column(NewColumnNames.DATE_OF_MOA).OnTable(OLD_TABLE_NAME).To(OldColumnNames.DATE_OF_MOA);
            Rename.Column(NewColumnNames.COMPANY_NEGOTIATING_COMMITTEE).OnTable(OLD_TABLE_NAME)
                  .To(OldColumnNames.COMPANY_NEGOTIATING_COMMITTEE);
            Rename.Column(NewColumnNames.UNION_NEGOTIATING_COMMITTEE).OnTable(OLD_TABLE_NAME)
                  .To(OldColumnNames.UNION_NEGOTIATING_COMMITTEE);
            Rename.Column(NewColumnNames.CONTRACT_EXTENDED).OnTable(OLD_TABLE_NAME)
                  .To(OldColumnNames.CONTRACT_EXTENDED);
            Rename.Column(NewColumnNames.CONTRACT_EXTENSION_DATE).OnTable(OLD_TABLE_NAME)
                  .To(OldColumnNames.CONTRACT_EXTENSION_DATE);
            Rename.Column(NewColumnNames.COMPANY_KEY_OBJECTIVES_SUMMARY).OnTable(OLD_TABLE_NAME)
                  .To(OldColumnNames.COMPANY_KEY_OBJECTIVES_SUMMARY);
            Rename.Column(NewColumnNames.RATIFICATION_VOTE_FOR).OnTable(OLD_TABLE_NAME)
                  .To(OldColumnNames.RATIFICATION_VOTE_FOR);
            Rename.Column(NewColumnNames.RATIFICATION_VOTE_AGAINST).OnTable(OLD_TABLE_NAME)
                  .To(OldColumnNames.RATIFICATION_VOTE_AGAINST);
            Rename.Column(NewColumnNames.TOTAL_BARGAINING_UNIT_MEMBERS).OnTable(OLD_TABLE_NAME)
                  .To(OldColumnNames.TOTAL_BARGAINING_UNIT_MEMBERS);

            Alter.Column(OldColumnNames.OPCODE)
                 .OnTable(OLD_TABLE_NAME)
                 .AsAnsiString(4)
                 .Nullable();

            Execute.Sql(
                "UPDATE {0} SET {1} = OperatingCenters.OperatingCenterCode FROM OperatingCenters WHERE {0}.{1} = OperatingCenters.OperatingCenterId;",
                OLD_TABLE_NAME, OldColumnNames.OPCODE);

            Alter.Column(OldColumnNames.CONTRACT_EXTENDED)
                 .OnTable(OLD_TABLE_NAME)
                 .AsAnsiString(3)
                 .Nullable();

            Execute.Sql("UPDATE {0} SET {1} = CASE {1} WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' ELSE NULL END;",
                OLD_TABLE_NAME, OldColumnNames.CONTRACT_EXTENDED);

            Execute.Sql("UPDATE DataType SET Table_Name = '{0}' WHERE Table_Name = '{1}';", OLD_TABLE_NAME,
                NEW_TABLE_NAME);
        }
    }
}
