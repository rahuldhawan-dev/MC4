using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class LockoutFormAnswerMap : ClassMap<LockoutFormAnswer>
    {
        #region Constructors

        public LockoutFormAnswerMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.LockoutForm).Not.Nullable();
            References(x => x.LockoutFormQuestion).Not.Nullable();

            Map(x => x.Answer).Nullable();
            Map(x => x.Comments).Length(int.MaxValue).Nullable();
        }

        #endregion
    }
}
