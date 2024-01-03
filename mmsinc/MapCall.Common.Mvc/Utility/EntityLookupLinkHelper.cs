using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.TypeExtensions;
using MMSINC.Data;

namespace MapCall.Common.Utility
{
    public class EntityLookupLinkHelper
    {
        #region Private Members

        private readonly HtmlHelper _html;

        #endregion

        #region Properties

        public Type ViewModelType { get; protected set; }
        public string FieldName { get; protected set; }

        /// <summary>
        /// Some sites, like contractors, do not need this rendered. True by default.
        /// </summary>
        public static bool Enabled { get; set; }

        #endregion

        #region Constructors

        static EntityLookupLinkHelper()
        {
            Enabled = true;
        }

        public EntityLookupLinkHelper(HtmlHelper html, Type viewModelType, string fieldName)
        {
            _html = html;
            ViewModelType = viewModelType;
            FieldName = fieldName;
        }

        #endregion

        #region Private Methods

        protected bool IsTrueScotsman(Type type, out Type modelType)
        {
            modelType = null;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ViewModel<>))
            {
                modelType = type.GetGenericArguments()[0];
                return true;
            }

            return type != typeof(object) && IsTrueScotsman(type.BaseType, out modelType);
        }

        protected bool ShouldRender(out string typeName)
        {
            Type modelType;
            typeName = null;

            if (!Enabled)
            {
                return false;
            }

            if (DependencyResolver.Current.GetService<IAuthenticationService>().CurrentUserIsAdmin &&
                IsTrueScotsman(ViewModelType, out modelType) && modelType.HasPropertyNamed(FieldName))
            {
                var propertyType = modelType.GetProperty(FieldName, BindingFlags.Public | BindingFlags.Instance)
                                            .PropertyType;
                if (propertyType.IsSubclassOf(typeof(EntityLookup)))
                {
                    typeName = propertyType.Name;
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Exposed Methods

        public IHtmlString Render()
        {
            string typeName;
            if (!ShouldRender(out typeName))
            {
                return null;
            }

            // Need a space in the link text because ActionLink throws for null/empty.
            return _html.ActionLink(" ", "Index", typeName, new {area = ""},
                new {@class = "link-button entity-lookup-settings"});
        }

        #endregion
    }
}
