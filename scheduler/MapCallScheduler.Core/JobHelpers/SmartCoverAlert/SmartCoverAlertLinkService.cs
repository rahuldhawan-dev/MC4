using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using log4net;
using System.Net.Http;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using Newtonsoft.Json;
using System.Configuration;
using MapCallScheduler.JobHelpers.SmartCoverAlert.Models;
using MMSINC.Utilities;

namespace MapCallScheduler.JobHelpers.SmartCoverAlert
{
    public class SmartCoverAlertLinkService : ISmartCoverAlertLinkService
    {
        #region Constants

        public const string LOCATIONS_LIST_WEB_METHOD = "smartcover/api/locations/list.php",
                            ACKNOWLEDGE_ALERTS_WEB_METHOD = "smartcover/api/locations/alarms/ack.php";

        #endregion

        #region Private methods

        protected readonly ILog _log;
        private IRepository<MapCall.Common.Model.Entities.SmartCoverAlert> _smartCoverAlertRepository;
        private IRepository<SmartCoverAlertAlarm> _smartCoverAlertAlarmRepository;
        private IRepository<SmartCoverAlertApplicationDescriptionType> _smartCoverAlertApplicationDescriptionTypeRepository;
        private IRepository<SmartCoverAlertAlarmType> _smartCoverAlertAlarmTypeRepository;
        private IRepository<SmartCoverAlertSmartCoverAlertType> _smartCoverAlertSmartCoverAlertTypeRepository;
        private IRepository<SmartCoverAlertType> _smartCoverAlertTypeRepository;
        private readonly IHttpClient _httpClient;

        #endregion

        #region Constructors

        public SmartCoverAlertLinkService(ILog log,
            IRepository<MapCall.Common.Model.Entities.SmartCoverAlert> smartCoverAlertRepository,
            IRepository<SmartCoverAlertAlarm> smartCoverAlertAlarmRepository,
            IRepository<SmartCoverAlertApplicationDescriptionType> smartCoverAlertApplicationDescriptionTypeRepository,
            IRepository<SmartCoverAlertAlarmType> smartCoverAlertAlarmTypeRepository,
            IRepository<SmartCoverAlertSmartCoverAlertType> smartCoverAlertSmartCoverAlertTypeRepository,
            IRepository<SmartCoverAlertType> smartCoverAlertTypeRepository,
            IHttpClientFactory httpClient)
        {
            _log = log;
            _smartCoverAlertRepository = smartCoverAlertRepository;
            _smartCoverAlertAlarmRepository = smartCoverAlertAlarmRepository;
            _smartCoverAlertApplicationDescriptionTypeRepository = smartCoverAlertApplicationDescriptionTypeRepository;
            _smartCoverAlertAlarmTypeRepository = smartCoverAlertAlarmTypeRepository;
            _smartCoverAlertSmartCoverAlertTypeRepository = smartCoverAlertSmartCoverAlertTypeRepository;
            _smartCoverAlertTypeRepository = smartCoverAlertTypeRepository;
            _httpClient = httpClient.Build();
        }

        #endregion

        #region Public Methods

        public void Process()
        {
            _httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["AwAppsApiBaseUrl"]);
            _httpClient.DefaultRequestHeaders?.Clear();
            _httpClient.DefaultRequestHeaders?.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders?.Add("x-api-key",
                ConfigurationManager.AppSettings["SmartCoverAlertsKey"]);

            AcknowledgeAlerts().Wait();
            GetData().Wait();

            _httpClient.Dispose();
        }

        #endregion

        #region Private Methods

        private async Task GetData()
        {
            try
            {
                using (HttpResponseMessage response =
                    await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, LOCATIONS_LIST_WEB_METHOD)))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var alertResponse = response.Content.ReadAsStringAsync().Result;
                        var result = JsonConvert.DeserializeObject<SmartCoverAlertResult>(alertResponse);

                        if (result != null && result.Locations != null)
                        {
                            string lastReadingVaue = string.Empty;

                            foreach (var record in result.Locations)
                            {
                                var entity = _smartCoverAlertRepository
                                            .Where(x => x.AlertId == Convert.ToInt32(record.Id)).FirstOrDefault();

                                if (entity == null)
                                {
                                    _log.Info($"Creating Entity for Alert {record.Id}");

                                    entity = new MapCall.Common.Model.Entities.SmartCoverAlert();
                                }
                                else
                                {
                                    _log.Info($"Updating Entity for Alert {record.Id}");
                                }

                                entity.AlertId = Convert.ToInt32(record.Id);
                                entity.SewerOpening = string.IsNullOrWhiteSpace(record.CustomKey)
                                    ? null
                                    : new SewerOpening { Id = int.Parse(record.CustomKey) };
                                entity.SewerOpeningNumber = record.Description;
                                entity.Latitude = record.Latitude;
                                entity.Longitude = record.Longitude;
                                entity.Elevation = record.Elevation;
                                entity.SensorToBottom = record.SensorToBottom;
                                entity.ManholeDepth = record.ManholeDepth;
                                entity.DateReceived = DateTime.Now;
                                entity.HighAlarmThreshold = record.HighAlarmThreshold;

                                foreach (var dataType in record.DataTypes)
                                {
                                    if (dataType.LastReading != null && dataType.LastReading.Length == 2)
                                    {
                                        if (dataType.LastReading[1] != null)
                                        {
                                            lastReadingVaue = Convert.ToString(dataType.LastReading[1]);
                                        }
                                    }

                                    switch (dataType.Description.ToLower().Trim())
                                    {
                                        case "powerpack voltage":
                                            entity.PowerPackVoltage = lastReadingVaue;
                                            break;
                                        case "water level above bottom":
                                            entity.WaterLevelAboveBottom = lastReadingVaue;
                                            break;
                                        case "temperature":
                                            entity.Temperature = lastReadingVaue;
                                            break;
                                        case "signal strength":
                                            entity.SignalStrength = lastReadingVaue;
                                            break;
                                        case "signal quality":
                                            entity.SignalQuality = lastReadingVaue;
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                var applicationDescriptionEntity = _smartCoverAlertApplicationDescriptionTypeRepository
                                                                  .Where(x => x.Description == record.ApplicationDescription)
                                                                  .FirstOrDefault();

                                if (applicationDescriptionEntity == null)
                                {
                                    applicationDescriptionEntity = new SmartCoverAlertApplicationDescriptionType {
                                        Description = record.ApplicationDescription
                                    };

                                    _smartCoverAlertApplicationDescriptionTypeRepository.Save(
                                        applicationDescriptionEntity);
                                }

                                entity.ApplicationDescription = applicationDescriptionEntity;

                                _smartCoverAlertRepository.Save(entity);

                                SaveAlarms(record.Alarms, entity, record);

                                SaveAlerts(record.Alerts, entity, record);

                                _log.Info($"Saved Alert {record.Id}");
                            }

                            _log.Info("Saved all Alerts");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _log.Error(e.ToString());
            }
        }

        private void SaveAlarms(SmartCoverAlertResultAlarms[] alarms, MapCall.Common.Model.Entities.SmartCoverAlert entity, SmartCoverAlertResultLocations alert)
        {
            if (alarms != null && alarms.Any())
            {
                foreach (var alarm in alarms)
                {
                    var alarmEntity =
                        _smartCoverAlertAlarmRepository.Where(x => x.AlarmId == int.Parse(alarm.Id)
                            && x.SmartCoverAlert.Id == entity.Id).FirstOrDefault();

                    if (alarmEntity == null)
                    {
                        _log.Info($"Creating Entity for Alarm - {alarm.Id} Alert - {alert.Id}");

                        alarmEntity = new SmartCoverAlertAlarm();
                    }
                    else
                    {
                        _log.Info($"Updating Entity for Alarm - {alarm.Id} Alert - {alert.Id}");
                    }

                    alarmEntity.AlarmId = int.Parse(alarm.Id);
                    alarmEntity.AlarmDate = DateTime.Parse(alarm.Date);
                    alarmEntity.Value = (decimal)alarm.Value;
                    alarmEntity.SmartCoverAlert = entity;
                    alarmEntity.Level = (decimal)(alert.SensorToBottom - alarm.Value);

                    var alarmTypeEntity = _smartCoverAlertAlarmTypeRepository
                                           .Where(x => x.Description == alarm.Description)
                                           .FirstOrDefault();

                    if (alarmTypeEntity == null)
                    {
                        alarmTypeEntity = new SmartCoverAlertAlarmType {
                            Description = alarm.Description
                        };

                        _smartCoverAlertAlarmTypeRepository.Save(alarmTypeEntity);
                    }

                    alarmEntity.AlarmType = alarmTypeEntity;

                    _smartCoverAlertAlarmRepository.Save(alarmEntity);
                }
            }
        }

        private void SaveAlerts(SmartCoverAlertResultAlerts[] alerts, MapCall.Common.Model.Entities.SmartCoverAlert entity, SmartCoverAlertResultLocations alert)
        {
            if (alerts != null && alerts.Any())
            {
                var alertTypes = _smartCoverAlertTypeRepository.GetAll().ToList();

                foreach (var item in alerts)
                {
                    var smartCoverAlertSmartCoverAlertTypeEntity =
                        _smartCoverAlertSmartCoverAlertTypeRepository.Where(x => x.SmartCoverAlertType.Id == int.Parse(item.Type)
                            && x.SmartCoverAlert.Id == entity.Id).FirstOrDefault();

                    if (smartCoverAlertSmartCoverAlertTypeEntity == null)
                    {
                        _log.Info($"Creating Entity for Alert - {item.Type} Alert - {alert.Id}");

                        smartCoverAlertSmartCoverAlertTypeEntity = new SmartCoverAlertSmartCoverAlertType {
                            SmartCoverAlert = entity
                        };

                        var alertTypeEntity = alertTypes.FirstOrDefault(x => x.Id == int.Parse(item.Type));

                        if (alertTypeEntity == null)
                        {
                            alertTypeEntity = new SmartCoverAlertType { Description = item.Description };

                            _smartCoverAlertTypeRepository.Save(alertTypeEntity);
                        }

                        smartCoverAlertSmartCoverAlertTypeEntity.SmartCoverAlertType = alertTypeEntity;

                        _smartCoverAlertSmartCoverAlertTypeRepository.Save(smartCoverAlertSmartCoverAlertTypeEntity);
                    }
                }
            }
        }

        private async Task AcknowledgeAlerts()
        {
            try
            {
                var alerts = _smartCoverAlertRepository.Where(x => x.Acknowledged && x.NeedsToSync).ToList();

                foreach (var smartCoverAlert in alerts)
                {
                    _log.Info($"Acknowledging Alert {smartCoverAlert.AlertId}");

                    var form = new MultipartFormDataContent {
                        { new StringContent(smartCoverAlert.AlertId.ToString()), "location" }
                    };

                    using (HttpResponseMessage response = await _httpClient.PostAsync(ACKNOWLEDGE_ALERTS_WEB_METHOD, form))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var acknowledgeResponse = response.Content.ReadAsStringAsync().Result;
                            var result = JsonConvert.DeserializeObject<SmartCoverAlertAcknowledgeResult>(acknowledgeResponse);

                            if (result.ResponseCode == 0)
                            {
                                smartCoverAlert.NeedsToSync = false;
                                smartCoverAlert.LastSyncedAt = DateTime.Now;
                                _smartCoverAlertRepository.Save(smartCoverAlert);
                            }
                            else
                            {
                                _log.Info(result.ResponseText);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _log.Error(e.ToString());
            }
        }

        #endregion
    }
}
