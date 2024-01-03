
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EmployeeAccountabilityActionMap : ClassMap<EmployeeAccountabilityAction>
    {
        public EmployeeAccountabilityActionMap()
        {
            Id(x => x.Id);
            //Original
            References(x => x.OperatingCenter, "OperatingCenterId").Not.Nullable();
            References(x => x.Employee, "EmployeeId").Not.Nullable();
            References(x => x.DisciplineAdministeredBy, "DisciplineAdministeredById").Not.Nullable();
            References(x => x.AccountabilityActionTakenType, "AccountabilityActionTakenTypeId").Not.Nullable();
            Map(x => x.AccountabilityActionTakenDescription).Nullable().Length(EmployeeAccountabilityAction.StringLengths.ACCOUNTABILITY_ACTION_TAKEN_DESCRIPTION).Not.Nullable();
            Map(x => x.DateAdministered).Not.Nullable();
            Map(x => x.StartDate).Nullable();
            Map(x => x.EndDate).Nullable();
            Map(x => x.NumberOfWorkDays).Nullable();
            References(x => x.Incident, "IncidentId").Nullable();
            References(x => x.Grievance).Nullable();
            //Modified
            Map(x => x.HasModifiedDiscipline).Nullable();
            References(x => x.ModifiedDisciplineAdministeredBy, "ModifiedDisciplineAdministeredById").Nullable();
            References(x => x.ModifiedAccountabilityActionTakenType, "ModifiedAccountabilityActionTakenTypeId").Nullable();
            Map(x => x.ModifiedAccountabilityActionTakenDescription).Nullable().Length(EmployeeAccountabilityAction.StringLengths.ACCOUNTABILITY_ACTION_TAKEN_DESCRIPTION);
            Map(x => x.DateModified).Nullable();
            Map(x => x.ModifiedStartDate).Nullable();
            Map(x => x.ModifiedEndDate).Nullable();
            Map(x => x.ModifiedNumberOfWorkDays).Nullable();
            Map(x => x.BackPayRequired).Nullable();
            //Notes&Docs
            HasMany(x => x.Documents).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.Notes).KeyColumn("LinkedId").Inverse().Cascade.None();
            //linked 
            HasMany(x => x.EmployeeAccountabilityActionEmployee)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
        }
    }
}