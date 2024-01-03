using System;
using MMSINC.Interface;
using MMSINC.Utilities.Permissions;

namespace MMSINC.DataPages.Permissions
{
    public interface IRoleBasedDataPagePermissions : IDataPagePermissions
    {
        IUser IUser { get; }
        string PageRole { get; }
    }

    /// <summary>
    /// This class is for replacing the redundant code checks for page access/adding/editing/deleting. Makes it easy to
    /// pass the same permissions instance to page's child controls that may want it too at some point.
    /// </summary>
    public class RoleBasedDataPagePermissions : IRoleBasedDataPagePermissions
    {
        #region Fields

        // These fields must only be accessed by their
        // property getters. 
        private IPermission _adminAccess;
        private IPermission _createAccess;
        private IPermission _deleteAccess;
        private IPermission _editAccess;
        private IPermission _readAccess;
        private IPermission _pageAccess;

        #endregion

        #region Properties

        public IModulePermissions ModulePermissions { get; private set; }

        public string PermissionName
        {
            get { return PageRole; }
        }

        public IUser IUser { get; set; }

        public string PageRole { get; set; }

        public virtual IPermission PageAccess
        {
            get
            {
                if (_pageAccess == null)
                {
                    var p = new AggregatedPermission();
                    p.AddPermission(AdminAccess);
                    p.AddPermission(ReadAccess);
                    p.AddPermission(EditAccess);
                    p.AddPermission(DeleteAccess);
                    p.AddPermission(CreateAccess);
                    _pageAccess = p;
                }

                return _pageAccess;
            }
        }

        public virtual IPermission AdminAccess
        {
            get
            {
                if (_adminAccess == null)
                {
                    _adminAccess = GetPermission(this.IUser.CanAdministrate(this.ModulePermissions));
                }

                return _adminAccess;
            }
        }

        public virtual IPermission CreateAccess
        {
            get
            {
                if (_createAccess == null)
                {
                    var p = new AggregatedPermission();
                    p.AddPermission(GetPermission(this.IUser.CanAdd(this.ModulePermissions)));
                    p.AddPermission(AdminAccess);
                    _createAccess = p;
                }

                return _createAccess;
            }
        }

        public virtual IPermission DeleteAccess
        {
            get
            {
                if (_deleteAccess == null)
                {
                    var p = new AggregatedPermission();
                    p.AddPermission(GetPermission(this.IUser.CanDelete(this.ModulePermissions)));
                    p.AddPermission(AdminAccess);
                    _deleteAccess = p;
                }

                return _deleteAccess;
            }
        }

        public virtual IPermission EditAccess
        {
            get
            {
                if (_editAccess == null)
                {
                    var p = new AggregatedPermission();
                    p.AddPermission(GetPermission(this.IUser.CanEdit(this.ModulePermissions)));
                    p.AddPermission(AdminAccess);
                    _editAccess = p;
                }

                return _editAccess;
            }
        }

        public virtual IPermission ReadAccess
        {
            get
            {
                if (_readAccess == null)
                {
                    var p = new AggregatedPermission();
                    p.AddPermission(GetPermission(this.IUser.CanRead(this.ModulePermissions)));
                    p.AddPermission(AdminAccess); // _adminAccess would be accessed through property getter instead.
                    _readAccess = p;
                }

                return _readAccess;
            }
        }

        #endregion

        #region Constructors

        public RoleBasedDataPagePermissions(IModulePermissions modPerm, IUser user)
        {
            if (modPerm == null)
            {
                throw new NullReferenceException("pageRoleName can not be null or empty.");
            }

            if (user == null)
            {
                throw new NullReferenceException("user");
            }

            this.IUser = user;
            this.ModulePermissions = modPerm;

            // Setting these as defaults, but they can be changed.
            PageRole = modPerm.Application + modPerm.Module;
        }

        #endregion

        #region Private Methods

        private static IPermission GetPermission(IPermissionsObject permObj)
        {
            var roleName = string.Format("{0}_{1}_{2}", permObj.SpecificPermissions.Application,
                permObj.SpecificPermissions.Module,
                permObj.Action);
            var p = new Permission(roleName);
            p.Allow = permObj.InAny();
            return p;
        }

        #endregion
    }
}
