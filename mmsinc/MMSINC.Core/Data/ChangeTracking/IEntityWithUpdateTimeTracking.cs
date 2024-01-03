using System;

namespace MMSINC.Data.ChangeTracking
{
    /// <summary>
    /// <see cref="IEntity"/> which records the <see cref="DateTime"/> at which an entity was most recently
    /// updated.
    /// </summary>
    public interface IEntityWithUpdateTimeTracking : IEntity
    {
        DateTime UpdatedAt { get; set; }
    }
}