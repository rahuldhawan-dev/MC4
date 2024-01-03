using System;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Metadata;
using NHibernate.Linq.Visitors;

namespace MMSINC.Data
{
    public class RecurrenceFrequency
    {
        #region Properties

        public int? Frequency { get; set; }

        public string FrequencyStr
        {
            get { return TryConvertFrequencyToString(Frequency); }
        }

        public FrequencyUnit? Unit { get; set; }

        public string UnitStr
        {
            get { return TryConvertFrequencyUnitToString(Unit); }
        }

        #endregion

        public RecurrenceFrequency() { }

        public RecurrenceFrequency(string frequency, string unit) : this(TryConvertFrequencyToInt(frequency), unit) { }

        public RecurrenceFrequency(int? frequency, string unit)
        {
            Frequency = frequency;
            Unit = TryConvertFrequencyUnitToEnum(unit);
        }

        private static int? TryConvertFrequencyToInt(string freq)
        {
            if (string.IsNullOrWhiteSpace(freq))
            {
                return null;
            }

            return int.Parse(freq);
        }

        private static string TryConvertFrequencyToString(int? freq)
        {
            if (freq.HasValue)
            {
                return freq.Value.ToString();
            }

            return null;
        }

        private static FrequencyUnit? TryConvertFrequencyUnitToEnum(string freqyDeqyUnit)
        {
            switch (freqyDeqyUnit)
            {
                case "Y":
                    return FrequencyUnit.Year;
                case "M":
                    return FrequencyUnit.Month;
                case "W":
                    ;
                    return FrequencyUnit.Week;
                case "D":
                    return FrequencyUnit.Day;
                default:
                    return null;
            }
        }

        private static string TryConvertFrequencyUnitToString(FrequencyUnit? freqUnit)
        {
            switch (freqUnit)
            {
                case FrequencyUnit.Year:
                    return "Y";
                case FrequencyUnit.Month:
                    return "M";
                case FrequencyUnit.Week:
                    return "W";
                case FrequencyUnit.Day:
                    return "D";
                default:
                    return null;
            }
        }

        public override string ToString()
        {
            return FrequencyStr + " " + Unit.ToString().Pluralize(Frequency == 1);
        }

        /// <summary>
        /// When is the expiration date given a specific date.
        /// </summary>
        /// <param name="date">Date you want to check the frequency for</param>
        /// <returns></returns>
        public DateTime GetExpirationDateFrom(DateTime date)
        {
            switch (Unit)
            {
                case FrequencyUnit.Day:
                    return date.AddDays(-(int)Frequency);
                case FrequencyUnit.Week:
                    return date.AddWeeks(-(int)Frequency);
                case FrequencyUnit.Month:
                    return date.AddMonths(-(int)Frequency);
                case FrequencyUnit.Year:
                    return date.AddYears(-(int)Frequency);
                default:
                    throw new InvalidOperationException("A proper frequency was not provided.");
            }
        }
    }

    public enum FrequencyUnit
    {
        Day,
        Week,
        Month,
        Year
    }
}
