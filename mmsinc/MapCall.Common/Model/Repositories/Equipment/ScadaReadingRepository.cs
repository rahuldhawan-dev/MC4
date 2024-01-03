using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class ScadaReadingRepository : RepositoryBase<ScadaReading>, IScadaReadingRepository
    {
        public const int RECENT_COUNT = 300;

        public ScadaReadingRepository(ISession session, IContainer container) : base(session, container) { }

        public IEnumerable<ScadaReading> GetRecentReadings(int tagNameId)
        {
            var qry = (from r in Linq where r.TagName.Id == tagNameId orderby r.Timestamp select r);
            return qry.Skip(qry.Count() - RECENT_COUNT).Take(RECENT_COUNT);
        }
    }

    public interface IScadaReadingRepository : IRepository<ScadaReading>
    {
        IEnumerable<ScadaReading> GetRecentReadings(int tagNameId);
    }
}
