using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ContractorMeterCrew : ReadOnlyEntityLookup
    {
        #region Properties

        public virtual Contractor Contractor { get; set; }
        public virtual int AMMeters { get; set; }
        public virtual int PMMeters { get; set; }
        public virtual int AMLargeMeters { get; set; }
        public virtual int PMLargeMeters { get; set; }
        public virtual bool IsActive { get; set; }

        #endregion
    }
}
