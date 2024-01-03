using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class EmployeeAbsenceClaimMap : EntityLookupMap<EmployeeAbsenceClaim>
    {
        public const string TABLE_NAME = "EmployeeAbsenceClaimTypes";

        public EmployeeAbsenceClaimMap()
        {
            Table(TABLE_NAME);
        }
    }
}
