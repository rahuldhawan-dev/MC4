using System;
using System.Collections.Generic;

namespace MMSINC.Utilities.ValueRanges
{
    public class IntValueRange : ValueRangeBase<int>
    {
        #region Properties

        public int Step { get; }

        #endregion

        #region Constructors

        public IntValueRange(int start, int end, int step = 1) : base(start, end)
        {
            if (end < start)
            {
                throw new ArgumentException($"'{nameof(start)}' must be less than or equal to '{nameof(end)}'.");
            }

            if (step < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(step), "Must be grater than 1.");
            }

            Step = step;
        }

        #endregion

        #region Private Methods

        protected override IEnumerable<int> GenerateValues()
        {
            for (var i = Start; i <= End; i += Step)
            {
                yield return i;
            }
        }

        #endregion
    }
}
