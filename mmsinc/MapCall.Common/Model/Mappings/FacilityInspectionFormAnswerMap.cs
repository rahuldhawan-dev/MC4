using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class FacilityInspectionFormAnswerMap : ClassMap<FacilityInspectionFormAnswer>
    {
        #region Constructors

        public FacilityInspectionFormAnswerMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.ApcInspectionItem).Not.Nullable();
            References(x => x.FacilityInspectionFormQuestion).Not.Nullable();
            Map(x => x.IsSafe).Nullable();
            Map(x => x.IsPictureTaken).Nullable();
            Map(x => x.Comments).Length(int.MaxValue).Nullable();
        }

        #endregion
    }
}
