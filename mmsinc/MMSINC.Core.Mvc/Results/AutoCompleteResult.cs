using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Utilities;
using NHibernate.Intercept;
using NHibernate.Proxy;

namespace MMSINC.Results
{
    public class AutoCompleteResult : ActionResult
    {
        #region Properties

        public IEnumerable Data { get; set; }
        public JsonRequestBehavior JsonRequestBehavior { get; set; }

        /// <summary>
        /// The property that holds the unique value that identifies a record on the server. 
        /// This is the value that is added to the hidden field and posted back to the server.
        /// </summary>
        public string DataProperty { get; set; }

        /// <summary>
        /// The text displayed in the select list.
        /// </summary>
        public string LabelProperty { get; set; }

        /// <summary>
        /// The value set in the textbox if this item is selected from the autocomplete list. If
        /// this value is not set, the Label field is used. 
        /// </summary>
        public string ValueProperty { get; set; }

        #endregion

        #region Constructor

        public AutoCompleteResult()
        {
            JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        }

        public AutoCompleteResult(IEnumerable data, string dataProperty, string labelProperty,
            string valueProperty = null) : this()
        {
            Data = data;
            DataProperty = dataProperty;
            LabelProperty = labelProperty;
            ValueProperty = valueProperty;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates an ActionResult that represents the json output for use with ExecuteResult.
        /// </summary>
        /// <returns></returns>
        internal protected virtual ActionResult CreateJsonResult()
        {
            var json = new JsonResult();
            json.Data = GetJsonData();
            json.JsonRequestBehavior = JsonRequestBehavior;
            return json;
        }

        private Type MaybeGetEntityType(Type type)
        {
            // if we're dealing with an NHibernate proxy type of some sort, we want to coerce that to the
            // actual entity type to avoid cache/type mismatch issues
            return (typeof(IFieldInterceptorAccessor).IsAssignableFrom(type) ||
                    typeof(INHibernateProxy).IsAssignableFrom(type))
                ? type.BaseType
                : type;
        }

        private IEnumerable<object> GetJsonData()
        {
            var d = GetDataAsGeneric().ToArray();
            if (!d.Any())
            {
                yield break;
            }

            // Make some expressions with that lovely utility thing you made.
            // Then make stuff.

            var type = MaybeGetEntityType(d.First().GetType());
            var dataFunc = Expressions.BuildGetterExpression(type, DataProperty).AsDynamic().Compile();
            var labelFunc = Expressions.BuildGetterExpression(type, LabelProperty).AsDynamic().Compile();
            dynamic valueFunc = null;

            if (!string.IsNullOrWhiteSpace(ValueProperty))
            {
                valueFunc = Expressions.BuildGetterExpression(type, ValueProperty).AsDynamic().Compile();
            }

            foreach (var item in d)
            {
                var dict = new Dictionary<string, object>();
                dict["data"] = dataFunc.DynamicInvoke(item);
                dict["label"] = labelFunc.DynamicInvoke(item);
                if (valueFunc != null)
                {
                    // ReSharper disable PossibleNullReferenceException
                    // I just checked that it != null you stupid resharper thing.
                    dict["value"] = valueFunc.DynamicInvoke(item);
                    // ReSharper restore PossibleNullReferenceException
                }

                yield return dict;
            }
        }

        // ReSharper disable ReturnTypeCanBeEnumerable.Local
        private object[] GetDataAsGeneric()
            // ReSharper restore ReturnTypeCanBeEnumerable.Local
        {
            if (Data == null)
            {
                return new object[0];
            }

            return Data.Cast<object>().ToArray();
        }

        #endregion

        public override void ExecuteResult(ControllerContext context)
        {
            CreateJsonResult().ExecuteResult(context);
        }
    }
}
