using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.ClassExtensions.RegexExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using Newtonsoft.Json;

namespace MapCallScheduler.JobHelpers.SapWaterQualityComplaint
{
    public class SapWaterQualityComplaintService : ISapWaterQualityComplaintService
    {
        #region Constants

        private const int WATER_QUALITY_SAP_NOTIFICATION_TYPE = 35;

        // Dictionary for mapping Sap Complaint Type to ours
        private static IDictionary<string, string> TYPE_LOOKUP = new Dictionary<string, string> {
            { "Stained Laundry", "Aesthetic-Stained Laundry" },
            { "Discolored Water", "Aesthetic-Discolored Water" },
            { "Illness", "Medical-Illness" },
            { "Particles/Sediments", "Aesthetics-Particles In Water" },
            { "Slippery/Slimy - Feel", "Aesthetics-Other" },
            { "Bitter - Taste", "Aesthetics-Taste Odor" },
            { "Odor/Taste - Chlorine", "Aesthetics-Taste Odor" },
            { "General Water Quality Inquiries/Requests", "Information Inquiry-Information Only" },
            { "Cloudy Water - Air/Milky/White", "Aesthetics-Cloudy / Milky" },
            { "Odor/Taste - Earthy/Musty", "Aesthetics-Taste Odor" },
            { "External Research/External Media Stories", "Information Inquiry-Information Only" },
            { "Odor - Rotten Egg", "Aesthetics-Taste Odor" },
            { "Colored Water", "Aesthetic-Discolored Water" },
            { "Moving organisms", "Medical-Other" },
            { "Boil/Do Not Use/Do not Consume", "Medical-Other" },
            { "Odor - Gasoline/Petroleum", "Aesthetics-Taste Odor" },
            { "Taste other", "Aesthetics-Taste Odor" },
            { "Odor other", "Aesthetics-Taste Odor" },
            { "Iron", "Aesthetics-Taste Odor" },
        };

        #endregion

        #region Private Members

        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IRepository<PlanningPlant> _planningPlantRepo;
        private readonly ISAPNotificationRepository _sapNotificationRepository;
        private readonly IRepository<WaterQualityComplaint> _wqComplaintRepository;
        private readonly IRepository<WaterQualityComplaintType> _typeRepository;
        private readonly IRepository<Town> _townRepository;
        private readonly IRepository<State> _stateRepository;
        private readonly IPremiseRepository _premiseRepository;
        private readonly ICoordinateRepository _coordinateRepository;
        private readonly ILog _log;
        private DateTime _currentDate;
        private IRepository<Note> _noteRepo;
        private IDataTypeRepository _dataTypeRepo;

        #endregion

        #region Constructors

        public SapWaterQualityComplaintService(
            IDateTimeProvider dateTimeProvider,
            IRepository<PlanningPlant> planningPlantRepository,
            IRepository<WaterQualityComplaint> waterQualityComplaintRepository,
            IRepository<WaterQualityComplaintType> typeRepository,
            IRepository<Town> townRepository,
            IRepository<State> stateRepository,
            IPremiseRepository premiseRepository,
            ICoordinateRepository coordinateRepository,
            ISAPNotificationRepository sapNotificationRepository,
            ILog log,
            IRepository<Note> noteRepository,
            IDataTypeRepository dataTypeRepository)
        {
            _dateTimeProvider = dateTimeProvider;
            _planningPlantRepo = planningPlantRepository;
            _wqComplaintRepository = waterQualityComplaintRepository;
            _typeRepository = typeRepository;
            _townRepository = townRepository;
            _stateRepository = stateRepository;
            _premiseRepository = premiseRepository;
            _coordinateRepository = coordinateRepository;
            _sapNotificationRepository = sapNotificationRepository;
            _log = log;
            _currentDate = _dateTimeProvider.GetCurrentDate();
            _noteRepo = noteRepository;
            _dataTypeRepo = dataTypeRepository;
        }

        #endregion

        #region Private Methods

        private string FormatPhoneNumber(string phoneNumber)
        {
            return string.IsNullOrEmpty(phoneNumber)
                ? null
                : new Regex(@"^(\d{3})[^\d]*(\d{3})[^\d]*(\d{4}).*").Replace(phoneNumber, "$1-$2-$3");
        }

        private string ParseBusinessPartner(string longText)
        {
            // the long text field comes in format "... Business Partner : XXXXXX  Customer Name : XXXX ..."
            // we need to grab all the integers after Business Partners
            var ret = new Regex(@"Business Partner : ([0-9]*)").TryGetGroup(longText, 1);
            return ret;
        }

        private string ParseNotes(string longText)
        {
            // the long text field comes in format "... Notes : XXXX XXXX XXX Premise : ..."
            // we need to grab all the text between 'Notes : ' and 'Premise : '
            var match = new Regex("Notes : (.*) Premise :").TryGetGroup(longText, 1);
            return match;
        }

        private DateTime? ParseDateTime(string date, string time)
        {
            // the date and time field comes in the format yyyyMMdd and HHmmss respectively
            return DateTime.TryParseExact(
                $"{date} {time}",
                "yyyyMMdd HHmmss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var datetime)
                ? datetime
                : (DateTime?)null;
        }

        private WaterQualityComplaintType LoadType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                return null;
            }

            // match the type returned by sap to the ones in our dictionary
            type = TYPE_LOOKUP.ContainsKey(type) ? TYPE_LOOKUP[type] : type;

            return string.IsNullOrWhiteSpace(type)
                ? null
                : _typeRepository.Where(t => t.Description == type).SingleOrDefault();
        }

        private Town LoadTown(string town, string state)
        {
            return _townRepository.Where(t => t.State.Abbreviation == state && t.ShortName == town).SingleOrDefault();
        }

        private State LoadState(string state)
        {
            return _stateRepository.Where(t => t.Abbreviation == state).SingleOrDefault();
        }

        private IEnumerable<Premise> FindMatchingPremises(string premiseNumber)
        {
            if (string.IsNullOrWhiteSpace(premiseNumber))
            {
                return Enumerable.Empty<Premise>();
            }

            premiseNumber = premiseNumber.Trim();
            return _premiseRepository.FindByPremiseNumber(premiseNumber);
        }

        private void TrySetOperatingCenterAndCoordinateFromPremiseNumber(WaterQualityComplaint wqc)
        {
            // WO0000000222927: Attempt to auto populate OperatingCenter and Coordinate from matching Premise.
            //      - Don't attempt matching if there is no PremiseNumber value or if no Premise record is returned.
            //      - Only set OperatingCenter if all Premise records have the same OperatingCenter value.
            //      - Only set Coordinate if all Premise records have exactly the same Coordinate Lat/Long values.

            var premises = FindMatchingPremises(wqc.PremiseNumber).ToList();

            if (!premises.Any())
            {
                return;
            }

            // Premise.OperatingCenter is nullable, though none of the rows in the db
            // have null OperatingCenter values at the time of writing this. Still, ignore
            // null OperatingCenters.
            var operatingCenters = premises.Where(x => x.OperatingCenter != null)
                                           .Select(x => x.OperatingCenter).Distinct().ToList();
            if (operatingCenters.Count == 1)
            {
                wqc.OperatingCenter = operatingCenters.Single();
            }

            // Premise.Coordinate is nullable, so ignore nulls.
            var coordinates = premises.Where(x => x.Coordinate != null).Select(x => x.Coordinate).ToList();

            // We can't guarantee which Premise record has the correct Coordinate values if they aren't
            // exactly the same. If any of the values are slightly different then we don't want to set
            // the coordinate at all since we can't verify which one is the correct one. Sometimes the
            // values are only off by .0001, sometimes the values are obviously wrong(ex: Latitude == 0).
            var hasValidCoordinate = coordinates.GroupBy(x => new { x.Latitude, x.Longitude }).Count() == 1;

            if (hasValidCoordinate)
            {
                wqc.Coordinate = _coordinateRepository.CloneAndSave(coordinates.First());
            }
        }

        private SearchSapNotification BuildSearchParameters()
        { 
            // planning plants should be list of comma separated codes
            var planningPlantCodes = string.Join(",", _planningPlantRepo.GetAll().Select(p => p.Code));

            var notificationSearch = new SearchSapNotification() {
                NotificationType = WATER_QUALITY_SAP_NOTIFICATION_TYPE.ToString(),
                PlanningPlant = planningPlantCodes,
                DateCreatedFrom = _currentDate.ToString("yyyyMMdd"),
                DateCreatedTo = _currentDate.ToString("yyyyMMdd")
            };
            return notificationSearch;
        }
        
        private SAPNotificationCollection GetSapNotifications(SearchSapNotification notificationSearch)
        { 
            // Get WQ Complaints from the SAP service
            _log.Info("Fetching WQ Complaints from SAP");
            _log.Info($"Request: {JsonConvert.SerializeObject(notificationSearch)}");
            var notifications = _sapNotificationRepository.Search(notificationSearch);

            if (!notifications.Items.Any())
            {
                _log.Info("No items returned");
                return null;
            }

            // When it fails due to auth error, it returns one item with error code
            if (string.IsNullOrEmpty(notifications.First().SAPNotificationNumber))
            {
                _log.Info($"SAP Error: {notifications.First().SAPErrorCode}");
                return null;
            }

            _log.Info($"Total WQ Complaints Returned: {notifications.Items.Count()}");
            return notifications;
        }
        
        private List<SAPNotification> FilterToBeProcessedNotifications(SAPNotificationCollection notifications)
        { 
            // Get today's inserted records
            var processedNotifications = _wqComplaintRepository.Where(
                w => w.DateComplaintReceived.HasValue &&
                     w.DateComplaintReceived >= _currentDate.BeginningOfDay() &&
                     w.DateComplaintReceived <= _currentDate.EndOfDay());

            // ToList required below so that ids are present in the list rather than an expression
            // to be evaluated later. Contains() will return false if ToList is removed.
            var processedIds = processedNotifications.Select(p => p.OrcomOrderNumber).ToList();

            // Find records that have not been inserted yet
            var toBeProcessedNotifications =
                notifications.Items.Where(n => !processedIds.Contains(n.SAPNotificationNumber)).ToList();
            
            _log.Info($"SAP WQ Complaints to be processed: {toBeProcessedNotifications.Count()}");
            
            return toBeProcessedNotifications;
        }
        
        private void SaveNotifications(List<SAPNotification> toBeProcessedNotifications)
        { 
            // Insert
            var wqComplaints = new List<WaterQualityComplaint>();
            foreach (var sapNotification in toBeProcessedNotifications)
            {
                try
                {
                    _log.Info(
                        $"Processing WQ Complaint SAPNotificationNumber: {sapNotification.SAPNotificationNumber}");
                    var wqc = new WaterQualityComplaint {
                        OrcomOrderNumber = sapNotification.SAPNotificationNumber,
                        Type = LoadType(sapNotification.CodingGroupCodeDescription),
                        DateComplaintReceived =
                            ParseDateTime(sapNotification.DateCreated, sapNotification.TimeCreated),
                        CustomerName = sapNotification.CustomerName,
                        HomePhoneNumber = FormatPhoneNumber(sapNotification.Telephone),
                        StreetNumber = sapNotification.House,
                        StreetName = sapNotification.Street1,
                        State = LoadState(sapNotification.State),
                        Town = LoadTown(sapNotification.City, sapNotification.State),
                        PremiseNumber = sapNotification.Premise,
                        AccountNumber = ParseBusinessPartner(sapNotification.NotificationLongText),
                        ComplaintDescription = ParseNotes(sapNotification.NotificationLongText),
                        Imported = true
                    };

                    TrySetOperatingCenterAndCoordinateFromPremiseNumber(wqc);

                    wqComplaints.Add(wqc);
                }
                catch (Exception e)
                {
                    _log.Error(
                        $"Error occurred while processing SAPNotificationNumber: {sapNotification.SAPNotificationNumber}. {e}");
                }
            }

            _wqComplaintRepository.Save(wqComplaints);

            var dataType = _dataTypeRepo.GetByTableName(WaterQualityComplaintMap.TABLE_NAME).Single();

            var notes = wqComplaints.Select(complaint => new Note { DataType = dataType, Text = complaint.ComplaintDescription, CreatedBy = "Scheduler - SAP Water Quality Complaint Service", LinkedId = complaint.Id }).ToList();

            _noteRepo.Save(notes);
        }
        
        #endregion

        public void Process()
        {
            try
            {
                _currentDate = _dateTimeProvider.GetCurrentDate();
                var notificationSearch = BuildSearchParameters();
                var notifications = GetSapNotifications(notificationSearch);
                
                if (notifications == null)
                {
                    return;
                }

                var toBeProcessedNotifications = FilterToBeProcessedNotifications(notifications);

                SaveNotifications(toBeProcessedNotifications);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }
    }
}
