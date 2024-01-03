using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class EmployeeSpokeWithNurseMap : EntityLookupMap<EmployeeSpokeWithNurse>
    {
        public EmployeeSpokeWithNurseMap()
        {
            Table("EmployeeSpokeWithNurse");
        }
    }
}
