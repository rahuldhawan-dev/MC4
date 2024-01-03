using System;
using MMSINC.Authentication;

namespace MMSINC.Data.ChangeTracking
{
    /// <summary>
    /// <see cref="IEntity"/> which records the <see cref="DateTime"/> at which an entity was first created
    /// and persisted in the system, as well as the <typeparamref name="TUser"/> who created it.
    /// </summary>
    public interface IEntityWithCreationTracking<TUser>
        : IEntityWithCreationTimeTracking, IEntityWithCreationUserTracking<TUser>
        where TUser : IAdministratedUser { }
}