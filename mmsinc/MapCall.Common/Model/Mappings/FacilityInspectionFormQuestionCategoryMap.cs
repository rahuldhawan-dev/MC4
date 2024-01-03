using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class FacilityInspectionFormQuestionCategoryMap : EntityLookupMap<FacilityInspectionFormQuestionCategory>
    {
        public FacilityInspectionFormQuestionCategoryMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
