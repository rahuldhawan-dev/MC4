using MMSINC.Interface;

namespace MMSINC.Utilities.Permissions
{
    public interface IPermissionsObject
    {
        IUser User { get; }
        IModulePermissions SpecificPermissions { get; }
        ModuleAction Action { get; }

        bool InAny();
        bool In(string opCntr);
    }
}
