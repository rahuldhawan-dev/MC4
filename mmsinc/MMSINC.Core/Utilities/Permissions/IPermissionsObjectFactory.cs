using MMSINC.Interface;

namespace MMSINC.Utilities.Permissions
{
    public interface IPermissionsObjectFactory
    {
        PermissionsObject Build(IUser user, IModulePermissions specificPermissions, ModuleAction action);
    }
}
