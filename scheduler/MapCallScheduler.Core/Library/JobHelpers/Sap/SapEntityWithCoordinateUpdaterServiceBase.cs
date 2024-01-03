using System;
using log4net;
using MapCall.Common.Model.Entities;
using MapCallScheduler.Library.Common;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.Library.JobHelpers.Sap 
{
    public abstract class SapEntityWithCoordinateUpdaterServiceBase<TFileRecord, TParser, TEntity, TRepository> : SapEntityUpdaterServiceBase<TFileRecord, TParser, TEntity, TRepository>
        where TFileRecord : new()
        where TParser : IFileParser<TFileRecord>
        where TRepository : IRepository<TEntity>
    {
        private readonly IRepository<Coordinate> _coordinateRepository;

        protected SapEntityWithCoordinateUpdaterServiceBase(TParser parser, TRepository repository, ILog log, IRepository<Coordinate> coordinateRepository) : base(parser, repository, log)
        {
            _coordinateRepository = coordinateRepository;
        }

        internal Coordinate GetCoordinate(string latitude, string longitude)
        {
            decimal lat, lng;

            return string.IsNullOrWhiteSpace(latitude) || string.IsNullOrWhiteSpace(longitude) ||
                   !decimal.TryParse(latitude, out lat) || !decimal.TryParse(longitude, out lng) ||
                   Math.Abs(lat) > 180 || Math.Abs(lng) > 180
                ? null
                : _coordinateRepository.Save(new Coordinate
                {
                    Latitude = lat,
                    Longitude = lng
                });
        }
    }
}
