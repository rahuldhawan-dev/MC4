using System.Security.Principal;
using MMSINC.Utilities.Permissions;

namespace MMSINC.Interface
{
    public interface IUser : IPrincipal
    {
        #region Properties

        string Name { get; }

        #endregion

        #region Methods

        IPermissionsObject CanRead(IModulePermissions perm);
        IPermissionsObject CanAdd(IModulePermissions perm);
        IPermissionsObject CanEdit(IModulePermissions perm);
        IPermissionsObject CanDelete(IModulePermissions perm);
        IPermissionsObject CanAdministrate(IModulePermissions perm);

        #endregion
    }
}
