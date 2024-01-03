using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Repositories
{
    public static class BusinessUnitRepositoryExtensions
    {
        public static IQueryable<BusinessUnit> FindByOperatingCenterForTDWorkOrders(this IRepository<BusinessUnit> repo, int operatingCenterId)
        {
            return repo.Where(bu => bu.Is271Visible
                                    && bu.OperatingCenter.Id == operatingCenterId
                                    && bu.Department.Id == Department.Indices.T_AND_D);
        }
    }
}
