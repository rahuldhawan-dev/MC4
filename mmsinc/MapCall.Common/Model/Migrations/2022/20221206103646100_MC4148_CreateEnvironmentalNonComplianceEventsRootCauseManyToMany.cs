using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221206103646100), Tags("Production")]
    public class MC4148CreateEnvironmentalNonComplianceEventsRootCauseManyToMany : Migration
    {
        public struct SQL
        {
            public const string CREATE_RECORDS = @"Insert Into EnvironmentalNonComplianceEventsEnvironmentalNonComplianceEventPrimaryRootCauses
            (EnvironmentalNonComplianceEventId,EnvironmentalNonComplianceEventPrimaryRootCauseId)
            Select ID,PrimaryRootCauseId
            From EnvironmentalNonComplianceEvents
            Where PrimaryRootCauseId Is Not Null";

            public const string ROLLBACK = @"Update env
            Set env.PrimaryRootCauseId = env2rc.EnvironmentalNonComplianceEventPrimaryRootCauseId
            From EnvironmentalNonComplianceEvents env
            Inner Join EnvironmentalNonComplianceEventsEnvironmentalNonComplianceEventPrimaryRootCauses env2rc
            On env.Id = env2rc.EnvironmentalNonComplianceEventId";
        }
        public override void Up()
        {
            Create.Table("EnvironmentalNonComplianceEventsEnvironmentalNonComplianceEventPrimaryRootCauses")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("EnvironmentalNonComplianceEventId", "EnvironmentalNonComplianceEvents", "Id", false, "FK_EnvironmentalNonComplianceEvents_EnvironmentalNonComplianceEventId").NotNullable()
                  .WithForeignKeyColumn("EnvironmentalNonComplianceEventPrimaryRootCauseId",
                       "EnvironmentalNonComplianceEventPrimaryRootCauses", "Id", false, "FK_EnvironmentalNonComplianceEventPrimaryRootCauses_EnvironmentalNonComplianceEventPrimaryRootCauseId").NotNullable();

            Execute.Sql(SQL.CREATE_RECORDS);

            Delete.ForeignKey("FK_NoticesOfViolation_NoticeOfViolationPrimaryRootCauses_PrimaryRootCauseId")
                  .OnTable("EnvironmentalNonComplianceEvents");

            Delete.Column("PrimaryRootCauseId").FromTable("EnvironmentalNonComplianceEvents");
        }

        public override void Down()
        {
            Alter.Table("EnvironmentalNonComplianceEvents")
                 .AddColumn("PrimaryRootCauseId")
                 .AsInt32()
                 .Nullable();

            Create.ForeignKey("FK_NoticesOfViolation_NoticeOfViolationPrimaryRootCauses_PrimaryRootCauseId")
                  .FromTable("EnvironmentalNonComplianceEvents")
                  .ForeignColumn("PrimaryRootCauseId")
                  .ToTable("EnvironmentalNonComplianceEventPrimaryRootCauses")
                  .PrimaryColumn("Id");

            Execute.Sql(SQL.ROLLBACK);

            Delete.Table("EnvironmentalNonComplianceEventsEnvironmentalNonComplianceEventPrimaryRootCauses");
        }
    }
}

