using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SewerMainInspectionGradeMap : EntityLookupMap<SewerMainInspectionGrade>
    {
        public SewerMainInspectionGradeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            ReadOnly();
        }
    }
}
