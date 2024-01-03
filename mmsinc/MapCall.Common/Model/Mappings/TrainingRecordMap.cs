using System;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class TrainingRecordMap : ClassMap<TrainingRecord>
    {
        public const string TABLE_NAME = "tblTrainingRecords";

        public TrainingRecordMap()
        {
            Table(TABLE_NAME);

            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("TrainingRecordID");

            References(x => x.TrainingModule).Nullable();
            References(x => x.Instructor).Nullable();
            References(x => x.SecondInstructor).Nullable();
            References(x => x.ClassLocation).Nullable();
            References(x => x.ProgramCoordinator).Column("ProgramCoordinator").Nullable();

            Map(x => x.HeldOn).Nullable();
            Map(x => x.CourseLocation);
            Map(x => x.OutsideInstructor);
            Map(x => x.OutsideInstructorTitle);
            Map(x => x.ScheduledDate).Column(AddFieldsToTrainingRecordsAndSuchForBug1738.ColumnNames.SCHEDULED_DATE);
            Map(x => x.MaximumClassSize)
               .Column(AddFieldsToTrainingRecordsAndSuchForBug1738.ColumnNames.MAXIMUM_CLASS_SIZE);
            Map(x => x.Canceled).Nullable();
            Map(x => x.AttendeesExportedDate).Nullable();
            Map(x => x.HasAttachedDocuments).ReadOnly()
                                            .Formula(String.Format(
                                                 "(CASE WHEN EXISTS (SELECT 1 FROM {0} dlv WHERE dlv.TableName = '{1}' AND dlv.LinkedId = TrainingRecordId) THEN 1 ELSE 0 END)",
                                                 CreateDocumentLinkView.VIEW_NAME, TABLE_NAME));
            Map(x => x.Exported)
               .Formula("(SELECT CASE WHEN AttendeesExportedDate IS NOT NULL THEN 1 ELSE 0 END)");

            // NOTE: Don't use datediff(hour) here, datediff(hour) does not include half hours and rounds down to the lowest whole number.
            Map(x => x.HasEnoughTrainingSessionsHoursForTrainingModule)
               .DbSpecificFormula(@"
                    (CASE WHEN (isNull(TrainingModuleID, 0) = 0) THEN 0 
                          WHEN ((SELECT Count(1) from TrainingSessions tss where tss.TrainingRecordID = TrainingRecordId) = 0) THEN 0
                          WHEN ((SELECT abs(sum(datediff(minute, tss.StartDateTime, tss.EndDateTime))) from TrainingSessions tss where tss.TrainingRecordID = TrainingRecordId) >= (SELECT (isNull(tm.TotalHours, 0) * 60) FROM tblTrainingModules tm where tm.TrainingModuleID = TrainingModuleId)) THEN 1
                          ELSE 0 END)").ReadOnly();
            Map(x => x.IsOpen)
               .DbSpecificFormula(
                    "(SELECT CASE WHEN dateadd(day, 1, ScheduledDate) > getdate() AND MaximumClassSize > (SELECT Count(1) FROM EmployeeLinkView elv WHERE elv.LinkedId = TrainingRecordId AND elv.TableName = 'tblTrainingRecords' AND elv.DataTypeName = 'Employees Scheduled') THEN 1 ELSE 0 END)")
               .ReadOnly();
            Map(x => x.CreatedBy)
               .DbSpecificFormula(
                    "(select top 1 P.FullName From AuditLogEntries ale join tblPermissions P on P.RecId = ale.UserId where ale.AuditEntryType = 'INSERT' and ale.EntityName = 'TrainingRecord' and ale.EntityId = TrainingRecordId)",
                    "(select P.FullName From AuditLogEntries ale join tblPermissions P on P.RecId = ale.UserId where ale.AuditEntryType = 'INSERT' and ale.EntityName = 'TrainingRecord' and ale.EntityId = TrainingRecordId LIMIT 1)");
            Map(x => x.MinSessionDate)
               .Formula(
                    "(SELECT MIN(ts.EndDateTime) FROM TrainingSessions ts WHERE ts.TrainingRecordId = TrainingRecordId)")
               .ReadOnly();
            Map(x => x.MaxSessionDate)
               .Formula(
                    "(SELECT MAX(ts.StartDateTime) FROM TrainingSessions ts WHERE ts.TrainingRecordId = TrainingRecordId)")
               .ReadOnly();

            Map(x => x.NextDueDate)
               .DbSpecificFormula(@"(
SELECT CASE req.TrainingFrequency
WHEN 0 THEN NULL ELSE
    CASE req.TrainingFrequencyUnit
        WHEN 'D' THEN dateadd(day, req.TrainingFrequency, HeldOn)
        WHEN 'W' THEN dateadd(day, req.TrainingFrequency * 7, HeldOn)
        WHEN 'M' THEN dateadd(month, req.TrainingFrequency, HeldOn)
        WHEN 'Y' THEN dateadd(year, req.TrainingFrequency, HeldOn)
        ELSE NULL
    END
END
FROM TrainingRequirements req INNER JOIN tblTrainingModules mod ON mod.TrainingRequirementId = req.Id WHERE mod.TrainingModuleId = TrainingModuleId)")
               .LazyLoad();

            HasMany(x => x.EmployeesScheduledOrAttended)
               .KeyColumn("LinkedId")
               .Where("DataTypeName IN ('Employees Scheduled', 'Employees Attended')")
               .Inverse().Cascade.None();

            HasMany(x => x.TrainingRecordDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.TrainingRecordNotes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.EmployeesScheduled)
               .KeyColumn("LinkedId").Inverse().Cascade.None().Not.LazyLoad();
            HasMany(x => x.EmployeesAttended)
               .KeyColumn("LinkedId").Inverse().Cascade.None().Not.LazyLoad();
            HasMany(x => x.TrainingSessions)
               .KeyColumn("TrainingRecordId")
               .Cascade.AllDeleteOrphan().Inverse();
        }
    }
}
