using MMSINC.Authentication;

namespace MMSINC.Data.ChangeTracking
{
    /// <summary>
    /// <see cref="IEntity"/> which records the <typeparamref name="TUser"/> who created the entity when it
    /// was first persisted.
    /// </summary>
    public interface IEntityWithCreationUserTracking<TUser> : IEntity
        where TUser : IAdministratedUser
    {
        TUser CreatedBy { get; set; }
    }
}