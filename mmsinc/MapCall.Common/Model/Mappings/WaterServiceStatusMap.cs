﻿using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class WaterServiceStatusMap : EntityLookupMap<WaterServiceStatus>
    {
        public WaterServiceStatusMap()
        {
            Table("WaterServiceStatuses");
        }
    }
}