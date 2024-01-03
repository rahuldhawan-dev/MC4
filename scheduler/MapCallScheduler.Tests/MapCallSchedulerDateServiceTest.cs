using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using Quartz.Impl.AdoJobStore;
using StructureMap;

namespace MapCallScheduler.Tests
{
    [TestClass]
    public class MapCallSchedulerDateServiceTest
    {
        #region Private Members

        private Mock<IMapCallSchedulerConfiguration> _config;
        private TestDateTimeProvider _dateTimeProvider;
        private MapCallSchedulerDateService _target;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject((_config = new Mock<IMapCallSchedulerConfiguration>()).Object);
            _container.Inject<IDateTimeProvider>(_dateTimeProvider = new TestDateTimeProvider());

            _target = _container.GetInstance<MapCallSchedulerDateService>();
        }

        #endregion

        private MapCallSchedulerDateService ResetService(string startTime = null)
        {
            _config.SetupGet(x => x.StartTime).Returns(startTime);
            return _target;
        }

        [TestMethod]
        public void TestGetStartDateTimeWorksProperly()
        {
            var today = DateTime.Now.Date;

            _dateTimeProvider.SetNow(today);

            _config.SetupGet(x => x.StartTime).Returns("IMMEDIATE");

            Assert.AreEqual(today.AddSeconds(1), _target.GetStartDateTime());

            _config.SetupGet(x => x.StartTime).Returns("12");

            Assert.AreEqual(today.AddHours(12), _target.GetStartDateTime());

            _config.SetupGet(x => x.StartTime).Returns("12:30");

            Assert.AreEqual(today.AddHours(12).AddMinutes(30), _target.GetStartDateTime());

            MyAssert.Throws<InvalidConfigurationException>(() => ResetService("8").GetStartDateTime());
            MyAssert.Throws<InvalidConfigurationException>(() => ResetService("08:3").GetStartDateTime());
            MyAssert.Throws<InvalidConfigurationException>(() => ResetService("24").GetStartDateTime());
            MyAssert.Throws<InvalidConfigurationException>(() => ResetService("00:60").GetStartDateTime());

            for (var i = 0; i < 24; i++)
            {
                var hour = i.ToString().PadLeft(2, '0');
                MyAssert.DoesNotThrow(() => ResetService(hour).GetStartDateTime());
                for (var j = 0; j < 60; j++)
                {
                    var time = hour + ":" + j.ToString().PadLeft(2, '0');
                    MyAssert.DoesNotThrow(() => ResetService(time).GetStartDateTime());
                }
            }
        }
    }
}