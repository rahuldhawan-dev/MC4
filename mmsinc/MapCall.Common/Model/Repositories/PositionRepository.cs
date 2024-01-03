using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class PositionRepository : RepositoryBase<Position>, IPositionRepository
    {
        public PositionRepository(ISession session, IContainer container) : base(session, container) { }

        public IEnumerable<Position> GetDistinctCategories()
        {
            return Linq.GroupBy(p => p.Category).Select(p => new Position {Category = p.Key});
        }

        public IEnumerable<Position> GetDistinctPositions()
        {
            return (from p in Linq
                    orderby p.OpCode
                    group p by new {p.OpCode, p.PositionDescription, p.Id}
                    into grp
                    select
                        new Position(grp.Key.Id) {
                            OpCode = grp.Key.OpCode,
                            PositionDescription = grp.Key.PositionDescription
                        });
        }
    }

    public interface IPositionRepository : IRepository<Position>
    {
        IEnumerable<Position> GetDistinctCategories();
        IEnumerable<Position> GetDistinctPositions();
    }
}
