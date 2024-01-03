using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class SensorRepositoryTest : MapCallMvcInMemoryDatabaseTestBase<Sensor, SensorRepository>
    {
        #region Tests

        [TestMethod]
        public void TestLinqPropertyQueriesEfficiently()
        {
            GetEntityFactory<Sensor>().Create();

            // Evict everything to ensure we're querying the database.
            // Cause we don't want a cache of the Project, Site, Board, or EquipmentSensor either.
            Session.Clear();

            Interceptor.Init();
            // Requery
            // Need to use GetAll since Find uses Session.Get instead of Linq/Criteria.
            var resensor = Repository.GetAll().Single();

            Assert.AreEqual(1, Interceptor.PreparedStatements.Count,
                "There should only have been a single select statement.");

            Assert.IsTrue(Interceptor.PreparedStatements.Single().StartsWithCaseInsensitive("select sensor"),
                "Sanity check, make sure we're got something resembling the correct statement.");
            Assert.IsTrue(Interceptor.PreparedStatements.Single().ToString().Contains("from Sensors"),
                "Sanity check, make sure we're got something resembling the correct statement.");

            // Call one of the lazyloaded refs so Nhibernate does its lazy loading.
            // If the Linq fetching was done correctly, nothing should have happened.
            var lazy = resensor.ToString(); // ToString calls all sorts of lazy things.

            Assert.AreEqual(1, Interceptor.PreparedStatements.Count, "No additional selects should have been made.");
        }

        [TestMethod]
        public void TestGetReadingsReturnsAllReadingsForTheGivenSensorsAndTheGivenDateRange()
        {
            var sensorOne = GetFactory<SensorFactory>().Create(new {Name = "Sensor One"});
            var sensorTwo = GetFactory<SensorFactory>().Create(new {Name = "Sensor Two"});
            // Make some readings
            var startDate = new DateTime(2014, 1, 1, 0, 0, 0);
            var endDate = startDate.AddDays(1);

            var sensOneReadGood =
                GetFactory<ReadingFactory>().Create(new {Sensor = sensorOne, DateTimeStamp = startDate});
            var sensOneReadBad = GetFactory<ReadingFactory>().Create(new {Sensor = sensorOne, DateTimeStamp = endDate});
            var sensTwoReadGood =
                GetFactory<ReadingFactory>().Create(new {Sensor = sensorTwo, DateTimeStamp = startDate});
            var sensTwoReadBad = GetFactory<ReadingFactory>().Create(new {Sensor = sensorTwo, DateTimeStamp = endDate});

            Assert.IsTrue(Repository.GetReadings(new[] {sensorOne}, startDate, endDate).Contains(sensOneReadGood));
            Assert.IsFalse(Repository.GetReadings(new[] {sensorOne}, startDate, endDate).Contains(sensTwoReadGood));
            Assert.IsFalse(Repository.GetReadings(new[] {sensorOne}, startDate, endDate).Contains(sensOneReadBad));
            Assert.IsFalse(Repository.GetReadings(new[] {sensorOne}, startDate, endDate).Contains(sensTwoReadBad));

            Assert.IsFalse(Repository.GetReadings(new[] {sensorTwo}, startDate, endDate).Contains(sensOneReadGood));
            Assert.IsTrue(Repository.GetReadings(new[] {sensorTwo}, startDate, endDate).Contains(sensTwoReadGood));
            Assert.IsFalse(Repository.GetReadings(new[] {sensorTwo}, startDate, endDate).Contains(sensOneReadBad));
            Assert.IsFalse(Repository.GetReadings(new[] {sensorTwo}, startDate, endDate).Contains(sensTwoReadBad));

            Assert.IsTrue(Repository.GetReadings(new[] {sensorOne, sensorTwo}, startDate, endDate)
                                    .Contains(sensOneReadGood));
            Assert.IsTrue(Repository.GetReadings(new[] {sensorOne, sensorTwo}, startDate, endDate)
                                    .Contains(sensTwoReadGood));
            Assert.IsFalse(Repository.GetReadings(new[] {sensorOne, sensorTwo}, startDate, endDate)
                                     .Contains(sensOneReadBad));
            Assert.IsFalse(Repository.GetReadings(new[] {sensorOne, sensorTwo}, startDate, endDate)
                                     .Contains(sensTwoReadBad));
        }

        #region Kilowatt

        [TestMethod]
        public void TestGetGroupedReadingCalculationsForDailyKilowattReadings()
        {
            var kiloWatt = GetFactory<KilowattSensorMeasurementTypeFactory>().Create();

            var sensorOne = GetFactory<SensorFactory>().Create(new {Name = "Sensor One", MeasurementType = kiloWatt});
            var sensorTwo = GetFactory<SensorFactory>().Create(new {Name = "Sensor Two", MeasurementType = kiloWatt});
            // Make some readings
            var startDate = new DateTime(2014, 1, 1, 0, 0, 0);
            var endDate = startDate.AddDays(2); // So the range includes startDate + one day. 

            // Day one
            GetFactory<ReadingFactory>().Create(new {Sensor = sensorOne, DateTimeStamp = startDate, ScaledData = 1001});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddMinutes(15), ScaledData = 2002});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddMinutes(75), ScaledData = 2002});
            GetFactory<ReadingFactory>().Create(new {Sensor = sensorTwo, DateTimeStamp = startDate, ScaledData = 5005});
            GetFactory<ReadingFactory>().Create(new {Sensor = sensorTwo, DateTimeStamp = startDate, ScaledData = 6006});

            // Day two
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddDays(1), ScaledData = 3003});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddDays(1), ScaledData = 4004});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorTwo, DateTimeStamp = startDate.AddDays(1), ScaledData = 7007});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorTwo, DateTimeStamp = startDate.AddDays(1), ScaledData = 8008});

            // Bad days
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddDays(-1), ScaledData = 1001});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddDays(2), ScaledData = 1001});

            var result = Repository.GetGroupedReadingCalculations(new[] {sensorOne, sensorTwo}, ReadingGroupType.Daily,
                startDate, endDate, true);
            Assert.AreEqual(4004d, result.Totals[startDate]);
            Assert.AreEqual(5505.5d, result.Totals[startDate.AddDays(1)]);
            Assert.IsFalse(result.Calculations.Any(x => x.Date == startDate.AddDays(-1)));
            Assert.IsFalse(result.Calculations.Any(x => x.Date == startDate.AddDays(2)));
        }

        [TestMethod]
        public void TestGetGroupedReadingCalculationsForHourlyKilowattReadings()
        {
            var kiloWatt = GetFactory<KilowattSensorMeasurementTypeFactory>().Create();

            var sensorOne = GetFactory<SensorFactory>().Create(new {Name = "Sensor One", MeasurementType = kiloWatt});
            // Make some readings
            var startDate = new DateTime(2014, 1, 1, 0, 0, 0);
            var endDate = startDate.AddDays(2); // So the range includes startDate + one day. 

            // First hour
            GetFactory<ReadingFactory>().Create(new {Sensor = sensorOne, DateTimeStamp = startDate, ScaledData = 1000});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddMinutes(15), ScaledData = 2000});
            // Second hour
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddMinutes(75), ScaledData = 4000});

            var result = Repository.GetGroupedReadingCalculations(new[] {sensorOne}, ReadingGroupType.Hourly, startDate,
                endDate, true);
            Assert.AreEqual(750d, result.Totals[startDate]);
            Assert.AreEqual(1000d, result.Totals[startDate.AddHours(1)]);
        }

        [TestMethod]
        public void TestGetGroupedReadingCalculationsForQuarterHourlyKilowattReadings()
        {
            var kiloWatt = GetFactory<KilowattSensorMeasurementTypeFactory>().Create();

            var sensorOne = GetFactory<SensorFactory>().Create(new {Name = "Sensor One", MeasurementType = kiloWatt});
            // Make some readings
            var startDate = new DateTime(2014, 1, 1, 0, 0, 0);
            var endDate = startDate.AddDays(2); // So the range includes startDate + one day. 

            // First hour
            GetFactory<ReadingFactory>().Create(new {Sensor = sensorOne, DateTimeStamp = startDate, ScaledData = 1000});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddMinutes(15), ScaledData = 2000});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddMinutes(75), ScaledData = 4000});

            var result = Repository.GetGroupedReadingCalculations(new[] {sensorOne}, ReadingGroupType.QuarterHour,
                startDate, endDate, true);
            Assert.AreEqual(250d, result.Totals[startDate]);
            Assert.AreEqual(500d, result.Totals[startDate.AddMinutes(15)]);
            Assert.AreEqual(1000d, result.Totals[startDate.AddMinutes(75)]);
        }

        #endregion

        #region Watts

        [TestMethod]
        public void TestGetGroupedReadingCalculationsForDailyWattReadings()
        {
            var watt = GetFactory<WattSensorMeasurementTypeFactory>().Create();

            var sensorOne = GetFactory<SensorFactory>().Create(new {Name = "Sensor One", MeasurementType = watt});
            var sensorTwo = GetFactory<SensorFactory>().Create(new {Name = "Sensor Two", MeasurementType = watt});
            // Make some readings
            var startDate = new DateTime(2014, 1, 1, 0, 0, 0);
            var endDate = startDate.AddDays(2); // So the range includes startDate + one day. 

            // Day one
            GetFactory<ReadingFactory>().Create(new {Sensor = sensorOne, DateTimeStamp = startDate, ScaledData = 1001});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddMinutes(15), ScaledData = 2002});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddMinutes(75), ScaledData = 2002});
            GetFactory<ReadingFactory>()
               .Create(new {Sensor = sensorTwo, DateTimeStamp = startDate, ScaledData = -5005});
            GetFactory<ReadingFactory>().Create(new {Sensor = sensorTwo, DateTimeStamp = startDate, ScaledData = 6006});

            // Day two
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddDays(1), ScaledData = 3003});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddDays(1), ScaledData = 4004});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorTwo, DateTimeStamp = startDate.AddDays(1), ScaledData = -7007});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorTwo, DateTimeStamp = startDate.AddDays(1), ScaledData = 8008});

            // Bad days
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddDays(-1), ScaledData = 1001});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddDays(2), ScaledData = -1001});

            var result = Repository.GetGroupedReadingCalculations(new[] {sensorOne, sensorTwo}, ReadingGroupType.Daily,
                startDate, endDate, true);
            Assert.AreEqual(4.004d, result.Totals[startDate]);
            Assert.AreEqual(5.5055d, result.Totals[startDate.AddDays(1)]);
            Assert.IsFalse(result.Calculations.Any(x => x.Date == startDate.AddDays(-1)));
            Assert.IsFalse(result.Calculations.Any(x => x.Date == startDate.AddDays(2)));
        }

        [TestMethod]
        public void TestGetGroupedReadingCalculationsForHourlyWattReadings()
        {
            var watt = GetFactory<WattSensorMeasurementTypeFactory>().Create();

            var sensorOne = GetFactory<SensorFactory>().Create(new {Name = "Sensor One", MeasurementType = watt});
            // Make some readings
            var startDate = new DateTime(2014, 1, 1, 0, 0, 0);
            var endDate = startDate.AddDays(2); // So the range includes startDate + one day. 

            // First hour
            GetFactory<ReadingFactory>().Create(new {Sensor = sensorOne, DateTimeStamp = startDate, ScaledData = 1000});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddMinutes(15), ScaledData = -2000});
            // Second hour
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddMinutes(75), ScaledData = 4000});

            var result = Repository.GetGroupedReadingCalculations(new[] {sensorOne}, ReadingGroupType.Hourly, startDate,
                endDate, true);
            Assert.AreEqual(0.750d, result.Totals[startDate]);
            Assert.AreEqual(1.000d, result.Totals[startDate.AddHours(1)]);
        }

        [TestMethod]
        public void TestGetGroupedReadingCalculationsForQuarterHourlyWattReadings()
        {
            var watt = GetFactory<WattSensorMeasurementTypeFactory>().Create();

            var sensorOne = GetFactory<SensorFactory>().Create(new {Name = "Sensor One", MeasurementType = watt});
            // Make some readings
            var startDate = new DateTime(2014, 1, 1, 0, 0, 0);
            var endDate = startDate.AddDays(2); // So the range includes startDate + one day. 

            // First hour
            GetFactory<ReadingFactory>().Create(new {Sensor = sensorOne, DateTimeStamp = startDate, ScaledData = 1000});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddMinutes(15), ScaledData = 2000});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddMinutes(75), ScaledData = -4000});

            var result = Repository.GetGroupedReadingCalculations(new[] {sensorOne}, ReadingGroupType.QuarterHour,
                startDate, endDate, true);
            Assert.AreEqual(0.250d, result.Totals[startDate]);
            Assert.AreEqual(0.500d, result.Totals[startDate.AddMinutes(15)]);
            Assert.AreEqual(1.000d, result.Totals[startDate.AddMinutes(75)]);
        }

        #endregion

        #region KiloWatt Hours

        [TestMethod]
        public void TestGetGroupedReadingCalculationsForDailyKilowattHourReadings()
        {
            var kwh = GetFactory<KilowattHoursSensorMeasurementTypeFactory>().Create();

            var sensorOne = GetFactory<SensorFactory>().Create(new {Name = "Sensor One", MeasurementType = kwh});
            var sensorTwo = GetFactory<SensorFactory>().Create(new {Name = "Sensor Two", MeasurementType = kwh});
            // Make some readings
            var startDate = new DateTime(2014, 1, 1, 0, 0, 0);
            var endDate = startDate.AddDays(2); // So the range includes startDate + one day. 

            // Day one
            GetFactory<ReadingFactory>().Create(new {Sensor = sensorOne, DateTimeStamp = startDate, ScaledData = 1001});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddMinutes(15), ScaledData = 2002});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddMinutes(75), ScaledData = 2002});
            GetFactory<ReadingFactory>()
               .Create(new {Sensor = sensorTwo, DateTimeStamp = startDate, ScaledData = -5005});
            GetFactory<ReadingFactory>().Create(new {Sensor = sensorTwo, DateTimeStamp = startDate, ScaledData = 6006});

            // Day two
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddDays(1), ScaledData = 3003});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddDays(1), ScaledData = 4004});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorTwo, DateTimeStamp = startDate.AddDays(1), ScaledData = -7007});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorTwo, DateTimeStamp = startDate.AddDays(1), ScaledData = 8008});

            // Bad days
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddDays(-1), ScaledData = 1001});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddDays(2), ScaledData = -1001});

            var result = Repository.GetGroupedReadingCalculations(new[] {sensorOne, sensorTwo}, ReadingGroupType.Daily,
                startDate, endDate, true);
            Assert.AreEqual(16016, result.Totals[startDate]);
            Assert.AreEqual(22022, result.Totals[startDate.AddDays(1)]);
            Assert.IsFalse(result.Calculations.Any(x => x.Date == startDate.AddDays(-1)));
            Assert.IsFalse(result.Calculations.Any(x => x.Date == startDate.AddDays(2)));
        }

        [TestMethod]
        public void TestGetGroupedReadingCalculationsForHourlyKilowattHoursReadings()
        {
            var kwh = GetFactory<KilowattHoursSensorMeasurementTypeFactory>().Create();

            var sensorOne = GetFactory<SensorFactory>().Create(new {Name = "Sensor One", MeasurementType = kwh});
            // Make some readings
            var startDate = new DateTime(2014, 1, 1, 0, 0, 0);
            var endDate = startDate.AddDays(2); // So the range includes startDate + one day. 

            // First hour
            GetFactory<ReadingFactory>().Create(new {Sensor = sensorOne, DateTimeStamp = startDate, ScaledData = 1000});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddMinutes(15), ScaledData = -2000});
            // Second hour
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddMinutes(75), ScaledData = 4000});

            var result = Repository.GetGroupedReadingCalculations(new[] {sensorOne}, ReadingGroupType.Hourly, startDate,
                endDate, true);
            Assert.AreEqual(3000, result.Totals[startDate]);
            Assert.AreEqual(4000, result.Totals[startDate.AddHours(1)]);
        }

        [TestMethod]
        public void TestGetGroupedReadingCalculationsForQuarterHourlyKilowattHoursReadings()
        {
            var kwh = GetFactory<KilowattHoursSensorMeasurementTypeFactory>().Create();

            var sensorOne = GetFactory<SensorFactory>().Create(new {Name = "Sensor One", MeasurementType = kwh});
            // Make some readings
            var startDate = new DateTime(2014, 1, 1, 0, 0, 0);
            var endDate = startDate.AddDays(2); // So the range includes startDate + one day. 

            // First hour
            GetFactory<ReadingFactory>().Create(new {Sensor = sensorOne, DateTimeStamp = startDate, ScaledData = 1000});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddMinutes(15), ScaledData = 2000});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddMinutes(75), ScaledData = -4000});

            var result = Repository.GetGroupedReadingCalculations(new[] {sensorOne}, ReadingGroupType.QuarterHour,
                startDate, endDate, true);
            Assert.AreEqual(1000, result.Totals[startDate]);
            Assert.AreEqual(2000, result.Totals[startDate.AddMinutes(15)]);
            Assert.AreEqual(4000, result.Totals[startDate.AddMinutes(75)]);
        }

        #endregion

        #endregion
    }
}
