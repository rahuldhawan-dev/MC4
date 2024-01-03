using MMSINC.Authentication;

namespace MMSINC.Data.ChangeTracking
{
    /// <summary>
    /// <see cref="IEntity"/> with full creation and update tracking as specified by
    /// <see cref="IEntityWithCreationTracking{TUser}"/> and <see cref="IEntityWithUpdateTracking{TUser}"/>.
    /// </summary>
    public interface IEntityWithChangeTracking<TUser>
        : IEntityWithCreationTracking<TUser>, IEntityWithUpdateTracking<TUser>
        where TUser : IAdministratedUser { }
}