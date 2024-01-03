using MMSINC.Authentication;
using NHibernate;

namespace MMSINC.Data.NHibernate
{
    /// <summary>
    /// NHibernate <see cref="IInterceptor"/> for setting change tracking fields (CreatedAt/By and
    /// UpdatedAt/By) on entities.
    /// </summary>
    /// <typeparam name="TUser">Type of user to use as creator/updater of records.</typeparam>
    public interface IChangeTrackingInterceptor<TUser> : IInterceptor
        where TUser : class, IAdministratedUser { }
}
