using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace MMSINC.Testing
{
    public class FakeAreaRegistration : AreaRegistration
    {
        #region Fields

        private string _areaName;

        #endregion

        #region Properties

        public override string AreaName
        {
            get { return _areaName; }
        }

        public Action<AreaRegistrationContext> RegisterAreaAction { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Mimics the internal method called by MVC. 
        /// </summary>
        public void CreateContextAndRegister(RouteCollection routes)
        {
            var baseMethod = typeof(AreaRegistration).GetMethod("CreateContextAndRegister",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            baseMethod.Invoke(this, new object[] {routes, null});
            // this.InvokeInstanceMethod("CreateContextAndRegister", new object[] { routes, null });   
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RegisterAreaAction(context);
        }

        public void SetAreaName(string areaName)
        {
            _areaName = areaName;
        }

        #endregion
    }
}
