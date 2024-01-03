using System;

namespace MMSINC.Utilities.WorkDayEngine.DateFinders
{
    internal class EasterFinder : DateFinder
    {
        #region Private Members

        private int? _firstDigit, _remain19;

        #endregion

        #region Properties

        private int firstDigit
        {
            get
            {
                if (_firstDigit == null)
                    _firstDigit = Year / 100;
                return _firstDigit.Value;
            }
        }

        private int remain19
        {
            get
            {
                if (_remain19 == null)
                    _remain19 = Year % 19;
                return _remain19.Value;
            }
        }

        #endregion

        #region Constructors

        internal EasterFinder(int year)
            : base(year) { }

        #endregion

        #region Private Methods

        private int FindTableA(int temp)
        {
            var tA = temp + 21;
            if (temp == 29)
                tA -= 1;
            if (temp == 28 && remain19 > 10)
                tA -= 1;
            return tA;
        }

        private int FindTableB(int tA)
        {
            return (tA - 19) % 7;
        }

        private int FindTableC()
        {
            var tC = (40 - firstDigit) % 4;
            if (tC == 3)
                tC += 1;
            if (tC > 1)
                tC += 1;
            return tC;
        }

        private int FindTableD()
        {
            var temp = Year % 100;
            return (temp + temp / 4) % 7;
        }

        private int FindTableE(int tB, int tC, int tD)
        {
            return ((20 - tB - tC - tD) % 7) + 1;
        }

        #endregion

        #region Exposed Methods

        internal override DateTime FindDate()
        {
            int day, month;
            int temp;
            int tA, tB, tC, tD, tE;

            // calculate the PFM date
            temp = (firstDigit - 15) / 2 + 202 - 11 * remain19;

            switch (firstDigit)
            {
                case 21:
                case 24:
                case 25:
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:
                case 32:
                case 34:
                case 35:
                case 38:
                    temp -= 1;
                    break;
                case 33:
                case 36:
                case 37:
                case 39:
                case 40:
                    temp -= 2;
                    break;
            }

            temp = temp % 30;

            tA = FindTableA(temp);

            // find the next Sunday
            tB = FindTableB(tA);

            tC = FindTableC();

            tD = FindTableD();

            tE = FindTableE(tB, tC, tD);

            day = tA + tE;

            // calculate real day and month
            if (day > 31)
            {
                day -= 31;
                month = 4;
            }
            else
                month = 3;

            return new DateTime(Year, month, day);
        }

        #endregion
    }
}
