using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class LockoutFormQuestionMap : ClassMap<LockoutFormQuestion>
    {
        #region Constructors

        public LockoutFormQuestionMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.Category).Not.Nullable();

            Map(x => x.Question).Length(int.MaxValue).Not.Nullable();
            Map(x => x.IsActive).Not.Nullable();
            Map(x => x.DisplayOrder).Not.Nullable();
        }

        #endregion
    }
}
