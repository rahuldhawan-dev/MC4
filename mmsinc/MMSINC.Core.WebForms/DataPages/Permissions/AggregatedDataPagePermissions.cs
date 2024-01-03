using System;
using System.Collections.Generic;
using System.Linq;

namespace MMSINC.DataPages.Permissions
{
    public interface IAggregatedDataPagePermissions : IDataPagePermissions
    {
        ICollection<IDataPagePermissions> Permissions { get; }
    }

    public class AggregatedDataPagePermissions : IAggregatedDataPagePermissions
    {
        #region Fields

        private IDataPagePermissions _internalPermissions;

        #endregion

        #region Properties

        public string PermissionName
        {
            get { return InternalPermissions.PermissionName; }
        }

        public IPermission AdminAccess
        {
            get { return InternalPermissions.AdminAccess; }
        }

        public IPermission CreateAccess
        {
            get { return InternalPermissions.CreateAccess; }
        }

        public IPermission DeleteAccess
        {
            get { return InternalPermissions.DeleteAccess; }
        }

        public IPermission EditAccess
        {
            get { return InternalPermissions.EditAccess; }
        }

        public IPermission PageAccess
        {
            get { return InternalPermissions.PageAccess; }
        }

        public IPermission ReadAccess
        {
            get { return InternalPermissions.ReadAccess; }
        }

        public ICollection<IDataPagePermissions> Permissions { get; set; }

        private IDataPagePermissions InternalPermissions
        {
            get
            {
                if (_internalPermissions == null)
                {
                    _internalPermissions = CreateCachedInternalPermissions();
                }

                return _internalPermissions;
            }
        }

        #endregion

        #region Constructors

        public AggregatedDataPagePermissions()
        {
            Permissions = new List<IDataPagePermissions>();
        }

        #endregion

        #region Private Methods

        private IDataPagePermissions CreateCachedInternalPermissions()
        {
            if (!Permissions.Any())
            {
                throw new InvalidOperationException("Need permissions");
            }

            var permName = string.Join(", ",
                (from n in Permissions select n.PermissionName));

            var inter = new DataPagePermissions(permName);
            inter.CreateAccess = new AggregatedPermission((from p in Permissions select p.CreateAccess));
            inter.DeleteAccess = new AggregatedPermission((from p in Permissions select p.DeleteAccess));
            inter.EditAccess = new AggregatedPermission((from p in Permissions select p.EditAccess));
            inter.PageAccess = new AggregatedPermission((from p in Permissions select p.PageAccess));
            inter.ReadAccess = new AggregatedPermission((from p in Permissions select p.ReadAccess));
            inter.AdminAccess = new AggregatedPermission((from p in Permissions select p.AdminAccess));

            return inter;
        }

        #endregion
    }
}
