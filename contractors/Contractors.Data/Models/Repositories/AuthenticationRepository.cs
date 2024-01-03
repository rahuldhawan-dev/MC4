using MapCall.Common.Authentication;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using NHibernate;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class AuthenticationRepository : AuthenticationRepositoryBase<ContractorUser>
    {
        #region Consts

        // bug 4077: Max attempts is 6 before lock out.
        public const int MAXIMUM_FAILED_LOGIN_ATTEMPTS = 6;

        #endregion

        #region Fields

        private static readonly ContractorUserCredentialPolicy _passwordRequirement =  new ContractorUserCredentialPolicy();

        #endregion
        
        #region Properties

        public override ICredentialPolicy CredentialPolicy => _passwordRequirement;

        #endregion

        #region Construcotrs

        public AuthenticationRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Private Methods

        public override void OnInvalidPassword(ContractorUser user)
        {
            base.OnInvalidPassword(user);
            user.FailedLoginAttemptCount += 1;

            // NOTE: We do NOT want to set IsActive to true here ever.
            if (user.FailedLoginAttemptCount >= CredentialPolicy.MaximumFailedLoginAttemptCount)
            {
                user.IsActive = false;
            }
            Save(user);
        }

        public override void OnValidPassword(ContractorUser user)
        {
            base.OnValidPassword(user);
            user.FailedLoginAttemptCount = 0;
            Save(user);
        }

        #endregion
    }
}
