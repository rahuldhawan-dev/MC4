using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    public interface IThingWithSyncing : IEntity
    {
        bool NeedsToSync { get; set; }
        DateTime? LastSyncedAt { get; set; }
    }
}
