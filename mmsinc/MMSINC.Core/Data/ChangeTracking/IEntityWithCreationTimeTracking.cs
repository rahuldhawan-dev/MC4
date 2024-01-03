using System;

namespace MMSINC.Data.ChangeTracking
{
    /// <summary>
    /// <see cref="IEntity"/> which records the <see cref="DateTime"/> at which an entity was first created
    /// and persisted in the system.
    /// </summary>
    public interface IEntityWithCreationTimeTracking : IEntity
    {
        DateTime CreatedAt { get; set; }
    }
}
