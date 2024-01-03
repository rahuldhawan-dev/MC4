using System;
using System.Web.Security;

namespace MMSINC.Authentication
{
    // NOTE: Don't expose MembershipUser from this class.
    // It makes testing that much more complicated as that
    // class would also need to be wrapped up just to be mockable.
    // 
    // This class is also not unit-testable because of the static
    // Membership class. Functional tests will need to cover any usage.

    /// <inheritdoc />
    public class MembershipHelper : IMembershipHelper
    {
        /// <inheritdoc />
        public MembershipCreateStatus CreateUser(string username, string password, string email, string passwordQuestion,
            string passwordAnswer, bool isApproved)
        {
            MembershipCreateStatus membershipCreateStatus;
            Membership.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, out membershipCreateStatus);
            return membershipCreateStatus;
        }

        /// <inheritdoc />
        public string ResetPassword(string username)
        {
            // NOTE: If the MembershipProvider is setup to require question/answer, then ResetPassword
            // will fail since a password answer was not supplied. 
            var user = Membership.GetUser(username);
            if (user == null)
            {
                throw new InvalidOperationException($"Can't reset password. No such user: {username}");
            }
            return user.ResetPassword();
        }
        
        /// <inheritdoc />
        public bool UnlockUser(string username)
        {
            var user = Membership.GetUser(username);
            if (user == null)
            {
                throw new InvalidOperationException($"Can't unlock user. No such user: {username}");
            }
            return user.UnlockUser();
        }
        
        /// <inheritdoc />
        public void UpdateApproval(string username, bool isApproved)
        {
            var user = Membership.GetUser(username);
            if (user == null)
            {
                throw new InvalidOperationException($"Can't update user approved status. No such user: {username}");
            }

            user.IsApproved = isApproved;
            Membership.UpdateUser(user);
        }

        /// <inheritdoc />
        public void UpdateUserEmail(string username, string email)
        {
            var user = Membership.GetUser(username);
            if (user == null)
            {
                throw new InvalidOperationException($"Can't update user email. No such user: {username}");
            }

            user.Email = email;
            Membership.UpdateUser(user);
        }

        /// <inheritdoc />
        public bool UserExists(string username)
        {
            return Membership.GetUser(username) != null;
        }

        /// <inheritdoc />
        public bool ValidateUser(string user, string password)
        {
            return Membership.ValidateUser(user, password);
        }
    }
}
