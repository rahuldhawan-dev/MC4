using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MMSINC.Testing
{
    /// <summary>
    /// Helper for testing <see cref="JsonResult"/>s in controller tests.
    /// </summary>
    public class JsonResultTester
    {
        #region Private Members

        private readonly List<object> _data;

        #endregion

        #region Properties

        public int Count => _data.Count;

        #endregion

        #region Constructors

        /// <param name="data">
        /// Assumed to have a property called "Data" which should be an <see cref="IEnumerable{T}"/> of
        /// objects whose properties would be serialized to JSON.
        /// </param>
        public JsonResultTester(object data)
        {
            _data = (data.GetPropertyValueByName("Data") as IEnumerable<object>).ToList();
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Assert that the value of the property specified by <paramref name="propertyName"/> of the item
        /// at the optional <paramref name="rowIndex"/> matches the <paramref name="expectedValue"/>.
        /// </summary>
        [DebuggerStepThrough]
        public void AreEqual(object expectedValue, string propertyName, int rowIndex = 0, string message = null)
        {
            var resultValue = _data[rowIndex].GetPropertyValueByName(propertyName);

            if (expectedValue is int)
            {
                if (resultValue is int)
                {
                    resultValue = Convert.ToInt32(resultValue);
                }
            }
            else if (expectedValue is decimal)
            {
                if (resultValue is double value)
                {
                    resultValue = Convert.ToDecimal(value);
                }
            }
            else if (expectedValue is long)
            {
                resultValue = Convert.ToInt64(resultValue);
            }
            else if (expectedValue is DateTime)
            {
                if (resultValue is double value)
                {
                    var date = DateTime.FromOADate(value);
                    date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
                    resultValue = date;
                }
            }

            Assert.AreEqual(expectedValue, resultValue, message);
        }

        /// <summary>
        /// Assert that the number of items in the <see cref="IEnumerable{T}"/> matches
        /// <paramref name="expectedCount"/>.
        /// </summary>
        [DebuggerStepThrough]
        public void CountEquals(int expectedCount, string message = null)
        {
            Assert.AreEqual(expectedCount, Count, message);
        }

        #endregion
    }
}
