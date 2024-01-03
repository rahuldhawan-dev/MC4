using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MMSINC.Testing
{
    public class FakeViewEngine : ViewEngineBase
    {
        #region Properties

        public Dictionary<string, IView> Views { get; } = new Dictionary<string, IView>();

        public Dictionary<string, IView> PartialViews { get; } = new Dictionary<string, IView>();

        #endregion

        #region Public Methods

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName,
            bool useCache)
        {
            if (PartialViews.ContainsKey(partialViewName))
            {
                return new ViewEngineResult(PartialViews[partialViewName], this);
            }

            if (ThrowIfViewIsNotRegistered)
            {
                throw new Exception("No PartialView is registered with FakeViewEngine named '" + partialViewName + "'");
            }

            return new ViewEngineResult(Enumerable.Empty<string>());
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName,
            string masterName, bool useCache)
        {
            if (Views.ContainsKey(viewName))
            {
                return new ViewEngineResult(Views[viewName], this);
            }

            if (ThrowIfViewIsNotRegistered)
            {
                throw new Exception("No View is registered with FakeViewEngine named '" + viewName + "'");
            }

            return new ViewEngineResult(Enumerable.Empty<string>());
        }

        public override void ReleaseView(ControllerContext controllerContext, IView view) { }

        #endregion
    }
}
