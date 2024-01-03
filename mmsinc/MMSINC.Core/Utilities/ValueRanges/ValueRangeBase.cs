using System.Collections;
using System.Collections.Generic;

namespace MMSINC.Utilities.ValueRanges
{
    public abstract class ValueRangeBase<T> : IEnumerable<T>
    {
        #region Private Members

        protected IEnumerable<T> _values;

        #endregion

        #region Properties

        public T Start { get; }
        public T End { get; }

        protected virtual IEnumerable<T> Values => _values ?? (_values = GenerateValues());

        #endregion

        #region Constructors

        public ValueRangeBase(T start, T end)
        {
            Start = start;
            End = end;
        }

        #endregion

        #region Private Methods

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Abstract Methods

        protected abstract IEnumerable<T> GenerateValues();

        #endregion

        #region Exposed Methods

        public virtual IEnumerator<T> GetEnumerator() => Values.GetEnumerator();

        #endregion
    }
}
