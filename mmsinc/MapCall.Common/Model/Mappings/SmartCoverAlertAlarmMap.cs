using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SmartCoverAlertAlarmMap : ClassMap<SmartCoverAlertAlarm>
    {
        public SmartCoverAlertAlarmMap()
        {
            Id(x => x.Id);

            DynamicUpdate();

            References(x => x.SmartCoverAlert).Column("SmartCoverAlertId").Not.Nullable();
            References(x => x.AlarmType).Not.Nullable();

            Map(x => x.AlarmId).Not.Nullable();
            Map(x => x.Value).Not.Nullable();
            Map(x => x.AlarmDate).Not.Nullable();
            Map(x => x.Level).Not.Nullable();
        }
    }
}
