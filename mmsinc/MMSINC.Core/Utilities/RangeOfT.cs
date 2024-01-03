using System;

namespace MMSINC.Utilities
{
    public interface IRange
    {
        bool IsInRange(object value);
        object GetMin();
        object GetMax();
    }

    /// <summary>
    /// Represents a range of values. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// 
    /// NOTE: There's no operator overloads going on here because I didn't
    /// wanna deal with making them when they're of no use. -Ross
    ///
    /// Also, this class can go away whenever we upgrade to a version of .NET
    /// that has System.Range.
    ///
    /// This is implementing IComparable as a hack for the RequiredWhenAttribute
    /// that should 
    /// </remarks>
    public struct Range<T> : IRange where T : IComparable, IComparable<T>
    {
        #region Fields

        #endregion

        #region Properties

        public T MinValue { get; }
        public T MaxValue { get; }

        #endregion

        #region Constructor

        public Range(T min, T max)
        {
            if (min.CompareTo(max) == 1)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Minimum value '{0}' can not be greater than the maximum value '{1}'", min, max));
            }

            MinValue = min;
            MaxValue = max;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns true if the given value falls within, or is equal to, the MinValue and MaxValue values.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsInRange(T value)
        {
            return (MinValue.CompareTo(value) <= 0 && value.CompareTo(MaxValue) <= 0);
        }

        public static bool IsInRange(T min, T max, T value)
        {
            return new Range<T>(min, max).IsInRange(value);
        }

        public bool IsInRange(object value)
        {
            return this.IsInRange((T)value);
        }

        public object GetMin()
        {
            return MinValue;
        }

        public object GetMax()
        {
            return MaxValue;
        }

        #endregion
    }
}
