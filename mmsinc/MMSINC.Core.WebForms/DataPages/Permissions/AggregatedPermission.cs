using System;
using System.Collections.Generic;
using System.Linq;

namespace MMSINC.DataPages.Permissions
{
    /// <summary>
    /// Represents an IPermission that gets its values from a collection of IPermission objects.
    /// </summary>
    public interface IAggregatedPermission : IPermission
    {
        IEnumerable<IPermission> Permissions { get; }
    }

    // TODO: This will someday need a way to require all permissions probably.

    /// <summary>
    /// Implementation of IAggregatedPermission. This class is used for combining multiple IPermission
    /// instances together. 
    /// </summary>
    public class AggregatedPermission : IAggregatedPermission
    {
        // This class "freezes" the first time IsAllowed/IsDenied is called.
        // Permissions can be added/removed but it won't have any effect.
        // Dunno if we should throw an exception there or not. 

        // TODO: Test that duplicates can't be added. 

        #region Fields

        // Using a hashset to prevent duplicate permissions.
        private readonly ICollection<IPermission> _permissions = new HashSet<IPermission>();

        // Do not use the field accessor. Call the property accessor. 
        private IPermission _frozenPermission;

        #endregion

        #region Properties

        public string Name
        {
            get { return InternalPermission.Name; }
        }

        /// <summary>
        /// Returns true if any of the child permissions have IsAllowed = true 
        /// and none of them have IsDenied = true. 
        /// </summary>
        public bool IsAllowed
        {
            get { return InternalPermission.IsAllowed; }
        }

        public bool IsDenied
        {
            get { return InternalPermission.IsDenied; }
        }

        public IEnumerable<IPermission> Permissions
        {
            get { return _permissions; }
        }

        private IPermission InternalPermission
        {
            get
            {
                if (_frozenPermission == null)
                {
                    _frozenPermission = CreateFrozenInternalPermission();
                }

                return _frozenPermission;
            }
        }

        #endregion

        #region Constructors

        public AggregatedPermission() { }

        public AggregatedPermission(IEnumerable<IPermission> perms)
        {
            foreach (var p in perms)
            {
                AddPermission(p);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Creates an IPermission instance that caches the collective value of all the child permissions.
        /// </summary>
        /// <returns></returns>
        internal protected virtual IPermission CreateFrozenInternalPermission()
        {
            if (!Permissions.Any())
            {
                throw new InvalidOperationException(
                    "AggregatedPermission instances must include atleast one IPermission instance in its Permissions collection");
            }

            // Not sure what to do here so right now we can have it
            // join all the child permission names together I guess. 
            // This property's not of much use outside of throwing
            // errors and debugging anyway. 
            var names = (from p in Permissions select p.Name);
            var name = string.Join(", ", names);

            var allow = (from p in Permissions where p.IsAllowed select p).Any();
            var deny = (from p in Permissions where p.IsDenied select p).Any();

            var perm = new Permission(name);
            perm.Allow = (allow && !deny);
            perm.Deny = deny;

            return perm;
        }

        #endregion

        #region Public Methods

        public void AddPermission(IPermission permission)
        {
            if (permission == null)
            {
                throw new ArgumentNullException("permission");
            }

            if (permission == this)
            {
                throw new InvalidOperationException(
                    "Can not add self to own permissions list.");
            }

            _permissions.Add(permission);
        }

        public void Demand()
        {
            InternalPermission.Demand();
        }

        #endregion
    }
}
