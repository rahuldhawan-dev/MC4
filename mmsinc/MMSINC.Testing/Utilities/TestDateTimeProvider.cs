using System;
using MMSINC.Utilities;
using StructureMap;

namespace MMSINC.Testing.Utilities
{
    public class TestDateTimeProvider : DateTimeProvider
    {
        #region Private Members

        private DateTime _now;

        #endregion

        #region Constructors

        [DefaultConstructor]
        public TestDateTimeProvider() : this(DateTime.Now) { }

        public TestDateTimeProvider(DateTime now)
        {
            _now = now;
        }

        #endregion

        #region Exposed Methods

        public override DateTime GetCurrentDate()
        {
            return _now;
        }

        public void SetNow(DateTime now)
        {
            _now = now;
        }

        #endregion
    }
}
