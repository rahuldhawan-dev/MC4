using System.Security.Principal;
using MMSINC.Interface;

namespace MMSINC.Utilities.Permissions
{
    public class SiteUserWrapper : IUser
    {
        #region Private Members

        protected IPrincipal _innerUser;
        protected readonly IPermissionsObjectFactory _permissionsObjectFactory;
        protected IIdentity _identity;
        protected string _name;

        private bool? _canActuallyRead;

        #endregion

        #region Constructors

        public SiteUserWrapper(IPrincipal innerUser, IPermissionsObjectFactory permissionsObjectFactory)
        {
            _innerUser = innerUser;
            _permissionsObjectFactory = permissionsObjectFactory;
        }

        #endregion

        #region Properties

        public virtual IIdentity Identity
        {
            get
            {
                if (_identity == null)
                    _identity = _innerUser.Identity;
                return _identity;
            }
        }

        public string Name
        {
            get
            {
                if (_name == null)
                    _name = Identity.Name;
                return _name;
            }
        }

        #endregion

        #region Exposed Methods

        public bool IsInRole(string role)
        {
            return (_innerUser.IsInRole(role));
        }

        public IPermissionsObject CanRead(IModulePermissions perm)
        {
            return _permissionsObjectFactory.Build(this, perm, ModuleAction.Read);
        }

        public IPermissionsObject CanAdd(IModulePermissions perm)
        {
            return _permissionsObjectFactory.Build(this, perm, ModuleAction.Add);
        }

        public IPermissionsObject CanEdit(IModulePermissions perm)
        {
            return _permissionsObjectFactory.Build(this, perm, ModuleAction.Edit);
        }

        public IPermissionsObject CanDelete(IModulePermissions perm)
        {
            return _permissionsObjectFactory.Build(this, perm, ModuleAction.Delete);
        }

        public IPermissionsObject CanAdministrate(IModulePermissions perm)
        {
            return _permissionsObjectFactory.Build(this, perm, ModuleAction.Administrate);
        }

        #endregion
    }
}
