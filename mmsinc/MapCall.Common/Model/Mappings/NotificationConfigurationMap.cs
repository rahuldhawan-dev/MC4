using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class NotificationConfigurationMap : ClassMap<NotificationConfiguration>
    {
        #region Contructors

        public NotificationConfigurationMap()
        {
            Id(x => x.Id);

            References(x => x.Contact).Not.Nullable();
            References(x => x.OperatingCenter).Nullable();

            HasManyToMany(x => x.NotificationPurposes)
               .Table("NotificationConfigurationsNotificationPurposes")
               .ParentKeyColumn("NotificationConfigurationId")
               .ChildKeyColumn("NotificationPurposeId");
        }

        #endregion
    }
}
