using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class OneCallMarkoutMessageTypeMap : EntityLookupMap<OneCallMarkoutMessageType>
    {
        public OneCallMarkoutMessageTypeMap()
        {
            Table("OneCallMarkoutMessageTypes");
        }
    }
}
