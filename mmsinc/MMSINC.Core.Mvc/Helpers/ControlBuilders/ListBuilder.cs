using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MMSINC.Helpers
{
    // ReSharper disable once TypeParameterCanBeVariant
    public interface IFluentListBuilder<T> where T : ListBuilder<T>
    {
        #region Abstract Methods

        T WithEmptyText(string emptyText);
        T WithItems(IEnumerable<SelectListItem> items);
        T WithSelectedValues(IEnumerable<object> selectedValues);

        #endregion
    }

    /// <summary>
    /// Base builder class for all controls that act like lists with selected values.
    /// </summary>
    public abstract class ListBuilder<TBuilder> : ControlBuilder<TBuilder>, IFluentListBuilder<TBuilder>
        where TBuilder : ListBuilder<TBuilder>
    {
        // Giving this an initial capacity of one since most things using this will only
        // ever have one selected value. This prevents having to make an extra memory allocation
        // after one item is inserted(after which it changes the capacity to 4).

        #region Private Members

        private readonly List<object> _selectedValues = new List<object>(1);

        private readonly List<SelectListItem> _items = new List<SelectListItem>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the text used for the default empty item. If this value is null, an empty item will not be added. 
        /// </summary>
        public string EmptyText { get; set; }

        /// <summary>
        /// Gets the collection of items that are rendered.
        /// </summary>
        public List<SelectListItem> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Gets the collection of selected values for the builder. Multiple values
        /// are allowed depending on what the inheriting class does with it.
        /// </summary>
        public List<object> SelectedValues
        {
            get { return _selectedValues; }
        }

        #endregion

        #region Private Methods

        private IEnumerable<string> GetSelectedValues()
        {
            foreach (var s in SelectedValues)
            {
                if (s != null && (s as string) != string.Empty)
                {
                    if (s.GetType().IsEnum)
                    {
                        // We can't ToString an enum directly because that will return
                        // its named value instead. Also because not all enums are the
                        // default int type, we need to convert it to an int before
                        // converting that to a string.
                        yield return Convert.ToString(Convert.ToInt32(s));
                    }

                    yield return Convert.ToString(s);
                }
            }
        }

        /// <summary>
        /// Returns the Items values with the Selected value set based on
        /// the values in the SelectedValues property.
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<SelectListItem> GetItems()
        {
            var selected = GetSelectedValues().ToArray();

            if (EmptyText != null)
            {
                yield return new SelectListItem {
                    Selected = selected.Contains(string.Empty) || selected.Contains(null),
                    Text = EmptyText,
                    Value = string.Empty
                };
            }

            foreach (var item in Items)
            {
                // Return a new item so that we aren't directly modifying the
                // select items given to us. This will allow for the original items to be 
                // cached in some instances without causing threading problems.
                yield return new SelectListItem {
                    Selected = selected.Contains(item.Value),
                    Text = item.Text,
                    Value = item.Value
                };
            }
        }

        /// <summary>
        /// Replaces the Items collection with the supplied values.
        /// </summary>
        /// <param name="items"></param>
        protected void DoWithItems(IEnumerable<SelectListItem> items)
        {
            Items.Clear();
            if (items != null)
            {
                Items.AddRange(items);
            }
        }

        protected void DoWithEmptyText(string emptyText)
        {
            EmptyText = emptyText;
        }

        /// <summary>
        /// Replaces the SelectedValues collection with the supplied values.
        /// </summary>
        /// <param name="selectedValues"></param>
        protected void DoWithSelectedValues(IEnumerable<object> selectedValues)
        {
            SelectedValues.Clear();
            if (selectedValues != null)
            {
                SelectedValues.AddRange(selectedValues);
            }
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Returns the completed html string for this control.
        /// </summary>
        /// <returns></returns>
        protected abstract override string CreateHtmlString();

        #endregion

        #region Exposed Methods

        public virtual TBuilder WithEmptyText(string emptyText)
        {
            DoWithEmptyText(emptyText);
            return (TBuilder)this;
        }

        public virtual TBuilder WithItems(IEnumerable<SelectListItem> items)
        {
            DoWithItems(items);
            return (TBuilder)this;
        }

        public virtual TBuilder WithSelectedValues(IEnumerable<object> selectedValues)
        {
            DoWithSelectedValues(selectedValues);
            return (TBuilder)this;
        }

        #endregion
    }
}
