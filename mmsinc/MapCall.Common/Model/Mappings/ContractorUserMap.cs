using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ContractorUserMap : ClassMap<ContractorUser>
    {
        #region Constructor

        public ContractorUserMap()
        {
            Id(x => x.Id, "ContractorUserID");

            Map(x => x.Email).Unique().Length(ContractorUser.EMAIL_MAX_LENGTH);
            Map(x => x.IsActive, "HasAccess").Not.Nullable();
            Map(x => x.Password).LazyLoad().Length(ContractorUser.PASSWORD);
            Map(x => x.PasswordSalt).LazyLoad();
            Map(x => x.PasswordQuestion).LazyLoad().Length(ContractorUser.QUESTION_MAX_LENGTH);
            Map(x => x.PasswordAnswer).LazyLoad().Length(ContractorUser.PASSWORD_ANSWER);
            Map(x => x.FailedLoginAttemptCount).Not.Nullable();
            Map(x => x.IsAdmin);
            Map(x => x.LastLoggedInAt).Nullable();

            // The fetch is being set here since virtually every time we need user info
            // we also need the contractor.
            References(x => x.Contractor).Not.Nullable().Fetch.Join();
        }

        #endregion
    }
}
