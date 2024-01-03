using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class MunicipalValveZoneRepository : RepositoryBase<MunicipalValveZone>, IMunicipalValveZoneRepository
    {
        #region Reports

        public MunicipalValveZoneRepository(ISession session, IContainer container) : base(session, container) { }

        public IEnumerable<MunicipalValveZoneReportItem> GetMunicipalValveZoneReportItems(
            ISearchSet<MunicipalValveZoneReportItem> search)
        {
            MunicipalValveZoneReportItem result = null;
            OperatingCenter opc = null;
            Town town = null;
            ValveSize sizeThingus = null;
            ValveZone smallValveZone = null;
            ValveZone largeValveZone = null;
            Valve valve = null;

            var query = Session.QueryOver<MunicipalValveZone>()
                               .JoinAlias(x => x.OperatingCenter, () => opc)
                               .JoinAlias(x => x.Town, () => town)
                               .JoinAlias(x => x.SmallValveZone, () => smallValveZone)
                               .JoinAlias(x => x.LargeValveZone, () => largeValveZone);

            query.SelectList(x => x
                                 .SelectGroup(y => opc.OperatingCenterCode).WithAlias(() => result.OperatingCenter)
                                 .SelectGroup(y => opc.Id)
                                 .SelectGroup(y => town.ShortName).WithAlias(() => result.Town)
                                 .SelectGroup(y => town.Id)
                                 .SelectGroup(y => largeValveZone.Description).WithAlias(() => result.LargeValveZone)
                                 .SelectGroup(y => largeValveZone.Id)
                                 .SelectGroup(y => smallValveZone.Description).WithAlias(() => result.SmallValveZone)
                                 .SelectGroup(y => smallValveZone.Id)
                                 .SelectSubQuery(QueryOver.Of<Valve>()
                                                          .Where(v => v.OperatingCenter.Id == opc.Id &&
                                                                      v.Town.Id == town.Id &&
                                                                      v.ValveZone.Id == largeValveZone.Id &&
                                                                      v.Status.Id == AssetStatus.Indices.ACTIVE)
                                                          .ToRowCountQuery()).WithAlias(() => result.LargeValves)
                                 .SelectSubQuery(QueryOver.Of<Valve>()
                                                          .Where(v => v.OperatingCenter.Id == opc.Id &&
                                                                      v.Town.Id == town.Id &&
                                                                      v.ValveZone.Id == smallValveZone.Id &&
                                                                      v.Status.Id == AssetStatus.Indices.ACTIVE)
                                                          .ToRowCountQuery()).WithAlias(() => result.SmallValves)
            );

            query.OrderBy(() => opc.OperatingCenterCode).Asc()
                 .OrderBy(() => town.ShortName).Asc();

            query.TransformUsing(Transformers.AliasToBean<MunicipalValveZoneReportItem>());

            return Search(search, query);
        }

        #endregion
    }

    public interface IMunicipalValveZoneRepository : IRepository<MunicipalValveZone>
    {
        IEnumerable<MunicipalValveZoneReportItem> GetMunicipalValveZoneReportItems(
            ISearchSet<MunicipalValveZoneReportItem> search);
    }
}
