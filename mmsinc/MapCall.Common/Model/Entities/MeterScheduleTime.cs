using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MeterScheduleTime : EntityLookup
    {
        #region Properties

        public virtual bool AM { get; set; }

        #endregion
    }
}
