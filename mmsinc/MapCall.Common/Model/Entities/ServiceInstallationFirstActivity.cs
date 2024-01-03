﻿using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ServiceInstallationFirstActivity : EntityLookup
    {
        public virtual string CodeGroup { get; set; }
        public virtual string SAPCode { get; set; }
    }
}
