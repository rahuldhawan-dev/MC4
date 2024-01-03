using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using StructureMap;

namespace MapCall.Common.Configuration
{
    public class MapResultFactory : IMapResultFactory
    {
        #region Private Members

        protected readonly IContainer _container;

        #endregion

        #region Constructors

        public MapResultFactory(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Exposed Methods

        public MapResult Build<TEntity, TRepository, TSearchSet>(
            ModelStateDictionary modelState,
            TRepository repo,
            TSearchSet search)
            where TEntity : IThingWithCoordinate
            where TRepository : IRepository<TEntity>
            where TSearchSet : ISearchSet<TEntity>
        {
            return Build(
                modelState,
                () => {
                    search.EnablePaging = false;
                    return (IEnumerable<IThingWithCoordinate>)repo.Search(search);
                });
        }

        public MapResult Build<TEntity, TSearchSet>(
            ModelStateDictionary modelState,
            IRepository<TEntity> repo,
            TSearchSet search)
            where TEntity : IThingWithCoordinate
            where TSearchSet : ISearchSet<TEntity>
        {
            return Build<TEntity, IRepository<TEntity>, TSearchSet>(modelState, repo, search);
        }

        public MapResult Build(
            ModelStateDictionary modelState,
            Func<IEnumerable<IThingWithCoordinate>> searchFn)
        {
            var mapResult = _container.GetInstance<MapResult>();

            if (!modelState.IsValid)
            {
                return mapResult;
            }

            mapResult.CreateCoordinateSet(searchFn());

            return mapResult;
        }

        #endregion
    }
}
