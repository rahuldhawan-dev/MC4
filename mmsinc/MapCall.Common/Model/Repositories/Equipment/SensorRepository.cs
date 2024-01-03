using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Linq;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public enum ReadingGroupType
    {
        /// <summary>
        /// Return reading calculations that are grouped by 15 minute intervals.
        /// </summary>
        QuarterHour,

        /// <summary>
        /// Return reading calculations that are grouped by hour.
        /// </summary>
        Hourly,

        /// <summary>
        /// Return reading calculations that are grouped by day.
        /// </summary>
        Daily
    }

    public interface ISensorRepository : IRepository<Sensor>
    {
        IEnumerable<Reading> GetReadings(IEnumerable<Sensor> sensors, DateTime startDate, DateTime endDate);

        ReadingCalculationSummary GetGroupedReadingCalculations(IEnumerable<Sensor> sensors, ReadingGroupType groupType,
            DateTime startDate, DateTime endDate, bool includeReadingEntries = false);
    }

    public class SensorRepository : RepositoryBase<Sensor>, ISensorRepository
    {
        #region Consts

        private const int ROUNDING_DECIMAL_PLACES = 2;

        #endregion

        #region Constructor

        public SensorRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Properties

        public override IQueryable<Sensor> Linq
        {
            get
            {
                // The fetching preloads all of the stuff needed for Sensor.ToString() to work
                // and not end up firing off 6000 individual sql commands.
                return base.Linq
                            // Also if you don't fetch Equipment, every time Sensor.ToString is called
                            // it fires off a search for EquipmentSensor. God forbid NHibernate had a
                            // way to specify a property is lazy until you explicitly access it(rather
                            // than when you access any lazy property it then loads all of them).
                           .Fetch(x => x.Equipment)
                           .Fetch(x => x.Board)
                           .ThenFetch(x => x.Site)
                           .ThenFetch(x => x.Project);
            }
        }

        #endregion

        #region Private Methods

        protected IQueryable<Reading> GetQueryableReadingsForSensors(IEnumerable<Sensor> sensors, DateTime startDate,
            DateTime endDate)
        {
            return Session.Query<Reading>()
                          .Where(x => sensors.Contains(x.Sensor))
                          .Where(x => startDate <= x.DateTimeStamp && x.DateTimeStamp < endDate);
        }

        private IEnumerable<ReadingCalculation> GetGroupedReadingsSummary(Sensor sensor, ReadingGroupType groupType,
            DateTime startDate, DateTime endDate)
        {
            var readings = GetQueryableReadingsForSensors(new[] {sensor}, startDate, endDate);

            IQueryable<IGrouping<DateTime, Reading>> group;

            switch (groupType)
            {
                case ReadingGroupType.QuarterHour:
                    // Sensor readings are inserted to the db in 15 minute intervals already.
                    group = readings.GroupBy(x => x.DateTimeStamp);
                    break;

                case ReadingGroupType.Hourly:
                    // Nhibernate can not properly generate a group by statement when using
                    // the DateTime constructor. This is calling ToArray in order to read the
                    // hourly stuff into memory before continuing with the calculations. This
                    // should not be an issue since hourly calculations are only done for the
                    // span of a day, so not too many rows will need to be read.
                    group = readings.ToArray().AsQueryable().GroupBy(x =>
                        new DateTime(x.DateTimeStamp.Year, x.DateTimeStamp.Month, x.DateTimeStamp.Day,
                            x.DateTimeStamp.Hour, 0, 0));
                    break;

                case ReadingGroupType.Daily:
                    group = readings.GroupBy(x => x.DateTimeStamp.Date);
                    break;

                default:
                    throw new NotSupportedException(groupType.ToString());
            }

            var measType = (readings.Any())
                ? readings.First().Sensor.MeasurementType.Description.ToLower()
                : string.Empty;
            switch (measType)
            {
                case SensorMeasurementType.Descriptions.KILOWATT:
                    // Working on the assumption that all sensor readings for the "kw" sensors
                    // are recording data in 15 minute intervals. 

                    // kWh = kw * hours(so 0.25 = 15 minutes)
                    return
                        group.Select(
                                  x =>
                                      new ReadingCalculation {
                                          Date = x.Key,
                                          Sensor = sensor,
                                          Value = x.Sum(y => y.ScaledData) * 0.25
                                      })
                             .ToList();

                case SensorMeasurementType.Descriptions.WATT:
                    // Working on the assumption that all sensor readings for the "watt" sensors
                    // are recording data in 15 minute intervals. 

                    // kWh = w * hours(so 0.25 = 15 minutes) / 1000
                    return
                        group.Select(
                                  x =>
                                      new ReadingCalculation {
                                          Date = x.Key,
                                          Sensor = sensor,
                                          Value = x.Sum(y => Math.Abs(y.ScaledData)) * 0.25 / 1000
                                      })
                             .ToList();

                case SensorMeasurementType.Descriptions.KILOWATT_HOURS:
                    return
                        group.Select(
                                  x =>
                                      new ReadingCalculation {
                                          Date = x.Key,
                                          Sensor = sensor,
                                          Value = x.Sum(y => Math.Abs(y.ScaledData))
                                      })
                             .ToList();

                default:
                    throw new NotSupportedException(measType);
            }
        }

        #endregion

        #region Public Methods

        public IEnumerable<Reading> GetReadings(IEnumerable<Sensor> sensors, DateTime startDate, DateTime endDate)
        {
            return GetQueryableReadingsForSensors(sensors, startDate, endDate);
        }

        /// <summary>
        /// Returns calculations for a group of sensors where the values are assumed to be of a specific SensorMeasurementType. 
        /// </summary>
        /// <param name="sensors">The sensors used to get reading calculations.</param>
        /// <param name="groupType">How the values should be grouped together for calculations.</param>
        /// <param name="measType">The unit of measure all the sensor readings should be calculated as.</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="includeReadingEntries">Set to true if you want all the Reading objects returned. You more than likely do not want them. False by default.</param>
        /// <returns></returns>
        public ReadingCalculationSummary GetGroupedReadingCalculations(IEnumerable<Sensor> sensors,
            ReadingGroupType groupType,
            DateTime startDate, DateTime endDate, bool includeReadingEntries = false)
        {
            var summary = new ReadingCalculationSummary();
            if (includeReadingEntries)
            {
                summary.Readings = GetReadings(sensors, startDate, endDate);
            }

            // NHibernate doesn't like doing GroupBy(x => new { multiple columns }) so we have to go the slightly longer
            // route of querying for each sensor individually. 
            summary.Calculations = sensors.SelectMany(x => GetGroupedReadingsSummary(x, groupType, startDate, endDate))
                                          .ToArray();
            summary.Totals = summary.Calculations
                                    .GroupBy(x => x.Date)
                                    .ToDictionary(x => x.Key, x => x.Sum(y => y.Value));

            return summary;
        }

        #endregion
    }
}
