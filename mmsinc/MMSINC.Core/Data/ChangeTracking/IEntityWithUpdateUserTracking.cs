using MMSINC.Authentication;

namespace MMSINC.Data.ChangeTracking
{
    /// <summary>
    /// <see cref="IEntity"/> which records the <typeparamref name="TUser"/> who most recently updated an
    /// entity. 
    /// </summary>
    public interface IEntityWithUpdateUserTracking<TUser> : IEntity
        where TUser : IAdministratedUser
    {
        TUser UpdatedBy { get; set; }
    }
}