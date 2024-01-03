using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IMainCrossingRepository : IRepository<MainCrossing>
    {
        IQueryable<MainCrossing> GetByTownIdForWorkOrders(int townId);
        IEnumerable<MainCrossing> GetByOperatingCenter(int opcId);
        IQueryable<MainCrossing> GetByOperatingCenterForSelect(int opcId);
        IEnumerable<AssetCoordinate> GetAssetCoordinates(IAssetCoordinateSearch search);
        IQueryable<MainCrossing> GetByOperatingCentersForSelect(int[] ids);
    }

    public class MainCrossingRepository : RepositoryBase<MainCrossing>, IMainCrossingRepository
    {
        #region Properties

        public override ICriteria Criteria
        {
            get
            {
                // Speed up map pages so it isn't making a million selects for the coordinates.
                return base.Criteria.SetFetchMode("Coordinate", FetchMode.Eager);
            }
        }

        #endregion;

        #region Public Methods

        public IQueryable<MainCrossing> GetByTownIdForWorkOrders(int townId)
        {
            return (from mc in Linq
                    where mc.Town != null
                          && mc.Town.Id == townId
                    orderby mc.Town.ShortName
                    select mc);
        }

        public IEnumerable<MainCrossing> GetByOperatingCenter(int opcId)
        {
            return Linq.Where(x => x.OperatingCenter.Id == opcId);
        }

        public IQueryable<MainCrossing> GetByOperatingCenterForSelect(int opcId)
        {
            return (from mc in Linq
                    where mc.OperatingCenter.Id == opcId
                    select mc);
        }

        public IEnumerable<AssetCoordinate> GetAssetCoordinates(IAssetCoordinateSearch search)
        {
            // NOTE: NO ROLE FILTERING IS DONE FOR THIS QUERY. Alex said so. -Ross 4/15/2015
            // NOTE 2: If you fix something in here then the same change most likely needs to be made in ValveRepository.
            //         And also in Hydrant.ToAssetCoordinate.

            search.EnablePaging = false;
            var query = Session.QueryOver<MainCrossing>();

            Coordinate coord = null;
            // We don't want null coordinates included, so inner join
            query.JoinAlias(x => x.Coordinate, () => coord, JoinType.InnerJoin);
            OperatingCenter opc = null;
            query.JoinAlias(x => x.OperatingCenter, () => opc);

            query.Where(() => coord.Latitude >= search.LatitudeMin && coord.Latitude <= search.LatitudeMax)
                 .Where(() => coord.Longitude >= search.LongitudeMin && coord.Longitude <= search.LongitudeMax);

            HydrantAssetCoordinate result = null;
            query.SelectList(x => x.Select(h => h.Id).WithAlias(() => result.Id)
                                   .Select(() => coord.Latitude).WithAlias(() => result.Latitude)
                                   .Select(() => coord.Longitude).WithAlias(() => result.Longitude)
                                   .Select(h => h.RequiresInspection).WithAlias(() => result.RequiresInspection)
                                   .Select(() => true).WithAlias(() => result.IsActive)
                                   .Select(() => true).WithAlias(() => result.IsPublic));

            // This query need an order by statement or else it will return the results in 
            // a random order for some reason. This is to ensure the data's always returned
            // in the exact same order so it can be displayed in the same order. 
            query.OrderBy(x => x.Id).Asc();

            query.TransformUsing(Transformers.AliasToBean<MainCrossingAssetCoordinate>());
            return Search(search, query);
        }

        public IQueryable<MainCrossing> GetByOperatingCentersForSelect(int[] ids)
        {
            return (from mc in Linq
                    where ids.Contains(mc.OperatingCenter.Id)
                    select mc);
        }

        #endregion

        #region Constructors

        public MainCrossingRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion
    }
}
