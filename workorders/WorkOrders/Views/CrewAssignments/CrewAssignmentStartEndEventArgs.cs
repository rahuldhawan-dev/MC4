using System;

namespace WorkOrders.Views.CrewAssignments
{
    public class CrewAssignmentStartEndEventArgs : EventArgs
    {
        #region Constants

        public struct CommandNames
        {
            public const string START = "Start",
                                END = "End";
        }

	    #endregion

        #region Enumerations

        public enum Commands
        {
            Start, End
        }

        #endregion

        #region Private Members

        private readonly int _crewAssignmentID;
        private readonly Commands _command;
        private readonly DateTime _date;
        private readonly float? _employeesOnJob;

        #endregion

        #region Properties

        public int CrewAssignmentID
        {
            get { return _crewAssignmentID; }
        }

        public Commands Command
        {
            get { return _command; }
        }

        public DateTime Date
        {
            get { return _date; }
        }

        public float? EmployeesOnJob
        {
            get { return _employeesOnJob; }
        }

        #endregion

        #region Constructors

        public CrewAssignmentStartEndEventArgs(int crewAssignmentID, string command, DateTime date) : this(crewAssignmentID, command.ToEnumValue(), date)
        {
        }

        public CrewAssignmentStartEndEventArgs(int crewAssignmentID, Commands command, DateTime date)
        {
            _crewAssignmentID = crewAssignmentID;
            _command = command;
            _date = date;
        }

        public CrewAssignmentStartEndEventArgs(int crewAssignmentID, Commands command, DateTime date, float? employeesOnJob)
            : this(crewAssignmentID, command, date)
        {
            _employeesOnJob = employeesOnJob;
        }

        public CrewAssignmentStartEndEventArgs(int crewAssignmentID, String command, DateTime date, float? employeesOnJob)
            : this(crewAssignmentID, command.ToEnumValue(), date, employeesOnJob)
        {
        }

        #endregion
    }

    internal static class Extensions
    {
        #region Extension Methods

        public static string ToString(this CrewAssignmentStartEndEventArgs.Commands command)
        {
            switch (command)
            {
                case CrewAssignmentStartEndEventArgs.Commands.Start:
                    return CrewAssignmentStartEndEventArgs.CommandNames.START;
                default:
                    return CrewAssignmentStartEndEventArgs.CommandNames.END;
            }
        }

        public static CrewAssignmentStartEndEventArgs.Commands ToEnumValue(this string command)
        {
            switch (command)
            {
                case CrewAssignmentStartEndEventArgs.CommandNames.START:
                    return CrewAssignmentStartEndEventArgs.Commands.Start;
                case CrewAssignmentStartEndEventArgs.CommandNames.END:
                    return CrewAssignmentStartEndEventArgs.Commands.End;
                default:
                    throw new ArgumentOutOfRangeException("command", command,
                        "Could not parse Command value.");
            }
        }

        #endregion
    }

    public delegate void CrewAssignmentStartEndEventHandler(
        object sender, CrewAssignmentStartEndEventArgs e);

    public delegate void CrewAssignmentPrioritizeEventHandler(
        object sender, CrewAssignmentPrioritizeEventArgs e);

    public delegate void CrewAssignmentDeleteEventHandler(
        object sender, CrewAssignmentDeleteEventArgs e);
}
