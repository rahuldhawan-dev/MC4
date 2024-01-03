using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SmartCoverAlertSmartCoverAlertTypeMap : ClassMap<SmartCoverAlertSmartCoverAlertType>
    {
        public SmartCoverAlertSmartCoverAlertTypeMap()
        {
            Id(x => x.Id);

            References(x => x.SmartCoverAlert).Not.Nullable();
            References(x => x.SmartCoverAlertType).Not.Nullable();
        }
    }
}
