using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Common;

namespace MMSINC.Results
{
    public class CalendarResult<TEntity> : JsonResult
    {
        #region Private Members

        protected readonly IEnumerable<TEntity> _source;
        protected readonly IList<CalendarItem> _data;
        protected readonly Func<TEntity, CalendarItem> _conversionFn;

        #endregion

        #region Constructors

        public CalendarResult(IEnumerable<TEntity> source, Func<TEntity, CalendarItem> conversionFn)
        {
            _source = source;
            _data = new List<CalendarItem>();
            Data = _data;
            _conversionFn = conversionFn;
        }

        #endregion

        #region Exposed Methods

        public override void ExecuteResult(ControllerContext context)
        {
            _source.Each(e => _data.Add(_conversionFn(e)));

            base.ExecuteResult(context);
        }

        #endregion
    }
}
