using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160909113635460), Tags("Production")]
    public class AddColumnsForRecurringProjectsForBug2823 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "Update PipeDataLookupValues SET IsEnabled = 0 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Non-Break Complaint Frequency')" +
                "Update PipeDataLookupValues SET IsEnabled = 0 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Residential Customers Affected')" +
                "Update PipeDataLookupValues SET IsEnabled = 0 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Commercial Customers Affected')" +
                "Update PipeDataLookupValues SET IsEnabled = 0 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Decade Installed')" +
                "Update PipeDataLookupValues SET IsEnabled = 0 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Pipe Diameter')" +
                "Update PipeDataLookupValues SET IsEnabled = 0 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Pipe Material')" +
                "Update PipeDataLookupValues SET IsEnabled = 0 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Pressure Class Adequacy')" +
                "Update PipeDataLookupValues SET IsEnabled = 0 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Current Fire Flow Adequacy')" +
                "Update PipeDataLookupValues SET IsEnabled = 0 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Future Fire Flow Adequacy')" +
                "Update PipeDataLookupValues SET IsEnabled = 0 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Current Hydraulic Adequacy')" +
                "Update PipeDataLookupValues SET IsEnabled = 0 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Future Hydraulic Adequacy')" +
                "Update PipeDataLookupValues SET IsEnabled = 0 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Main Break Frequency (breaks per line per year)')");

            // Mains
            Create.Table("RecurringProjectMains")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("RecurringProjectId", "RecurringProjects", nullable: false)
                  .WithColumn("Layer").AsAnsiString(50).NotNullable()
                  .WithColumn("Guid").AsAnsiString(38).NotNullable()
                  .WithColumn("LikelyhoodOfFailure").AsDecimal(18, 2).NotNullable()
                  .WithColumn("ConsequenceOfFailure").AsDecimal(18, 2).NotNullable()
                  .WithColumn("TotalInfoMasterScore").AsAnsiString(2).Nullable()
                  .WithColumn("Length").AsDecimal(18, 2).NotNullable();

            this.CreateLookupTableWithValues("OverrideInfoMasterReasons", "Moratorium", "Local Politics",
                "GIS Data Incorrect", "Hydraulics", "Relocation Need");

            Alter.Table("RecurringProjects")
                 .AddColumn("OverrideInfoMasterDecision").AsBoolean().Nullable()
                 .AddForeignKeyColumn("OverrideInfoMasterReasonId", "OverrideInfoMasterReasons").Nullable()
                 .AddColumn("OverrideInfoMasterJustification").AsCustom("text").Nullable()
                 .AddColumn("TotalInfoMasterScore").AsAnsiString(2).Nullable();

            this.AddNotificationType("Field Services", "Projects", "ProjectsRP GIS Data Incorrect");
        }

        public override void Down()
        {
            this.RemoveNotificationType("Field Services", "Projects", "GIS Data Incorrect");

            //this.DeleteLookupTableLookup("RecurringProjects", "OverrideInfoMasterReasonId", "OverrideInfoMasterReasons");
            Delete.ForeignKeyColumn("RecurringProjects", "OverrideInfoMasterReasonId", "OverrideInfoMasterReasons");
            Delete.Table("OverrideInfoMasterReasons");
            Delete.Column("OverrideInfoMasterJustification").FromTable("RecurringProjects");
            Delete.Column("OverrideInfoMasterDecision").FromTable("RecurringProjects");

            Delete.Table("RecurringProjectMains");

            Execute.Sql(
                "Update PipeDataLookupValues SET IsEnabled = 1 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Non-Break Complaint Frequency')" +
                "Update PipeDataLookupValues SET IsEnabled = 1 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Residential Customers Affected')" +
                "Update PipeDataLookupValues SET IsEnabled = 1 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Commercial Customers Affected')" +
                "Update PipeDataLookupValues SET IsEnabled = 1 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Decade Installed')" +
                "Update PipeDataLookupValues SET IsEnabled = 1 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Pipe Diameter')" +
                "Update PipeDataLookupValues SET IsEnabled = 1 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Pipe Material')" +
                "Update PipeDataLookupValues SET IsEnabled = 1 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Pressure Class Adequacy')" +
                "Update PipeDataLookupValues SET IsEnabled = 1 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Current Fire Flow Adequacy')" +
                "Update PipeDataLookupValues SET IsEnabled = 1 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Future Fire Flow Adequacy')" +
                "Update PipeDataLookupValues SET IsEnabled = 1 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Current Hydraulic Adequacy')" +
                "Update PipeDataLookupValues SET IsEnabled = 1 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Future Hydraulic Adequacy')" +
                "Update PipeDataLookupValues SET IsEnabled = 1 where PipeDataLookupTypeID = (select Id from PipeDataLookupTypes where Description = 'Main Break Frequency (breaks per line per year)')");
        }
    }
}
