using System;

namespace MMSINC.Authentication
{
    /// <summary>
    /// Interface represents the properties for a User that are unique to that user. Needed for AuthenticationService
    /// </summary>
    public interface IUniqueUser
    {
        int Id { get; }

        /// <summary>
        /// Gets the unique name used as a login name(Email in Permits/Contractors, UserName in MapCall)
        /// </summary>
        string UniqueName { get; }
    }

    public interface IAdministratedUser : IUniqueUser
    {
        #region Properties

        bool IsAdmin { get; }
        bool HasAccess { get; }
        string Email { get; }
        string Password { get; }
        Guid PasswordSalt { get; }
        DateTime? LastLoggedInAt { get; set; }

        #endregion
    }

    public interface IUserWithProfile : IAdministratedUser
    {
        #region Properties

        int? CustomerProfileId { get; set; }
        DateTime? ProfileLastVerified { get; set; }

        #endregion
    }
}
