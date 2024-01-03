using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.StringExtensions;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class AuthenticationRepository : AuthenticationRepositoryBase<User>
    {
        #region Constructors

        #region Constructor

        public AuthenticationRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// MapCall/MapCallMVC uses usernames, not email addresses.
        /// </summary>
        protected override bool ValidateEmailAddress(string email)
        {
            return !string.IsNullOrWhiteSpace(email);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This is hacked to find the user by UserName instead of by Email. 
        /// </summary>
        public override User GetUser(string userName)
        {
            // TODO: Test this entire method.
            userName = userName.SanitizeAndDowncase();
            return Linq.SingleOrDefault(x => x.UserName == userName);
        }
        
        #endregion
    }
}
