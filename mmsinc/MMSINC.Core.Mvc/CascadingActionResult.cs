using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data;
using MMSINC.Helpers;

namespace MMSINC
{
    public class CascadingActionResult : ActionResult
    {
        #region Properties

        /// <summary>
        /// The data that will be converted to SelectListItems.
        /// Be careful using this property as it could be an IQueryable and
        /// you don't want it to enumerate several times.
        /// </summary>
        public IEnumerable Data { get; set; }
        public JsonRequestBehavior JsonRequestBehavior { get; set; }

        // TODO: There are a couple places using this to pre-select a value in the
        // result action. This might be able to be done better in a way that makes more sense.
        // Cascading actions are meant to be a simple search function. They're not supposed
        // to be used for any other UX other than getting data. We could solve this by allowing
        // the CascadingActionResult to include extra data for each record, then having the client-side
        // decide if it needs to do something with it in on a page-specific basis.
        [Obsolete("This property is no longer necessary with the introduction to ControlHelper.")]
        public object SelectedValue { get; set; }

        public string TextField { get; set; }
        public string ValueField { get; set; }

        /// <summary>
        /// If true, the select list items/json items are returned in order
        /// by their display text. True by default.
        /// </summary>
        public bool SortItemsByTextField { get; set; }

        #endregion

        #region Constructors

        public CascadingActionResult()
        {
            // This is default behavior because cascading actions
            // are almost always going to be gets.
            JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            SortItemsByTextField = true;
        }

        public CascadingActionResult(IEnumerable data) : this()
        {
            Data = data;
        }

        public CascadingActionResult(IQueryable data, string textField, string valueField)
            : this(data.SelectDynamic(textField, valueField).Result as IEnumerable, textField, valueField) { }

        public CascadingActionResult(IEnumerable data, string textField, string valueField)
            : this(data)
        {
            TextField = textField;
            ValueField = valueField;
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
            json.Data = GetJsonItems();
            json.JsonRequestBehavior = JsonRequestBehavior;
            return json;
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Returns the SelectListItems created from the given Data.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<SelectListItem> GetSelectListItems(bool addEmptyText = true)
        {
            Data.ThrowIfNull("Data");

            var list = GenerateList(Data, ValueField, TextField, SelectedValue);

            // This should only be needed for GetJsonItems.
            if (addEmptyText && list.Any())
            {
                // We need to return the empty selected value prompt.
                // We don't add this if there aren't any items because we're assuming
                // that the select list will be in "disabled" mode in this case.
                //
                // Selected is set up this way, as opposed to throwing an exception, to match
                // what SelectList does internally(ie, it ignores the selected value if it's
                // not found in the list).
                //
                // Also note that this item will be removed by detergent if used in a multiselect.
                yield return new SelectListItem {
                    Value = "", Text = ControlHelper.DEFAULT_EMPTY_TEXT_FOR_DROPDOWNS,
                    Selected = !list.Any(i => i.Selected)
                };
            }

            var potentiallySortedList = (SortItemsByTextField ? list.OrderBy(x => x.Text).ToArray() : list.ToArray());

            foreach (var item in potentiallySortedList)
            {
                yield return item;
            }
        }

        private List<SelectListItem> GenerateList(IEnumerable data, string valueField, string textField,
            object selectedValue)
        {
            var items = new List<SelectListItem>();

            foreach (var datum in data)
            {
                if (datum.HasPublicProperty(valueField))
                {
                    var value = datum.GetPropertyValueByName(valueField);
                    items.Add(new SelectListItem {
                        Value = value.ToString(),
                        Text = datum.GetPropertyValueByName(textField)?.ToString(),
                        Selected = value.ToString() == selectedValue?.ToString()
                    });
                }
                else if (datum.HasPublicField(valueField))
                {
                    var value = datum.GetFieldValueByName(valueField);
                    items.Add(new SelectListItem {
                        Value = value.ToString(),
                        Text = datum.GetFieldValueByName(textField)?.ToString(),
                        Selected = value.ToString() == selectedValue?.ToString()
                    });
                }
                else
                {
                    throw new ArgumentException(
                        $"Cound not find field or property named '{valueField}' on type '{datum.GetType()}'");
                }
            }

            return items;
        }

        /// <summary>
        /// Returns anonymous object versions of the items returned from GetSelectListItems.
        /// This is to reduce clutter when rendered to json.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<object> GetJsonItems()
        {
            // We're recreating the select list item objects for json results
            // because we don't want to send back more than we need. ie: we
            // don't need to send back a selected value attribute for
            // all of the json results.

            foreach (var item in GetSelectListItems())
            {
                if (SelectedValue != null && (item.Selected || item.Value == SelectedValue.ToString()))
                {
                    yield return new {value = item.Value, text = item.Text, selected = true};
                }
                else
                {
                    yield return new {value = item.Value, text = item.Text};
                }
            }
        }

        /// <summary>
        /// Executes the CascadingActionResult as a JsonResult.
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            CreateJsonResult().ExecuteResult(context);
        }

        #endregion
    }

    public class CascadingActionResult<TEntity, TDisplay> : CascadingActionResult
        where TDisplay : DisplayItem<TEntity>
    {
        private static IEnumerable<TDisplay> GatherData(IQueryable<TEntity> data)
        {
            return ((IQueryable<TDisplay>)data.SelectDynamic<TEntity, TDisplay>().Result).ToList();
        }

        public CascadingActionResult(IQueryable<TEntity> data, string textField = "Display", string valueField = "Id") :
            base(GatherData(data), textField, valueField) { }
    }
}
