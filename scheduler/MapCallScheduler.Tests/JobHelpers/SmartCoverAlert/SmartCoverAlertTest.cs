using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using log4net;
using MapCall.Common.Model.Entities;
using MapCallScheduler.JobHelpers.SmartCoverAlert;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using SmartCoverAlertEntity = MapCall.Common.Model.Entities.SmartCoverAlert;

namespace MapCallScheduler.Tests.JobHelpers.SmartCoverAlert
{
    [TestClass]
    public class SmartCoverAlertTest : InMemoryDatabaseTest<MapCall.Common.Model.Entities.SmartCoverAlert>
    {
        #region Private Members

        private SmartCoverAlertLinkService _target;
        private Mock<ILog> _log;
        private Mock<IRepository<SmartCoverAlertEntity>> _mockSmartCoverAlertRepo;
        private Mock<IRepository<SmartCoverAlertAlarm>> _mockSmartCoverAlertAlarmRepo;
        private Mock<IRepository<SmartCoverAlertApplicationDescriptionType>>
            _mockSmartCoverAlertApplicationDescriptionTypeRepository;
        private Mock<IRepository<SmartCoverAlertAlarmType>> _mockSmartCoverAlertAlarmTypeRepository;
        private Mock<IRepository<SmartCoverAlertSmartCoverAlertType>> _mockSmartCoverAlertSmartCoverAlertTypeRepo;
        private Mock<IRepository<SmartCoverAlertType>> _mockSmartCoverAlertTypeRepo;
        private Mock<IHttpClientFactory> _httpClientFactory;
        private Mock<IHttpClient> _httpClient;

        private string sampleResponse =
            "{\"response_code\":0,\"locations\":[{\"id\":27606,\"description\":\"A105\",\"application\":5,\"application_description\":\"Collection Level\",\"latitude\":39.291097,\"longitude\":-74.553445,\"elevation\":7.4904,\"sensor_to_bottom\":16,\"manhole_depth\":80,\"distance_style\":3,\"archived\":0,\"alarm_state\":0,\"alert_state\":0,\"advisory_state\":0,\"alarms\":[{\"id\": 2755685,\"value\": -9.0635639,\"description\": \"High Level Alarm\",\"date\": \"2021-12-17 15:35\"}],\"alerts\":[{\"type\": 1,\"description\": \"Low Battery\"}],\"high_alarm_enabled\":1,\"high_alarm_threshold\":11,\"high_advisory_enabled\":1,\"high_advisory_threshold\":9,\"low_alarm_enabled\":0,\"low_alarm_threshold\":-66,\"flow_width\":8,\"data_types\":[{\"id\":1,\"description\":\"PowerPack Voltage\",\"unit\":\"V\",\"last_reading\":[\"2022-01-12 04:54\",3.44]},{\"id\":2,\"description\":\"Water Level above Bottom\",\"unit\":\"in\",\"last_reading\":[\"2022-01-12 08:54\",2.1]},{\"id\":3,\"description\":\"Temperature\",\"unit\":\"F\",\"last_reading\":[\"2022-01-12 04:54\",44.6]},{\"id\":4,\"description\":\"Signal Strength\",\"unit\":\"B\",\"last_reading\":[\"2022-01-12 04:54\",5]},{\"id\":5,\"description\":\"Signal Quality\",\"last_reading\":[\"2022-01-12 04:54\",15]}],\"alarm_holdoff\":null,\"tilt_alarm_1_enabled\":0,\"tilt_alarm_2_enabled\":0,\"tilt_recheck_enabled\":0,\"system_timing\":5}]}";
        private string sampleAcknowledgeResponse =
            "{\"response_code\":0,\"response_text\":\"Location's alarm(s) successfully acknowledged\"}";

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject((_log = new Mock<ILog>()).Object);
            _mockSmartCoverAlertRepo = new Mock<IRepository<MapCall.Common.Model.Entities.SmartCoverAlert>>();
            _container.Inject(_mockSmartCoverAlertRepo.Object);
            _mockSmartCoverAlertAlarmRepo = new Mock<IRepository<SmartCoverAlertAlarm>>();
            _container.Inject(_mockSmartCoverAlertAlarmRepo.Object);
            _mockSmartCoverAlertApplicationDescriptionTypeRepository = new Mock<IRepository<SmartCoverAlertApplicationDescriptionType>>();
            _container.Inject(_mockSmartCoverAlertApplicationDescriptionTypeRepository.Object);
            _mockSmartCoverAlertAlarmTypeRepository = new Mock<IRepository<SmartCoverAlertAlarmType>>();
            _container.Inject(_mockSmartCoverAlertAlarmTypeRepository.Object);
            _mockSmartCoverAlertSmartCoverAlertTypeRepo =
                new Mock<IRepository<SmartCoverAlertSmartCoverAlertType>>();
            _container.Inject(_mockSmartCoverAlertSmartCoverAlertTypeRepo.Object);
            _mockSmartCoverAlertTypeRepo = new Mock<IRepository<SmartCoverAlertType>>();
            _container.Inject(_mockSmartCoverAlertTypeRepo.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestProcessSavesSmartCoverAlerts()
        {
            string url = ConfigurationManager.AppSettings["AwAppsApiBaseUrl"];

            HttpResponseMessage httpResponse = new HttpResponseMessage {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(sampleResponse)
            };
            HttpResponseMessage httpAcknowledgeResponse = new HttpResponseMessage {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(sampleAcknowledgeResponse)
            };

            _httpClientFactory = new Mock<IHttpClientFactory>();
            _httpClient = _httpClientFactory.SetupMock(x => x.Build());
            _httpClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>()))
                       .ReturnsAsync(httpResponse);
            _httpClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                       .ReturnsAsync(httpAcknowledgeResponse);

            _target = new SmartCoverAlertLinkService(_log.Object, 
                _mockSmartCoverAlertRepo.Object, 
                _mockSmartCoverAlertAlarmRepo.Object,
                _mockSmartCoverAlertApplicationDescriptionTypeRepository.Object,
                _mockSmartCoverAlertAlarmTypeRepository.Object,
                _mockSmartCoverAlertSmartCoverAlertTypeRepo.Object,
                _mockSmartCoverAlertTypeRepo.Object,
                _httpClientFactory.Object);

            SmartCoverAlertEntity acknowledgeAlert = new SmartCoverAlertEntity {
                NeedsToSync = true
            };

            SmartCoverAlertEntity alert = null;

            SmartCoverAlertAlarm alarm = null;

            SmartCoverAlertSmartCoverAlertType alertType = null;

            _mockSmartCoverAlertRepo.Setup(x => x.Where(t => t.Acknowledged && t.NeedsToSync))
                                    .Returns(new[] { acknowledgeAlert }.AsQueryable());

            _mockSmartCoverAlertRepo.Setup(x => x.Save(It.IsAny<SmartCoverAlertEntity>()))
                                    .Callback<SmartCoverAlertEntity>(x => alert = x);

            _mockSmartCoverAlertAlarmRepo.Setup(x => x.Save(It.IsAny<SmartCoverAlertAlarm>()))
                                    .Callback<SmartCoverAlertAlarm>(x => alarm = x);

            _mockSmartCoverAlertSmartCoverAlertTypeRepo
               .Setup(x => x.Save(It.IsAny<SmartCoverAlertSmartCoverAlertType>()))
               .Callback<SmartCoverAlertSmartCoverAlertType>(x => alertType = x);

            _target.Process();

            Assert.IsNotNull(alert);
            Assert.IsNotNull(alarm);
            Assert.IsNotNull(alertType);
            Assert.IsNotNull(acknowledgeAlert.LastSyncedAt);
            Assert.IsFalse(acknowledgeAlert.NeedsToSync);
        }

        #endregion
    }
}
