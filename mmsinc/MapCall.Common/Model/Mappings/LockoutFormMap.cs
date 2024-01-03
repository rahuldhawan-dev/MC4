using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class LockoutFormMap : ClassMap<LockoutForm>
    {
        #region Constructors

        public LockoutFormMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.Facility).Not.Nullable();
            References(x => x.Equipment).Nullable();
            References(x => x.LockoutReason).Not.Nullable();
            References(x => x.IsolationPoint).Not.Nullable();
            References(x => x.LockoutDevice).Nullable();
            References(x => x.EquipmentType).Nullable();
            References(x => x.OutOfServiceAuthorizedEmployee).Not.Nullable();
            References(x => x.SupervisorInvolved).Nullable();
            References(x => x.ReturnToServiceAuthorizedEmployee)
               .Column("ReturnToServiceAuthorizedEmployeeId")
               .Nullable();
            References(x => x.AuthorizedManagementPerson).Nullable();
            References(x => x.LockRemovalMethod).Nullable();
            References(x => x.Contractor).Nullable();
            References(x => x.ProductionWorkOrder).Nullable();

            Map(x => x.ContractorFirstName).Nullable().Length(LockoutForm.StringLengths.CONTRACTOR_FIRST_NAME);
            Map(x => x.ContractorLastName).Nullable().Length(LockoutForm.StringLengths.CONTRACTOR_LAST_NAME);
            Map(x => x.ContractorPhone).Nullable().Length(LockoutForm.StringLengths.CONTRACTOR_PHONE);

            Map(x => x.LockoutDateTime).Not.Nullable();
            Map(x => x.ReasonForLockout).Not.Nullable();
            Map(x => x.ContractorLockOutTagOut).Nullable();
            Map(x => x.LocationOfLockoutNotes).Not.Nullable();
            Map(x => x.AdditionalLockoutNotes);
            Map(x => x.OutOfServiceDateTime).Not.Nullable();
            Map(x => x.ReturnedToServiceNotes);
            Map(x => x.ReturnedToServiceDateTime);
            Map(x => x.SameAsInstaller).Nullable();
            Map(x => x.DateOfContact).Nullable();
            Map(x => x.MethodOfContact)
               .Length(LockoutForm.StringLengths.METHOD_OF_CONTACT)
               .Nullable();
            Map(x => x.OutcomeOfContact).Length(int.MaxValue).CustomSqlType("text").Nullable();

            Map(x => x.EmployeeAcknowledgedTraining).Not.Nullable().Default("false");

            Map(x => x.IsolationPointDescription).Length(25).Nullable();

            HasMany(x => x.LockoutFormNotes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.LockoutFormDocuments).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.LockoutFormAnswers).KeyColumn("LockoutFormId").Inverse().Cascade.AllDeleteOrphan();
        }

        #endregion
    }
}
