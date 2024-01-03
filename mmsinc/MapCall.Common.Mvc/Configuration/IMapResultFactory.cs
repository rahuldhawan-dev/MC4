using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Configuration
{
    public interface IMapResultFactory
    {
        #region Abstract Methods

        MapResult Build<TEntity, TRepository, TSearchSet>(
            ModelStateDictionary modelState,
            TRepository repo,
            TSearchSet search)
            where TEntity : IThingWithCoordinate
            where TRepository : IRepository<TEntity>
            where TSearchSet : ISearchSet<TEntity>;
        
        MapResult Build<TEntity, TSearchSet>(
            ModelStateDictionary modelState,
            IRepository<TEntity> repo,
            TSearchSet search)
            where TEntity : IThingWithCoordinate
            where TSearchSet : ISearchSet<TEntity>;

        MapResult Build(
            ModelStateDictionary modelState,
            Func<IEnumerable<IThingWithCoordinate>> searchFn);

        #endregion
    }
}
