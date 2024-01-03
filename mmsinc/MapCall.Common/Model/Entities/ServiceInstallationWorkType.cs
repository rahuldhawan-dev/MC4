using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ServiceInstallationWorkType : EntityLookup
    {
        public virtual string CodeGroup { get; set; }
        public virtual string SAPCode { get; set; }
    }
}
