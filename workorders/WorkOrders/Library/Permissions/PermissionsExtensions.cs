using MMSINC.Interface;
using WorkOrders.Model;

namespace WorkOrders.Library.Permissions
{
    public static class PermissionsExtensions
    {
        #region Constants

        public struct Roles
        {
            public const string ADMIN_FORMAT =
                "{0}_Field Services_Work Management_User Administrator";
            public const string USER_FORMAT =
                "{0}_Field Services_Work Management_Read";
        }

        #endregion

        #region Static Methods

        public static bool HasAccessTo(IUser user, OperatingCenter opCntr)
        {
            return user.IsInRole(string.Format(Roles.USER_FORMAT,opCntr.OpCntr));
        }

        public static bool IsAdminFor(IUser user, OperatingCenter opCntr)
        {
            return
                user.IsInRole(string.Format(Roles.ADMIN_FORMAT, opCntr.OpCntr));
        }

        #endregion
    }
}
