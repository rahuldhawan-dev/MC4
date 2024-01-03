using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    // Maybe make this something like EntityContact and make it generic somehow.
    [Serializable]
    public class TownContact : IEntity, IThingWithContact
    {
        #region Properties

        public virtual int Id { get; set; }

        // These three fields should be made into a unique index or something if it's possible.
        public virtual Town Town { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual ContactType ContactType { get; set; }

        #endregion
    }
}
