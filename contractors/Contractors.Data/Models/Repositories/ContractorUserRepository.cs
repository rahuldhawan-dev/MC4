using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Exceptions;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using StructureMap;
using AuthenticationService = Contractors.Data.Library.AuthenticationService;

namespace Contractors.Data.Models.Repositories
{
    public class ContractorUserRepository : SecuredRepositoryBase<ContractorUser, ContractorUser>, IContractorUserRepository
    {
        #region Properties

        public override IQueryable<ContractorUser> Linq
        {
            get
            {
                return (from c in base.Linq where c.Contractor.Id == CurrentUser.Contractor.Id select c);
            }
        }

        public override ICriteria Criteria
        {
            get
            {
                return base.Criteria
                    .Add(Restrictions.Eq("Contractor.Id", CurrentUser.Contractor.Id));
            }
        }

        #endregion

        #region Constructors

        public ContractorUserRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }

        #endregion

        #region Private Methods

        protected virtual ContractorUser CreateNewUserInstance()
        {
            return new ContractorUser();
        }

        #endregion

        #region Public Methods

        public bool IsAdmin(string email)
        {
            var user = GetUser(email);
            if (user == null)
            {
                throw AuthenticationException.UserDoesNotExist(email);
            }

            return user.IsAdmin;
        }

        public ContractorUser GetUser(string email)
        {
            AuthenticationService.ThrowIfInvalidEmail(email);
            email = email.SanitizeAndDowncase();
            var user = (from c in Session.Query<ContractorUser>() where c.Email == email select c).SingleOrDefault();

            if (user != null)
            {
                Session.Evict(user);
            }

            return user;
        }

        #endregion

        //#region Public Static Methods

        //public static string GetErrorMessageForUserLoginAttempt(UserLoginAttemptStatus status)
        //{
        //    switch (status)
        //    {
        //        case UserLoginAttemptStatus.InvalidEmail:
        //            return "You must enter a valid email address.";
        //        case UserLoginAttemptStatus.BadPassword:
        //        case UserLoginAttemptStatus.UnknownUser:
        //            return "User does not exist or password is incorrect.";
        //        case UserLoginAttemptStatus.AccessDisabled:
        //            return "Access is not enabled.";
        //        default:
        //            throw new NotSupportedException();
        //    }
        //}

        //#endregion
    }
}
