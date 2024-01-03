using System.Web.Security;

namespace MMSINC.Authentication
{
    /// <summary>
    /// Wrapper interface for dealing with the Membership class's static methods.
    /// </summary>
    public interface IMembershipHelper
    {
        /// <summary>
        /// Creates a new MembershipUser.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="securityQuestion"></param>
        /// <param name="securityAnswer"></param>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        MembershipCreateStatus CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved);

        /// <summary>
        /// Resets the user's password to a randomly generated string. The returned value is that password.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        string ResetPassword(string username);

        /// <summary>
        /// Unlocks the membership account for a given username.
        /// </summary>
        /// <param name="username"></param>
        bool UnlockUser(string username);

        /// <summary>
        /// Updates the user's IsApproved status. If this is false, their account will not have access.
        /// </summary>
        void UpdateApproval(string username, bool isApproved);

        /// <summary>
        /// Updates the user's email address.
        /// </summary>
        void UpdateUserEmail(string username, string email);

        /// <summary>
        /// Returns true/false for whether a user exists for the MembershipProvider.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool UserExists(string username);

        /// <summary>
        /// Validates that a user/password combination is valid.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool ValidateUser(string user, string password);
    }
}
