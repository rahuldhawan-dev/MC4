using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class UserViewedMap : ClassMap<UserViewed>
    {
        public UserViewedMap()
        {
            Table("UserViewed");
            Id(x => x.Id);
            Map(x => x.ViewedAt).Not.Nullable();

            References(x => x.User)
               .Not.Nullable();

            #region Images

            // NOTE: All of these are NotFound.Ignore() because we aren't deleting
            // logs of images that no longer exist in the database.

            References(x => x.TapImage, "TapID")
               .Nullable()
               .NotFound.Ignore();
            References(x => x.ValveImage, "ValveID")
               .Nullable()
               .NotFound.Ignore();
            References(x => x.AsBuiltImage, "AsBuiltID")
               .Nullable()
               .NotFound.Ignore();

            #endregion
        }
    }
}
