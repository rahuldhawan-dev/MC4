using MMSINC.Exceptions;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using StructureMap;

namespace MMSINC.Data.NHibernate
{
    // TODO: Replace the version in WaterOutages with this
    // TODO: Move this next to the LINQ repository class
    // TODO?: Session is disposable, we should probably dispose of it.
    /// <inheritdoc />
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Constants

        internal const int DEFAULT_PAGE_SIZE = 10;

        #endregion

        #region Fields

        protected readonly IContainer _container;

        #endregion

        #region Properties

        protected ISession Session { get; private set; }

        public virtual IQueryable<TEntity> Linq
        {
            get { return Session.Query<TEntity>(); }
        }

        public virtual ICriteria Criteria
        {
            get
            {
                return
                    Session.CreateCriteria<TEntity>(
                        typeof(TEntity).Name.ToLower());
            }
        }

        //protected virtual Expression<Func<TEntity, bool>> DefaultSort { get; set; }

        #endregion

        #region Constructors

        public RepositoryBase(ISession session, IContainer container)
        {
            Session = session;
            _container = container;
        }

        #endregion

        #region Exposed Methods

        public virtual void Delete(TEntity entity)
        {
            Session.Delete(entity);
            Session.Flush();
        }

        public virtual void Update(TEntity entity)
        {
            Session.Update(entity);
            Session.Flush();
            // this was added to speed up GIS coordinate imports.  it defeats change-tracking,
            // so if you're relying on change-tracking and something is messing you up, it might
            // be this.
            Session.Clear();
        }

        public virtual TEntity Save(TEntity entity)
        {
            Session.Save(entity);
            Session.Flush();
            return entity;
        }

        public virtual void Save(IEnumerable<TEntity> entities)
        {
            // Flushing is an expensive operation. Waiting for
            // all entities to save before flushing significantly
            // reduces the time it takes to complete.
            foreach (var entity in entities)
            {
                Session.Save(entity);
            }

            Session.Flush();
        }

        public bool Exists(int id)
        {
            return Find(id) != null;
        }

        public virtual TEntity Find(int id)
        {
            // NOTE: if you're locking things down in the Linq and Criteria
            // properties, you should use SecuredRepositoryBase instead

            // Use Session.Get so we can get the cached instance
            // for the session. Using Criteria or Linq to find an
            // entity by id results in the database being queried
            // again and again.
            var ret = Session.Get<TEntity>(id);

            if (ret != null)
            {
                _container.BuildUp(ret);
            }

            return ret;
        }

        public virtual TEntity Load(int id)
        {
            return Session.Load<TEntity>(id);
        }

        public virtual Dictionary<int, TEntity> FindManyByIds(IEnumerable<int> ids)
        {
            // This method's meant to be used when you need to query for several records by id at the same time.
            // It's generally faster than calling Find in a loop since it does it as one query rather than
            // multiple.

            // This will throw an exception if you try to find more than 2100 records(nhibernate parameter limit).
            // You probably should not be doing that. However, if you need to do that then you might want to consider
            // chunking the number of ids by 2000 or something.

            var distinctIds = ids.Distinct().ToList();

            // Make sure to use Criteria so additional filtering from repository overrides get included.
            var ugh = Criteria.Add(Restrictions.In(Projections.Id(), distinctIds));
            var results = ugh.List<TEntity>();
            var metadata = Session.SessionFactory.GetClassMetadata(typeof(TEntity));

            var resultDict = results.ToDictionary(x => (int)metadata.GetIdentifier(x), x => x);

            // resultDict will not include the ids that didn't have a matching result. We want to
            // include nulls for those since the regular Find method would also return null for
            // missing records.
            foreach (var missingId in distinctIds.Except(resultDict.Keys))
            {
                resultDict.Add(missingId, null);
            }

            return resultDict;
        }

        public int GetIdentifier(TEntity entity)
        {
            // We have to use this method instead of just Session.GetIdentifier
            // because Session.GetIdentifier will throw a TransientObjectException
            // and it's really stupid and I hate it. -Ross 1/23/2012
            var metadata = Session.SessionFactory.GetClassMetadata(typeof(TEntity));
            return (int)metadata.GetIdentifier(entity);
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> p)
        {
            return Linq.Where(p);
        }

        public bool Any(Expression<Func<TEntity, bool>> p)
        {
            return Linq.Any(p);
        }

        public int GetCountForSearchSet<T>(ISearchSet<T> search)
            where T : class
        {
            var query = Session.QueryOver<T>();
            return GetCount(search, query.UnderlyingCriteria);
        }

        // This shouldn't need to be public anymore after the old search method is removed.
        public int GetCountForCriterion(ICriterion critter, IDictionary<string, string> aliases = null,
            ICriterion additionalCriterion = null)
        {
            var criteria = Criteria;
            MapAliases(critter, criteria, aliases, additionalCriterion);
            return GetCountForCriteria(criteria);
        }

        // This shouldn't need to be public anymore after the old search method is removed.
        public int GetCountForCriteria(ICriteria criteria)
        {
            // Clone the criteria passed in because otherwise there's no way to use
            // the criteria a second time(can't remove the projection).
            var clonedCritter = (ICriteria)criteria.Clone();

            // Projections.RowCount() ensures the query does a SELECT COUNT(*) rather
            // than returning all the rows and mapping them to entities.
            // HOWEVER, this projection causes all other projections to be removed, including
            // group by statements. This results in either:
            //      1) Getting the wrong count entirely.
            //      2) SQL Server throwing an error because of an order by statement trying
            //         to order by a group by column that isn't there anymore.

            // To at least get around the order by error, we can clear the order by statements
            // which is what IQueryOver.ToRowCountQuery does.
            clonedCritter.ClearOrders();
            return clonedCritter.SetProjection(Projections.RowCount()).UniqueResult<int>();
        }

        [Obsolete("Use the other Search methods when possible.")]
        public ICriteria Search(ICriterion critter, IDictionary<string, string> aliases = null,
            ICriterion additionalCriterion = null)
        {
            var criteria = Criteria;
            MapAliases(critter, criteria, aliases, additionalCriterion);
            return criteria;
        }

        public IEnumerable<TModel> Search<TModel>(ISearchSet<TModel> args)
        {
            return Search(args, (ICriteria)null);
        }

        public IEnumerable<TModel> Search<TModel>(
            ISearchSet<TModel> args,
            IQueryOver query,
            Action<ISearchMapper> searchMapperCallback = null)
        {
            return Search(args, query.UnderlyingCriteria, searchMapperCallback);
        }

        public IEnumerable<TModel> Search<TModel>(ISearchSet<TModel> args, ICriteria criteria,
            Action<ISearchMapper> searchMapperCallback = null,
            int? maxResults = null)
        {
            criteria = GenerateCriteriaForSearchSet(args, criteria, searchMapperCallback);

            if (maxResults.HasValue)
            {
                var count = ((ICriteria)criteria.Clone())
                           .SetProjection(Projections.Count(Projections.Id()))
                           .UniqueResult<int>();
                if (count > maxResults.Value)
                {
                    args.Count = count;
                    return null;
                }
            }

            var list = new List<TModel>();
            criteria.List(list);
            if (!args.EnablePaging)
            {
                args.Count = list.Count;
            }

            list.ForEach(thing => _container.BuildUp(thing));
            args.Results = list;

            // This is returned for convenience.
            return args.Results;
        }

        public int GetCount<TModel>(ISearchSet<TModel> args, ICriteria criteria)
        {
            criteria = GenerateCriteriaForSearchSet(args, criteria);
            return GetCountForCriteria(criteria);
        }

        /// <summary>
        /// Takes an ISearchSet and returns an ICriteria which can be used for searching. NOTE: This does
        /// not actually perform the search!
        /// </summary>
        /// <param name="args">
        /// <inheritdoc cref="Search{TModel}(ISearchSet{TModel})"
        ///             path="/param[@name='args']" />
        /// </param>
        /// <param name="criteria">
        /// <inheritdoc cref="Search{TModel}(ISearchSet{TModel},ICriteria,Action{ISearchMapper},int?)"
        ///             path="/param[@name='criteria']" />
        /// </param>
        /// <param name="searchMapperCallback">
        /// <inheritdoc cref="Search{TModel}(ISearchSet{TModel},IQueryOver,Action{ISearchMapper})"
        ///             path="/param[@name='searchMapperCallback']" />
        /// </param>
        protected ICriteria GenerateCriteriaForSearchSet<TModel>(
            ISearchSet<TModel> args,
            ICriteria criteria,
            Action<ISearchMapper> searchMapperCallback = null)
        {
            if (criteria == null)
            {
                criteria = Criteria;
            }

            ApplySearchMapping(args, criteria, searchMapperCallback);
            ApplySearchPaging(args, criteria);
            ApplySearchSorting(args, criteria);

            return criteria;
        }

        public IQueryable<TEntity> GetAll()
        {
            return Linq;
        }

        /// <summary>
        /// Default returns GetAll, override this in your repository
        /// to add a custom sort
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetAllSorted()
        {
            return Linq;
        }

        /// <summary>
        /// It's better to make a reusable repository instead of using this
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetAllSorted(Expression<Func<TEntity, object>> sort)
        {
            return Linq.OrderBy(sort);
        }

        public IEnumerable<TAs> GetAllAs<TAs>(Expression<Func<TEntity, TAs>> expression)
        {
            return Linq.Select(expression);
        }

        public IEnumerable<TEntity> BuildPaginatedQuery(int pageIndex,
            int pageSize, ICriterion filter, string sort = null, bool sortAsc = true)
        {
            var criteria = Criteria.Add(filter);
            return BuildPaginatedQuery(pageIndex, pageSize, criteria, sort, sortAsc);
        }

        public IEnumerable<TEntity> BuildPaginatedQuery(int pageIndex,
            int pageSize, ICriteria criteria, string sort = null, bool sortAsc = true)
        {
            if (sort != null)
            {
                var sorts = sort.Split('.');
                if (sorts.Length > 2)
                    throw new DomainLogicException("Only one level of sorting is supported.");
                if (sorts.Length == 2)
                {
                    var critter = criteria as CriteriaImpl;
                    if (critter == null ||
                        critter.IterateSubcriteria().All(x => x.GetCriteriaByAlias(sorts[0]) == null))
                        criteria.CreateCriteria(sorts[0], sorts[0], JoinType.LeftOuterJoin);
                }

                criteria.AddOrder(sortAsc
                    ? Order.Asc(Projections.Property(sort))
                    : Order.Desc(Projections.Property(sort)));
            }

            return criteria.SetFirstResult(pageIndex * pageSize).SetMaxResults(pageSize).List<TEntity>();
        }

        public void ClearSession()
        {
            Session.Clear();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns true if an ICriteria object has an alias with the given name.
        /// </summary>
        /// <param name="that"></param>
        /// <param name="associationPath"></param>
        /// <returns></returns>
        private static bool HasAliasForAssociationPath(ICriteria that, string associationPath)
        {
            // Where are we getting an ICriteria that's not an implementation that would cause this to fail?
            // I have to assume this was only done for a test somewhere that was mocking ICriteria.
            if (!(that is CriteriaImpl))
            {
                // nothing else we can do really
                return false;
            }

            return ((CriteriaImpl)that).IterateSubcriteria().Any(subCriteria => subCriteria.Path == associationPath);
        }

        protected virtual ISearchMapper CreateSearchMapper(ISearchSet args, Type entityType, ISession session)
        {
            return new SearchMapper(args, entityType, session);
        }

        protected virtual AbstractCriterion GetIdEqCriterion(int id)
        {
            return Restrictions.IdEq(id);
        }

        protected void MapAliases(ICriterion critter, ICriteria criteria, IDictionary<string, string> aliases = null,
            ICriterion additionalCriterion = null)
        {
            aliases = aliases ?? new Dictionary<string, string>();

            foreach (var alias in aliases.Where(alias => !HasAliasForAssociationPath(criteria, alias.Value)))
            {
                // Performance note: Adding aliases seems to make NHibernate automatically select every
                // field on the joined entity. This results in returning a whole lot of extra data
                // that might not be needed. -Ross 2/12/2015
                criteria.CreateAlias(alias.Value, alias.Key, JoinType.LeftOuterJoin);
            }

            criteria.Add(critter);
            if (additionalCriterion != null)
            {
                criteria.Add(additionalCriterion);
            }
        }

        /// <summary>
        /// Applies all of the search values to the criteria. This does not perform the actual search.
        /// </summary>
        /// <param name="args">
        /// <inheritdoc cref="Search{TModel}(ISearchSet{TModel})"
        ///             path="/param[@name='args']" />
        /// </param>
        /// <param name="criteria">
        /// <inheritdoc cref="Search{TModel}(ISearchSet{TModel},ICriteria,Action{ISearchMapper},int?)"
        ///             path="/param[@name='criteria']" />
        /// </param>
        /// <param name="searchMapperCallback">
        /// <inheritdoc cref="Search{TModel}(ISearchSet{TModel},IQueryOver,Action{ISearchMapper})"
        ///             path="/param[@name='searchMapperCallback']" />
        /// </param>
        protected void ApplySearchMapping<TModel>(
            ISearchSet<TModel> args,
            ICriteria criteria,
            Action<ISearchMapper> searchMapperCallback = null)
        {
            // Do search mapping
            var mapper = CreateSearchMapper(args, typeof(TEntity), Session);

            searchMapperCallback?.Invoke(mapper);

            criteria.Add(mapper.Map());

            // May need to add aliases prior to mapping.
            var aliases = mapper.GetAliases();
            foreach (var alias in aliases)
            {
                if (!HasAliasForAssociationPath(criteria, alias.Value))
                {
                    // Performance note: Adding aliases seems to make NHibernate automatically select every
                    // field on the joined entity. This results in returning a whole lot of extra data
                    // that might not be needed. If you're seeing all sorts of columns being selected and
                    // it's slowing things down, consider returning a view model with the specific columns 
                    // selected. -Ross 3/5/2015
                    criteria.CreateAlias(alias.Value, alias.Key, JoinType.LeftOuterJoin);
                }
            }
        }

        /// <summary>
        /// Applies the paging criteria to a search model.
        /// </summary>
        /// <param name="args">
        /// <inheritdoc cref="Search{TModel}(ISearchSet{TModel})"
        ///             path="/param[@name='args']" />
        /// </param>
        /// <param name="criteria">
        /// <inheritdoc cref="Search{TModel}(ISearchSet{TModel},ICriteria,Action{ISearchMapper},int?)"
        ///             path="/param[@name='criteria']" />
        /// </param>
        protected void ApplySearchPaging(ISearchSet args, ICriteria criteria)
        {
            // EnablePaging must explicitly be set to false if in order to disable it. This is because the
            // value will most likely never be set and we generally page our results by default anyway.
            if (args.EnablePaging)
            {
                // Do not do a count if EnablePaging is disabled. GetCountForCriteria can potentially
                // change the query because it calls SetProjection. This causes problems in SQL Server where
                // an order by clause does not have a matching group by clause. See HydrantRepository report
                // methods for an example.
                args.Count = GetCountForCriteria(criteria);
                args.PageNumber = Math.Max(1, args.PageNumber);
                args.PageSize = args.PageSize <= 0 ? DEFAULT_PAGE_SIZE : args.PageSize;
                args.PageCount = args.Count <= args.PageSize
                    ? 1
                    : (int)Math.Ceiling(args.Count / (decimal)args.PageSize);

                var zeroBasedIndexPageNumber = args.PageNumber - 1;
                criteria.SetFirstResult(zeroBasedIndexPageNumber * args.PageSize).SetMaxResults(args.PageSize);
            }
            else
            {
                args.PageCount = 1;
                args.PageNumber = 1;
            }
        }

        /// <summary>
        /// Applies sorting to a search criteria. This does not run the actual search.
        /// </summary>
        /// <param name="args">
        /// <inheritdoc cref="Search{TModel}(ISearchSet{TModel})"
        ///             path="/param[@name='args']" />
        /// </param>
        /// <param name="criteria">
        /// <inheritdoc cref="Search{TModel}(ISearchSet{TModel},ICriteria,Action{ISearchMapper},int?)"
        ///             path="/param[@name='criteria']" />
        /// </param>
        protected void ApplySearchSorting(ISearchSet args, ICriteria criteria)
        {
            var sortArgs = GetSortArgs(args);
            if (!string.IsNullOrWhiteSpace(sortArgs.SortBy))
            {
                var sorts = sortArgs.SortBy.Split('.');
                if (sorts.Length > 2)
                {
                    throw new DomainLogicException("Only one level of sorting is supported.");
                }

                if (sorts.Length == 2)
                {
                    // Not sure what this does.
                    var critter = criteria as CriteriaImpl;
                    if (critter == null ||
                        critter.IterateSubcriteria().All(x => x.GetCriteriaByAlias(sorts[0]) == null))
                        criteria.CreateCriteria(sorts[0], sorts[0], JoinType.LeftOuterJoin);
                }

                var sort = Projections.Property(sortArgs.SortBy);
                criteria.AddOrder(sortArgs.SortAscending ? Order.Asc(sort) : Order.Desc(sort));
            }
        }

        private static SearchSetSortArgs GetSortArgs(ISearchSet args)
        {
            if (!string.IsNullOrWhiteSpace(args.SortBy))
            {
                return new SearchSetSortArgs {
                    SortBy = args.SortBy,
                    SortAscending = args.SortAscending
                };
            }

            return new SearchSetSortArgs {
                SortBy = args.DefaultSortBy,
                SortAscending = args.DefaultSortAscending
            };
        }

        #endregion

        #region Helper classes

        private sealed class SearchSetSortArgs
        {
            public string SortBy { get; set; }
            public bool SortAscending { get; set; }
        }

        #endregion
    }

#pragma warning disable 612,618
    /// <inheritdoc />
    public interface IRepository<TEntity> : IBaseRepository<TEntity>
#pragma warning restore 612,618
    {
        #region Exposed Properties

        IQueryable<TEntity> Linq { get; }
        ICriteria Criteria { get; }

        #endregion

        #region Exposed Methods

        int GetCountForCriterion(ICriterion criterion, IDictionary<string, string> aliases = null,
            ICriterion additionalCriterion = null);

        int GetCountForCriteria(ICriteria criteria);

        /// <summary>
        /// Get the count of <typeparamref name="T"/> records which would be returned by the query
        /// represented by <paramref name="search"/>.  This will effectively run a
        /// <code>SELECT COUNT(1) FROM ...</code> query.
        /// </summary>
        int GetCountForSearchSet<T>(ISearchSet<T> search)
            where T : class;

        ICriteria Search(
            ICriterion criterion,
            IDictionary<string, string> aliases = null,
            ICriterion additionalCriterion = null);

        /// <summary>
        /// Performs a search (including all mapping, paging, and sorting) using the given ISearchSet
        /// instance as parameters. Uses the default <see cref="Criteria"/> instance for querying.
        /// </summary>
        /// <param name="args">
        /// Search set with filter properties along with paging and soring state/information, forming the
        /// basis of the query to be performed.
        /// </param>
        IEnumerable<TModel> Search<TModel>(ISearchSet<TModel> args);

        /// <inheritdoc cref="Search{TModel}(ISearchSet{TModel})" />
        /// <remarks>
        /// If <paramref name="maxResults"/> is not null, a count will be performed and compared against
        /// that value prior to the query actually running, and if it is exceeded the query will not run and
        /// null will be returned.
        /// </remarks>
        /// <param name="criteria">
        /// Optional criteria to search against, if set, rather than using the default
        /// <see cref="Criteria"/> instance.
        /// </param>
        /// <param name="searchMapperCallback">
        /// <inheritdoc cref="Search{TModel}(ISearchSet{TModel},IQueryOver,Action{ISearchMapper})"
        ///             path="/param[@name='searchMapperCallback']" />
        /// </param>
        /// <param name="maxResults">
        /// Optional max number of results to return, null means infinite.
        /// </param>
        IEnumerable<TModel> Search<TModel>(
            // ReSharper disable once InvalidXmlDocComment
            ISearchSet<TModel> args,
            ICriteria criteria,
            Action<ISearchMapper> searchMapperCallback = null,
            int? maxResults = null);

        /// <inheritdoc cref="Search{TModel}(ISearchSet{TModel})" />
        /// <param name="query">IQueryOver instance to use as the initial query.</param>
        /// <param name="searchMapperCallback">
        /// Optional callback method for modifying the ISearchMapper instance before mapping occurs.
        /// </param>
        IEnumerable<TModel> Search<TModel>(
            // ReSharper disable once InvalidXmlDocComment
            ISearchSet<TModel> args,
            IQueryOver query,
            Action<ISearchMapper> searchMapperCallback = null);

        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAllSorted();
        IQueryable<TEntity> GetAllSorted(Expression<Func<TEntity, object>> sort);

        IEnumerable<TAs> GetAllAs<TAs>(Expression<Func<TEntity, TAs>> expression);

        IEnumerable<TEntity> BuildPaginatedQuery(int pageIndex, int pageSize, ICriterion filter, string sort = null,
            bool sortAsc = true);

        IEnumerable<TEntity> BuildPaginatedQuery(int pageIndex, int pageSize, ICriteria criteria, string sort = null,
            bool sortAsc = true);

        /// <summary>
        /// Does not perform a database lookup, returns TEntity with given id.
        /// </summary>
        TEntity Load(int id);

        /// <summary>
        /// Query for instances of <typeparamref name="TEntity"/> which satisfy the predicate expression
        /// <paramref name="p"/>.  <see cref="IQueryable{TEntity}"/> result is further
        /// filterable/sortable/projectable before an actual query is performed.
        /// </summary>
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> p);

        /// <summary>
        /// Determine whether any instance of <typeparamref name="TEntity"/> satisfy the predicate
        /// expression <paramref name="p"/>.  This will effectively run a
        /// <code>SELECT COUNT(1) FROM ...</code> query and return a boolean indicating whether the returned
        /// count is equal to zero.
        /// </summary>
        bool Any(Expression<Func<TEntity, bool>> p);
        
        Dictionary<int, TEntity> FindManyByIds(IEnumerable<int> ids);

        void ClearSession();

        #endregion
    }

    /// <summary>
    /// This exists merely as a non-generic interace to simplify runtime type checking.
    /// </summary>
    [Obsolete]
    public interface IRepository : IBaseRepository { }
}
