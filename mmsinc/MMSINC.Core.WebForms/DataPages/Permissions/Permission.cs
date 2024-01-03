using System;
using System.Security;

namespace MMSINC.DataPages.Permissions
{
    // This is for a really basic permissions system. It shouldn't
    // require anymore than this. 

    public interface IPermission
    {
        string Name { get; }

        bool IsAllowed { get; }
        bool IsDenied { get; }

        /// <summary>
        /// Call this method when you a permission is required to be allowed. 
        /// An exception will be thrown if the user does not have access. 
        /// </summary>
        void Demand();
    }

    /// <summary>
    /// Basic Permission class.
    /// </summary>
    /// <remarks>
    /// 
    /// Permissions can exist in three states:
    /// 
    /// Allowed: [IsAllowed = true : IsDenied = false]
    ///     Permission is allowed.
    /// 
    /// NotAllowed: [IsAllowed = false : IsDenied = false]
    ///     Permission is not allowed.
    /// 
    /// Denied: [IsAllowed = false : IsDenied = true]
    ///     Permission is not allowed and overrides any Allowed 
    ///     permission when used in combination with more than
    ///     one permission.
    /// 
    /// </remarks>
    public class Permission : IPermission
    {
        #region Properties

        public string Name { get; private set; }

        public bool IsAllowed
        {
            get
            {
                Verify();
                return (Allow && !IsDenied);
            }
        }

        public bool IsDenied
        {
            get
            {
                Verify();
                return Deny;
            }
        }

        // These are here so we can explicitly set each value without any magic.
        public bool Allow { get; set; }
        public bool Deny { get; set; }

        #endregion

        #region Constructors

        public Permission(string permissionName)
        {
            if (string.IsNullOrWhiteSpace(permissionName))
            {
                throw new ArgumentNullException("permissionName");
            }

            Name = permissionName;
        }

        #endregion

        #region Private Methods

        private void Verify()
        {
            if (Allow && Deny)
            {
                throw new InvalidOperationException("A permission can not have Allow and Deny set to true.");
            }
        }

        #endregion

        #region Public Methods

        public void Demand()
        {
            Verify();
            if (!IsAllowed)
            {
                throw new SecurityException("No access");
            }
        }

        #endregion
    }
}
