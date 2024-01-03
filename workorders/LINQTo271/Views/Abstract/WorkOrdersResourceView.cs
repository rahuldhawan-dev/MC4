using System.Security.Principal;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Utilities.Permissions;
using MMSINC.View;

namespace LINQTo271.Views.Abstract
{
    public abstract class WorkOrdersResourceView<TEntity> : ResourceView<TEntity>, IResourceView
        where TEntity : class
    {
        #region Properties

        public abstract IButton BackToListButton { get; }

        #if DEBUG && !LOCAL
        // There wont be an actual user unless your logged in. 
        // This is why we need to do this.
        public override IUser IUser
        {
            get
            {
                if (_iUser == null)
                    _iUser = base.IUser ?? new DebuggingUser();
                return _iUser;
            }
        }
        #endif

        #endregion

        #region Private Methods

        protected void ToggleBackToListButton(bool visible)
        {
            if (BackToListButton != null)
                BackToListButton.Visible = visible;
        }

        #endregion

        #region Exposed Methods

        public override void SetViewMode(ResourceViewMode newMode)
        {
            base.SetViewMode(newMode);
            ToggleBackToListButton(newMode != ResourceViewMode.List);
        }

        public override void ToggleDetail(bool visible)
        {
            if (DetailView != null)
                base.ToggleDetail(visible);
        }

        public override void ToggleList(bool visible)
        {
            if (ListView != null)
                base.ToggleList(visible);
        }

        public override void ToggleSearch(bool visible)
        {
            if (SearchView != null)
                base.ToggleSearch(visible);
        }

        #endregion
    }

    #if DEBUG
    public class DebuggingUser : IUser
    {
        #region Constants

        #if DEBUG && !STAGING
        public const string DEBUGGING_USER_NAME = "Mr. D. Buggin";
        #endif
        #if DEBUG && STAGING
        public const string DEBUGGING_USER_NAME = "prichards";
        #endif

        #endregion

        public DebuggingUser()
        {
            
        }

        public IIdentity Identity
        {
            get { return null; }
        }

        public string Name
        {
            // his name hopkin green frog
            get { return DEBUGGING_USER_NAME; }
        }

        public bool IsInRole(string role)
        {
            // who took my frog?  i will find my frog.
            return true;
        }

        public IPermissionsObject CanRead(IModulePermissions perm)
        {
            return new DebuggingPermissionsObject();
        }

        public IPermissionsObject CanAdd(IModulePermissions perm)
        {
            return new DebuggingPermissionsObject();
        }

        public IPermissionsObject CanEdit(IModulePermissions perm)
        {
            return new DebuggingPermissionsObject();
        }

        public IPermissionsObject CanDelete(IModulePermissions perm)
        {
            return new DebuggingPermissionsObject();
        }

        public IPermissionsObject CanAdministrate(IModulePermissions perm)
        {
            return new DebuggingPermissionsObject();
        }
    }

    public class DebuggingPermissionsObject : PermissionsObject
    {
        public DebuggingPermissionsObject() : this(null, null, null, ModuleAction.Administrate)
        {
        }

        public DebuggingPermissionsObject(IRoleManager roleManager, IUser user, IModulePermissions specificPermissions, ModuleAction action) : base(roleManager, user, specificPermissions, action)
        {
        }

        public override bool In(string opCntrID)
        {
            return true;
        }

        public override bool InAny()
        {
            return true;
        }
    }
    #endif
}
