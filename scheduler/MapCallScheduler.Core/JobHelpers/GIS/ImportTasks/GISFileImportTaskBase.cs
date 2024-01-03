using System;
using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MapCallScheduler.Library.JobHelpers.FileImports;
using MMSINC.Data;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.JobHelpers.GIS.ImportTasks
{
    public abstract class GISFileImportTaskBase<TEntity, TRepository>
        : FileImportTaskBase<
            IGISFileDownloadService,
            IGISFileParser,
            GISFileParser.ParsedRecord,
            TEntity,
            TRepository>
        where TRepository : IRepository<TEntity>
        where TEntity : class, IEntity, IThingWithCoordinate, new()
    {
        #region Constructors

        protected GISFileImportTaskBase(IGISFileDownloadService downloadService, IGISFileParser parser, TRepository repository, ILog log) : base(downloadService, parser, repository, log) { }

        #endregion

        #region Private Methods

        protected override void MapFields(TEntity entity, GISFileParser.ParsedRecord update)
        {
            if (entity.Coordinate == null)
            {
                entity.Coordinate = new Coordinate { Latitude = update.Latitude, Longitude = update.Longitude };
            }
            else
            {
                entity.Coordinate.Latitude = update.Latitude;
                entity.Coordinate.Longitude = update.Longitude;
            }
        }

        protected override void Save(TEntity entity)
        {
            _repository.Update(entity);
        }

        protected override TEntity Find(GISFileParser.ParsedRecord record)
        {
            var entity = _repository.Find(record.Id);

            if (entity == null)
            {
                throw new InvalidOperationException($"Could not find {typeof(TEntity).Name} with id {record.Id}.");
            }

            return entity;
        }

        #endregion
    }

    public abstract class GISFileImportTaskBase<TEntity>
        : GISFileImportTaskBase<TEntity, IRepository<TEntity>>
        where TEntity : class, IEntity, IThingWithCoordinate, new()
    {
        #region Constructors

        protected GISFileImportTaskBase(IGISFileDownloadService downloadService, IGISFileParser parser, IRepository<TEntity> repository, ILog log) : base(downloadService, parser, repository, log) { }

        #endregion
    }
}
