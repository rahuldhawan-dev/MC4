using System;

namespace MMSINC.Interface
{
    public interface IQueryString
    {
        #region Operators

        string this[string key] { get; }

        #endregion

        #region Methods

        string GetValue(string key);
        TValue GetValue<TValue>(string key);
        TValue GetValue<TValue>(string key, Func<string, TValue> fn);

        #endregion
    }
}
