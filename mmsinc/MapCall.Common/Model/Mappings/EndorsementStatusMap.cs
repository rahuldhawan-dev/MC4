using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EndorsementStatusMap : ClassMap<EndorsementStatus>
    {
        #region Constants

        public const string TABLE_NAME = "EndorsementStatuses";

        #endregion

        #region Constructors

        public EndorsementStatusMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity().Not.Nullable();
            Map(x => x.Description);
        }

        #endregion
    }
}
