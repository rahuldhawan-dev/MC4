using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230925092958645), Tags("Production")]
    public class MC5913_ReplaceLookupTypesInSewerOverflow : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("SewerOverflowDischargeLocations", 50,
                "Runs On Ground and Absorbs Into Soil",
                "Ditch or Detention Basin",
                "Storm Sewer",
                "Body of Water",
                "Other");
            
            this.CreateLookupTableWithValues("SewerOverflowTypes", 50,
                "SSO",
                "CSO - Approved Location",
                "CSO - Unapproved Location");
            
            this.CreateLookupTableWithValues("SewerOverflowCauses", 50,
                "Pipe Failure",
                "Debris (Rags/Paper)",
                "Grease",
                "Roots",
                "Power Failure",
                "Pump Station / Mechanical Failure",
                "Inflow & Infiltration",
                "Vandalism",
                "Pipe - Capacity/Design");
            
            // Create notes for SewerOverflows that contained any StoppageTypes. The note will contain the data so it's not lost.
            Execute.Sql(
                @"DECLARE @dataTypeTableId AS INT = (SELECT DataTypeID FROM Datatype WHERE Table_Name = 'SewerOverflows');
                INSERT INTO Note (Note, CreatedAt, DataLinkID, DataTypeID, CreatedBy)
                SELECT
                    CONCAT('Legacy field ""Stoppage Type"" was removed from the system. Previously, this field was set to ""', st.Description, '""'),
                    GETDATE(),
                    link.SewerOverflowId,
                    @dataTypeTableId, 
                    'mcadmin'
                FROM SewerOverflowsSewerStoppageTypes link
                INNER JOIN SewerStoppageTypes st ON link.SewerStoppageTypeId = st.SewerStoppageTypeID");

            Delete.Table("SewerOverflowsSewerStoppageTypes");
            Delete.Table("SewerStoppageTypes");
            
            // Create notes for SewerOverflows that contained any OverflowReasons. The note will contain the data so it's not lost.
            Execute.Sql(
                @"DECLARE @dataTypeTableId AS INT = (SELECT DataTypeID FROM Datatype WHERE Table_Name = 'SewerOverflows');
                INSERT INTO Note (Note, CreatedAt, DataLinkID, DataTypeID, CreatedBy)
                SELECT
                    CONCAT('Legacy field ""Sewer Overflow Reasons"" was removed from the system. Previously, this field was set to ""', st.Description, '""'),
                    GETDATE(),
                    link.SewerOverflowID,
                    @dataTypeTableId, 
                    'mcadmin'
                FROM SewerOverflowsSewerOverflowReasons link
                INNER JOIN SewerOverflowReasons st ON link.SewerOverflowReasonId = st.Id");

            Delete.Table("SewerOverflowsSewerOverflowReasons");
            Delete.Table("SewerOverflowReasons");
        }
        
        public override void Down()
        {
            Delete.Table("SewerOverflowCauses");
            Delete.Table("SewerOverflowTypes");
            Delete.Table("SewerOverflowDischargeLocations");
           
            // Re-create the StoppageTypes table and retrieve any data from Notes
            Create.Table("SewerStoppageTypes")
                  .WithIdentityColumn("SewerStoppageTypeID")
                  .WithColumn("Description").AsAnsiString(255).NotNullable();
            
            Insert.IntoTable("SewerStoppageTypes")
                  .Row(new { Description = "Main" })
                  .Row(new { Description = "Lateral" })
                  .Row(new { Description = "Blockage" })
                  .Row(new { Description = "Mechanical and Power Failure" })
                  .Row(new { Description = "Wet Weather and I/I" })
                  .Row(new { Description = "Line Break" })
                  .Row(new { Description = "Other" })
                  .Row(new { Description = "CSO Approved Location" })
                  .Row(new { Description = "Plant Bypass/Flow" })
                  .Row(new { Description = "CSO Unapproved Location" });
            
            Create.Table("SewerOverflowsSewerStoppageTypes")
                  .WithForeignKeyColumn("SewerOverflowId", "SewerOverflows", "SewerOverflowID", false)
                  .WithForeignKeyColumn("SewerStoppageTypeId", "SewerStoppageTypes", "SewerStoppageTypeID", false);
            
            // Parse StoppageTypes from Notes associated with SewerOverflows, and add those StoppageTypes back to their associated SewerOverflows records.
            Execute.Sql(
                @"DECLARE @parsePosition AS INT = 94;
                INSERT INTO SewerOverflowsSewerStoppageTypes (SewerOverflowId, SewerStoppageTypeId) 
                SELECT n.DataLinkID, st.SewerStoppageTypeID FROM Note n 
                    INNER JOIN SewerStoppageTypes st ON st.Description = SUBSTRING(n.Note, @parsePosition, CHARINDEX('""', n.Note, @parsePosition) - @parsePosition) 
                WHERE n.Note LIKE 'Legacy field ""Stoppage Type""%'");
            
            Execute.Sql("DELETE FROM Note WHERE Note LIKE 'Legacy field \"Stoppage Type\"%';");
            
            // Re-create the SewerOverflowReasons table and retrieve any data from Notes
            this.CreateLookupTableWithValues("SewerOverflowReasons", 50, 
                "Grease", 
                "Other",
                "Roots",
                "Cave In",
                "Paper",
                "Rags",
                "Debris");
            
            Create.Table("SewerOverflowsSewerOverflowReasons")
                  .WithForeignKeyColumn("SewerOverflowID", "SewerOverflows", "SewerOverflowID", false)
                  .WithForeignKeyColumn("SewerOverflowReasonId", "SewerOverflowReasons", "Id", false);
            
            // Parse StoppageTypes from Notes associated with SewerOverflows, and add those StoppageTypes back to their associated SewerOverflows records.
            Execute.Sql(
                @"DECLARE @parsePosition AS INT = 103;
                INSERT INTO SewerOverflowsSewerOverflowReasons (SewerOverflowID, SewerOverflowReasonId) 
                SELECT n.DataLinkID, sor.Id FROM Note n 
                    INNER JOIN SewerOverflowReasons sor ON sor.Description = SUBSTRING(n.Note, @parsePosition, CHARINDEX('""', n.Note, @parsePosition) - @parsePosition) 
                WHERE n.Note LIKE 'Legacy field ""Sewer Overflow Reasons""%'");
            
            Execute.Sql("DELETE FROM Note WHERE Note LIKE 'Legacy field \"Sewer Overflow Reasons\"%';");
        }
    }
}

