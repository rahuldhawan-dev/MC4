using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IEmployeeStatusRepository : IRepository<EmployeeStatus> { }

    public static class EmployeeStatusRepositoryExtensions
    {
        public static EmployeeStatus GetActiveStatus(this IRepository<EmployeeStatus> that)
        {
            return that.Find(EmployeeStatus.Indices.ACTIVE);
        }

        public static EmployeeStatus GetInactiveStatus(this IRepository<EmployeeStatus> that)
        {
            return that.Find(EmployeeStatus.Indices.INACTIVE);
        }

        public static EmployeeStatus GetStatusByDescription(this IRepository<EmployeeStatus> that, string desc)
        {
            return that.Where(x => x.Description == desc).Single();
        }
    }

    public class EmployeeStatusRepository : RepositoryBase<EmployeeStatus>, IEmployeeStatusRepository
    {
        #region Constructor

        public EmployeeStatusRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion
    }
}
