using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class FacilityInspectionFormQuestionMap : ClassMap<FacilityInspectionFormQuestion>
    {
        #region Constructors

        public FacilityInspectionFormQuestionMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.Category).Not.Nullable();
            Map(x => x.Question).Length(int.MaxValue).Not.Nullable();
            Map(x => x.Weightage).Not.Nullable();
            Map(x => x.DisplayOrder).Not.Nullable();
        }

        #endregion
    }
}
