using System.Web.Mvc;
using MMSINC.Common;
using StructureMap;

namespace MMSINC.ErrorHandlers
{
    public class ErrorEmailer : IExceptionFilter
    {
        #region Fields

        private readonly IContainer _container;

        #endregion

        #region Constructors

        public ErrorEmailer(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Methods

        public void OnException(ExceptionContext filterContext)
        {
            using (var mailer = new Utilities.ErrorHandling.ErrorEmailer())
            {
                mailer.SendEmail(new HttpContextWrapper(_container, filterContext.HttpContext),
                    filterContext.Exception);
            }
        }

        #endregion
    }
}
