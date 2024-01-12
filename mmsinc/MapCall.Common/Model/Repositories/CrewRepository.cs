using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Impl;
using NHibernate.Loader.Criteria;
using NHibernate.Persister.Entity;
using NHibernate.Transform;
using StructureMap;
using System.Collections.Generic;

namespace MapCall.Common.Model.Repositories
{
    public class CrewRepository : OperatingCenterSecuredRepositoryBase<Crew>, ICrewRepository
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        #endregion

        public CrewRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }

        public override RoleModules Role => ROLE;

        public IEnumerable<CrewWorkOrderDetails> GetCrewWorkOrders(ISearchCrewForWorkOrders search)
        {
            #region alias
            
            Crew c = null;
            CrewAssignment ca = null;
            WorkOrder wo = null; 
            Street s = null; 
            WorkDescription wd = null; 
            Town t = null; 
            TownSection ts = null; 
            CrewWorkOrderDetails resultItem = null;
            
            #endregion

            var query = Session.QueryOver(() => c)
                               .Where(() => c.Id == search.Id) // Add the condition here
                               .JoinAlias(() => c.CrewAssignments, () => ca)
                               .JoinAlias(() => ca.WorkOrder, () => wo)
                               .JoinAlias(() => wo.Street, () => s)
                               .JoinAlias(() => wo.WorkDescription, () => wd)
                               .JoinAlias(() => wo.Town, () => t)
                               .JoinAlias(() => wo.TownSection, () => ts);
            
            var sqlQuery1 = GetGeneratedSql(query.UnderlyingCriteria);
            
            var queryResult = Session.QueryOver(() => c)
                                     .Where(() => c.Id == search.Id) // Add the condition here
                                     .JoinAlias(() => c.CrewAssignments, () => ca)
                                     .JoinAlias(() => ca.WorkOrder, () => wo)
                                     .JoinAlias(() => wo.Street, () => s)
                                     .JoinAlias(() => wo.WorkDescription, () => wd)
                                     .JoinAlias(() => wo.Town, () => t)
                                     .JoinAlias(() => wo.TownSection, () => ts)
                                     .SelectList(list => list
                                                        .Select(Projections.Property(() => wo.Id)).WithAlias(() => resultItem.Id)
                                                        .Select(Projections.Property(() => wo.DateReceived)).WithAlias(() => resultItem.DateReceived)
                                                        .Select(Projections.Property(() => wo.StreetNumber)).WithAlias(() => resultItem.StreetNumber)
                                                        .Select(Projections.Property(() => s.Name)).WithAlias(() => resultItem.Street)
                                                        .Select(Projections.Property(() => t.FullName)).WithAlias(() => resultItem.Town)
                                                        .Select(Projections.Property(() => s.FullStName)).WithAlias(() => resultItem.NearestCrossStreet)
                                                        .Select(Projections.Property(() => ts.Name)).WithAlias(() => resultItem.TownSection)
                                                        .Select(Projections.Property(() => wd.Description)).WithAlias(() => resultItem.WorkDescription)
                                                        .Select(Projections.Property(() => wd.MarkoutRequired)).WithAlias(() => resultItem.MarkoutRequired)
                                      )
                                     .TransformUsing(Transformers.AliasToBean<CrewWorkOrderDetails>())
                                     .List<CrewWorkOrderDetails>();

            return queryResult;
        }
        public string GetGeneratedSql(ICriteria criteria)
        {
            var criteriaImpl = (CriteriaImpl)criteria;
            var sessionImpl = (SessionImpl)criteriaImpl.Session;
            var factory = (SessionFactoryImpl)sessionImpl.SessionFactory;
            var implementors = factory.GetImplementors(criteriaImpl.EntityOrClassName);
            var loader = new CriteriaLoader((IOuterJoinLoadable)factory.GetEntityPersister(implementors[0]), factory, criteriaImpl, implementors[0], sessionImpl.EnabledFilters);

            return loader.SqlString.ToString();
        }
    }


    public interface ICrewRepository : IRepository<Crew>
    {
        IEnumerable<CrewWorkOrderDetails> GetCrewWorkOrders(ISearchCrewForWorkOrders search);
    }
}
