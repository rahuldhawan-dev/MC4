using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class StateRepository : RepositoryBase<State>, IStateRepository
    {
        #region Constructors

        public StateRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Exposed Methods

        public IEnumerable<State> GetWithFacilities()
        {
            return (from s in Linq where s.Towns.Any(t => t.Facilities.Any()) select s);
        }

        public State FindByAbbreviation(string abbreviation)
        {
            return Linq.SingleOrDefault(x => x.Abbreviation == abbreviation);
        }

        #endregion
    }

    public interface IStateRepository : IRepository<State>
    {
        #region Abstract Methods

        IEnumerable<State> GetWithFacilities();

        State FindByAbbreviation(string abbreviation);

        #endregion
    }
}
