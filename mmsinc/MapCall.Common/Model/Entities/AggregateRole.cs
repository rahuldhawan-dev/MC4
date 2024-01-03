using MapCall.Common.Model.Entities.Users;
using System;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class AggregateRole
    {
        #region Properties

        /// <summary>
        /// This will either be the UserRole.Id or
        /// a hyphenated id of the User.Id and RoleGroupRole.Id
        /// </summary>
        public virtual string CompositeId { get; set; }
        public virtual User User { get; set; }

        /// <summary>
        /// Gets the individual User Role that this AggregateRole
        /// represents if one exists. Otherwise, the role values
        /// come from the RoleGroupRole.
        /// </summary>
        public virtual Role UserRole { get; set; }

        /// <summary>
        /// Gets the RoleGroup this AggregateRole is linked to
        /// if it comes from a RoleGroupRole.
        /// </summary>
        public virtual RoleGroup RoleGroup { get; set; }

        /// <summary>
        /// Gets the RoleGroupRole that this AggregateRole
        /// represents. If this is null, then the role values come
        /// from UserRole.
        /// </summary>
        public virtual RoleGroupRole RoleGroupRole { get; set; }

        public virtual Module Module { get; set; }
        public virtual RoleAction Action { get; set; }

        /// <summary>
        /// If null, this Role represents *all* OperatingCenters.
        /// </summary>
        public virtual OperatingCenter OperatingCenter { get; set; }

        /// <summary>
        /// Returns true if this role applies to all OperatingCenters.
        /// </summary>
        public virtual bool IsValidForAnyOperatingCenter => (OperatingCenter == null);

        #endregion

        #region Constructors

        public AggregateRole() {}

        /// <summary>
        /// This constructor should not be used outside of tests.
        /// </summary>
        /// <param name="role"></param>
        public AggregateRole(Role role)
        {
            UserRole = role;
            OperatingCenter = role.OperatingCenter;
            Action = role.Action;
            Module = role.Module;
            User = role.User;
        }

        #endregion
    }
}
