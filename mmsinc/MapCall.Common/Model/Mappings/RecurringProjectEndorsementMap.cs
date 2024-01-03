using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class RecurringProjectEndorsementMap : ClassMap<RecurringProjectEndorsement>
    {
        #region Constructors

        public RecurringProjectEndorsementMap()
        {
            Id(x => x.Id);

            References(x => x.RecurringProject)
               .Not.Nullable();
            References(x => x.User);
            References(x => x.EndorsementStatus)
               .Not.Nullable();

            Map(x => x.EndorsementDate).Not.Nullable();
            Map(x => x.Comment);
        }

        #endregion
    }
}
