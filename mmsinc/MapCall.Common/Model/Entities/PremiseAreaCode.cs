﻿using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PremiseAreaCode : EntityLookup, ISAPLookup
    {
        public virtual string SAPCode { get; set; }
    }
}
