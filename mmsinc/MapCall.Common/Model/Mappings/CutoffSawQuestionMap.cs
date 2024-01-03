using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class CutoffSawQuestionMap : ClassMap<CutoffSawQuestion>
    {
        #region Constructors

        public CutoffSawQuestionMap()
        {
            Id(x => x.Id, "CutoffSawQuestionID");

            Map(x => x.Question);
            Map(x => x.SortOrder);
            Map(x => x.IsActive);
        }

        #endregion
    }
}
