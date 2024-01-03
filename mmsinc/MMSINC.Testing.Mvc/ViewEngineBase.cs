using System;
using System.Linq;
using System.Web.Mvc;
using MMSINC.ClassExtensions.IEnumerableExtensions;

namespace MMSINC.Testing
{
    public abstract class ViewEngineBase : IViewEngine
    {
        #region Private Members

        private bool _isInitialized;
        private IViewEngine[] _previousEngines;

        #endregion

        #region Properties

        public bool ThrowIfViewIsNotRegistered { get; set; }

        #endregion

        #region Constructor

        public ViewEngineBase()
        {
            ThrowIfViewIsNotRegistered = true;
        }

        #endregion

        #region Abstract Methods

        public abstract ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName,
            bool useCache);

        public abstract ViewEngineResult FindView(ControllerContext controllerContext, string viewName,
            string masterName, bool useCache);

        public abstract void ReleaseView(ControllerContext controllerContext, IView view);

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Clears out ViewEngines.Engines and adds this instance to the collection.
        /// </summary>
        public void Init()
        {
            if (_isInitialized)
            {
                throw new Exception($"{GetType().Name} is already initialized.");
            }

            _previousEngines = ViewEngines.Engines.ToArray();
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(this);

            _isInitialized = true;
        }

        /// <summary>
        /// Restores ViewEngines.Engines to the previous collection of engines.
        /// </summary>
        public void Reset()
        {
            if (!_isInitialized)
            {
                throw new Exception($"{GetType().Name} has not been initialized. Init must be called prior to Reset.");
            }

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.AddRange(_previousEngines);
            _previousEngines = null;
            _isInitialized = false;
        }

        #endregion
    }
}
