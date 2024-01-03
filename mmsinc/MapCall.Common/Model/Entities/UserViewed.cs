using System;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class UserViewed : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual User User { get; set; }
        public virtual DateTime ViewedAt { get; set; }
        public virtual TapImage TapImage { get; set; }
        public virtual AsBuiltImage AsBuiltImage { get; set; }
        public virtual ValveImage ValveImage { get; set; }

        #endregion
    }
}
