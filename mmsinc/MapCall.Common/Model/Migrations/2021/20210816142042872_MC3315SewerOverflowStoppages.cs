using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210816142042872), Tags("Production")]
    // ReSharper disable once InconsistentNaming
    public class MC3315SewerOverflowStoppages : Migration
    {
        public override void Up()
        {
            Create.Table("SewerOverflowsSewerStoppageTypes")
                  .WithForeignKeyColumn("SewerOverflowId", "SewerOverflows", "SewerOverflowID", nullable: false)
                  .WithForeignKeyColumn("SewerStoppageTypeId", "SewerStoppageTypes", "SewerStoppageTypeID", nullable: false);

            Execute.Sql(@"
                insert into SewerOverflowsSewerStoppageTypes (SewerOverflowId, SewerStoppageTypeId) 
                select SewerOverflowID
                     , SewerStoppageTypeID
                  from SewerOverflows 
                 where SewerStoppageTypeID is not null
            ");

            Delete.ForeignKeyColumn("SewerOverflows", "SewerStoppageTypeID", "SewerStoppageTypes", "SewerStoppageTypeID");

            Alter.Table("SewerOverflows")
                 .AddForeignKeyColumn("WasteWaterSystemId", "WasteWaterSystems")
                 .AddColumn("IsSystemUnderConsentOrder").AsBoolean().NotNullable().WithDefaultValue(false)
                 .AddColumn("IsSystemNewlyAcquired").AsBoolean().NotNullable().WithDefaultValue(false);

            Rename.Column("DEPCaseNumber").OnTable("SewerOverflows").To("EnforcingAgencyCaseNumber");

            Alter.Table("SewerStoppageTypes")
                 .AlterColumn("Description").AsAnsiString(255).NotNullable();

            Execute.Sql(@"
                set identity_insert SewerStoppageTypes on;
                insert into SewerStoppageTypes (SewerStoppageTypeID, Description) values (7, 'CSO Unapproved Location - Other');
                insert into SewerStoppageTypes (SewerStoppageTypeID, Description) values (8, 'CSO Approved Location');
                insert into SewerStoppageTypes (SewerStoppageTypeID, Description) values (9, 'Plant Bypass/Flow');
                set identity_insert SewerStoppageTypes off;
                update SewerStoppageTypes set Description = 'CSO Unapproved Location - Blockage' where SewerStoppageTypeID = 3;                
                update SewerStoppageTypes set Description = 'CSO Unapproved Location - Mechanical and Power Failure' where SewerStoppageTypeID = 4;
                update SewerStoppageTypes set Description = 'CSO Unapproved Location - Wet Weather & I/I' where SewerStoppageTypeID = 5;
                update SewerStoppageTypes set Description = 'CSO Unapproved Location - Line Break' where SewerStoppageTypeID = 6;
            ");
        }

        public override void Down()
        {
            Rename.Column("EnforcingAgencyCaseNumber").OnTable("SewerOverflows").To("DEPCaseNumber");

            Delete.Table("SewerOverflowsSewerStoppageTypes");

            Execute.Sql(@"
                delete from SewerStoppageTypes where SewerStoppageTypeID in (7, 8, 9);
                update SewerStoppageTypes set Description = 'Wet Weather and I/I' where SewerStoppageTypeID = 5;
                update SewerStoppageTypes set Description = 'Blockage' where SewerStoppageTypeID = 3;
                update SewerStoppageTypes set Description = 'Line Break' where SewerStoppageTypeID = 6;
                update SewerStoppageTypes set Description = 'Mechanical and Power Failure' where SewerStoppageTypeID = 4;
            ");

            Delete.Column("IsSystemUnderConsentOrder").FromTable("SewerOverflows");
            Delete.Column("IsSystemNewlyAcquired").FromTable("SewerOverflows");

            Alter.Table("SewerOverflows")
                 .AddForeignKeyColumn("SewerStoppageTypeId", "SewerStoppageTypes", "SewerStoppageTypeID");
                 
            Delete.ForeignKeyColumn("SewerOverflows", "WasteWaterSystemId", "WasteWaterSystems");
        }
    }
}

