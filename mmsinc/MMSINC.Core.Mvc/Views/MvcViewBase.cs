using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Helpers;
using StructureMap;
using StructureMap.Attributes;

namespace MMSINC.Views
{
    /// <summary>
    /// Abstract base class for all layouts/views/partial views/whathaveyous.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MvcViewBase<T> : WebViewPage<T>
    {
        #region Consts

        public const string SHARED_VIEWDATA_KEY = "MvcViewBase<T> Shared ViewData!",
                            DOCUMENT_CLASSES_KEY = "document css classes";

        #endregion

        #region Fields

        private ViewDataDictionary _sharedViewData;
        private FormHelper<T> _formHelper;
        private ControlHelper<T> _controlHelper;
        protected IContainer _container;

        #endregion

        #region Properties

        [SetterProperty]
        public IContainer Container
        {
            set => _container = value;
        }

        /// <summary>
        /// Returns the current IAuthenticationService instance.
        /// </summary>
        [SetterProperty]
        public IAuthenticationService Authentication { get; set; }

        /// <summary>
        /// Replacement for HtmlHelper extensions. Use these when possible!
        /// </summary>
        public ControlHelper<T> Control
        {
            get
            {
                return _controlHelper ?? (_controlHelper =
                    new ControlHelper<T>(ViewContext, ViewData, RouteTable.Routes, _container));
            }
        }

        /// <summary>
        /// A set of css classes that are attached to the html tag of the page.
        /// </summary>
        public HashSet<string> DocumentClasses
        {
            get { return GetSharedData(DOCUMENT_CLASSES_KEY, () => new HashSet<string>()); }
        }

        /// <summary>
        /// Make form things.
        /// </summary>
        public FormHelper<T> Form
        {
            get { return _formHelper ?? (_formHelper = new FormHelper<T>(Html)); }
        }

        /// <summary>
        /// Returns the ViewDataDictionary instance being shared by
        /// all views/partials for the current rendering. Use this if a view
        /// or partial needes to set values that would be used for
        /// a master layout.
        /// </summary>
        protected ViewDataDictionary SharedViewData
        {
            get
            {
                if (_sharedViewData == null)
                {
                    var vd = ViewContext.Controller.ViewData;
                    _sharedViewData = (ViewDataDictionary)vd[SHARED_VIEWDATA_KEY];
                    if (_sharedViewData == null)
                    {
                        _sharedViewData = new ViewDataDictionary();
                        vd[SHARED_VIEWDATA_KEY] = _sharedViewData;
                    }
                }

                return _sharedViewData;
            }
        }

        public IViewModelFactory ViewModelFactory => _container.GetInstance<IViewModelFactory>();

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns a shared instance of an object based on the given key. If there is no shared instance,
        /// then the func provided is used to create a new shared instance.
        /// </summary>
        protected TObj GetSharedData<TObj>(string key, Func<TObj> sharedDataCreatorFunc) where TObj : class
        {
            var val = (TObj)SharedViewData[key];
            if (val == null)
            {
                val = sharedDataCreatorFunc();
                SharedViewData[key] = val;
            }

            return val;
        }

        #endregion

        #region Public Methods

        public string GetDocumentClasses()
        {
            return string.Join(" ", DocumentClasses.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        #endregion
    }
}
