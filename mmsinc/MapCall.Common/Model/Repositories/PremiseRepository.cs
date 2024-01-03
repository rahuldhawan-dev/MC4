using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Transform;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    /// <inheritdoc cref="IPremiseRepository" />
    public class PremiseRepository : RepositoryBase<Premise>, IPremiseRepository
    {
        #region Constructors

        public PremiseRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Exposed Methods

        public void Evict(Premise premise)
        {
            Session.Evict(premise);
        }

        public IEnumerable<IThingWithCoordinate> SearchForMap(ISearchPremiseForMap search)
        {
            PremiseCoordinate result = null;
            Coordinate coord = null;
            MapIcon icon = null;
            MapIconOffset offset = null;

            var query =
                Session
                   .QueryOver<Premise>()
                   .Inner.JoinAlias(x => x.Coordinate, () => coord)
                   .Inner.JoinAlias(x => x.Icon, () => icon)
                   .Left.JoinAlias(_ => icon.Offset, () => offset)
                   .SelectList(
                        x =>
                            x.Select(p => p.Id).WithAlias(() => result.Id)
                             .Select(_ => coord.Latitude).WithAlias(() => result.Latitude)
                             .Select(_ => coord.Longitude).WithAlias(() => result.Longitude)
                             .Select(_ => icon.Id).WithAlias(() => result.MapIconId)
                             .Select(_ => icon.FileName).WithAlias(() => result.MapIconFileName)
                             .Select(_ => icon.Height).WithAlias(() => result.MapIconHeight)
                             .Select(_ => icon.Width).WithAlias(() => result.MapIconWidth)
                             .Select(_ => offset.Id).WithAlias(() => result.MapIconOffsetId)
                             .Select(_ => offset.Description).WithAlias(
                                  () => result.MapIconOffsetDescription))
                   .TransformUsing(Transformers.AliasToBean<PremiseCoordinate>());

            return Search(search, query);
        }

        #endregion
    }

    public static class PremiseRepositoryExtensions
    {
        public static IQueryable<Premise> FindByPremiseNumber(
            this IRepository<Premise> that,
            string premiseNumber)
        {
            // This is used by BacterialWaterSamples to validate that a premise number
            // matches to an actual premise. BWS doesn't link to an actual Premise record.
            premiseNumber = premiseNumber.Trim();
            
            // need to call .Linq.Where instead of just .Where so the data can be mocked in
            return that.Linq.Where(x => x.PremiseNumber == premiseNumber);
        }

        public static IQueryable<Premise> FindActivePremiseByPremiseNumberDeviceLocationAndInstallation(
            this IRepository<Premise> that,
            string premiseNumber, string deviceLocation, string installation)
        {
            premiseNumber = premiseNumber.Trim();

            // need to call .Linq.Where instead of just .Where so the data can be mocked in
            var query = that.Linq.Where(x =>
                x.PremiseNumber == premiseNumber
                && x.StatusCode.Id == PremiseStatusCode.Indices.ACTIVE
            );
            if (!string.IsNullOrEmpty(installation))
            {
                query = query.Where(x => x.Installation == installation);
            }
            if (!string.IsNullOrEmpty(deviceLocation))
            {
                query = query.Where(x => x.DeviceLocation == deviceLocation);
            }
            return query;
        }
    }

    public interface IPremiseRepository : IRepository<Premise>
    {
        #region Abstract Methods

        void Evict(Premise premise);
        /// <summary>
        /// Perform a search of premises, mapping the results to <see cref="IThingWithCoordinate"/>
        /// instances to be displayed on a map.
        /// </summary>
        IEnumerable<IThingWithCoordinate> SearchForMap(ISearchPremiseForMap search);

        #endregion
    }
}
