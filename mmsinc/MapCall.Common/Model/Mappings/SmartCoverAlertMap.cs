using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorizeNet.APICore;
using FluentNHibernate.Data;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SmartCoverAlertMap : ClassMap<SmartCoverAlert>
    {
        public SmartCoverAlertMap()
        {
            Id(x => x.Id);

            DynamicUpdate();

            References(x => x.SewerOpening).Column("SewerOpeningId").Nullable();
            References(x => x.AcknowledgedBy).Nullable();
            References(x => x.ApplicationDescription).Nullable();

            Map(x => x.AlertId).Not.Nullable();
            Map(x => x.SewerOpeningNumber).Nullable().Length(SmartCoverAlert.StringLengths.SEWER_OPENING_NUMBER);
            Map(x => x.Latitude).Nullable();
            Map(x => x.Longitude).Nullable();
            Map(x => x.Elevation).Nullable();
            Map(x => x.SensorToBottom).Nullable();
            Map(x => x.ManholeDepth).Nullable();
            Map(x => x.DateReceived).Not.Nullable();
            Map(x => x.Acknowledged).Not.Nullable();
            Map(x => x.PowerPackVoltage).Nullable().Length(SmartCoverAlert.StringLengths.POWER_PACK_VOLTAGE);
            Map(x => x.WaterLevelAboveBottom).Nullable().Length(SmartCoverAlert.StringLengths.WATER_LEVEL_ABOVE_BOTTOM);
            Map(x => x.Temperature).Nullable().Length(SmartCoverAlert.StringLengths.TEMPERATURE);
            Map(x => x.SignalStrength).Nullable().Length(SmartCoverAlert.StringLengths.SIGNAL_STRENGTH);
            Map(x => x.SignalQuality).Nullable().Length(SmartCoverAlert.StringLengths.SIGNAL_QUALITY);
            Map(x => x.AcknowledgedOn).Nullable();
            Map(x => x.HighAlarmThreshold).Nullable();
            Map(x => x.NeedsToSync).Not.Nullable();
            Map(x => x.LastSyncedAt).Nullable();

            HasMany(x => x.Notes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.WorkOrders).KeyColumn("SmartCoverAlertId");
            HasMany(x => x.SmartCoverAlertAlarms).KeyColumn("SmartCoverAlertId").Cascade.None();
            HasMany(x => x.SmartCoverAlertSmartCoverAlertTypes).KeyColumn("SmartCoverAlertId").Cascade.All();
        }
    }
}
