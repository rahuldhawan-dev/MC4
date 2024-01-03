using System;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Exceptions;
using MMSINC.Utilities.WorkDayEngine;
using WorkOrders.Model;

namespace WorkOrders.Library
{
    /// <summary>
    /// Custom WorkDayEngine, with methods to determine the ready/expiration
    /// dates of Markouts.
    /// </summary>
    public class WorkOrdersWorkDayEngine : WorkDayEngine<WorkOrdersWorkDayEngineConfiguration>
    {
        /*
         * ORIGINAL SPECIFICATION FROM KEVIN KEANE, SENT 10/15/2008:
         * For NJ One Call, they observe the state holidays.   They are as follows:

         * New Years Day
         * Martin Luther Kings Birthday
         * Lincoln's Birthday
         * Washington's birthday
         * Good Friday
         * Memorial Day
         * Independence Day
         * Labor Day
         * Columbus Day
         * Election Day
         * Vetran's Day
         * Thanksgiving Day
         * Christmas Day

         * The following "ready to dig" rules apply.

         * When a routine markout is called in, before 5:00pm on a regular day, three
         * full days must be allowed .   Simply put

         * Date called +3 full business days = ready status (4th day) Date called + 
         * 10 full business days (day after 10th business day) markout expires. if
         * start date is null If start date is within Date called + 10 full business
         * days, expire date is 45 business days after call date.
         */

        #region Constants

        private struct ReadyDateSpans
        {
            public const short ROUTINE = 3,
                               EMERGENCY = 0;
        }

        private struct CallDateSpans
        {
            public const short ROUTINE = ReadyDateSpans.ROUTINE * -1,
                               EMERGENCY = ReadyDateSpans.EMERGENCY * -1;
        }

        private struct ExpirationDateSpans
        {
            public struct ROUTINE
            {
                public const short WORK_STARTED = 45,
                                   WORK_NOT_STARTED = 10;
            }

            public struct EMERGENCY
            {
                public const short WORK_STARTED = 0,
                                   WORK_NOT_STARTED = 0;
            }
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Determines the ready date for a given Markout, based on the date
        /// said Markout was called in, and the set requirement.
        /// </summary>
        /// <param name="dateCalled">Date the Markout was initially called in.
        /// </param>
        /// <param name="requirement">MarkoutRequirement to specify when the
        /// Markout should be ready.</param>
        /// <returns>A DateTime indicating when the Markout should be ready.
        /// </returns>
        public static DateTime GetReadyDate(DateTime dateCalled, MarkoutRequirement requirement)
        {
            switch (requirement.RequirementEnum)
            {
                case MarkoutRequirementEnum.Routine:
                    return GetRoutineReadyDate(dateCalled);
                case MarkoutRequirementEnum.Emergency:
                    return GetEmergencyReadyDate(dateCalled);
                default:
                    throw new DomainLogicException("Cannot get Markout ready date when Markout is not required.");
            }
        }

        public static DateTime GetCallDate(DateTime dateNeeded, MarkoutRequirement markoutRequirement)
        {
            switch (markoutRequirement.RequirementEnum)
            {
                case MarkoutRequirementEnum.Routine:
                    return GetRoutineCallDate(dateNeeded);
                default:
                    throw new DomainLogicException("Cannot get Markout call date when Markout is not required.");
            }
        }



        /// <summary>
        /// Determines the expiration date for a given Markout, based on the
        /// date said Markout was called in, the set requirement, and whether
        /// or not the work has been started.
        /// </summary>
        /// <param name="dateCalled">Date the Markout was initially called in.
        /// </param>
        /// <param name="requirement">MarkoutRequirement to specify when the
        /// Markout should expire.</param>
        /// <param name="workStarted">Boolean value indicating whether or not
        /// the work has been started.</param>
        /// <returns>A DateTime indicating when the Markout should expire.
        /// </returns>
        public static DateTime GetExpirationDate(DateTime dateCalled, MarkoutRequirement requirement, bool workStarted)
        {
            switch (requirement.RequirementEnum)
            {
                case MarkoutRequirementEnum.Routine:
                    switch (dateCalled.DayOfWeek)
                    {
                        // If a non-emergency Work Order is called in on a Sat or Sun the called in date is considered to be on the following Monday.
                        case DayOfWeek.Saturday:
                        case DayOfWeek.Sunday:
                            return GetRoutineExpirationDate(GetFollowingDayOfWeek(dateCalled, DayOfWeek.Monday), workStarted);
                        default:
                            return GetRoutineExpirationDate(dateCalled, workStarted);
                    }
                case MarkoutRequirementEnum.Emergency:
                    return GetEmergencyExpirationDate(dateCalled, workStarted);
                default:
                    throw new DomainLogicException("Cannot get Markout expiration date when Markout is not required.");
            }
        }

        /// <summary>
        /// Determines the expiration date for a given Markout, based on the
        /// date said Markout was called in, and the set requirement.  This
        /// overload assumes that work has not yet been started.
        /// </summary>
        /// <param name="dateCalled">Date the Markout was initially called in.
        /// </param>
        /// <param name="requirement">MarkoutRequirement to specify when the
        /// Markout should expire.</param>
        /// <returns>A DateTime indicating when the Markout should expire.
        /// </returns>
        public static DateTime GetExpirationDate(DateTime dateCalled, MarkoutRequirement requirement)
        {
            return GetExpirationDate(dateCalled, requirement, false);
        }

        #endregion

        #region Private Static Methods

        private static DateTime GetRoutineCallDate(DateTime dateNeeded)
        {
            // TODO: Account for Tuesday
            return DecrementByDays(dateNeeded, CallDateSpans.ROUTINE);
        }

        public static DateTime GetRoutineReadyDate(DateTime dateCalled)
        {
            // fix for Good Friday 2011:
            if (dateCalled.IsHoliday(Configuration))
            {
                dateCalled = dateCalled.AddDays(1);
            }

            switch (dateCalled.DayOfWeek)
            {
                case DayOfWeek.Tuesday:
                    return GetRoutineReadyDateFromTuesday(dateCalled);
                // If a non-emergency Work Order is called in on a Sat or Sun the called in date is considered to be on the following Monday.
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    var startDate = GetFollowingDayOfWeek(dateCalled,
                        DayOfWeek.Monday);
                    while (startDate.IsHoliday(Configuration))
                    {
                        startDate = startDate.AddDays(1);
                    }
                    return GetRoutineReadyDate(startDate);
                default:
                    return IncrementByDays(dateCalled, ReadyDateSpans.ROUTINE);
            }
        }

        private static DateTime GetFollowingDayOfWeek(DateTime dateCalled, DayOfWeek dayOfWeek)
        {
            var curDate = dateCalled;
            while (curDate.DayOfWeek != dayOfWeek)
            {
                curDate = curDate.AddDays(1);
            }
            return curDate;
        }

        private static DateTime GetRoutineReadyDateFromTuesday(DateTime dateCalled)
        {
            var curDate = dateCalled;
            while (curDate.DayOfWeek != DayOfWeek.Sunday)
            {
                if (curDate.IsHoliday(Configuration))
                {
                    return IncrementByDays(dateCalled, ReadyDateSpans.ROUTINE);
                }
                curDate = curDate.GetNextDay();
            }
            return dateCalled.GetDayFromWeek(DayOfWeek.Saturday);
        }

        private static DateTime GetEmergencyReadyDate(DateTime dateCalled)
        {
            return IncrementByDays(dateCalled, ReadyDateSpans.EMERGENCY);
        }

        private static DateTime GetRoutineExpirationDate(DateTime dateCalled, bool workStarted)
        {
            return IncrementByDays(dateCalled.GetPreviousDay(),
                workStarted ? ExpirationDateSpans.ROUTINE.WORK_STARTED :
                              ExpirationDateSpans.ROUTINE.WORK_NOT_STARTED, true);
        }

        private static DateTime GetEmergencyExpirationDate(DateTime dateCalled, bool workStarted)
        {
            return IncrementByDays(dateCalled,
                workStarted ? ExpirationDateSpans.EMERGENCY.WORK_STARTED :
                              ExpirationDateSpans.EMERGENCY.WORK_NOT_STARTED);
        }



        #endregion

    }

    /// <summary>
    /// Configuration for the WorkOrdersWorkDayEngine, including all of the NJ
    /// state holidays.
    /// </summary>
    public class WorkOrdersWorkDayEngineConfiguration : WorkDayEngineConfiguration
    {
        #region Properties

        #pragma warning disable 1591

        public override bool UseChristmas
        {
            get { return true; }
        }
        public override bool UseColumbusDay
        {
            get { return true; }
        }
        public override bool UseElectionDay
        {
            get { return true; }
        }
        public override bool UseGoodFriday
        {
            get { return true; }
        }
        public override bool UseIndependenceDay
        {
            get { return true; }
        }
        public override bool UseLaborDay
        {
            get { return true; }
        }
        public override bool UseLincolnsBirthday
        {
            get { return true; }
        }
        public override bool UseMartinLutherKingDay
        {
            get { return true; }
        }
        public override bool UseMemorialDay
        {
            get { return true; }
        }
        public override bool UseNewYearsDay
        {
            get { return true; }
        }
        public override bool UseThanksgiving
        {
            get { return true; }
        }
        public override bool UseVeteransDay
        {
            get { return true; }
        }
        public override bool UseWashingtonsBirthday
        {
            get { return true; }
        }

        #pragma warning restore 1591

        #endregion
    }
}
