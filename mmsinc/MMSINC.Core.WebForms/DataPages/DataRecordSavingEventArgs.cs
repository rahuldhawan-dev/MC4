using System;
using System.Collections;
using System.ComponentModel;

namespace MMSINC.DataPages
{
    public class DataRecordSavingEventArgs : CancelEventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the parameter values that will be sent to the data source.
        /// </summary>
        public IDictionary Values { get; private set; }

        public int RecordId { get; private set; }
        public DataRecordSaveTypes SaveType { get; private set; }
        public IDictionary OldValues { get; private set; }

        /// <summary>
        /// Set to true if the record saving should be cancelled.
        /// </summary>
        public bool Cancel { get; set; }

        #endregion

        #region Constructors

        public DataRecordSavingEventArgs(DataRecordSaveTypes saveType, int recordId, IDictionary savedValues,
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
            return (T)Convert.ChangeType(Values[key], typeof(T));
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
