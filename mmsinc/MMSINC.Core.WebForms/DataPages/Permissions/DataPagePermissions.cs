using System;

namespace MMSINC.DataPages.Permissions
{
    public interface IDataPagePermissions
    {
        #region Properties

        string PermissionName { get; }
        IPermission AdminAccess { get; } // was IsAdmin
        IPermission CreateAccess { get; } // was CanAddRecords
        IPermission DeleteAccess { get; } // was CanDeleteRecords
        IPermission EditAccess { get; } // was CanEditRecords
        IPermission PageAccess { get; } // was IsAuthorizedToAccessPage
        IPermission ReadAccess { get; }

        #endregion
    }

    /// <summary>
    /// This class is for replacing the redundant code checks for page access/adding/editing/deleting. Makes it easy to
    /// pass the same permissions instance to page's child controls that may want it too at some point.
    /// 
    /// use RoleBasedDataPagePermission for roles stuff.
    /// </summary>
    public class DataPagePermissions : IDataPagePermissions
    {
        // Everything in this class could be made virtual if need be.

        #region Properties

        public string PermissionName { get; set; }
        public IPermission AdminAccess { get; set; }
        public IPermission CreateAccess { get; set; }
        public IPermission DeleteAccess { get; set; }
        public IPermission EditAccess { get; set; }
        public IPermission PageAccess { get; set; }
        public IPermission ReadAccess { get; set; }

        #endregion

        #region Constructors

        public DataPagePermissions(string permissionName)
        {
            if (string.IsNullOrWhiteSpace(permissionName))
            {
                throw new NullReferenceException("permissionName");
            }

            this.PermissionName = permissionName.Trim();
            AdminAccess = CreatePermission("Admin");
            CreateAccess = CreatePermission("Create");
            DeleteAccess = CreatePermission("Delete");
            EditAccess = CreatePermission("Edit");
            PageAccess = CreatePermission("Page");
            ReadAccess = CreatePermission("Read");
        }

        #endregion

        #region Private Methods

        private Permission CreatePermission(string name)
        {
            return new Permission(PermissionName + ": " + name);
        }

        #endregion

        #region IDataPagePermission implementation

        IPermission IDataPagePermissions.AdminAccess
        {
            get { return AdminAccess; }
        }

        IPermission IDataPagePermissions.CreateAccess
        {
            get { return CreateAccess; }
        }

        IPermission IDataPagePermissions.DeleteAccess
        {
            get { return DeleteAccess; }
        }

        IPermission IDataPagePermissions.EditAccess
        {
            get { return EditAccess; }
        }

        IPermission IDataPagePermissions.PageAccess
        {
            get { return PageAccess; }
        }

        IPermission IDataPagePermissions.ReadAccess
        {
            get { return ReadAccess; }
        }

        #endregion
    }
}
