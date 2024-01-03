using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201230153143959), Tags("Production")]
    public class MC2813FixSewerOverflowReasonsTables : Migration
    {
        public struct TableNames
        {
            public const string OLD_TABLE_CAUSES = "SewerOverflowCauses",
                                TABLE_REASONS = "SewerOverflowReasons",
                                NEW_TABLE_REASONS = "SewerOverflowsSewerOverflowReasons";
        }

        public struct ForeignKeys
        {
            public const string
                OLD_CAUSES_FK =
                    "FK_SewerOverflowReasons_SewerOverflowCauses_SewerOverflowCauseID",
                OLD_OVERFLOW_ID_FK =
                    "FK_SewerOverflowReasons_SewerOverflows_SewerOverflowID",
                NEW_REASONS_FK =
                    "FK_SewerOverflowsSewerOverflowReasons_SewerOverflowReasons_SewerOverflowReasonId",
                NEW_OVERFLOW_ID_FK =
                    "FK_SewerOverflowsSewerOverflowReasons_SewerOverflows_SewerOverflowId";
        }

        public struct PrimaryKeys
        {
            public const string
                CAUSES_PK = "PK_SewerOverflowCauses",
                REASONS_PK = "PK_SewerOverflowReasons";
        }

        public override void Up()
        {
            Delete.ForeignKey(ForeignKeys.OLD_CAUSES_FK).OnTable(TableNames.TABLE_REASONS);
            Delete.ForeignKey(ForeignKeys.OLD_OVERFLOW_ID_FK).OnTable(TableNames.TABLE_REASONS);
            Delete.PrimaryKey(PrimaryKeys.CAUSES_PK).FromTable(TableNames.OLD_TABLE_CAUSES);

            Rename.Table(TableNames.TABLE_REASONS).To(TableNames.NEW_TABLE_REASONS);
            Rename.Table(TableNames.OLD_TABLE_CAUSES).To(TableNames.TABLE_REASONS);
            Rename.Column("SewerOverflowCauseID").OnTable(TableNames.NEW_TABLE_REASONS).To("SewerOverflowReasonId");
            Rename.Column("SewerOverflowCauseID").OnTable(TableNames.TABLE_REASONS).To("Id");

            Create.PrimaryKey(PrimaryKeys.REASONS_PK).OnTable(TableNames.TABLE_REASONS).Column("Id");
            Create.ForeignKey(ForeignKeys.NEW_OVERFLOW_ID_FK).FromTable(TableNames.NEW_TABLE_REASONS)
                  .ForeignColumn("SewerOverflowId").ToTable("SewerOverflows").PrimaryColumn("SewerOverflowId");
            Create.ForeignKey(ForeignKeys.NEW_REASONS_FK).FromTable(TableNames.NEW_TABLE_REASONS)
                  .ForeignColumn("SewerOverflowReasonId").ToTable(TableNames.TABLE_REASONS).PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey(ForeignKeys.NEW_OVERFLOW_ID_FK).OnTable(TableNames.NEW_TABLE_REASONS);
            Delete.ForeignKey(ForeignKeys.NEW_REASONS_FK).OnTable(TableNames.NEW_TABLE_REASONS);
            Delete.PrimaryKey(PrimaryKeys.REASONS_PK).FromTable(TableNames.TABLE_REASONS);

            Rename.Column("SewerOverflowReasonId").OnTable(TableNames.NEW_TABLE_REASONS).To("SewerOverflowCauseID");
            Rename.Column("Id").OnTable(TableNames.TABLE_REASONS).To("SewerOverflowCauseID");
            Rename.Table(TableNames.TABLE_REASONS).To(TableNames.OLD_TABLE_CAUSES);
            Rename.Table(TableNames.NEW_TABLE_REASONS).To(TableNames.TABLE_REASONS);

            Create.PrimaryKey(PrimaryKeys.CAUSES_PK).OnTable(TableNames.OLD_TABLE_CAUSES)
                  .Column("SewerOverflowCauseID");
            Create.ForeignKey(ForeignKeys.OLD_CAUSES_FK).FromTable(TableNames.TABLE_REASONS)
                  .ForeignColumn("SewerOverflowID").ToTable("SewerOverflows").PrimaryColumn("SewerOverflowID");
            Create.ForeignKey(ForeignKeys.OLD_OVERFLOW_ID_FK).FromTable(TableNames.TABLE_REASONS)
                  .ForeignColumn("SewerOverflowCauseID").ToTable(TableNames.OLD_TABLE_CAUSES)
                  .PrimaryColumn("SewerOverflowCauseID");
        }
    }
}
