using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class CovidIssueMap : ClassMap<CovidIssue>
    {
        public CovidIssueMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.Employee).Not.Nullable();
            References(x => x.RequestType).Not.Nullable();
            References(x => x.SubmissionStatus).Not.Nullable();
            References(x => x.OutcomeCategory).Nullable();
            References(x => x.QuarantineStatus).Nullable();
            References(x => x.PersonnelArea)
               .Nullable(); // This is a required field now, but older records might not have a value set.
            References(x => x.ReleaseReason).Nullable();
            References(x => x.HumanResourcesManager).Nullable();

            Map(x => x.SupervisorsCell).Length(CovidIssue.StringLengths.SUPERVISORS_CELL).Nullable();
            Map(x => x.LocalEmployeeRelationsBusinessPartner).Length(CovidIssue.StringLengths.LOCAL_ERBP).Nullable();
            Map(x => x.LocalEmployeeRelationsBusinessPartnerCell).Length(CovidIssue.StringLengths.LOCAL_ERBP_CELL)
                                                                 .Nullable();
            Map(x => x.SubmissionDate).Not.Nullable();
            Map(x => x.QuestionFromEmail).Length(int.MaxValue).Not.Nullable();
            Map(x => x.OutcomeDescription).Length(int.MaxValue).Nullable();
            Map(x => x.StartDate).Nullable();
            Map(x => x.ReleaseDate).Nullable();
            Map(x => x.EstimatedReleaseDate).Nullable();
            Map(x => x.QuarantineReason).Length(int.MaxValue).Nullable();
            Map(x => x.PersonalEmailAddress).Length(CovidIssue.StringLengths.PERSONAL_EMAIL_ADDRESS).Nullable();
            Map(x => x.HealthDepartmentNotification).Nullable();
            References(x => x.WorkExposure, "WorkExposureId").Nullable();
            References(x => x.FaceCoveringWorn, "FaceCoveringWornId").Nullable();
            References(x => x.AvoidableCloseContact, "AvoidableCloseContactId").Nullable();

            HasMany(x => x.Documents).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.Notes).KeyColumn("LinkedId").Inverse().Cascade.None();
        }
    }
}
