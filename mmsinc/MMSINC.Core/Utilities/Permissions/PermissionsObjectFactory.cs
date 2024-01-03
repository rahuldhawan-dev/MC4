using MMSINC.Interface;
using StructureMap;

namespace MMSINC.Utilities.Permissions
{
    public class PermissionsObjectFactory : IPermissionsObjectFactory
    {
        private readonly IContainer _container;

        public PermissionsObjectFactory(IContainer container)
        {
            _container = container;
        }

        public PermissionsObject Build(IUser user, IModulePermissions specificPermissions, ModuleAction action)
        {
            return _container
                  .With(user)
                  .With(specificPermissions)
                  .With(action)
                  .GetInstance<PermissionsObject>();
        }
    }
}
