using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class ScadaTagNameRepository : RepositoryBase<ScadaTagName>, IScadaTagNameRepository
    {
        public ScadaTagNameRepository(ISession session, IContainer container) : base(session, container) { }

        public IQueryable<ScadaTagNameDisplayItem> FindByPartialNameMatch(string partialName)
        {
            if (string.IsNullOrWhiteSpace(partialName) || partialName.Length < 2)
            {
                return Enumerable.Empty<ScadaTagNameDisplayItem>().AsQueryable();
            }

            var where = Linq.Where(_ => true);
            var split = partialName.Split(' ');
            foreach (var item in split)
            {
                where = where.Where(e =>
                    e.TagName.Contains(item) ||
                    e.Description.Contains(item));
            }

            return (IQueryable<ScadaTagNameDisplayItem>)where.SelectDynamic<ScadaTagName, ScadaTagNameDisplayItem>()
                                                             .Result;
        }
    }

    public interface IScadaTagNameRepository : IRepository<ScadaTagName>
    {
        IQueryable<ScadaTagNameDisplayItem> FindByPartialNameMatch(string partialName);
    }
}
