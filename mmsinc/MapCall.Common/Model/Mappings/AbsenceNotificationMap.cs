using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class AbsenceNotificationMap : ClassMap<AbsenceNotification>
    {
        public AbsenceNotificationMap()
        {
            Id(x => x.Id, "AbsenceNotificationID");

            References(x => x.Employee, "Employee").Nullable();
            References(x => x.SubmittedBy).Not.Nullable();
            References(x => x.FamilyMedicalLeaveActCase, "FMLACaseID").Nullable();
            References(x => x.EmployeeAbsenceClaim, "EmployeeAbsenceClaim").Nullable();
            References(x => x.EmployeeFMLANotification).Not.Nullable();
            References(x => x.ProgressiveDiscipline).Nullable();
            References(x => x.AbsenceStatus).Nullable();

            Map(x => x.LastDayOfWork).Nullable();
            Map(x => x.StartDate).Nullable();
            Map(x => x.EndDate).Nullable();
            Map(x => x.TotalHoursOfAbsence).Nullable();
            Map(x => x.SupervisorNotes).Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.HumanResourcesReviewed, "HRReviewed").Not.Nullable();
            Map(x => x.HumanResourcesNotes, "HRNotes").Nullable();
            Map(x => x.PackageDateDue, "FMLAPackageDateDue").Nullable();
            Map(x => x.PackageDateSent, "FMLAPackageDateSent").Nullable();
            Map(x => x.ReturnToWorkNote).Not.Nullable();
            Map(x => x.ProgressiveDisciplineAdministered).Nullable();

            // Notes/Docs
            HasMany(x => x.AbsenceNotificationDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.AbsenceNotificationNotes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
        }
    }
}
