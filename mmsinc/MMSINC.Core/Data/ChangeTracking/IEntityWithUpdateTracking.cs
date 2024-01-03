using System;
using MMSINC.Authentication;

namespace MMSINC.Data.ChangeTracking
{
    /// <summary>
    /// <see cref="IEntity"/> which records the <see cref="DateTime"/> at which an entity was most recently
    /// updated, as well as the <typeparamref name="TUser"/> who updated it.
    /// </summary>
    public interface IEntityWithUpdateTracking<TUser>
        : IEntityWithUpdateTimeTracking, IEntityWithUpdateUserTracking<TUser>
        where TUser : IAdministratedUser { }
}