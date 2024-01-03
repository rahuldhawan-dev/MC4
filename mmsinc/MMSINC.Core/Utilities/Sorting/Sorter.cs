using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MMSINC.Utilities.Sorting
{
    public class Sorter : ISorter
    {
        #region Private Members

        private IEnumerable _obj;

        #endregion

        #region Constructors

        public Sorter(IEnumerable obj)
        {
            _obj = obj;
        }

        #endregion

        #region Exposed Methods

        //TODO: Make this recursive
        public IEnumerable<TObject> Sort<TObject>(string sortExpression)
        {
            sortExpression += "";
            var parts = sortExpression.Split(' ');
            var descending = false;

            if (parts.Length > 0 && parts[0] != "")
            {
                var property = parts[0];

                if (parts.Length == 1)
                {
                    Func<TObject, object> keySelector = x => x.GetPropertyValueByName(property);
                    return ((IEnumerable<TObject>)_obj).OrderBy(keySelector);
                }

                if (parts.Length == 2)
                {
                    @descending = parts[1].ToLower().Contains("esc");
                    Func<TObject, object> keySelector = x => x.GetPropertyValueByName(property);

                    return @descending
                        ? ((IEnumerable<TObject>)_obj).OrderByDescending(keySelector)
                        : ((IEnumerable<TObject>)_obj).OrderBy(keySelector);
                }

                if (parts.Length > 2)
                {
                    @descending = parts[1].ToLower().Contains("esc");
                    Func<TObject, object> keySelector = x => x.GetPropertyValueByName(parts[0]);
                    var results = @descending
                        ? ((IEnumerable<TObject>)_obj).OrderByDescending(keySelector)
                        : ((IEnumerable<TObject>)_obj).OrderBy(keySelector);

                    Func<TObject, object> secondKeySelector = x => x.GetPropertyValueByName(parts[2]);
                    @descending = parts[3].ToLower().Contains("esc");
                    return (@descending)
                        ? results.ThenByDescending(secondKeySelector)
                        : results.ThenBy(secondKeySelector);
                }
            }

            return (IEnumerable<TObject>)_obj;
        }

        #endregion
    }

    public interface ISorter
    {
        #region Methods

        IEnumerable<TObject> Sort<TObject>(string sortExpression);

        #endregion
    }
}
