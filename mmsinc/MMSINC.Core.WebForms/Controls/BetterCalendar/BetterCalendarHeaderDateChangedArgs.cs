using System;

namespace MMSINC.Controls.BetterCalendar
{
    public class BetterCalendarHeaderDateChangedArgs : EventArgs
    {
        #region Properties

        public BetterCalendarHeaderDateChangeType ChangeType { get; private set; }
        public int Difference { get; private set; }

        #endregion

        #region Constructors

        public BetterCalendarHeaderDateChangedArgs(BetterCalendarHeaderDateChangeType changeType, int diff)
        {
            ChangeType = changeType;
            Difference = diff;
        }

        #endregion
    }

    public enum BetterCalendarHeaderDateChangeType
    {
        Month,
        Year
    }
}
