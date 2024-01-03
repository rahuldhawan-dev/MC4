using System;

namespace MMSINC.Authentication
{
    public interface IRetrievablePasswordUser : IAdministratedUser
    {
        #region Properties

        String PasswordQuestion { get; }
        String PasswordAnswer { get; }

        #endregion
    }
}
