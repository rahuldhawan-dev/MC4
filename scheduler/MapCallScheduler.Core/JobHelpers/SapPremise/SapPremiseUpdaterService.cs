using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Library.JobHelpers.Sap;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.JobHelpers.SapPremise
{
    public class SapPremiseUpdaterService : SapEntityWithCoordinateUpdaterServiceBase<SapPremiseFileRecord, ISapPremiseFileParser, Premise, IRepository<Premise>>, ISapPremiseUpdaterService
    {
        #region Constants

        public const int CLEAR_SESSION_INTERVAL = 25;

        #endregion

        #region Private Members

        private readonly IRepository<MeterReadingRoute> _routeRepository;
        private readonly IRepository<PremiseDistrict> _districtRepository;
        private readonly IRepository<PremiseAreaCode> _areaCodeRepository;
        private readonly IRepository<RegionCode> _regionCodeRepository;
        private readonly ITownRepository _townRepository;
        private readonly IMeterReadingRouteReadingDateRepository _routeReadingDateRepository;
        private readonly IRepository<PlanningPlant> _planningPlantRepository;
        private readonly IRepository<ServiceUtilityType> _serviceUtilityTypeRepository;
        private readonly IRepository<ServiceSize> _serviceSizeRepository;
        private readonly IRepository<MeterLocation> _meterLocationRepository;
        private readonly IRepository<PremiseStatusCode> _premiseStatusRepository;
        private readonly IRepository<PremiseCriticalCareType> _premiseCriticalCareTypeRepository;
        private readonly IRepository<PublicWaterSupply> _publicWaterSupplyRepository;
        private readonly IRepository<PremiseType> _premiseTypeRepository;
        private readonly IDictionary<string, PlanningPlant> _planningPlantDictionary;
        private readonly IDictionary<Tuple<string, string>, Town> _townDictionary;
        private readonly IDictionary<Tuple<string, string>, MeterReadingRoute> _routeDictionary;
        private readonly IDictionary<Tuple<string, string>, PremiseDistrict> _districtDictionary;
        private readonly IDictionary<Tuple<string, string>, PremiseAreaCode> _areaCodeDictionary;
        private readonly IDictionary<Tuple<string, string>, RegionCode> _regionCodeDictionary;
        private readonly IDictionary<string, ServiceUtilityType> _utilityTypeDictionary;
        private readonly IDictionary<string, ServiceSize> _serviceSizeDictionary;
        private readonly IDictionary<string, MeterLocation> _meterLocationDictionary;
        private readonly IDictionary<Tuple<string, DateTime>, bool> _routeReadingDateDictionary;
        private readonly IDictionary<string, PremiseStatusCode> _statusCodeDictionary;
        private readonly IDictionary<string, PremiseCriticalCareType> _criticalCareTypeDictionary;
        private readonly IDictionary<Tuple<string, string, string, string>, int> _premiseDictionary;
        private readonly IDictionary<string, PublicWaterSupply> _publicWaterSupplyDictionary;
        private readonly IDictionary<string, PremiseType> _premiseTypeDictionary;

        #endregion

        #region Constructors

        public SapPremiseUpdaterService(ISapPremiseFileParser parser, IRepository<Premise> repository, ILog log, IRepository<MeterReadingRoute> routeRepository, IRepository<PremiseDistrict> districtRepository, IRepository<PremiseAreaCode> areaCodeRepository, IRepository<RegionCode> regionCodeRepository, ITownRepository townRepository, IMeterReadingRouteReadingDateRepository routeReadingDateRepository, IRepository<PlanningPlant> planningPlantRepository, IRepository<ServiceUtilityType> serviceUtilityTypeRepository, IRepository<ServiceSize> serviceSizeRepository, IRepository<Coordinate> coordinateRepository, IRepository<MeterLocation> meterLocationRepository, IRepository<PremiseStatusCode> premiseStatusRepository, IRepository<PremiseCriticalCareType> premiseCriticalCareTypeRepository, IRepository<PublicWaterSupply> publicWaterSupplyRepository, IRepository<PremiseType> premiseTypeRepository) : base(parser, repository, log, coordinateRepository)
        {
            _routeRepository = routeRepository;
            _districtRepository = districtRepository;
            _areaCodeRepository = areaCodeRepository;
            _regionCodeRepository = regionCodeRepository;
            _townRepository = townRepository;
            _routeReadingDateRepository = routeReadingDateRepository;
            _planningPlantRepository = planningPlantRepository;
            _serviceUtilityTypeRepository = serviceUtilityTypeRepository;
            _serviceSizeRepository = serviceSizeRepository;
            _meterLocationRepository = meterLocationRepository;
            _premiseStatusRepository = premiseStatusRepository;
            _premiseCriticalCareTypeRepository = premiseCriticalCareTypeRepository;
            _publicWaterSupplyRepository = publicWaterSupplyRepository;
            _premiseTypeRepository = premiseTypeRepository;

            _planningPlantDictionary = new Dictionary<string, PlanningPlant>();
            _townDictionary = new Dictionary<Tuple<string, string>, Town>();
            _routeDictionary = new Dictionary<Tuple<string, string>, MeterReadingRoute>();
            _districtDictionary = new Dictionary<Tuple<string, string>, PremiseDistrict>();
            _areaCodeDictionary = new Dictionary<Tuple<string, string>, PremiseAreaCode>();
            _regionCodeDictionary = new Dictionary<Tuple<string, string>, RegionCode>();
            _utilityTypeDictionary = new Dictionary<string, ServiceUtilityType>();
            _serviceSizeDictionary = new Dictionary<string, ServiceSize>();
            _meterLocationDictionary = new Dictionary<string, MeterLocation>();
            _routeReadingDateDictionary = new Dictionary<Tuple<string, DateTime>, bool>();
            _statusCodeDictionary = new Dictionary<string, PremiseStatusCode>();
            _criticalCareTypeDictionary = new Dictionary<string, PremiseCriticalCareType>();
            _publicWaterSupplyDictionary = new Dictionary<string, PublicWaterSupply>();
            _premiseTypeDictionary = new Dictionary<string, PremiseType>();
            _premiseDictionary = new Dictionary<Tuple<string, string, string, string>, int>();
        }

        #endregion

        #region Private Methods

        protected override void MapRecord(Premise entity, SapPremiseFileRecord record, int currentLine)
        {
            MapField(entity, record, x => x.PremiseNumber);
            MapField(entity, record, x => x.ConnectionObject);
            MapField(entity, record, x => x.DeviceCategory);
            MapField(entity, record, x => x.DeviceLocation);
            MapField(entity, record, x => x.Equipment);
            MapField(entity, record, x => x.DeviceSerialNumber);
            MapField(entity, record, x => x.ServiceAddressHouseNumber);
            MapField(entity, record, x => x.ServiceAddressFraction);
            MapField(entity, record, x => x.ServiceAddressApartment);
            MapField(entity, record, x => x.ServiceAddressStreet);
            MapField(entity, record, x => x.ServiceZip);
            MapField(entity, record, x => x.MeterLocationFreeText);
            MapField(entity, record, x => x.Installation);
            MapField(entity, record, x => x.MeterSerialNumber);
            MapField(entity, record, x => x.MajorAccountManager);
            MapField(entity, record, x => x.AccountManagerContactNumber);
            MapField(entity, record, x => x.AccountManagerEmail);

            entity.IsMajorAccount = record.IsMajorAccount;
            entity.RouteNumber = GetRouteMem(record.RouteNumber, record.RouteNumberDescription);
            entity.ServiceDistrict =
                GetDistrictMem(record.ServiceDistrict, record.ServiceDistrictDescription);
            entity.AreaCode = GetAreaCodeMem(record.AreaCode, record.AreaCodeDescription);
            entity.ServiceCity = GetCityMem(record.RegionCode, record.ServiceState);
            entity.PlanningPlant = GetPlanningPlantMem(record.OperatingCentre);
            entity.OperatingCenter = entity.PlanningPlant?.OperatingCenter;
            entity.RegionCode =
                GetRegionCodeMem(
                    record.RegionCode,
                    record.RegionCodeDescription,
                    entity.OperatingCenter?.State);
            entity.ServiceUtilityType = GetUtilityTypeMem(record.ServiceUtilityType);
            entity.MeterSize = GetServiceSizeMem(record.MeterSize);
            entity.MeterLocation = GetMeterLocationMem(record.MeterLocation);
            entity.Coordinate = GetCoordinate(record.Latitude, record.Longitude);
            entity.StatusCode = GetStatusCodeMem(record.StatusCode);
            entity.CriticalCareType = GetCriticalCareTypeMem(record.CriticalCareType);
            entity.PremiseType = GetPremiseTypeMem(record.PremiseType);
            entity.PublicWaterSupply = GetPublicWaterSupplyMem(record.Pwsid);
            MapRouteReadingDates(entity.RouteNumber, record.NextMeterReadingdate);
        }

        private PremiseType GetPremiseTypeMem(string abbreviation)
        {
            return _premiseTypeDictionary.ContainsKey(abbreviation)
                ? _premiseTypeDictionary[abbreviation]
                : _premiseTypeDictionary[abbreviation] = GetPremiseType(abbreviation);
        }

        private PremiseType GetPremiseType(string abbreviation)
        {
            return string.IsNullOrWhiteSpace(abbreviation)
                ? null
                : _premiseTypeRepository.Where(x => x.Abbreviation == abbreviation).Single();
        }

        private PublicWaterSupply GetPublicWaterSupplyMem(string pwsid)
        {
            return _publicWaterSupplyDictionary.ContainsKey(pwsid)
                ? _publicWaterSupplyDictionary[pwsid]
                : _publicWaterSupplyDictionary[pwsid] = GetPublicWaterSupply(pwsid);
        }

        private PublicWaterSupply GetPublicWaterSupply(string pwsid)
        {
            return string.IsNullOrWhiteSpace(pwsid)
                ? null
                : _publicWaterSupplyRepository.Where(x => x.Identifier == pwsid).FirstOrDefault();
        }

        private MeterLocation GetMeterLocationMem(string sapCode)
        {
            return _meterLocationDictionary.ContainsKey(sapCode)
                ? _meterLocationDictionary[sapCode]
                : _meterLocationDictionary[sapCode] = GetMeterLocation(sapCode);
        }

        private MeterLocation GetMeterLocation(string sapCode)
        {
            if (string.IsNullOrWhiteSpace(sapCode))
            {
                return null;
            }

            sapCode = sapCode.PadLeft(4, '0');

            return _meterLocationRepository.Where(m => m.SAPCode == sapCode)
                                           .Select(ml => new MeterLocation {Id = ml.Id}).Single();
        }
        
        private PremiseStatusCode GetStatusCodeMem(string status)
        {
            return _statusCodeDictionary.ContainsKey(status)
                ? _statusCodeDictionary[status]
                : _statusCodeDictionary[status] = GetStatusCode(status);
        }

        private PremiseStatusCode GetStatusCode(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return null;
            }

            var results = _premiseStatusRepository
                         .Where(s => s.Description == status).Select(s => new PremiseStatusCode {Id = s.Id});

            if (!results.Any())
            {
                throw new ArgumentException(
                    $"Value '{status}' does not appear to be a valid {nameof(PremiseStatusCode)}.");
            }

            return results.Single();
        }

        private PremiseCriticalCareType GetCriticalCareTypeMem(string criticalcaretype)
        {
            return _criticalCareTypeDictionary.ContainsKey(criticalcaretype)
                ? _criticalCareTypeDictionary[criticalcaretype]
                : _criticalCareTypeDictionary[criticalcaretype] = GetCriticalCareType(criticalcaretype);
        }

        private PremiseCriticalCareType GetCriticalCareType(string criticalcaretype)
        {
            if (string.IsNullOrWhiteSpace(criticalcaretype))
            {
                return null;
            }

            var results = _premiseCriticalCareTypeRepository
                         .Where(s => s.Description == criticalcaretype)
                         .Select(s => new PremiseCriticalCareType {Id = s.Id});

            if (!results.Any())
            {
                throw new ArgumentException(
                    $"Value '{criticalcaretype}' does not appear to be a valid {nameof(PremiseCriticalCareType)}.");
            }

            return results.Single();
        }

        private ServiceSize GetServiceSizeMem(string meterSize)
        {
            return _serviceSizeDictionary.ContainsKey(meterSize)
                ? _serviceSizeDictionary[meterSize]
                : _serviceSizeDictionary[meterSize] = GetServiceSize(meterSize);
        }

        private ServiceSize GetServiceSize(string meterSize)
        {
            if (string.IsNullOrWhiteSpace(meterSize))
            {
                return null;
            }

            meterSize = meterSize.PadLeft(4, '0');

            return _serviceSizeRepository
                  .Where(s => s.SAPCode == meterSize)
                  .Select(ss => new ServiceSize { Id = ss.Id })
                  .Single();
        }

        private ServiceUtilityType GetUtilityTypeMem(string serviceUtilityType)
        {
            return _utilityTypeDictionary.ContainsKey(serviceUtilityType)
                ? _utilityTypeDictionary[serviceUtilityType]
                : _utilityTypeDictionary[serviceUtilityType] = GetUtilityType(serviceUtilityType);
        }

        private ServiceUtilityType GetUtilityType(string serviceUtilityType)
        {
            if (string.IsNullOrWhiteSpace(serviceUtilityType))
            {
                return null;
            }

            return _serviceUtilityTypeRepository.Where(t => t.Type == serviceUtilityType).Select(sut => new ServiceUtilityType {Id = sut.Id}).Single();
        }

        private PlanningPlant GetPlanningPlantMem(string operatingCentre)
        {
            return _planningPlantDictionary.ContainsKey(operatingCentre)
                ? _planningPlantDictionary[operatingCentre]
                : (_planningPlantDictionary[operatingCentre] = GetPlanningPlant(operatingCentre));
        }

        private PlanningPlant GetPlanningPlant(string operatingCentre)
        {
            if (string.IsNullOrWhiteSpace(operatingCentre))
            {
                return null;
            }

            return _planningPlantRepository
                  .Where(pp => pp.Code == operatingCentre)
                  .Select(pp => new PlanningPlant {
                       Id = pp.Id,
                       OperatingCenter = new OperatingCenter {
                           Id = pp.OperatingCenter.Id,
                           State = new State {
                               Id = pp.OperatingCenter.State.Id
                           }
                       }
                   })
                  .SingleOrDefault();
        }

        private bool RouteReadingDateExistsMem(string sapCode, DateTime readingDate)
        {
            var tup = new Tuple<string, DateTime>(sapCode, readingDate);

            if (_routeReadingDateDictionary.ContainsKey(tup))
            {
                return _routeReadingDateDictionary[tup];
            }

            _routeReadingDateDictionary[tup] = true;
            return _routeReadingDateRepository.Where(d =>
                d.MeterReadingRoute.SAPCode == sapCode && d.ReadingDate == readingDate).Select(x => x.Id).Any();
        }

        private void MapRouteReadingDates(MeterReadingRoute route, string dateStr)
        {
            if (route == null || string.IsNullOrWhiteSpace(dateStr))
            {
                return;
            }

            foreach (
                var date in
                dateStr.Split(',').Select(s => DateTime.ParseExact(s, "MM/dd/yyyy", CultureInfo.InvariantCulture)))
            {
                if (!RouteReadingDateExistsMem(route.SAPCode, date))
                {
                    var obj = _routeReadingDateRepository.Save(new MeterReadingRouteReadingDate {
                        MeterReadingRoute = route,
                        ReadingDate = date
                    });

                    _routeReadingDateRepository.Evict(obj);
                }
            }
        }

        private Town GetCity(string regionCode, string state)
        {
            return
                _townRepository.Where(
                    t => t.DistrictId == regionCode && t.State.Abbreviation == state).Select(t => new Town {Id = t.Id}).SingleOrDefault();
        }

        private Town GetCityMem(string regionCode, string state)
        {
            var tup = new Tuple<string, string>(regionCode, state);
            return _townDictionary.ContainsKey(tup)
                ? _townDictionary[tup]
                : _townDictionary[tup] = GetCity(regionCode, state);
        }

        private PremiseAreaCode GetAreaCodeMem(string areaCode, string areaCodeDescription)
        {
            var tup = new Tuple<string, string>(areaCode, areaCodeDescription);
            return _areaCodeDictionary.ContainsKey(tup)
                ? _areaCodeDictionary[tup]
                : _areaCodeDictionary[tup] = GetAreaCode(areaCode, areaCodeDescription);
        }

        private PremiseAreaCode GetAreaCode(string areaCode, string areaCodeDescription)
        {
            if (string.IsNullOrWhiteSpace(areaCode))
            {
                return null;
            }

            return _areaCodeRepository.Where(a => a.SAPCode == areaCode).Select(ac => new PremiseAreaCode {Id = ac.Id}).SingleOrDefault() ??
                   _areaCodeRepository.Save(new PremiseAreaCode {SAPCode = areaCode, Description = areaCodeDescription});
        }

        private RegionCode GetRegionCodeMem(string regionCode, string regionCodeDescription, State state)
        {
            var tup = new Tuple<string, string>(regionCode, regionCodeDescription);
            return _regionCodeDictionary.ContainsKey(tup)
                ? _regionCodeDictionary[tup]
                : _regionCodeDictionary[tup] = GetRegionCode(regionCode, regionCodeDescription, state);
        }

        private RegionCode GetRegionCode(string regionCode, string regionCodeDescription, State state)
        {
            if (string.IsNullOrWhiteSpace(regionCode) || state == null)
            {
                return null;
            }

            var existing = _regionCodeRepository
                          .Where(a => a.SAPCode == regionCode && a.State.Id == state.Id)
                          .Select(rc => new RegionCode { Id = rc.Id })
                          .SingleOrDefault();
            return existing ??
                   _regionCodeRepository.Save(new RegionCode {
                       SAPCode = regionCode,
                       Description = regionCodeDescription,
                       State = state
                   });
        }

        private PremiseDistrict GetDistrictMem(string district, string description)
        {
            var tup = new Tuple<string, string>(district, description);
            return _districtDictionary.ContainsKey(tup)
                ? _districtDictionary[tup]
                : _districtDictionary[tup] = GetDistrict(district, description);
        }

        private PremiseDistrict GetDistrict(string district, string description)
        {
            if (string.IsNullOrWhiteSpace(district))
            {
                return null;
            }

            return _districtRepository.Where(d => d.SAPCode == district).Select(d => new PremiseDistrict {Id = d.Id}).SingleOrDefault() ??
                   _districtRepository.Save(new PremiseDistrict {SAPCode = district, Description = description});
        }

        private MeterReadingRoute GetRoute(string routeNumber, string description)
        {
            if (string.IsNullOrWhiteSpace(routeNumber))
            {
                return null;
            }

            try
            {
                return _routeRepository.Where(r => r.SAPCode == routeNumber).Select(rr => new MeterReadingRoute {Id = rr.Id, SAPCode = rr.SAPCode}).SingleOrDefault() ??
                       _routeRepository.Save(new MeterReadingRoute {SAPCode = routeNumber, Description = description});
            }
            catch (InvalidOperationException e)
            {
                throw new Exception(
                    $"Error loading single Meter Reading Route with route number: '{routeNumber}', description: '{description}",
                    e);
            }
        }

        private MeterReadingRoute GetRouteMem(string routeNumber, string description)
        {
            var tup = new Tuple<string, string>(routeNumber, description);
            return _routeDictionary.ContainsKey(tup)
                ? _routeDictionary[tup]
                : _routeDictionary[tup] = GetRoute(routeNumber, description);
        }

        protected override Premise FindOrCreateEntity(SapPremiseFileRecord record, int currentLine)
        {
            return FindOrCreateEntityMem(record.PremiseNumber, record.DeviceLocation, record.ConnectionObject,
                record.Installation, currentLine);
        }

        private Premise FindOrCreateEntityMem(string premiseNumber, string deviceLocation, string connectionObject,
            string installation, int currentLine)
        {
            var tup = new Tuple<string, string, string, string>(premiseNumber, deviceLocation, connectionObject,
                installation);

            int? existingId;

            if (_premiseDictionary.ContainsKey(tup))
            {
                existingId = _premiseDictionary[tup];
            }
            else
            {
                existingId = FindEntity(premiseNumber, deviceLocation, connectionObject, installation, currentLine);
            }

            if (existingId.HasValue && existingId.Value > 0)
            {
                _premiseDictionary[tup] = existingId.Value;
                return _repository.Find(existingId.Value);
            }

            return new Premise();
        }

        private int? FindEntity(string premiseNumber, string deviceLocation, string connectionObject,
            string installation, int currentLine)
        {
            var results =
                _repository.Where(
                    p =>
                        p.PremiseNumber == premiseNumber &&
                        p.DeviceLocation == deviceLocation &&
                        p.ConnectionObject == connectionObject &&
                        p.Installation == installation).Select(p => p.Id);
            try
            {
                return results.SingleOrDefault();
            }
            catch (InvalidOperationException e)
            {
                throw new Exception(
                    $"Line {currentLine}: Error loading single premise with PremiseNumber: '{premiseNumber}', DeviceLocation: '{deviceLocation}', ConnectionObject: '{connectionObject}', Installation: '{installation}'", e);
            }
        }

        protected override void LogRecord(SapPremiseFileRecord record)
        {
            throw new NotImplementedException();
        }

        protected void LogRecord(SapPremiseFileRecord record, Premise entity, int currentIndex, int total, long elapsed)
        {
            var finished = currentIndex + 1;
            var per = elapsed / finished;
            var percentFinished = (finished * 100m) / total;
            var remaining = per * (total - finished);

            _log.Info(
                $"{(entity.Id == 0 ? "Inserting" : "Updating")} premise {finished} of {total}: {record.PremiseNumber}, {record.DeviceLocation}, {record.ConnectionObject}, {record.ServiceUtilityType}");
            _log.Info(
                $@"{percentFinished:#.##}% complete, elapsed {TimeSpan.FromMilliseconds(elapsed):hh\:mm\:ss}, per {TimeSpan.FromMilliseconds(per)}, remaining {TimeSpan.FromMilliseconds(remaining):hh\:mm\:ss}");
        }

        #endregion

        #region Exposed Methods

        public override void Process(FileData sapFile)
        {
            _log.Info($"Parsing file '{sapFile.Filename}'...");

            var records = _parser.Parse(sapFile);
            var total = records.Count();
            _log.Info($"Found {total} records...");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var current = 0;

            foreach (var record in records)
            {
                var entity = FindOrCreateEntity(record, current + 2);
                LogRecord(record, entity, current, total, stopWatch.ElapsedMilliseconds);
                MapRecord(entity, record, current + 2);

                current++;

                if (!string.IsNullOrWhiteSpace(entity.PremiseNumber))
                {
                    _repository.Save(entity);
                }

                if (current % CLEAR_SESSION_INTERVAL == 0)
                {
                    _repository.ClearSession();
                }
            }
        }

        #endregion
    }
}
