using System;
using System.Collections.Generic;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ProductionWorkOrderFrequency : IEntity
    {
        #region Constants

        public struct Indices
        {
            public const int
                DAILY = 1,
                WEEKLY = 2,
                BI_MONTHLY = 3,
                MONTHLY = 4,
                QUARTERLY = 5,
                EVERY_FOUR_MONTHS = 6,
                BI_ANNUAL = 7,
                ANNUAL = 8,
                EVERY_TWO_YEARS = 9,
                EVERY_THREE_YEARS = 10,
                EVERY_FOUR_YEARS = 11,
                EVERY_FIVE_YEARS = 12,
                EVERY_TEN_YEARS = 13,
                EVERY_FIFTEEN_YEARS = 14,
                EVERY_TWO_MONTHS = 15;
        }

        public struct StringLengths
        {
            public const int NAME = 50,
                             ABBREVIATION = 3,
                             DESCRIPTION = 250;
        }

        public struct Display
        {
            public const string NAME = "Frequency";
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        [View(Display.NAME)]
        public virtual string Name { get; set; }

        public virtual string Abbreviation { get; set; }

        public virtual string Description { get; set; }

        [DoesNotExport]
        public virtual int SortOrder { get; set; }

        [DoesNotExport]
        public virtual int ForecastYearSpan { get; set; }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Name;
        }

        public virtual DateTime GetFrequencyNextEndDate(DateTime startDate)
        {
            return GetFrequencyNextEndDate(Id, startDate);
        }

        public static DateTime GetFrequencyNextEndDate(int frequency, DateTime startDate)
        {
            DateTime endDate = startDate;
            switch (frequency)
            {
                case Indices.DAILY:
                    // The end date for daily will be today, thus the variable initialization above takes care of this case.
                    break;
                case Indices.WEEKLY:
                    // The end date for weekly will be the next Saturday (even if today is Saturday)
                    endDate = endDate.Next(DayOfWeek.Saturday);
                    break;
                case Indices.BI_MONTHLY:
                    // The end date for bi-monthly will be either the 14th or the last day of the month.
                    if (endDate.Day != 14 && endDate.Day != startDate.GetEndOfMonth().Day)
                    {
                        endDate = startDate.Day < 15
                            ? startDate.GetBeginningOfMonth().AddDays(13)
                            : startDate.GetEndOfMonth();
                    }
                    break;
                case Indices.MONTHLY:
                    // Tne end date for monthly will be the last day of the month.
                    endDate = startDate.GetEndOfMonth();
                    break;
                case Indices.EVERY_TWO_MONTHS:
                    // The end date for every two months will be 2/28[29]/yyyy, 4/30/yyyy, 6/30/yyyy, 8/31/yyyy, 10/31/yyyy, or 12/31/yyyy.
                    endDate = new DateTime(endDate.Year, startDate.GetYearSixth() * 2, 1).GetEndOfMonth();
                    break;
                case Indices.QUARTERLY:
                    // The end date for quarterly will be the last day of the quarter. 3/31/yyyy, 6/30/yyyy, 9/30/yyyy or 12/31/yyyy.
                    endDate = new DateTime(startDate.Year, startDate.GetQuarter() * 3, 1).GetEndOfMonth();
                    break;
                case Indices.EVERY_FOUR_MONTHS:
                    // The end date for every four months will be 4/30/yyyy, 8/31/yyyy or 12/31/yyyy.
                    endDate = new DateTime(endDate.Year, startDate.GetYearThird() * 4, 1).GetEndOfMonth();
                    break;
                case Indices.BI_ANNUAL:
                    // The end date for bi-annual will be 6/30/yyyy or 12/31/yyyy.
                    endDate = startDate.Month >= 7
                        ? new DateTime(startDate.Year, 12, 31)
                        : new DateTime(startDate.Year, 6, 30);
                    break;
                case Indices.ANNUAL:
                    // The end date for annual will be the last day of the year.
                    endDate = new DateTime(startDate.Year, 12, 31);
                    break;
                case Indices.EVERY_TWO_YEARS:
                case Indices.EVERY_THREE_YEARS:
                case Indices.EVERY_FOUR_YEARS:
                case Indices.EVERY_FIVE_YEARS:
                    // The end date for every 'n' years will be the last day of the n'th year.
                    // For annual (above) that is the last day of this year. For every two years it will be the last day of next year, and so on.
                    var years = frequency - Indices.ANNUAL;
                    endDate = new DateTime(startDate.AddYears(years).Year, 12, 31);
                    break;
                case Indices.EVERY_TEN_YEARS:
                    // The end date for every ten years will be the last day of the tenth year.
                    endDate = new DateTime(startDate.AddYears(9).Year, 12, 31);
                    break;
                case Indices.EVERY_FIFTEEN_YEARS:
                    // The end date for every fifteen years will be the last day of the fifteenth year.
                    endDate = new DateTime(startDate.AddYears(14).Year, 12, 31);
                    break;
                default:
                    // return DateTime reset to zero.
                    endDate = new DateTime();
                    break;
            }

            return endDate;
        }

        /// <summary>
        /// GetFrequencyDates - instance method version - must be called from a ProductionWorkOrderFrequencyId object instance.
        /// </summary>
        /// <param name="start" type="DateTime">Start of the searchable range is inclusive.</param>
        /// <param name="end" type="DateTime">End of the searchable range is exclusive as the end date is essentially the end of the previous day and the start of a new day
        /// in which none of the day has yet passed. To include a day as the final day in the date range, specify the next day as end date.</param>
        /// <returns>An enumerated list of qualifying dates; list may be empty</returns>
        public virtual IEnumerable<DateTime> GetFrequencyDates(DateTime start, DateTime end)
        {
            return GetFrequencyDates(Id, start, end);
        }

        /// <summary>
        /// GetFrequencyDates - static method version
        /// </summary>
        /// <param name="frequency" type="int">Frequency is the Id representing an indexed constant value for the work order frequency.</param>
        /// <param name="start" type="DateTime">Start of the searchable range is inclusive.</param>
        /// <param name="end" type="DateTime">End of the searchable range is exclusive as the end date is essentially the end of the previous day and the start of a new day
        /// in which none of the day has yet passed. To include a day as the final day in the date range, specify the next day as end date.</param>
        /// <returns>An enumerated list of qualifying dates; list may be empty</returns>
        public static IEnumerable<DateTime> GetFrequencyDates(int frequency, DateTime start, DateTime end)
        {
            var frequencyDates = new List<DateTime>();
            if (frequency == 0 || start > end)
            {
                return frequencyDates;
            }

            var nextDate = start;

            /* The process of calculating the correct dates for each frequency requires two iterations of a switch on the frequency.
                In order to implement the two iterations in one visual display of the switch we are using a function delegate, "formula", to perform delayed execution.
                Once the code has broken out of the switch the formula will be executed to calculate the successive qualifying dates. */
            Func<DateTime> formula = null;

            /* The "nextDate =" first line (except for DAILY) determines the first qualifying date for the frequency.
               The "formula = ()" second line provides a dynamic calculation for determining any additional dates for the specific frequency. */
            switch (frequency)
            {
                case Indices.DAILY:
                    // nextDate already set to the first daily date
                    formula = () => nextDate.GetNextDay();
                    break;
                case Indices.WEEKLY:
                    /* Sunday is the qualifying date each week, thus we subtract the day of the week from the number of days to be advanced to reach the next Sunday.
                      The previous Sunday may be before the start date. If the start date is a Sunday we begin with it. */
                    nextDate = nextDate.DayOfWeek == DayOfWeek.Sunday ? nextDate : nextDate.Next(DayOfWeek.Sunday);
                    formula = () => nextDate.GetNextWeek();
                    break;
                case Indices.BI_MONTHLY:
                    /* The 1st and 15th of each month are the only qualifying dates.
                       This is a case where the the formula is equivalent to the calculation for the first qualifying date, thus if the date is not already a qualifying date
                       advance to the first qualifying date by executing the formula. */
                    formula = () => nextDate.Day >= 15
                        ? nextDate.GetBeginningOfMonth().AddMonths(1)
                        : nextDate.AddDays(15 - nextDate.Day);
                    if (nextDate.Day != 1 && nextDate.Day != 15)
                    {
                        nextDate = formula();
                    }

                    break;
                case Indices.MONTHLY:
                    // This is the same situation as the last case, where the formula is used to find the first qualifying date if the current date does not qualify.
                    formula = () => nextDate.GetBeginningOfMonth().AddMonths(1);
                    if (nextDate.Day != 1)
                    {
                        nextDate = formula();
                    }
                    break;
                case Indices.EVERY_TWO_MONTHS:
                    // January 1st, March 1st, May 1st, July 1st, September 1st, and November 1st are the six qualifying dates of the year.
                    // If the current day is the first day of the year, first of March, May, July, September, or first of November we have our first qualifying date
                    if (!(nextDate.Day == 1 && nextDate.Month % 2 == 1))
                    {
                        /* If the current date's month is December then the first qualifying date is the next New Year's Day.
                           If the current date's month is October then the first qualifying date is November 1st of the current year.
                           If the current date's month is August then the first qualifying date is September 1st of the current year. 
                           If the current date's month is June then the first qualifying date is July 1st of the current year.
                           If the current date's month is April then the first qualifying date is May 1st of the current year.
                           Otherwise the first qualifying date is March 1st of the current year.
                           Have to do November separately or it causes an overflow on month */
                        nextDate = nextDate.Month < 11
                            ? new DateTime(nextDate.Year, (nextDate.GetYearSixth() * 2) + 1, 1)
                            : nextDate.Month == 11
                                ? new DateTime(nextDate.Year, 12, 1)
                                : new DateTime(nextDate.AddYears(1).Year, 1, 1);
                    }

                    formula = () => nextDate.AddMonths(2);
                    break;
                case Indices.QUARTERLY:
                    /*  January 1st, April 1st, July 1st and October 1st are the only four qualifying dates of the year
                        If the current date is the first day of any quarter we have a first qualifying date and there is nothing more to do. */
                    if (!(nextDate.Day == 1 && nextDate.Month % 3 == 1))
                    {
                        /* If the current date's month is October or greater then the first qualifying date is the next New Year's Day.
                           If the current date's month is July, August or September then the first qualifying date is October 1st of the current year.
                           If the current date's month is April, May or June then the first qualifying date is July 1st of the current year.
                           Otherwise the first qualifying date is April 1st of the current year. */
                        nextDate = nextDate.Month > 9
                            ? new DateTime(nextDate.AddYears(1).Year, 1, 1)
                            : nextDate = nextDate.Month > 6
                                ? new DateTime(nextDate.Year, 10, 1)
                                : nextDate = nextDate.Month > 3
                                    ? new DateTime(nextDate.Year, 7, 1)
                                    : new DateTime(nextDate.Year, 4, 1);
                    }

                    formula = () => nextDate.AddMonths(3);
                    break;
                case Indices.EVERY_FOUR_MONTHS:
                    // January 1st, May 1st and September 1st are the three qualifying dates of the year.
                    // If the current day is the first day of the year, first of May or first of September we have our first qualifying date
                    if (!(nextDate.Day == 1 && nextDate.Month % 4 == 1))
                    {
                        /* If the current date's month is greater than August then the first qualifying date is the next New Year's Day.
                           If the current date's month is greater than April then the first qualifying date is September 1st of the current year.
                           Otherwise the first qualifying date is May 1st of the current year. */
                        nextDate = nextDate.Month > 8
                            ? new DateTime(nextDate.AddYears(1).Year, 1, 1)
                            : nextDate = new DateTime(nextDate.Year, (nextDate.GetYearThird() * 4) + 1, 1);
                    }

                    formula = () => nextDate.AddMonths(4);
                    break;
                case Indices.BI_ANNUAL:
                    // January 1st and July 1st are the only qualifying dates.
                    var julyFirst = new DateTime(nextDate.Year, 7, 1);
                    nextDate = nextDate > julyFirst
                        ? new DateTime(nextDate.AddYears(1).Year, 1, 1)
                        : julyFirst;
                    formula = () => nextDate.AddMonths(6);
                    break;
                case Indices.ANNUAL:
                    // January 1st is the only qualifying date. If the current date is not the first day of the year then advance to the next New Year's Day.
                    if (nextDate.DayOfYear != 1)
                    {
                        nextDate = new DateTime(nextDate.AddYears(1).Year, 1, 1);
                    }

                    formula = () => nextDate.AddYears(1);
                    break;
                case Indices.EVERY_TWO_YEARS:
                case Indices.EVERY_THREE_YEARS:
                case Indices.EVERY_FOUR_YEARS:
                case Indices.EVERY_FIVE_YEARS:
                    // these cases all begin on January 1st. A variable is used to make the calculation of qualifying dates for the respective years
                    var years = frequency - Indices.BI_ANNUAL;
                    nextDate = new DateTime(nextDate.AddYears(years).Year, 1, 1);
                    formula = () => nextDate.AddYears(years);
                    break;
                case Indices.EVERY_TEN_YEARS:
                    // this case begins on January 1st every ten years.
                    nextDate = new DateTime(nextDate.AddYears(10).Year, 1, 1);
                    formula = () => nextDate.AddYears(10);
                    break;
                case Indices.EVERY_FIFTEEN_YEARS:
                    // this case begins on January 1st every fifteen years.
                    nextDate = new DateTime(nextDate.AddYears(15).Year, 1, 1);
                    formula = () => nextDate.AddYears(15);
                    break;
                default:
                    // Throw Exception
                    formula = () => end.AddDays(1);
                    break;
            }

            while (nextDate < end)
            {
                frequencyDates.Add(nextDate);
                nextDate = formula();
            }

            return frequencyDates;
        }

        /// <summary>
        /// Uses <see cref="GetFrequencyDates(System.DateTime,System.DateTime)"/> to generate a forecast
        /// and ensures at least <paramref name="minResults"/> projected dates will be returned.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="minResults"></param>
        /// <returns></returns>
        public virtual IEnumerable<DateTime> GetForecastDates(DateTime start, int minResults = 5)
        {
            if (ForecastYearSpan == default)
            {
                throw new Exception($"The property '{nameof(ForecastYearSpan)}' of {nameof(ProductionWorkOrderFrequency)} '{Name}' must have a non-zero value in order to generate a forecast.");
            }

            var end = start.AddYears(ForecastYearSpan);
            var dates = GetFrequencyDates(Id, start, end);
            var count = 0;

            // Yield all values of initial forecast between start and end, no matter how many values are returned.
            foreach (var date in dates)
            {
                yield return date;
                ++count;
            }

            // If the result count is less than minResults, expand the forecast and keep yielding values until we get the desired count.
            while (count < minResults)
            {
                var newStart = end;
                end = end.AddYears(ForecastYearSpan);

                var nextDates = GetFrequencyDates(newStart, end);
                foreach (var nextDate in nextDates)
                {
                    yield return nextDate;
                    
                    ++count;
                    if (count >= minResults)
                    {
                        yield break;
                    }
                }
            }
        }

        #endregion
    }
}