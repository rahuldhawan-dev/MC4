using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MapCallScheduler.Library.JobHelpers.Sap;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallScheduler.JobHelpers.LeakAlert
{
    public class LeakAlertUpdaterService : SapEntityWithCoordinateUpdaterServiceBase<LeakAlertFileRecord, ILeakAlertFileParser, EchoshoreLeakAlert, IRepository<EchoshoreLeakAlert>>, ILeakAlertUpdaterService
    {
        #region Private Members

        private readonly IDictionary<string, PointOfInterestStatus> _pointOfInterestStatusDictionary;
        private readonly IDictionary<string, EchoshoreSite> _echoshoreSiteDictionary;
        
        private readonly IRepository<PointOfInterestStatus> _pointOfInterestRepository;
        private readonly IRepository<EchoshoreSite> _echoshoreSiteRepository;
        private readonly IRepository<Coordinate> _coordinateRepository;
        private readonly IRepository<Hydrant> _hydrantRepository;
        
        #endregion

        #region Constructors
        public LeakAlertUpdaterService(ILeakAlertFileParser parser, IRepository<EchoshoreLeakAlert> repository, ILog log, IRepository<PointOfInterestStatus> pointOfInterestStatusRepository, IRepository<Coordinate> coordinateRepository, IRepository<EchoshoreSite> echoshoreSiteRepository, IRepository<Hydrant> hydrantRepository) : base(parser, repository, log, coordinateRepository)
        {
            _pointOfInterestRepository = pointOfInterestStatusRepository;
            _pointOfInterestStatusDictionary = new Dictionary<string, PointOfInterestStatus>();
            _echoshoreSiteRepository = echoshoreSiteRepository;
            _echoshoreSiteDictionary = new Dictionary<string, EchoshoreSite>();
            _hydrantRepository = hydrantRepository;
        }

        #endregion

        #region Private Methods

        protected override void MapRecord(EchoshoreLeakAlert entity, LeakAlertFileRecord record, int currentLine)
        {
            MapField(entity, record, x => x.Note);
            entity.DatePCNCreated = getDate(record.DatePCNCreated);
            entity.PointOfInterestStatus = GetPointOfInterestStatusMem(record.POIStatus);
            entity.Coordinate = GetCoordinate(record.Latitude, record.Longitude);
            entity.EchoshoreSite = GetEchoshoreSiteMem(record.SiteName);
            entity.Hydrant1 = GetHydrant(record.SocketID1, entity.EchoshoreSite);
            entity.Hydrant2 = GetHydrant(record.SocketID2, entity.EchoshoreSite);
            entity.Hydrant1Text = record.SocketID1;
            entity.Hydrant2Text = record.SocketID2;
            entity.PersistedCorrelatedNoiseId = int.Parse(record.PersistedCorrelatedNoiseId);
            entity.DistanceFromHydrant1 = decimal.Parse(record.DistanceFrom1);
            entity.DistanceFromHydrant2 = decimal.Parse(record.DistanceFrom2);
            entity.FieldInvestigationRecommendedOn = getDate(record.FieldInvestigationRecommendedOn);
        }

        /// <summary>
        /// Converted UTC formatted date to EST because MapCall stores all dates in EST
        /// Source Format: 2018-10-01T08:36:27Z
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private DateTime getDate(string date)
        {
            //read the utc time
            var parsedDate = DateTime.ParseExact(date, "yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture);
            //convert to est/edt
            return TimeZoneInfo.ConvertTime(parsedDate, TimeZoneInfo.Utc, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
        }

        private Hydrant GetHydrant(string hydrantNumber, EchoshoreSite site)
        {
            if (string.IsNullOrEmpty(hydrantNumber))
            {
                throw new KeyNotFoundException($"Hydrant was not provided.");
            }
            if (hydrantNumber.Length > EchoshoreLeakAlert.StringLengths.SOCKET_ID_TEXT)
            {
                throw new ConstraintException($"Hydrant Number length was greater than {EchoshoreLeakAlert.StringLengths.SOCKET_ID_TEXT}");
            }

            var hydrant = _hydrantRepository.Where(h => h.HydrantNumber == hydrantNumber
                                                        && h.OperatingCenter.Id == site.OperatingCenter.Id
                                                        && h.Status.Id == AssetStatus.Indices.ACTIVE)
                                            .Select(h => new Hydrant { Id = h.Id })
                                            .SingleOrDefault();
            return hydrant;
        }

        private EchoshoreSite GetEchoshoreSite(string recordSiteName)
        {
            if (string.IsNullOrEmpty(recordSiteName))
            {
                throw new KeyNotFoundException($"Site Name was not provided.");
            }

            var site = _echoshoreSiteRepository.Where(s => s.Description == recordSiteName)
                                               .Select(s => new EchoshoreSite {
                                                    Id = s.Id, Description = s.Description,
                                                    Town = new Town {Id = s.Town.Id},
                                                    OperatingCenter = new OperatingCenter {Id = s.OperatingCenter.Id}
                                                })
                                               .SingleOrDefault();
            if (site == null)
            {
                throw new KeyNotFoundException($"Unable to find EchoshoeSite: {recordSiteName}");
            }

            return site;
        }

        private EchoshoreSite GetEchoshoreSiteMem(string recordSiteName)
        {
            return _echoshoreSiteDictionary.ContainsKey(recordSiteName)
                ? _echoshoreSiteDictionary[recordSiteName]
                : _echoshoreSiteDictionary[recordSiteName] = GetEchoshoreSite(recordSiteName);
        }

        private PointOfInterestStatus GetPointOfInterestStatusMem(string recordPoiStatus)
        {
            return _pointOfInterestStatusDictionary.ContainsKey(recordPoiStatus)
                ? _pointOfInterestStatusDictionary[recordPoiStatus]
                : (_pointOfInterestStatusDictionary[recordPoiStatus] = GetPointOfInterestStatus(recordPoiStatus));

        }

        private PointOfInterestStatus GetPointOfInterestStatus(string recordPoiStatus)
        {
            if (string.IsNullOrWhiteSpace(recordPoiStatus))
            {
                throw new KeyNotFoundException($"Point of Interest Status was not provided.");
            }

            var pointOfInterestStatus = _pointOfInterestRepository.Where(pois => pois.Description == recordPoiStatus)
                                             .Select(pois => new PointOfInterestStatus { Id = pois.Id, Description = pois.Description}).SingleOrDefault();
            if (pointOfInterestStatus == null)
            {
                throw new KeyNotFoundException($"Unable to find Point of Interest Status :{recordPoiStatus}");
            }
            return pointOfInterestStatus;
        }

        protected override EchoshoreLeakAlert FindOrCreateEntity(LeakAlertFileRecord record, int currentLine)
        {
            return new EchoshoreLeakAlert();
        }

        protected override void LogRecord(LeakAlertFileRecord record)
        {
            _log.Info($"Adding Leak: {record.SocketID1} {record.SocketID2}, {record.SiteName}");
        }

        #endregion
    }
}
