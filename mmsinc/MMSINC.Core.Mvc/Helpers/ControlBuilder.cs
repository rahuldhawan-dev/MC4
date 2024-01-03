using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMSINC.ClassExtensions;

namespace MMSINC.Helpers
{
    // ReSharper disable once TypeParameterCanBeVariant
    public interface IFluentControlBuilder<T> where T : ControlBuilder<T>
    {
        #region Abstract Methods

        T With(object htmlAttributes);
        T With(string key, object value);
        T WithCssClass(string cssClass);
        T WithId(string id);
        T WithName(string name);

        #endregion
    }

    public abstract class ControlBuilder : IHtmlString
    {
        #region Private Members

        // protected string _rendered;
        protected readonly Dictionary<string, object> _htmlAttributes = new Dictionary<string, object>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the dictionary of html attributes used by this control builder.
        /// </summary>
        public IDictionary<string, object> HtmlAttributes
        {
            get { return _htmlAttributes; }
        }

        /// <summary>
        /// Gets the html name used for this control.
        /// </summary>
        public string Name
        {
            get { return (string)GetHtmlAttributeValue("name"); }
            set { DoWith("name", value); }
        }

        /// <summary>
        /// Gets the html id used for this control.
        /// </summary>
        public string Id
        {
            get { return (string)GetHtmlAttributeValue("id"); }
            set { DoWith("id", value); }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns a value from the HtmlAttributes property if it exists. Returns null if there is no matching key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected object GetHtmlAttributeValue(string key)
        {
            object val;
            if (_htmlAttributes.TryGetValue(key, out val))
            {
                return val;
            }

            return null;
        }

        /// <summary>
        /// Returns a tag builder with its html attributes set.
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="includeUnobtrusiveValidation">Set to false if unobtrusive validation attributes should not be rendered. True by default.</param>
        protected TagBuilder CreateTagBuilder(string tagName, bool includeUnobtrusiveValidation = true)
        {
            var tb = new TagBuilder(tagName);

            var mergeAttributes = (includeUnobtrusiveValidation ? _htmlAttributes : new Dictionary<string, object>());

            if (!includeUnobtrusiveValidation)
            {
                var unobtrusive = GetUnobtrusiveHtmlAttributes().Keys;
                foreach (var attr in _htmlAttributes)
                {
                    if (!unobtrusive.Contains(attr.Key))
                    {
                        mergeAttributes.Add(attr.Key, attr.Value);
                    }
                }
            }

            tb.MergeAttributes(mergeAttributes);

            return tb;
        }

        /// <summary>
        /// Returns only the unobtrusive validation attributes that exist in the
        /// HtmlAttributes dictionary. 
        /// </summary>
        protected Dictionary<string, object> GetUnobtrusiveHtmlAttributes()
        {
            return HtmlAttributes.Where(kv => kv.Key.StartsWith("data-val")).ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        /// <summary>
        /// Takes an anonymous object, or a dictionary, and adds the key value pairs
        /// as html attributes. 
        /// 
        /// Inheritors must create a public override for this so the correct type
        /// is returned.
        /// </summary>
        protected void DoWith(object htmlAttributes)
        {
            var attr = HtmlHelperExtensions.AnonymousObjectToHtmlAttributes(htmlAttributes);
            foreach (var kv in attr)
            {
                DoWith(kv.Key, kv.Value);
            }
        }

        /// <summary>
        /// Adds an html attribute and value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void DoWith(string key, object value)
        {
            HtmlAttributes[key] = value;
        }

        /// <summary>
        /// Same as AddCssClass. Mainly here for consistency with the fluent With calls.
        /// </summary>
        /// <param name="cssClass"></param>
        protected void DoWithCssClass(string cssClass)
        {
            AddCssClass(cssClass);
        }

        /// <summary>
        /// Sets the Id property.
        /// </summary>
        /// <param name="id"></param>
        protected void DoWithId(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Sets the Name property.
        /// </summary>
        /// <param name="name"></param>
        protected void DoWithName(string name)
        {
            Name = name;
        }
        
        #endregion

        #region Abstract Methods

        protected abstract string CreateHtmlString();

        public abstract string ToHtmlString();
        public abstract void AddCssClass(string cssClass);

        #endregion
    }

    /// <summary>
    /// Base class for building MVC controls.
    /// </summary>
    public abstract class ControlBuilder<TBuilder> : ControlBuilder, IFluentControlBuilder<TBuilder>
        where TBuilder : ControlBuilder<TBuilder>
    {
        #region Exposed Methods

        public override void AddCssClass(string cssClass)
        {
            object curClass;
            if (_htmlAttributes.TryGetValue("class", out curClass))
            {
                cssClass = curClass + " " + cssClass;
            }

            _htmlAttributes["class"] = cssClass;
        }

        public override string ToHtmlString()
        {
            //if (_rendered == null)
            //{
            //    _rendered = CreateHtmlString();
            //}
            return CreateHtmlString();
        }

        public sealed override string ToString()
        {
            return ToHtmlString();
        }

        /// <summary>
        /// Takes an anonymous object, or a dictionary, and adds the key value pairs
        /// as html attributes. 
        /// </summary>
        public virtual TBuilder With(object htmlAttributes)
        {
            DoWith(htmlAttributes);
            return (TBuilder)this;
        }

        /// <summary>
        /// Adds an html attribute and value.
        /// </summary>
        public virtual TBuilder With(string key, object value)
        {
            DoWith(key, value);
            return (TBuilder)this;
        }

        /// <summary>
        /// Adds a css class to the list of css classes for this control.
        /// </summary>
        public virtual TBuilder WithCssClass(string cssClass)
        {
            DoWithCssClass(cssClass);
            return (TBuilder)this;
        }

        /// <summary>
        /// Sets the id attribute for this control.
        /// </summary>
        public virtual TBuilder WithId(string id)
        {
            DoWithId(id);
            return (TBuilder)this;
        }

        /// <summary>
        /// Sets the name attribute of this control.
        /// </summary>
        public virtual TBuilder WithName(string name)
        {
            DoWithName(name);
            return (TBuilder)this;
        }

        #endregion
    }
}
