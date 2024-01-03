using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class CustomerLocationRepository : RepositoryBase<CustomerLocation>, ICustomerLocationRepository
    {
        #region Constructor

        public CustomerLocationRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        public IEnumerable<CustomerLocation> SearchWithHasVerifiedCoordinate(ISearchCustomerLocation search)
        {
            var criteria = Criteria;
            if (search.HasVerifiedCoordinate.HasValue)
            {
                var coordinateCrit = DetachedCriteria.For<CustomerCoordinate>("cc")
                                                     .SetProjection(Projections.Constant(1))
                                                     .Add(Restrictions.And(
                                                          Restrictions.EqProperty("customerlocation.Id",
                                                              "cc.CustomerLocation.Id"),
                                                          Restrictions.Eq("cc.Verified", true)));

                if (search.HasVerifiedCoordinate == true)
                {
                    criteria.Add(Subqueries.Exists(coordinateCrit));
                }
                else
                {
                    criteria.Add(Subqueries.NotExists(coordinateCrit));
                }
            }

            return Search(search, criteria);
        }

        public IEnumerable<CustomerLocation> GetDistinctStates()
        {
            return Linq.GroupBy(cl => cl.State).Select(cl => new CustomerLocation {State = cl.Key})
                       .OrderBy(cl => cl.State);
        }

        public IEnumerable<CustomerLocation> GetDistinctCitiesByState(string state)
        {
            return Linq.Where(cl => cl.State == state).GroupBy(cl => cl.City)
                       .Select(cl => new CustomerLocation {City = cl.Key}).OrderBy(cl => cl.City);
        }
    }

    public interface ICustomerLocationRepository : IRepository<CustomerLocation>
    {
        IEnumerable<CustomerLocation> GetDistinctCitiesByState(string state);
        IEnumerable<CustomerLocation> GetDistinctStates();
        IEnumerable<CustomerLocation> SearchWithHasVerifiedCoordinate(ISearchCustomerLocation search);
    }
}
