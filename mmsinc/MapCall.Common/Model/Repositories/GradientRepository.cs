using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class GradientRepository : RepositoryBase<Gradient>, IGradientRepository
    {
        public GradientRepository(ISession session, IContainer container) : base(session, container) { }

        public IQueryable<Gradient> GetByTown(params int[] ids)
        {
            return ids.Length == 0
                ? Linq 
                : Linq.Where(g => g.Towns.Any(t => ids.Contains(t.Id)));
        }
    }

    public interface IGradientRepository : IRepository<Gradient>
    {
        IQueryable<Gradient> GetByTown(params int[] ids);
    }
}
