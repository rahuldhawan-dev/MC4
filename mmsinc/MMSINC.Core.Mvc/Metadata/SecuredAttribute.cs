using System;
using MMSINC.Authentication;

namespace MMSINC.Metadata
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SecuredAttribute : Attribute
    {
        /// <summary>
        /// Whether or not the value should be secured for site admins as well
        /// (false means admins should be able to edit, thus not secured for them).
        ///
        /// Default is true.
        /// </summary>
        public bool AppliesToAdmins { get; set; }

        /// <summary>
        /// This should return true if the current user is allowed to edit the current value.
        /// Return false if the value must be secured instead.
        /// </summary>
        /// <param name="srv"></param>
        /// <returns></returns>
        public virtual bool UserCanEdit(IAuthenticationService<IAdministratedUser> srv)
        {
            if (!AppliesToAdmins)
            {
                return srv.CurrentUserIsAdmin;
            }
            return false;
        }

        public SecuredAttribute()
        {
            AppliesToAdmins = true;
        }
    }
}
