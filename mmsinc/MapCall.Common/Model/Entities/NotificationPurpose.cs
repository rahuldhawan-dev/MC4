using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class NotificationPurpose : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual Module Module { get; set; }
        public virtual string Purpose { get; set; }

        #endregion

        #region Exposed Methods

        public override string ToString() => Purpose;

        #endregion  
    }
}
