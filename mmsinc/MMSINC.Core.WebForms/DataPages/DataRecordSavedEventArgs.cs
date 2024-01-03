using System;
using System.Collections;
using MMSINC.ClassExtensions.TypeExtensions;

namespace MMSINC.DataPages
{
    public sealed class DataRecordSavedEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the affected record's id.
        /// </summary>
        public int RecordId { get; private set; }

        public DataRecordSaveTypes SaveType { get; private set; }
        public IDictionary Values { get; private set; }
        public IDictionary OldValues { get; private set; }

        #endregion

        #region Constructors

        public DataRecordSavedEventArgs(DataRecordSaveTypes saveType, int recordId, IDictionary savedValues,
            IDictionary oldValues = null)
        {
            RecordId = recordId;
            SaveType = saveType;
            Values = savedValues;
            OldValues = oldValues;
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Values are always stupid string dictionaries, this attempts to convert the values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetValue<T>(string key)
        {
            var val = Values[key];
            if (val == null && typeof(T).IsNullable())
            {
                return default(T);
            }

            if (typeof(T).IsNullable())
            {
                // Convert.ChangeType doesn't work with nullable types, so
                // have to do it this way.
                var nonNullableType = Nullable.GetUnderlyingType(typeof(T));
                return (T)Convert.ChangeType(val, nonNullableType);
            }

            return (T)Convert.ChangeType(val, typeof(T));
        }

        public string GetValue(string key)
        {
            return (string)Values[key];
        }

        public T GetOldValue<T>(string key)
        {
            return (T)Convert.ChangeType(OldValues[key], typeof(T));
        }

        public string GetOldValue(string key)
        {
            return (string)OldValues[key];
        }

        #endregion
    }
}
