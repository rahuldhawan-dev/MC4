﻿using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class MainBreakFlushMethodMap : EntityLookupMap<MainBreakFlushMethod>
    {
        public MainBreakFlushMethodMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Column("MainBreakFlushMethodID").Not.Nullable();
        }
    }
}
