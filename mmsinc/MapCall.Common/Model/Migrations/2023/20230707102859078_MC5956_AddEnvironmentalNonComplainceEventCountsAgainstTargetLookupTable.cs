using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230707102859078), Tags("Production")]
    public class MC5956_AddEnvironmentalNonComplainceEventCountsAgainstTargetLookupTable : Migration
    {
        #region Constants
        
        public struct TableNames
        {
            public const string COUNTS_AGAINST_TARGETS = "EnvironmentalNonComplianceEventCountsAgainstTargets",
                                ENVIRONMENTAL_NON_COMPLIANCE = "EnvironmentalNonComplianceEvents";
        }

        public struct ColumnNames
        {
            public const string COUNTS_AGAINST_TARGET = "CountsAgainstTarget";
        }
        
        public struct SqlCommands
        {
            public const string MIGRATE_BOOL_TO_LOOKUP_VALUE = "update e set e.CountsAgainstTarget = t.Id from EnvironmentalNonComplianceEvents e inner join EnvironmentalNonComplianceEventCountsAgainstTargets t on t.Description = (case when e.CountsAgainstTarget = 0 then 'No' when e.CountsAgainstTarget = 1 then 'Yes' end )",
                                ROLLBACK_MIGRATE_BOOL_TO_LOOKUP_VALUE = "update e set e.CountsAgainstTarget = t.bool from EnvironmentalNonComplianceEvents e, (select Id, (Case when description = 'Yes' then 1 when description = 'No' then 0 end) as bool from EnvironmentalNonComplianceEventCountsAgainstTargets) t where t.Id = e.CountsAgainstTarget";
        }

        public struct ForeignKeys
        { 
            public const string FK_COUNTS_AGAINST_TARGETS = "FK_EnvironmentalNonComplianceEvent_CountsAgainstTargetsId";  
        }

        #endregion Constants
        
        public override void Up()
        {
            this.CreateLookupTableWithValues(TableNames.COUNTS_AGAINST_TARGETS,
                "Expected to Count", "Not Expected to Count", "Yes", "No");
            Alter.Table(TableNames.ENVIRONMENTAL_NON_COMPLIANCE)
                 .AlterColumn("CountsAgainstTarget")
                 .AsInt32()
                 .Nullable(); //.ForeignKey("FK_EnvironmentalNonComplianceEvent_CountsAgainstTargetsId", TableNames.COUNTS_AGAINST_TARGETS, "Id");
            Execute.Sql(SqlCommands.MIGRATE_BOOL_TO_LOOKUP_VALUE);
            Create.ForeignKey(ForeignKeys.FK_COUNTS_AGAINST_TARGETS)
                  .FromTable(TableNames.ENVIRONMENTAL_NON_COMPLIANCE)
                  .ForeignColumn(ColumnNames.COUNTS_AGAINST_TARGET)
                  .ToTable(TableNames.COUNTS_AGAINST_TARGETS)
                  .PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey(ForeignKeys.FK_COUNTS_AGAINST_TARGETS)
                  .OnTable(TableNames.ENVIRONMENTAL_NON_COMPLIANCE);
            Execute.Sql(SqlCommands.ROLLBACK_MIGRATE_BOOL_TO_LOOKUP_VALUE);
            Delete.Table(TableNames.COUNTS_AGAINST_TARGETS);
            Alter.Table(TableNames.ENVIRONMENTAL_NON_COMPLIANCE)
                 .AlterColumn(ColumnNames.COUNTS_AGAINST_TARGET)
                 .AsBoolean()
                 .Nullable();
        }
    }
}