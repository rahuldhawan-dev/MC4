using MMSINC.Interface;

namespace MMSINC.Utilities.Permissions
{
    public interface ISecurityService
    {
        #region Properties

        IUser CurrentUser { get; }
        bool UserHasAccess { get; }
        bool IsAdmin { get; }

        #endregion

        #region Methods

        void Init(IUser user);

        #endregion
    }
}
