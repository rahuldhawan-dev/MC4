using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class NotificationPurposeMap : ClassMap<NotificationPurpose>
    {
        #region Constructors

        public NotificationPurposeMap()
        {
            Id(x => x.Id);
            Map(x => x.Purpose);
            References(x => x.Module);
        }

        #endregion
    }
}
