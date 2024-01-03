using MMSINC.Data;
using System;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ReadOnlyActionItemLink : IEntity, IActionItemLink
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual ActionItem ActionItem { get; set; }
        public virtual DataType DataType { get; set; }
        public virtual int LinkedId { get; set; }

        #endregion
    }

    [Serializable]
    public class ActionItem<T> : ReadOnlyActionItemLink, IEntity
    {
        public virtual T Entity { get; set; }
    }

    // Add all the classes here like ReadonlyNote & ReadOnlyDocument do
    [Serializable]
    public class IncidentActionItem : ReadOnlyActionItemLink, IEntity
    {
        public virtual Incident Incident { get; set; }
    }

    [Serializable]
    public class NearMissActionItem : ReadOnlyActionItemLink, IEntity
    {
        public virtual NearMiss NearMiss { get; set; }
    }

    [Serializable]
    public class GeneralLiabilityClaimActionItem : ReadOnlyActionItemLink, IEntity
    {
        public virtual GeneralLiabilityClaim GeneralLiabilityClaim { get; set; }
    }

    [Serializable]
    public class EndOfPipeExceedanceActionItem : ReadOnlyActionItemLink, IEntity
    {
        public virtual EndOfPipeExceedance EndOfPipeExceedance { get; set; }
    }
}
